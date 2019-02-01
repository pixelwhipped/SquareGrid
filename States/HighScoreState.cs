using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareGrid.Common;
using SquareGrid.Interfaces;
using SquareGrid.UI;
using SquareGrid.UI.Square_Grid;
using SquareGrid.Utilities;

namespace SquareGrid.States
{
    public class HighScoreState : Component
    {
       
        private float _scroll;
        public HighScoreState(BaseGame game, IComponent parent)
            : base(game,parent)
        {            
        }

        public override IComponent Update(GameTime gameTime, BaseGame game)
        {
            if (NextComponent != null)
                return NextComponent;
            
            _scroll -= 1.1f;
            var max = ((Game.GameData.Data.HighScores.Count * 60) + 100);
            if (max + _scroll <= 0)
                Back();
            return this;
        }

        public override void Draw(GameTime gameTime, BaseGame game)
        {
            SpriteBatch.Begin();
            var offset = new Vector2(0, 200 + _scroll);
            var sStr = Strings.HighScores;
            var p = Game.Font.MeasureString(offset, sStr);
            offset = new Vector2(Center.X - (p.Width / 2f), offset.Y + p.Height);
            if (offset.Y < 200)
                SpriteBatch.DrawString(Game.Font, sStr, offset, Color.White * ((offset.Y - 100f) / 100f));
            else
                SpriteBatch.DrawString(Game.Font, sStr, offset, Color.White);
            foreach (var s in Game.GameData.Data.HighScores)
            {
                sStr = s.Name + Strings.Tab + s.Score;
                p = Game.Font.MeasureString(offset, sStr);
                offset = new Vector2(Center.X - (p.Width / 2f), offset.Y + p.Height);
                if (offset.Y < 200)
                    SpriteBatch.DrawString(Game.Font, sStr, offset, GameColors.Colors[s.Color].Color * ((offset.Y - 100f) / 100f));
                else
                    SpriteBatch.DrawString(Game.Font, sStr, offset, GameColors.Colors[s.Color].Color);
            }
            SpriteBatch.End();
        }
    }
}
