using Gameserver.Interfaces;
using Hwdtech;
using System.Reflection;

namespace Gameserver.Strategies
{
    public class CreateAdapterStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            var objType = (Type)args[0];
            var targetType = (Type)args[1];

            var assemblyDict = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Gameserver.Adapter.Assembly");
            var keyPair = new KeyValuePair<Type, Type>(objType, targetType);

            if (!assemblyDict.ContainsKey(keyPair))
            {
                IoC.Resolve<Interfaces.ICommand>("Gameserver.Adapter.Compile", objType, targetType).Execute();
            }

            return assemblyDict[keyPair];
        }
    }
}
