using Hwdtech;
using Hwdtech.Ioc;
using Gameserver.Interfaces;
using Gameserver.Commands;
using Moq;

namespace TMovable.Commands
{
    public class TStartMovementCmd
    {
        private readonly Mock<IStartable> _movecmdstartableMock;
        private readonly Mock<IUObject> _uobjectMock;
        private readonly StartMovementCmd _startmovecmd;
        public TStartMovementCmd()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            _movecmdstartableMock = new Mock<IStartable>();
            _uobjectMock = new Mock<IUObject>();

            _movecmdstartableMock.SetupGet(m => m.Object)
                .Returns(_uobjectMock.Object)
                .Verifiable();

            _movecmdstartableMock.SetupGet(m => m.Parameters)
                .Returns(new Dictionary<string, object>())
                .Verifiable();

            _startmovecmd = new StartMovementCmd(_movecmdstartableMock.Object);
        }

        [Fact]
        public void RegisterPushTargetSuccess()
        {
            //Arrange
            var queueMock = new Mock<IQueue>();
            var mcmdMock = new Mock<Gameserver.Interfaces.ICommand>();
            var cmdMock = new Mock<Gameserver.Interfaces.ICommand>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Operation.Movement",
                (object[] args) => mcmdMock.Object)
                .Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.UObject.SetProperty",
                (object[] args) => cmdMock.Object)
                .Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Queue.Add",
                (object[] args) => queueMock.Object)
                .Execute();

            //Act
            _startmovecmd.Execute();

            //Assert
            _movecmdstartableMock.Verify(m => m.Parameters, Times.Once());
            queueMock.Verify(q => q.Add(It.IsAny<Gameserver.Interfaces.ICommand>()), Times.Once());
        }
    }
}
