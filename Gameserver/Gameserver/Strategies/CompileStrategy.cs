using Gameserver.Interfaces;
using Hwdtech;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace Gameserver.Strategies
{
    public class CompileStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            Assembly assembly;

            var code = (string)args[0];
            var type = (Type)args[1];

            var tree = CSharpSyntaxTree.ParseText(code);
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            var comp = CSharpCompilation.Create(type.ToString() + "Adapter").WithOptions(options).AddSyntaxTrees(tree);
            comp = comp.AddReferences(IoC.Resolve<IEnumerable<MetadataReference>>("Compile.References"));

            using (var ms = new MemoryStream())
            {
                var res = comp.Emit(ms);
                ms.Seek(0, SeekOrigin.Begin);
                assembly = Assembly.Load(ms.ToArray());
            }
            return assembly;

        }
    }
}
