using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SharpDX;
using SquareGrid.Content;
using SquareGrid.Utilities;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SquareGrid.UI.Menus
{
    public class ColorMenuItem : MenuItem
    {

        private readonly Rectangle _rightRegion;
        private readonly Rectangle _leftRegion;
        private readonly Getter<int> _get;
        private readonly Setter<int> _set;
        private readonly List<int> _colors;
        private int _index;

        public ColorMenuItem(BaseGame game, Rectangle bounds, Getter<int> get, Setter<int> set, string name, List<int> colors)
            : base(game, bounds, name)
        {
            _colors = colors;
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
                Left();
            }
            if (Selected && Game.KeyboardInput.TypedKey(Keys.Right))
            {
                Game.Audio.Play(Cues.Move01);
                Right();
            }

            if (_rightRegion.Contains(TapLoaction))
            {
                Game.Audio.Play(Cues.Move02);
                Right();
                TapLoaction = Vector2.Zero;
            }
            if (!_leftRegion.Contains(TapLoaction)) return;
            Game.Audio.Play(Cues.Move01);
            Left();
            TapLoaction = Vector2.Zero;
        }

        private void Left()
        {
            _index = _index == 0 ? _colors.Count- 1 : _index - 1;
            _set(_colors[_index]);
        }

        private void Right()
        {
            _index = ((_index + 1) > _colors.Count - 1) ? 0 : _index + 1;
            _set(_colors[_index]);
        }
        public override void Draw(GameTime gameTime)
        {
            var x = Game.Font.MeasureString(Vector2.Zero, GameColors.Colors[_get()].Name.ToUpperInvariant()).Width;
            x = (Bounds.Width - (_leftRegion.Width * 2) - x) / 2;
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Game.Font, GameColors.Colors[_get()].Name.ToUpperInvariant(), new Vector2(x + Bounds.X + _leftRegion.Width, Bounds.Y), (Selected) ? GameColors.Colors[_get()].Color * Game.FadeX2 : GameColors.Colors[_get()].Color);
            SpriteBatch.Draw(Textures.Forward, _rightRegion,
                             (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.Draw(Textures.Back, _leftRegion,
                 (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.End();
        }
    }
}
