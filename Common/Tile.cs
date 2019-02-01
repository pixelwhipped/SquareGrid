using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGrid.Common
{
    public struct Tile
    {
        public bool Top;
        public bool Bottom;
        public bool Left;
        public bool Right;
        public int Color;
        public bool IsClosed { get { return Top && Bottom && Left && Right; } }
    }
}
