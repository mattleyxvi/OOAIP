using Gameserver.Commands;
using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Strategies
{
    public class CreateStartMoveCmdStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            var obj = args[0];
            var cmd = new StartMovementCmd(IoC.Resolve<IStartable>("Gameserver.Adapter.Create", obj, typeof(IStartable)));
            return cmd;
        }
    }
}
