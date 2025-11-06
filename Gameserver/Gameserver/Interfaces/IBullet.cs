
namespace Gameserver.Interfaces
{
    public interface IBullet : IMovement
    {
        public string BulletType { get; set; }
    }
}
