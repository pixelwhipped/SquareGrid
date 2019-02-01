using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SquareGrid.Common;
using SquareGrid.Content;

namespace SquareGrid.UI.Menus
{
    public class ScaleMenuItem : MenuItem
    {

        private readonly Rectangle _rightRegion;
        private readonly Rectangle _leftRegion;
        private readonly Getter<float> _get;
        private readonly Setter<float> _set;
        public float Value { get { return _get(); } }

        public ScaleMenuItem(BaseGame game, Rectangle bounds, Getter<float> get, Setter<float> set, string name)
            : base(game, bounds, name)
        {
            var mx = Textures.Back.Width;
            var my = Textures.Back.Height;
            var xs = (float)Math.Min(my, Bounds.Height) / Math.Max(my, Bounds.Height);
            Bounds = new Rectangle(bounds.X, bounds.Y, (int)(bounds.Width + ((mx * xs) * 2)), bounds.Height);
            _rightRegion = new Rectangle((int)(bounds.X + bounds.Width + (mx * xs)), bounds.Y, (int)(mx * xs), bounds.Height);
            _leftRegion = new Rectangle(bounds.X, bounds.Y, (int)(mx * xs), bounds.Height);
            _get = get;
            _set = set;

        }
        public override void Update(GameTime gameTime)
        {
            if (TapLoaction != Vector2.Zero)
            {
                if (_rightRegion.Contains(TapLoaction))
                {
                    Game.Audio.Play(Cues.Move02);
                    _set(MathHelper.Clamp(_get() + 0.05f, 0f, 1f));
                    TapLoaction = Vector2.Zero;
                }
                if (_leftRegion.Contains(TapLoaction))
                {
                    Game.Audio.Play(Cues.Move01);
                    _set(MathHelper.Clamp(_get() - 0.05f, 0f, 1f));
                    TapLoaction = Vector2.Zero;
                }
            }
            if (Selected && Game.KeyboardInput.TypedKey(Keys.Left))
            {
                Game.Audio.Play(Cues.Move02);
                _set(MathHelper.Clamp(_get() - 0.05f, 0f, 1f));
            }
            if (!Selected || !Game.KeyboardInput.TypedKey(Keys.Right)) return;
            Game.Audio.Play(Cues.Move01);
            _set(MathHelper.Clamp(_get() + 0.05f, 0f, 1f));
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Game.Font, Name + Strings.Space + (int)Math.Round(_get() * 100) + Strings.Percent, new Vector2(Bounds.X + _leftRegion.Width, Bounds.Y), (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.Draw(Textures.Forward, _rightRegion,
                             (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.Draw(Textures.Back, _leftRegion,
                 (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.End();
        }
    }
}
