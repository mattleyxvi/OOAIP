using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Commands
{
    public class DeleteGameCommand: Interfaces.ICommand
    {
        private readonly int _gameid;
        public DeleteGameCommand(int gameid)
        {
            _gameid = gameid;
        }

        public void Execute()
        {
            var map = IoC.Resolve<IDictionary<int, IInjectable>>("Gameserver.Map");
            map[_gameid].Inject(IoC.Resolve<Interfaces.ICommand>("Gameserver.EmptyCommand"));
            var scopemap = IoC.Resolve<IDictionary<int, object>>("Gameserver.Scope.Map");
            scopemap.Remove(_gameid);
        }
    }
}
