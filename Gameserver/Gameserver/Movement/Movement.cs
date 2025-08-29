using Gameserver.Interfaces;

namespace Gameserver.Movement
{
    public class Movement : ICommand
    {
        private readonly IMovement _movable;
        public Movement(IMovement movable)
        {
            _movable = movable;
        }
        public void Execute() => _movable.position += _movable.speed;
    }
}
