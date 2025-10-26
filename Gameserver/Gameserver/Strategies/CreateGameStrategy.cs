using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Strategies
{
    public class CreateGameStrategy: IStrategy
    {
        public object Strategy(params object[] args)
        {
            int gameid = (int)args[0];
            var parentScope = args[1];
            var quant = (double)args[2];

            var gameQueue = IoC.Resolve<object>("Gameserver.Queue.New");
            var gameScope = IoC.Resolve<object>("Gameserver.Scope.New",gameid,parentScope,quant);
            var gameCommand = IoC.Resolve<Interfaces.ICommand>("Gameserver.Game.Command", gameQueue, gameScope);

            var cmdlist = new List<Interfaces.ICommand> { gameCommand};
            var macrocmd = IoC.Resolve<Interfaces.ICommand>("Gameserver.Command.MacroCommand", cmdlist);
            var injectcmd = IoC.Resolve<Interfaces.ICommand>("Gameserver.Command.InjectCommand", macrocmd);
            var repeatcmd = IoC.Resolve<Interfaces.ICommand>("Gameserver.Command.RepeatCommand",injectcmd);
            cmdlist.Add(repeatcmd);

            var gameMap = IoC.Resolve<IDictionary<int, Interfaces.ICommand>>("Gameserver.Map");
            gameMap.Add(gameid,injectcmd);

            return injectcmd;

        }
    }
}
