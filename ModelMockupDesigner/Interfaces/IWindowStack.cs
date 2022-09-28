using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Interfaces
{
    public interface IWindowStack
    {
        event EventHandler OnClosed;
        void CloseAsync();
        WindowParameters GetWindowParameters();
    }
}
