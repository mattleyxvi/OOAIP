using Gameserver.Interfaces;

namespace Gameserver.Commands
{
    public class MacroCmd : ICommand
    {
        private readonly IEnumerable<ICommand> _commands;
        public MacroCmd(IEnumerable<ICommand> commands) 
        {
            _commands = commands;
        }
        public void Execute()
        {
            _commands.ToList().ForEach(x => x.Execute());
        }
    }
}
