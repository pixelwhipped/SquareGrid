using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SquareGrid.Common;
using SquareGrid.Content;
using SquareGrid.Utilities;

namespace SquareGrid.UI.Menus
{
    public class PlayerTypeMenuItem : MenuItem
    {

        private readonly Rectangle _rightRegion;
        private readonly Rectangle _leftRegion;
        private readonly Getter<PlayerType> _get;
        private readonly Setter<PlayerType> _set;

        public PlayerTypeMenuItem(BaseGame game, Rectangle bounds, Getter<PlayerType> get, Setter<PlayerType> set, string name)
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
                if (_rightRegion.Contains(TapLoaction) || _leftRegion.Contains(TapLoaction))
                {
                    Game.Audio.Play(Cues.Move02);
                    //_set(MathHelper.Clamp(_get() - 0.05f, 0f, 1f));
                    Swap();
                    TapLoaction = Vector2.Zero;
                }
            }
            if ((!Selected || !Game.KeyboardInput.TypedKey(Keys.Left)) &&
                (!Selected || !Game.KeyboardInput.TypedKey(Keys.Right))) return;
            Game.Audio.Play(Cues.Move02);
            Swap();
        }
        private void Swap()
        {
            _set((_get() == PlayerType.Human)
                     ? PlayerType.Computer
                     : PlayerType.Human);
        }

        public override void Draw(GameTime gameTime)
        {
            var x = Game.Font.MeasureString(Vector2.Zero, Conversion.PlayerTypeToString(_get())).Width;
            x = (Bounds.Width - (_leftRegion.Width * 2) - x) / 2;
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Game.Font, Conversion.PlayerTypeToString(_get()), new Vector2(x + Bounds.X + _leftRegion.Width, Bounds.Y), (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.Draw(Textures.Forward, _rightRegion,
                             (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.Draw(Textures.Back, _leftRegion,
                 (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.End();
        }
    }
}
