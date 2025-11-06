using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Commands
{
    public class RegisterCommandsCmd : Interfaces.ICommand
    {
        public void Execute()
        {
            var dependencies = IoC.Resolve<IDictionary<string, IStrategy>>("Gameserver.Dependencies.Get");

            foreach (var dep in dependencies)
            {
                IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Command" + dep.Key, (object[] args) => dep.Value.Strategy(args)).Execute();
            }
        }
    }
}
