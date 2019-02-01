using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SquareGrid.Content;

namespace SquareGrid.UI.Menus
{
    public class CheckedMenuItem : MenuItem
    {
        private readonly Rectangle _clickRegion;
        private readonly Getter<bool> _get;
        private readonly Setter<bool> _set;
        public bool Checked
        {
            get { return _get(); }
        }

        public CheckedMenuItem(BaseGame game, Rectangle bounds, Getter<bool> get, Setter<bool> set, string name)
            : base(game, bounds, name)
        {
            var mx = Math.Max(Textures.Tick.Width, Textures.Cross.Width);
            var my = Math.Max(Textures.Tick.Height, Textures.Cross.Height);
            var xs = (float)Math.Min(my, Bounds.Height) / Math.Max(my, Bounds.Height);
            Bounds = new Rectangle(bounds.X, bounds.Y, (int)(bounds.Width + (mx * xs)), bounds.Height);
            _clickRegion = new Rectangle(bounds.X + bounds.Width, bounds.Y, (int)(mx * xs), bounds.Height);
            _get = get;
            _set = set;
        }

        public override void Update(GameTime gameTime)
        {
            if (_clickRegion.Contains(TapLoaction))
                {
                    Game.Audio.Play(Cues.Move01);
                    _set(!_get());
                    TapLoaction = Vector2.Zero;
                }


            if (!Selected || (!Game.KeyboardInput.TypedKey(Keys.Enter) && !Game.KeyboardInput.TypedKey(Keys.Space)))
                return;
            Game.Audio.Play(Cues.Move02);
            _set(!_get());
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Game.Font, Name, new Vector2(Bounds.X, Bounds.Y), (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            var t = _get() ? Textures.Tick : Textures.Cross;
            SpriteBatch.Draw(t, _clickRegion,
                             (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.End();
        }

    }
}
