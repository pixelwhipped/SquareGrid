using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SquareGrid.Common;
using SquareGrid.Content;

namespace SquareGrid.UI.Menus
{
    public class PlayersMenuItem : MenuItem
    {

        private readonly Rectangle _rightRegion;
        private readonly Rectangle _leftRegion;
        private readonly Getter<int> _get;
        private readonly Setter<int> _set;
        public float Value { get { return _get(); } }

        public PlayersMenuItem(BaseGame game, Rectangle bounds, Getter<int> get, Setter<int> set, string name)
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
            if (Selected && Game.KeyboardInput.TypedKey(Keys.Left))
            {
                Game.Audio.Play(Cues.Move02);
                _set((int)MathHelper.Clamp(_get() - 1f, 2f, 6f));
            }
            if (!Selected || !Game.KeyboardInput.TypedKey(Keys.Right)) return;
            Game.Audio.Play(Cues.Move01);
            _set((int)MathHelper.Clamp(_get() + 1f, 2f, 6f));
        }

        public void Update()
        {
            if (TapLoaction == Vector2.Zero) return;
            if (_rightRegion.Contains(TapLoaction))
            {
                Game.Audio.Play(Cues.Move02);
                _set((int)MathHelper.Clamp(_get() + 1f, 2f, 6f));
                TapLoaction = Vector2.Zero;
            }
            if (!_leftRegion.Contains(TapLoaction)) return;
            Game.Audio.Play(Cues.Move01);
            _set((int)MathHelper.Clamp(_get() - 1f, 2f, 6f));
            TapLoaction = Vector2.Zero;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Game.Font, Name + Strings.Space + _get(), new Vector2(Bounds.X + _leftRegion.Width, Bounds.Y), (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.Draw(Textures.Forward, _rightRegion,
                             (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.Draw(Textures.Back, _leftRegion,
                 (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.End();
        }
    }
}
