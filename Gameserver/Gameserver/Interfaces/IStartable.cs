namespace Gameserver.Interfaces
{
    public interface IStartable
    {
        public IUObject Object { get; }
        public IDictionary<string, object> Parameters { get; }
    }
}
