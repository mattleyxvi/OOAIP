using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Commands
{
    internal class DeleteUObjectCommand: Interfaces.ICommand
    {
        private int _id;
        public DeleteUObjectCommand(int id)
        {
            _id = id;
        }
        public void Execute() 
        {
            var dict = IoC.Resolve<IDictionary<int, IUObject>>("Gameserver.UObject.Dict");
            dict.Remove(_id);
        }
    }
}
