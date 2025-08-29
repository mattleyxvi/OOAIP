using Gameserver.Movement;

namespace Gameserver.Interfaces
{
    public interface IMovement
    {
        public Vector position { get; set; }
        public Vector speed { get; }
    }
}
