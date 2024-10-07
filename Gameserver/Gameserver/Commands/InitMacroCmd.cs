using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Commands
{
    public class InitMacroCmd : IStrategy
    {
        public object Strategy(params object[] args)
        {
            (var cmdname, var uobject) = ((string)args[0], (IUObject)args[1]);
            var commands = IoC.Resolve<IEnumerable<string>>("Gameserver.Config." + cmdname)
            .Select(command => IoC.Resolve<Interfaces.ICommand>(command, uobject)).ToList();
            return IoC.Resolve<Interfaces.ICommand>("Gameserver.Command.Macrocommand", commands);
        }
    }
}
