using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Strategies
{
    public class CreateScopeStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            int gameid = (int)args[0];
            var parentscope = args[1];
            var quant = (double)args[2];

            var gamescope = IoC.Resolve<object>("Scopes.New", parentscope);
            var scopemap = IoC.Resolve<IDictionary<int, object>>("Gameserver.Scopes.Map");
            scopemap.Add(gameid, gamescope);

            var currentscope = IoC.Resolve<object>("Scopes.Current");
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gamescope).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Get.Quantum", (object[] args) => (object)quant).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Queue.Add", (object[] args) => new QueueAddStrategy().Strategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.Queue.Take", (object[] args) => new QueueTakeStrategy().Strategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gamesver.UObject.Get", (object[] args) => new GetUObjectStrategy().Strategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Gameserver.UObject.Delete", (object[] args) => new DeleteUObjectStrategy().Strategy(args)).Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentscope).Execute();

            return gamescope;
        }
    }
}
