using Hwdtech;
using System.Reflection;

namespace Gameserver.Commands
{
    public class CompileAdapterCmd : Interfaces.ICommand
    {
        readonly Type _objectType;
        readonly Type _targetType;
        public CompileAdapterCmd(Type objectType, Type targetType)
        {
            _objectType = objectType;
            _targetType = targetType;
        }
        public void Execute()
        {
            var adapter = IoC.Resolve<string>("Gameserver.Adapter.Code", _objectType, _targetType);

            var assembly = IoC.Resolve<Assembly>("Compile", adapter, _targetType);

            var assemblyDict = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Gameserver.Adapter.Assembly");
            var keyPair = new KeyValuePair<Type, Type>(_objectType, _targetType);

            assemblyDict[keyPair] = assembly;
        }
    }
}
