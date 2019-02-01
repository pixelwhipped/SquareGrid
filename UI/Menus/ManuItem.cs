using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareGrid.Input;

namespace SquareGrid.UI.Menus
{
    public class MenuItem : IDisposable
    {
        public bool Selected;
        public Rectangle Bounds;
        public BaseGame Game;
        public SpriteBatch SpriteBatch;
        public string Name;

        public UnifiedInput UnifiedInput { get { return Game.UnifiedInput; } }
        public MenuItem(BaseGame game, Rectangle bounds, string name)
        {
            Game = game;
            Bounds = bounds;
            Name = name;
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            UnifiedInput.TapListeners.Add(Tap);
        }

        public Vector2 TapLoaction = Vector2.Zero;
        public void Tap(Vector2 value)
        {
            TapLoaction = value;
        }


        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
                SpriteBatch.DrawString(Game.Font, Name, new Vector2(Bounds.X, Bounds.Y), (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.End();
        }

        public void Dispose()
        {
            SpriteBatch.Dispose();
        }
    }
}
