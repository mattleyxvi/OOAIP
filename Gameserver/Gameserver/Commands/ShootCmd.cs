using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Commands
{
    public class ShootCmd : Interfaces.ICommand
    {
        private readonly IBullet _bullet;
        public ShootCmd(IBullet bullet)
        {
            _bullet = bullet;
        }

        public void Execute()
        {
            var bullet = IoC.Resolve<object>("Gameserver.Create.Bullet", _bullet);
            var cmd = IoC.Resolve<Interfaces.ICommand>("Gameserver.Create.Bullet.Move", bullet);
            IoC.Resolve<Interfaces.ICommand>("Gameserver.Queue.Add", cmd).Execute();
        }
    }
}
