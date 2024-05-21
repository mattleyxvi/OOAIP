using Hwdtech;
using Gameserver.Interfaces;

namespace Gameserver.Commands
{
    public class StartMovementCmd : Interfaces.ICommand
    {
        private readonly IStartable _startable;
        public StartMovementCmd(IStartable startable)
        {
            _startable = startable;
        }

        public void Execute()
        {
            _startable.Parameters.ToList().ForEach(parameter => IoC.Resolve<Interfaces.ICommand>(
                    "Gameserver.UObject.SetProperty",
                    _startable.Object,
                    parameter.Key,
                    parameter.Value
                ).Execute()
            );
            var cmd = IoC.Resolve<Interfaces.ICommand>("Gameserver.Operation.Movement", _startable.Object);
            IoC.Resolve<Interfaces.ICommand>("Gameserver.UObject.SetProperty", _startable.Object, "Movement", cmd).Execute();
            IoC.Resolve<IQueue>("Gameserver.Queue.Add").Add(cmd);
        }
    }
}