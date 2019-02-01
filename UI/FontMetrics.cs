using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquareGrid.UI
{
    public static class FontMetrics
    {
        public static Rectangle MeasureString(this SpriteFont font, Vector2 offset, string str)
        {
            var v = font.MeasureString(str);
            return new Rectangle((int)offset.X, (int)offset.Y, (int)v.X, (int)v.Y);
        }
    }
}
