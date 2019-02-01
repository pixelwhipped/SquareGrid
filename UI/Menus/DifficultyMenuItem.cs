using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SquareGrid.Common;
using SquareGrid.Content;
using SquareGrid.Utilities;

namespace SquareGrid.UI.Menus
{
    public class DifficultyMenuItem : MenuItem
    {

        private readonly Rectangle _rightRegion;
        private readonly Rectangle _leftRegion;
        private readonly Getter<Difficulty> _get;
        private readonly Setter<Difficulty> _set;

        public DifficultyMenuItem(BaseGame game, Rectangle bounds, Getter<Difficulty> get, Setter<Difficulty> set, string name)
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
                    Right();//_set((int)MathHelper.Clamp(_get() + 1f, 2f, 6f));
                    TapLoaction = Vector2.Zero;
                }
                if (_leftRegion.Contains(TapLoaction))
                {
                    Game.Audio.Play(Cues.Move01);
                    Left();//_set((int)MathHelper.Clamp(_get() - 1f, 2f, 6f));
                    TapLoaction = Vector2.Zero;
                }
            }
            if (Selected && Game.KeyboardInput.TypedKey(Keys.Left))
            {
                Game.Audio.Play(Cues.Move02);
                Left();
            }
            if (!Selected || !Game.KeyboardInput.TypedKey(Keys.Right)) return;
            Game.Audio.Play(Cues.Move01);
            Right();
        }

        private void Left()
        {
            _set((_get() == Difficulty.Easy)
                     ? Difficulty.Hard
                     : (_get() == Difficulty.Hard) ? Difficulty.Normal : Difficulty.Easy);
        }

        private void Right()
        {
            _set((_get() == Difficulty.Easy) ? Difficulty.Normal : (_get() == Difficulty.Normal) ? Difficulty.Hard : Difficulty.Easy);
        }
        public override void Draw(GameTime gameTime)
        {
            var x = Game.Font.MeasureString(Vector2.Zero, Conversion.DifficultyToString(_get())).Width;
            x = (Bounds.Width - (_leftRegion.Width * 2) - x) / 2;
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Game.Font, Conversion.DifficultyToString(_get()), new Vector2(x + Bounds.X + _leftRegion.Width, Bounds.Y), (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.Draw(Textures.Forward, _rightRegion,
                             (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.Draw(Textures.Back, _leftRegion,
                 (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.End();
        }
    }
}
