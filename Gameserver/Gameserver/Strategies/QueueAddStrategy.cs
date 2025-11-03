using Gameserver.Commands;
using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Strategies
{
    public class QueueAddStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            int gameid = (int)args[0];
            var cmd = (Interfaces.ICommand)args[1];

            var queue = IoC.Resolve<IQueue>("Gameserver.Get.Queue", gameid);

            return new ActionCommand(() => { queue.Add(cmd); });
        }
    }
}
