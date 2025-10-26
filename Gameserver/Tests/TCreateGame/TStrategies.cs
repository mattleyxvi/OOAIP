using Gameserver.Strategies;
using Hwdtech;
using Hwdtech.Ioc;

namespace Tests.TCreateGame
{
    public class TStrategies
    {
        private readonly object[] _args;
        public TStrategies()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            var scopemap = new Mock<IDictionary<int, object>>();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Scopes.Map", (object[] args) => scopemap.Object).Execute();
            var parentscope = IoC.Resolve<object>("Scopes.Current");
            _args = new[] { 1, parentscope, (double)1 };
            var newscope = new CreateScopeStrategy().Strategy(_args);
            IoC.Resolve<ICommand>("Scopes.Current.Set", newscope).Execute();
        }

        [Fact]
        public void DeleteGameSuccess()
        {
            var gamemap = new Mock<IDictionary<int, Gameserver.Interfaces.IInjectable>>();
            var injcmd = new Mock<Gameserver.Interfaces.IInjectable>();
            gamemap.SetupGet(x => x[It.IsAny<int>()]).Returns(injcmd.Object);
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Map", (object[] args) => gamemap.Object).Execute();
            var emptycmd = new Mock<Gameserver.Interfaces.ICommand>();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.EmptyCommand", (object[] args) => emptycmd.Object).Execute();
            var scopemap = new Mock<IDictionary<int, object>>();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Scope.Map", (object[] args) => scopemap.Object).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Game.Delete", (object[] args) => new DeleteGameStrategy().Strategy(args)).Execute();

            IoC.Resolve<Gameserver.Interfaces.ICommand>("Gameserver.Game.Delete", _args).Execute();

            gamemap.Verify(x => x[It.IsAny<int>()], Times.Once());
            injcmd.Verify(x => x.Inject(It.IsAny<Gameserver.Interfaces.ICommand>()), Times.Once());

        }

        [Fact]
        public void QueueAddSuccess()
        {
            var cmd = new Mock<Gameserver.Interfaces.ICommand>();
            var queue = new Mock<Gameserver.Interfaces.IQueue>();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Get.Queue", (object[] args) => queue.Object).Execute();

            IoC.Resolve<Gameserver.Commands.ActionCommand>("Gameserver.Queue.Add", 1, cmd.Object).Execute();

            queue.Verify(q => q.Add(It.IsAny<Gameserver.Interfaces.ICommand>()), Times.Once());
        }

        [Fact]
        public void QueueTakeSuccess()
        {
            var queue = new Mock<Gameserver.Interfaces.IQueue>();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.Get.Queue", (object[] args) => queue.Object).Execute();

            IoC.Resolve<Gameserver.Commands.ActionCommand>("Gameserver.Queue.Take", 1).Execute();

            queue.Verify(q => q.Take(), Times.Once());

        }

        [Fact]
        public void UObjectGetSuccess()
        {
            var obj = new Mock<Gameserver.Interfaces.IUObject>();
            var objdict = new Mock<IDictionary<int, Gameserver.Interfaces.IUObject>>();
            objdict.SetupGet(x => x[It.IsAny<int>()]).Returns(obj.Object);
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.UObject.Dict", (object[] args) => objdict.Object).Execute();

            var getres = IoC.Resolve<Gameserver.Interfaces.IUObject>("Gamesver.UObject.Get", 1);

            Assert.Equal(getres,obj.Object);
        }

        [Fact]
        public void UObjectDeleteSuccess()
        {
            var objdict = new Mock<IDictionary<int, Gameserver.Interfaces.IUObject>>();
            IoC.Resolve<ICommand>("IoC.Register", "Gameserver.UObject.Dict", (object[] args) => objdict.Object).Execute();

            IoC.Resolve<Gameserver.Interfaces.ICommand>("Gameserver.UObject.Delete", 1).Execute();

            objdict.Verify(d => d.Remove(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public void InjectCmdTest()
        {
            var cmd = new Mock<Gameserver.Interfaces.ICommand>();
            var newcmd = new Mock<Gameserver.Interfaces.ICommand>();
            var inj = new Gameserver.Commands.InjectCmd(cmd.Object);
            
            inj.Inject(newcmd.Object);
            inj.Execute();

            cmd.Verify(c => c.Execute(),Times.Never());
            newcmd.Verify(cmd => cmd.Execute(), Times.Once());

        }
    }
}
