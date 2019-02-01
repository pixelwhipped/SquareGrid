using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareGrid.Input;
using SquareGrid.Interfaces;

namespace SquareGrid.UI
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    namespace Square_Grid
    {
        public abstract class Component : IComponent, IDisposable
        {
            public KeyboardInput KeyboardInput
            {
                get { return _game.KeyboardInput; }
            }

            public UnifiedInput UnifiedInput
            {
                get { return _game.UnifiedInput; }
            }

            public Vector2 Center
            {
                get { return _game.Center; }
            }

            public float Width
            {
                get { return _game.Width; }
            }

            public float Height
            {
                get { return _game.Height; }
            }

            public BaseGame Game
            {
                get { return _game; }
            }
            private readonly SpriteBatch _spriteBatch;

            public SpriteBatch SpriteBatch
            {
                get { return _spriteBatch; }
            }

            public Settings Settings
            {
                get{return _game.Settings;}
            }

            public GamePersistance<GameData> GameData
            {
                get { return _game.GameData; }
            }

            public bool IsVisible
            {
                get { return Math.Abs(Transition - 0f) > float.Epsilon; }
            }

            private float _transition;

            public float Transition
            {
                get { return _transition; }
                set { _transition = MathHelper.Clamp(value, 0f, 1f); }
            }

            public bool HasPrevious {
                get { return PreviousComponent != null; }
            }

            public IComponent NextComponent { get; set; }

            public IComponent PreviousComponent;
            private readonly BaseGame _game;

            protected Component(BaseGame game, IComponent previous)
            {
                _game = game;                
                if (previous != null)
                    previous.NextComponent = null;
                PreviousComponent = previous;
                NextComponent = null;
                _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
                _game.KeyboardInput.IsOSKVisable = false;

            }

            public abstract IComponent Update(GameTime gameTime, BaseGame grid);

            public abstract void Draw(GameTime gameTime, BaseGame grid);

            public virtual void Back()
            {
                PreviousComponent.NextComponent = null;
                NextComponent = PreviousComponent;
            }


            public void Dispose()
            {
                SpriteBatch.Dispose();
            }
        }
    }

}
