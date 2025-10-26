using Gameserver.Strategies;
using Hwdtech;
using Hwdtech.Ioc;

namespace Tests.TCreateGame
{
    public class TCreateGame
    {
        public TCreateGame()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void CreateGameSuccess()
        {
            var cmdmock = new Mock<Gameserver.Interfaces.ICommand>();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Queue.New", (object[] args) => (object)1).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Scope.New", (object[] args) => (object)1).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Game.Command", (object[] args) => cmdmock.Object).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Command.MacroCommand", (object[] args) => cmdmock.Object).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Command.InjectCommand", (object[] args) => cmdmock.Object).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Command.RepeatCommand", (object[] args) => cmdmock.Object).Execute();
            var gamemap = new Mock<IDictionary<int, Gameserver.Interfaces.ICommand>>();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Map", (object[] args) => gamemap.Object).Execute();

            var creategamestrategy = new CreateGameStrategy();
            creategamestrategy.Strategy(1, 1, (double)1);

            gamemap.Verify(d => d.Add(It.IsAny<int>(), It.IsAny<Gameserver.Interfaces.ICommand>()), Times.Once());
        }

        [Fact]
        public void CreateScopeSuccess()
        {
            var scopemap = new Mock<IDictionary<int, object>>();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Scopes.Map", (object[] args) => scopemap.Object).Execute();

            var createscope = new CreateScopeStrategy();
            createscope.Strategy(1, IoC.Resolve<object>("Scopes.Current"), (double)1);

            scopemap.Verify(d => d.Add(It.IsAny<int>(), It.IsAny<object>()), Times.Once());
        }
    }
}
