using Gameserver.Interfaces;
using Gameserver.Commands;

namespace Gameserver.Strategies
{
    public class DeleteGameStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            int gameid = (int)args[0];
            return  new DeleteGameCommand(gameid);
        }
    }
}
