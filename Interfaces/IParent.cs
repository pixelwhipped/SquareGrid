using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGrid.Interfaces
{
    public interface IParent
    {
        void ShowToast(string toast, string title = "Achievement", TimeSpan? time = null);
        void ShowPause(bool p);
    }
}
