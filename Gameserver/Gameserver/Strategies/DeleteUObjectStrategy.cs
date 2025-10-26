using Gameserver.Interfaces;
using Gameserver.Commands;

namespace Gameserver.Strategies
{
    public class DeleteUObjectStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            var id = (int)args[0];
            return new DeleteUObjectCommand(id);
        }
    }
}
