using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using Gameserver.Commands;
using Gameserver.Interfaces;

namespace TMovable.Commands
{
    public class TCreateMacroCmd
    {
        private readonly Mock<Gameserver.Interfaces.ICommand> _cmdMock;
        private readonly Mock<IUObject> _uobjectMock;
        public TCreateMacroCmd()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            _cmdMock = new Mock<Gameserver.Interfaces.ICommand>();
            _uobjectMock = new Mock<IUObject>();
        }

        [Fact]
        public void SuccessfullCreateMacrocommand()
        {
            _cmdMock.Setup(x => x.Execute()).Verifiable();

            var actionname = "Movement";

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register", "Gameserver.Config." + actionname,
                (object[] args) => new List<string> { "Gameserver.Command.Movement" }).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register", "Gameserver.Command.Macrocommand.Create",
                (object[] args) => new InitMacroCmd().Strategy(args[0], args[1])).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register", "Gameserver.Command.Movement",
                (object[] args) => _cmdMock.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Command.Macrocommand",
                (object[] args) => _cmdMock.Object).Execute();

            IoC.Resolve<Gameserver.Interfaces.ICommand>("Gameserver.Command.Macrocommand.Create",
                actionname,
                _uobjectMock.Object).Execute();

            _cmdMock.VerifyAll();
        }

        [Fact]
        public void MacrocommandWorksGood()
        {
            _cmdMock.Setup(x => x.Execute()).Verifiable();

            new MacroCmd(
                new List<Gameserver.Interfaces.ICommand>
                {
                    _cmdMock.Object,
                    _cmdMock.Object,
                    _cmdMock.Object,
                    _cmdMock.Object,
                    _cmdMock.Object
                }
            ).Execute();

            _cmdMock.Verify(x => x.Execute(), Times.Exactly(5));
        }
    }
}