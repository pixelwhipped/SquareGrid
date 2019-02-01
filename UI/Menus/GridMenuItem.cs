using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SquareGrid.Common;
using SquareGrid.Content;
using SquareGrid.Utilities;

namespace SquareGrid.UI.Menus
{

    public class GridMenuItem : MenuItem
    {

        private readonly Rectangle _rightRegion;
        private readonly Rectangle _leftRegion;
        private readonly Getter<int> _get;
        private readonly Setter<int> _set;

        public int XTiles { get { return Game.GameData.Data.GameGridsVsMode[_get()].XTiles; } }
        public int YTiles { get { return Game.GameData.Data.GameGridsVsMode[_get()].YTiles; } }
        public int TileSize { get { return Game.GameData.Data.GameGridsVsMode[_get()].TileSize; } }
        public bool Locked { get { return Game.GameData.Data.GameGridsVsMode[_get()].Locked; } }

        private Color _colorA;
        private Color _colorB;
        private Color _colorC;
        private Color _colorD; 

        private float _rotation;
        private readonly Tween _rcolor;

        public GridMenuItem(BaseGame game, Rectangle bounds, Getter<int> get, Setter<int> set, string name)
            : base(game, bounds, name)
        {
            _colorA = new Color((float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble());
            _colorB = new Color((float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble());
            _colorC = new Color((float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble());
            _colorD = new Color((float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble());
            _rcolor = new Tween(new TimeSpan(0, 0, 0, 2), 0, 1);
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
            _rotation = MathHelper.WrapAngle(_rotation + 0.01f);
            _rcolor.Update(gameTime.ElapsedGameTime);
            if(_rcolor.IsComplete)
            {
                _colorA = _colorB;
                _colorB = new Color((float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble());
                _colorC = _colorD;
                _colorD = new Color((float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble());
                _rcolor.Reset();
            }

            if (TapLoaction != Vector2.Zero)
            {
                if (_rightRegion.Contains(TapLoaction))
                {
                    Game.Audio.Play(Cues.Move02);
                    //_set(MathHelper.Clamp(_get() - 0.05f, 0f, 1f));
                    Right();
                    TapLoaction = Vector2.Zero;
                }
                if (_leftRegion.Contains(TapLoaction))
                {
                    Game.Audio.Play(Cues.Move01);
                    //_set(MathHelper.Clamp(_get() + 0.05f, 0f, 1f));
                    Left();
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
            _set((_get() == 0) ? Game.GameData.Data.GameGridsVsMode.Count() - 1 : _get() - 1);
        }

        private void Right()
        {
            _set(((_get() + 1) > Game.GameData.Data.GameGridsVsMode.Count() - 1) ? 0 : _get() + 1);
        }
        public override void Draw(GameTime gameTime)
        {
            var xw = Game.Font.MeasureString(Vector2.Zero, Strings.Grid + Strings.Space  + XTiles + Strings.X + YTiles).Width;
            xw = (Bounds.Width - (_leftRegion.Width * 2) - xw) / 2;
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Game.Font, Strings.Grid + Strings.Space + XTiles + Strings.X + YTiles, new Vector2(xw + Bounds.X + _leftRegion.Width, Bounds.Y), (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.Draw(Textures.Forward, _rightRegion,
                             (Selected) ? Color.White * Game.FadeX2 : Color.Gray);
            SpriteBatch.Draw(Textures.Back, _leftRegion,
                 (Selected) ? Color.White * Game.FadeX2 : Color.Gray);

            var xo = (int)((Game.Width / 1.5f) - ((XTiles * (TileSize / 2f)) / 2f));
            var yo = (Game.Height / 2f) - ((YTiles * (TileSize / 2f)) / 2f);

            for (var y = 0; y < YTiles + 1; y++)
            {
                for (var x = 0; x < XTiles + 1; x++)
                {
                    DrawParticle((int)((x * TileSize / 1.5)) + xo, (int)((y * TileSize / 1.5) + yo));
                }
            }
            if (Locked)
            {
                SpriteBatch.Draw(Textures.Lock, new Vector2((int)(xo + (XTiles * (TileSize / 1.5))) - Textures.Lock.Width, (int)(yo + (YTiles * (TileSize / 1.5))) - Textures.Lock.Height), Color.White * .5f);
            }
            SpriteBatch.End();
        }
        public void DrawParticle(int x, int y)
        {
            SpriteBatch.Draw(Textures.Particle, new Vector2(x, y), null, Color.Lerp(_colorA, _colorB, _rcolor), _rotation, new Vector2(16, 16), 1f, SpriteEffects.None, 1);
            SpriteBatch.Draw(Textures.Particle, new Vector2(x, y), null, Color.Lerp(_colorC, _colorD, _rcolor), -_rotation, new Vector2(16, 16), 1f, SpriteEffects.None, 1);            
        }
    }
}
