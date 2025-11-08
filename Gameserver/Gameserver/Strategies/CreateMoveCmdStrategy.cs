using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Strategies
{
    public class CreateMoveCmdStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            var obj = args[0];
            var cmd = new Movement.Movement(IoC.Resolve<IMovement>("Gameserver.Adapter.Create", obj, typeof(IMovement)));
            return cmd;
        }
    }
}
