using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SquareGrid.Content;
using SquareGrid.Interfaces;
using SquareGrid.UI.Square_Grid;
using SquareGrid.Utilities;

namespace SquareGrid.States
{
    public class Intro : Component
    {     
        private readonly Tween _gmTween;
        private readonly Tween _gfTween;
        private readonly Tween _rmTween;
        private readonly Tween _rfTween;
        private readonly Tween _imTween;
        private readonly Tween _ifTween;
        private readonly Tween _dmTween;
        private readonly Tween _dfTween;
        private readonly Tween _sfTween;

        public Intro(BaseGame game,IComponent parent) : base(game,parent)
        {
            _gmTween = new Tween(new TimeSpan(0, 0, 0, 2), Width, Center.X - (Textures.G.Width + Textures.R.Width));
            _gfTween = new Tween(new TimeSpan(0, 0, 0, 2), 0, 1);
            _rmTween = new Tween(new TimeSpan(0, 0, 0, 2), Width, Center.X - Textures.G.Width);
            _rfTween = new Tween(new TimeSpan(0, 0, 0, 2), 0, 1);
            _imTween = new Tween(new TimeSpan(0, 0, 0, 2), Width, Center.X);
            _ifTween = new Tween(new TimeSpan(0, 0, 0, 2), 0, 1);
            _dmTween = new Tween(new TimeSpan(0, 0, 0, 2), Width, Center.X + Textures.I.Width);
            _dfTween = new Tween(new TimeSpan(0, 0, 0, 2), 0, 1);
            _sfTween = new Tween(new TimeSpan(0, 0, 0, 6), 0, 1);
        }

        public override IComponent Update(GameTime gameTime, BaseGame game)
        {
            if (NextComponent != null) return NextComponent;
            if (KeyboardInput.Any())
            {
                _gmTween.Finish();
                _gfTween.Finish();
                _rfTween.Finish();
                _rmTween.Finish();
                _ifTween.Finish();
                _imTween.Finish();
                _dfTween.Finish();
                _dmTween.Finish();
                _sfTween.Finish();
            }
            _gmTween.Update(gameTime.ElapsedGameTime);
            _gfTween.Update(gameTime.ElapsedGameTime);
            _sfTween.Update(gameTime.ElapsedGameTime);
            if (!(_gfTween > .5f)) return this;
            _rmTween.Update(gameTime.ElapsedGameTime);
            _rfTween.Update(gameTime.ElapsedGameTime);
            if (!(_rfTween > .5f)) return this;
            _imTween.Update(gameTime.ElapsedGameTime);
            _ifTween.Update(gameTime.ElapsedGameTime);
            if (_ifTween > .5f)
            {
                _dmTween.Update(gameTime.ElapsedGameTime);
                _dfTween.Update(gameTime.ElapsedGameTime);
            }
            if (_dfTween.IsComplete)
            {
                return new MainMenu(Game,null);
            }
            return this;
        }

        public override void Draw(GameTime gameTime, BaseGame game)
        {
            if (NextComponent != null) return;
            SpriteBatch.Begin();
                SpriteBatch.Draw(Textures.Square, new Vector2((Center.X - (Textures.Square.Width/2f))-30f, 10f), Color.White * _sfTween.Value);
                SpriteBatch.Draw(Textures.G, new Vector2(_gmTween.Value, 64f), Color.White * _gfTween.Value);
                SpriteBatch.Draw(Textures.R, new Vector2(_rmTween.Value, 64f), Color.White * _rfTween.Value);
                SpriteBatch.Draw(Textures.I, new Vector2(_imTween.Value, 64f), Color.White * _ifTween.Value);
                SpriteBatch.Draw(Textures.D, new Vector2(_dmTween.Value, 64f), Color.White * _dfTween.Value);
            SpriteBatch.End();
        }

        public override void Back()
        {
            if (HasPrevious) NextComponent = PreviousComponent;
        }
    }
}
