using Gameserver.Commands;
using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Strategies
{
    public class CreateShootCmdStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            var obj = args[0];
            var cmd = new ShootCmd(IoC.Resolve<IBullet>("Gameserver.Adapter.Create", obj, typeof(IBullet)));
            return cmd;
        }
    }
}
