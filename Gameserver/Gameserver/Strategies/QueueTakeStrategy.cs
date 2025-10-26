using Gameserver.Interfaces;
using Gameserver.Commands;
using Hwdtech;

namespace Gameserver.Strategies
{
    public class QueueTakeStrategy: IStrategy
    {
        public object Strategy(params object[] args)
        {
            int gameid = (int)args[0];
            var queue = IoC.Resolve<IQueue>("Gameserver.Get.Queue",gameid);
            return new ActionCommand(() => { queue.Take(); });
        }
    }
}
