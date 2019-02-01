using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareGrid.UI;
using SquareGrid.Utilities;

namespace SquareGrid.ParticleSystem
{
    public class ScoreParticle
    {
        public int X, Y, Tx, Ty;
        public int Score;
        public Tween ScoreFade;
        public Tween ExplosionFade;
        public Tween ExplosionScale;
        public FontTexture Font;
        public Texture2D Explosion;
        public Color Color;
        public int XDirection;
        public ScoreParticle(int x, int y, int tx, int ty, int score, FontTexture font, Texture2D tex, Color c)
        {

            XDirection = (int)(BaseGame.Random.Next(1) == 1 ? BaseGame.Random.NextDouble() * 5 : BaseGame.Random.NextDouble() * -5);
            X = x;
            Y = y;
            Tx = tx;
            Ty = ty;
            Score = score;
            Font = font;
            Explosion = tex;
            Color = c;
            ScoreFade = new Tween(new TimeSpan(0, 0, 0, 8), 1f, 0f);
            ExplosionFade = new Tween(new TimeSpan(0, 0, 0, 5), 1f, 0f);
            ExplosionScale = new Tween(new TimeSpan(0, 0, 0, 5), 0f, 4f);
        }

        public void Update(GameTime gameTime)
        {
            Y -= 4;
            X += XDirection;
            ScoreFade.Update(gameTime.ElapsedGameTime);
            ExplosionFade.Update(gameTime.ElapsedGameTime);
            ExplosionScale.Update(gameTime.ElapsedGameTime);
        }
        public void Draw(SpriteBatch batch)
        {

            batch.Draw(Explosion, new Vector2(Tx, Ty), null, Color * ExplosionFade.Value, 0f,
                       new Vector2(Explosion.Width / 2f, Explosion.Height / 2f), ExplosionScale.Value, SpriteEffects.None,
                       1f);
            batch.DrawString(Font, Score.ToString(), new Vector2(X, Y), Color * ScoreFade.Value);

        }
        public bool IsDead()
        {
            return ScoreFade.IsComplete && ExplosionFade.IsComplete;
        }
    }
}
