using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquareGrid.UI
{
    public class FontTexture
    {
        public Texture2D Font;
        public Dictionary<char, Rectangle> Rects;
        public int Height;
        public int Spacing;
        public FontTexture(Texture2D texture, int height, int padding, int spaceing, int[] rects)
        {
            Spacing = spaceing;
            Height = height;
            Font = texture;
            Rects = new Dictionary<char, Rectangle>();
            Rectangle badChar;
            var c = new[]
            {
                ' ', '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?',
                '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N' ,'O',
                'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', '^', '_',
                '`', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',
                'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '{', '|', '}', '~' 
            }; //Last is error
            var yoff = padding;
            var index = 0;
            //try
            //{
            for (var y = 0; y < 5; y++)
            {
                var xoff = padding;
                for (var x = 0; x < 16; x++)
                {
                    Rects.Add(c[index], new Rectangle(xoff + 1, yoff + 1, rects[index] - 2, height - 2));
                    xoff += rects[index] + padding;
                    index++;

                }
                yoff += padding + height;
            }
            //  }
            // catch (Exception e)
            // {
            //     var ex = e;
            //  }
        }

        public Rectangle MeasureString(Vector2 v, string p)
        {
            var w = 0;
            var chars = p.ToCharArray();
            foreach (var c in chars)
            {
                try
                {
                    w += Rects[c].Width - Spacing;
                }
                catch (Exception e)
                {
                    w += 0;
                }
            }
            return new Rectangle((int)v.X, (int)v.Y, w, Height);
        }

        public Vector2 MeasureString(string p)
        {
            var v = MeasureString(Vector2.Zero, p);
            return new Vector2(v.Width, v.Height);
        }
    }

    public static class FontExtension
    {
        public static void DrawString(this SpriteBatch batch, FontTexture f, string str, Vector2 posistion, Color color)
        {
            DrawString(batch, f, str, posistion, color, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }

        public static void DrawString(this SpriteBatch batch, FontTexture f, string str, Vector2 posistion, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float depth)
        {
            var xoff = posistion.X;
            foreach (var c in str.ToCharArray())
            {
                batch.Draw(f.Font, new Vector2(xoff, (int)posistion.Y), f.Rects[c],
                    color, rotation, origin, scale, effect, depth);
                xoff += f.Rects[c].Width * scale;
            }
        }
    }
}
