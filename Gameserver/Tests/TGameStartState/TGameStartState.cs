using Gameserver.Commands;
using Gameserver.Interfaces;
using Gameserver.Movement;
using Gameserver.Strategies;
using Hwdtech;
using Hwdtech.Ioc;

namespace Tests.TGameStartState
{
    public class TGameStartState
    {
        public TGameStartState()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void InitializeCommandsSuccess()
        {
            var objMock = new Mock<IUObject>();
            var dep = new Dictionary<string, IStrategy>
            {
                { "Movement", new CreateMoveCmdStrategy() },
                { "StartMovement", new CreateStartMoveCmdStrategy() },
                { "Shoot", new CreateShootCmdStrategy() }
            };
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Dependencies.Get", (object[] args) => dep).Execute();

            new RegisterCommandsCmd().Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Adapter.Create", (object[] args) => new Mock<IMovement>().Object).Execute();
            var movecmd = IoC.Resolve<Gameserver.Interfaces.ICommand>("Gameserver.Command.Movement", objMock.Object);

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Adapter.Create", (object[] args) => new Mock<IStartable>().Object).Execute();
            var startmovecmd = IoC.Resolve<Gameserver.Interfaces.ICommand>("Gameserver.Command.StartMovement", objMock.Object);

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Adapter.Create", (object[] args) => new Mock<IBullet>().Object).Execute();
            var shootcmd = IoC.Resolve<Gameserver.Interfaces.ICommand>("Gameserver.Command.Shoot", objMock.Object);

            Assert.Equal(typeof(Movement), movecmd.GetType());
            Assert.Equal(typeof(StartMovementCmd), startmovecmd.GetType());
            Assert.Equal(typeof(ShootCmd), shootcmd.GetType());
        }

        [Fact]
        public void BulletCommandTest()
        {
            var bulletMock = new Mock<IBullet>();
            var cmdMock = new Mock<Gameserver.Interfaces.ICommand>();
            cmdMock.Setup(x => x.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Create.Bullet", (object[] args) => (object)1).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Create.Bullet.Move", (object[] args) => cmdMock.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Queue.Add", (object[] args) => cmdMock.Object).Execute();

            var bulletCmd = new ShootCmd(bulletMock.Object);
            bulletCmd.Execute();

            cmdMock.Verify(x => x.Execute(), Times.Once());
        }

    }
}
