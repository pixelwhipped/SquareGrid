using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareGrid.Content;
using SquareGrid.Interfaces;
using SquareGrid.UI;
using SquareGrid.UI.Square_Grid;

namespace SquareGrid.States
{
    public class HelpState : Component
    {
        private static readonly string[] HelpLines =
        {
            "Use the grid points","to create lines",
            "once a square is ","complete that player",
            "receives points ","and another turn.",
            " ",
            "For each square you ","created during your",
            "turn you will receive ","additional points ",
            "based on your current",
            "consecutive square ","multiplier.",
            " ",
            "Unlock levels by ","completing the grids.",
            " ",
            "Navigate using the ","up, down, left",
            "and right keys ","and place the line",
            "by using the enter ","or space key."
        };       

        public HelpState(BaseGame grid,IComponent parent)
            : base(grid, parent)
        {            
        }

        public override IComponent Update(GameTime gameTime, BaseGame game)
        {
            return NextComponent ?? this;
        }

        public override void Draw(GameTime gameTime, BaseGame game)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Textures.Help, new Vector2(Width - Textures.Help.Width, Height - Textures.Help.Height), Color.White);
            var offset = new Vector2(10, 100);
            foreach (var helpLine in HelpLines.Select(t => t.ToUpperInvariant()))
            {
                SpriteBatch.DrawString(Game.Font, helpLine, offset, Color.White, 0, Vector2.Zero, 0.45f, SpriteEffects.None, 1);
                offset = new Vector2(offset.X, offset.Y + (60 * .45f));
            }
            SpriteBatch.End();
        }                
    }
}
