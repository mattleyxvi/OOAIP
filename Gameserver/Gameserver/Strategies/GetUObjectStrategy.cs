using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Strategies
{
    public class GetUObjectStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            int id = (int)args[0];
            var dict = IoC.Resolve<IDictionary<int, IUObject>>("Gameserver.UObject.Dict");

            return dict[id];
        }
    }
}
