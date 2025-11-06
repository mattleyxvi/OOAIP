using Gameserver.Commands;
using Gameserver.Interfaces;
using Gameserver.Strategies;
using Hwdtech;
using Hwdtech.Ioc;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace Tests.TCompileAdapter
{
    public class TCreateAdapterStrategy
    {
        public TCreateAdapterStrategy()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }


        [Fact]
        public void CreateAdapterSuccess()
        {
            var assemblyDict = new Dictionary<KeyValuePair<Type, Type>, Assembly>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Adapter.Assembly", (object[] args) => assemblyDict).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Adapter.Compile", (object[] args) => new CompileAdapterCmd((Type)args[0], (Type)args[1])).Execute();
            var code = @"using Gameserver.Movement;
        namespace Gameserver.Interfaces;
        public class IMovementAdapter : IMovement
        {
            public IMovementAdapter() {}
            public Vector position
            {
                get => new Vector(new int[] { 0, 0 });
                set => new Vector(new int[] { 0, 0 });
            }
            public Vector speed
            {
                get => new Vector(new int[] { 1, 1 });
            }
        }";
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Adapter.Code", (object[] args) => code).Execute();

            var metadataReferences = new List<MetadataReference>
         {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IMovement).Assembly.Location)
         };
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile.References", (object[] args) => metadataReferences).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile", (object[] args) => new CompileStrategy().Strategy(args)).Execute();
            var objType = typeof(object);
            var interfaceType = typeof(IMovement);

            var adapterAssembly = (Assembly)new CreateAdapterStrategy().Strategy(objType, interfaceType);

            Assert.Equal("Gameserver.Interfaces.IMovementAdapter", adapterAssembly.GetName().Name);
            adapterAssembly.GetType("Gameserver.Interfaces.IMovementAdapter");
            var o = Activator.CreateInstance(adapterAssembly.GetType("Gameserver.Interfaces.IMovementAdapter")!)!;
            Assert.Equal("Gameserver.Interfaces.IMovementAdapter", o.GetType().ToString());


        }
    }

}
