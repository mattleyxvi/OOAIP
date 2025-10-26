using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameserver.Interfaces
{
    public interface IInjectable
    {
        void Inject(ICommand cmd);
    }
}
