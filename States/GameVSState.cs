using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SquareGrid.Common;
using SquareGrid.Content;
using SquareGrid.Effects;
using SquareGrid.Input;
using SquareGrid.Interfaces;
using SquareGrid.ParticleSystem;
using SquareGrid.UI;
using SquareGrid.UI.Square_Grid;
using SquareGrid.Utilities;

namespace SquareGrid.States
{
    public interface IGridTileOwner
    {
        Tile[][] CurrentTiles { get; set; }
        Tile[][] PreviousTiles { get; set; }
        Player CurrentPlayer { get; }
        GameType GameType { get; }
        GamePersistance<GameData> GameData { get; set; }
    }
    public class GameVSState : Component, IGridTileOwner
    {
        public GameType GameType { get; private set; }
        public GamePersistance<GameData> GameData {
            get { return Game.GameData; }
            set { Game.GameData = value; } 
        }

        private readonly Player[] _players;
        private int _currentPlayer;

        public Tile[][] CurrentTiles { get; set; }
        public Tile[][] PreviousTiles { get; set; }
        private int _x;
        private int _y;
        private Vector2 _offset;
        private TileSelection _tileSelection;
        private readonly Tween _colorLerp;
        private bool _gameOver;
        private float _multiplier = 1f;
        private readonly List<ScoreParticle> _scoreParticles;
        private readonly Tween _cpuPlayerTween;
        private readonly Tween _gameOverFade;
        private readonly Tween _gameOverScale;
        private Vector2 _cm1 = Vector2.Zero;
        private Vector2 _cm2 = Vector2.Zero;
        private int _mx1;
        private int _my1;
        private int _mx2;
        private int _my2;
        private bool _unlock;
        private bool _scored;

        private bool _inAI;

        public Player CurrentPlayer { get { return _players[_currentPlayer]; } }

        private Player PreviousPlayer
        {
            get
            {
                return _currentPlayer - 1 < 0 ? _players[_players.Count() - 1] : _players[_currentPlayer - 1];
            }
        }

        private float _rotation;


        //Effect bloomExtractEffect;
        //Effect bloomCombineEffect;
        //Effect gaussianBlurEffect;
        
        //RenderTarget2D sceneRenderTarget;
        //RenderTarget2D renderTarget1;
        //RenderTarget2D renderTarget2;

        // Choose what display settings the bloom should use.
        //public BloomSettings Settings
        //{
        //    get { return settings; }
        //    set { settings = value; }
        //}

        //BloomSettings settings = BloomSettings.PresetSettings[0];


        // Optionally displays one of the intermediate buffers used
        // by the bloom postprocess, so you can see exactly what is
        // being drawn into each rendertarget.
        //public enum IntermediateBuffer
       // {
        //    PreBloom,
        //    BlurredHorizontally,
        //    BlurredBothWays,
        //    FinalResult,
        //}

        public GameVSState(BaseGame game,IComponent parent, GameType gameType, Player[] players)
            : base(game, parent)
        {
            UnifiedInput.TapListeners.Add(Tap);            
            GameType = gameType;
            _players = players;
            _scoreParticles = new List<ScoreParticle>();
            _tileSelection = TileSelection.X;
            _cpuPlayerTween = new Tween(new TimeSpan(0, 0, 0, 0, 500), 0, 1);
            _gameOverFade = new Tween(new TimeSpan(0, 0, 0, 8), 0, 1);
            _gameOverScale = new Tween(new TimeSpan(0, 0, 0, 8), 1, 2.5f);
            _players = players;
            _x = 0;
            _y = 0;
            try
            {
                _offset = new Vector2(
                    Center.X -
                    (Game.GameData.Data.GameGridsVsMode[GameType.Grid].XTiles * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize / 2f),
                    Center.Y -
                    (Game.GameData.Data.GameGridsVsMode[GameType.Grid].YTiles * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) / 2f);
            }
            catch (Exception e)
            {
                throw e;
            }
            _colorLerp = new Tween(new TimeSpan(2000L), 1f, 0f);
            CurrentTiles = new Tile[Game.GameData.Data.GameGridsVsMode[GameType.Grid].YTiles][];
            PreviousTiles = new Tile[Game.GameData.Data.GameGridsVsMode[GameType.Grid].YTiles][];
            for (var i = 0; i < CurrentTiles.Count(); i++)
            {
                CurrentTiles[i] = new Tile[Game.GameData.Data.GameGridsVsMode[GameType.Grid].XTiles];
                PreviousTiles[i] = new Tile[Game.GameData.Data.GameGridsVsMode[GameType.Grid].XTiles];
            }
            for (var i = 0; i < CurrentTiles.Count(); i++)
            {
                for (var j = 0; j < CurrentTiles[i].Count(); j++)
                {
                    CurrentTiles[i][j] = new Tile { Bottom = false, Top = false, Left = false, Right = false, Color = -1 };
                    PreviousTiles[i][j] = new Tile { Bottom = false, Top = false, Left = false, Right = false, Color = -1 };
                }
            }
            
            //bloomExtractEffect = new Effect(Game.GraphicsDevice,
            //        BaseGame.LoadStream(@"Content\Effects\BloomExtract.mgfxo"));
            //bloomCombineEffect = new Effect(Game.GraphicsDevice,
             //       BaseGame.LoadStream(@"Content\Effects\BloomCombine.mgfxo"));
            //gaussianBlurEffect = new Effect(Game.GraphicsDevice,
            //        BaseGame.LoadStream(@"Content\Effects\GaussianBlur.mgfxo"));
            

            // Look up the resolution and format of our main backbuffer.
            //PresentationParameters pp = game.GraphicsDevice.PresentationParameters;

            //int width = pp.BackBufferWidth;
            //int height = pp.BackBufferHeight;

            //SurfaceFormat format = pp.BackBufferFormat;

            // Create a texture for rendering the main scene, prior to applying bloom.
            //sceneRenderTarget = new RenderTarget2D(game.GraphicsDevice, width, height, false,
            //                                       format, pp.DepthStencilFormat, pp.MultiSampleCount,
            //                                       RenderTargetUsage.DiscardContents);

            // Create two rendertargets for the bloom processing. These are half the
            // size of the backbuffer, in order to minimize fillrate costs. Reducing
            // the resolution in this way doesn't hurt quality, because we are going
            // to be blurring the bloom images in any case.
            //width /= 2;
            //height /= 2;

            //renderTarget1 = new RenderTarget2D(game.GraphicsDevice, width, height, false, format, DepthFormat.None);
            //renderTarget2 = new RenderTarget2D(game.GraphicsDevice, width, height, false, format, DepthFormat.None);
        }

        private Vector2 TapLoaction = Vector2.Zero;
        public void Tap(Vector2 value)
        {
            TapLoaction = value;
        }

        public void MoveLeft()
        {
            if (_x == 0)
            {
                _tileSelection = (_tileSelection == TileSelection.X) ? TileSelection.Y : TileSelection.X;
                if (_y == Game.GameData.Data.GameGridsVsMode[GameType.Grid].YTiles) _y--;
            }
            else if (_x - 1 >= 0) _x--;
        }

        public void MoveRight()
        {
            if (_x < Game.GameData.Data.GameGridsVsMode[GameType.Grid].XTiles) _x++;
            if (_x >= Game.GameData.Data.GameGridsVsMode[GameType.Grid].XTiles && _tileSelection == TileSelection.Y)
            {

                _tileSelection = TileSelection.X;
                if (_y == Game.GameData.Data.GameGridsVsMode[GameType.Grid].YTiles) _y--;
            }
        }

        public void MoveUp()
        {
            if (_y - 1 >= 0 && _tileSelection != TileSelection.X) _y--;
            _tileSelection = ((_tileSelection == TileSelection.Y) ? TileSelection.X : TileSelection.Y);
            if (_x == Game.GameData.Data.GameGridsVsMode[GameType.Grid].XTiles) _x--;
        }

        public void MoveDown()
        {
            if (_y < Game.GameData.Data.GameGridsVsMode[GameType.Grid].YTiles && _tileSelection != TileSelection.Y) _y++;
            if (_y != Game.GameData.Data.GameGridsVsMode[GameType.Grid].YTiles)
                _tileSelection = ((_tileSelection == TileSelection.Y) ? TileSelection.X : TileSelection.Y);
            else
            {
                _tileSelection = TileSelection.Y;
            }
            if (_x == Game.GameData.Data.GameGridsVsMode[GameType.Grid].XTiles) _x--;
        }

        public bool EndTurn(bool isCpus)
        {
            bool pass = true;
            if (!isCpus)
            {
                switch (_tileSelection)
                {
                    case TileSelection.X:
                        if (_x == Game.GameData.Data.GameGridsVsMode[GameType.Grid].XTiles)
                        {
                            if (CurrentTiles[_y][_x - 1].Right)
                            {
                                pass = false;
                                break;
                            }
                            else
                            {
                                CurrentTiles[_y][_x - 1] = new Tile
                                {
                                    Left = CurrentTiles[_y][_x - 1].Left,
                                    Right = true,
                                    Top = CurrentTiles[_y][_x - 1].Top,
                                    Bottom = CurrentTiles[_y][_x - 1].Bottom,
                                    Color = CurrentPlayer.Color
                                };
                            }
                        }
                        else
                        {
                            if (CurrentTiles[_y][_x].Left)
                            {
                                pass = false;
                                break;
                            }
                            else
                            {
                                CurrentTiles[_y][_x] = new Tile
                                {
                                    Left = true,
                                    Right = CurrentTiles[_y][_x].Right,
                                    Top = CurrentTiles[_y][_x].Top,
                                    Bottom = CurrentTiles[_y][_x].Bottom,
                                    Color = CurrentPlayer.Color
                                };
                            }
                        }
                        if (_x > 0)
                        {
                            if (CurrentTiles[_y][_x - 1].Right)
                            {
                                pass = _x == Game.GameData.Data.GameGridsVsMode[GameType.Grid].XTiles;
                                if (pass == false) break;
                            }
                            else
                            {
                                CurrentTiles[_y][_x - 1] = new Tile
                                {
                                    Left = CurrentTiles[_y][_x - 1].Left,
                                    Right = true,
                                    Top = CurrentTiles[_y][_x - 1].Top,
                                    Bottom = CurrentTiles[_y][_x - 1].Bottom,
                                    Color = CurrentPlayer.Color
                                };
                            }
                        }
                        break;
                    case TileSelection.Y:
                        if (_y == Game.GameData.Data.GameGridsVsMode[GameType.Grid].YTiles)
                        {
                            if (CurrentTiles[_y - 1][_x].Bottom)
                            {
                                pass = false;
                                break;
                            }
                            else
                            {
                                CurrentTiles[_y - 1][_x] = new Tile
                                {
                                    Left = CurrentTiles[_y - 1][_x].Left,
                                    Right = CurrentTiles[_y - 1][_x].Right,
                                    Top = CurrentTiles[_y - 1][_x].Top,
                                    Bottom = true,
                                    Color = CurrentPlayer.Color
                                };
                            }
                        }
                        else
                        {
                            if (CurrentTiles[_y][_x].Top)
                            {
                                pass = false;
                                break;
                            }
                            else
                            {
                                CurrentTiles[_y][_x] = new Tile
                                {
                                    Left = CurrentTiles[_y][_x].Left,
                                    Right = CurrentTiles[_y][_x].Right,
                                    Top = true,
                                    Bottom = CurrentTiles[_y][_x].Bottom,
                                    Color = CurrentPlayer.Color
                                };
                            }
                        }
                        if (_y > 0)
                        {
                            if (CurrentTiles[_y - 1][_x].Bottom)
                            {
                                pass = _y == Game.GameData.Data.GameGridsVsMode[GameType.Grid].YTiles;
                                if (pass == false) break;
                            }
                            else
                            {
                                CurrentTiles[_y - 1][_x] = new Tile
                                {
                                    Left = CurrentTiles[_y - 1][_x].Left,
                                    Right = CurrentTiles[_y - 1][_x].Right,
                                    Top = CurrentTiles[_y - 1][_x].Top,
                                    Bottom = true,
                                    Color = CurrentPlayer.Color
                                };
                            }
                        }
                        break;
                }
            }
            if (pass && !isCpus)
            {
                if (!IsAndAddScore())
                {
                    _multiplier = 1;
                    _colorLerp.Reset();
                    _currentPlayer++;
                    if (_currentPlayer >= _players.Count())
                        _currentPlayer = 0;
                }
                else
                {
                    _multiplier += 0.5f;
                }
            }

            return pass;

        }

        private bool IsAndAddScore()
        {
            int found = 0;
            for (int y = 0; y < CurrentTiles.Count(); y++)
            {
                for (int x = 0; x < CurrentTiles[y].Count(); x++)
                {
                    if (CurrentTiles[y][x].IsClosed && !PreviousTiles[y][x].IsClosed)
                    {
                        found++;
                        var s = (int)((10 * found) * _multiplier);
                        AddScoreParticle(x, y, s);
                        _players[_currentPlayer].Score += s;
                    }
                    PreviousTiles[y][x] = CurrentTiles[y][x];
                }
            }

            return found != 0;
        }

        private void AddScoreParticle(int x, int y, int s)
        {
            Game.Audio.Play(Cues.Success);
            _scoreParticles.Add(new ScoreParticle((x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.X,
                                                    (y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.Y,
                                                    (((x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize / 2)) + (int)_offset.X),
                                                    (((y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize / 2)) + (int)_offset.Y),
                                                    s,
                                                    Game.Font, Textures.Explosion, GameColors.Colors[CurrentPlayer.Color].Color));
        }

        public void DrawTile(int x, int y)
        {            
                      
            if (CurrentTiles[y][x].Top)
                SpriteBatch.Draw(Textures.Top, new Rectangle((x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.X, (y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.Y, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize), Color.Lerp(GameColors.Colors[CurrentPlayer.Color].Color, GameColors.Colors[PreviousPlayer.Color].Color, _colorLerp.Value));
            if (CurrentTiles[y][x].Bottom)
                SpriteBatch.Draw(Textures.Bottom, new Rectangle((x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.X, (y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.Y, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize), Color.Lerp(GameColors.Colors[CurrentPlayer.Color].Color, GameColors.Colors[PreviousPlayer.Color].Color, _colorLerp.Value));
            if (CurrentTiles[y][x].Left)
                SpriteBatch.Draw(Textures.Left, new Rectangle((x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.X, (y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.Y, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize), Color.Lerp(GameColors.Colors[CurrentPlayer.Color].Color, GameColors.Colors[PreviousPlayer.Color].Color, _colorLerp.Value));
            if (CurrentTiles[y][x].Right)
                SpriteBatch.Draw(Textures.Right, new Rectangle((x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.X, (y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.Y, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize), Color.Lerp(GameColors.Colors[CurrentPlayer.Color].Color, GameColors.Colors[PreviousPlayer.Color].Color, _colorLerp.Value));
            if (CurrentTiles[y][x].IsClosed)
                SpriteBatch.Draw(Textures.Center, new Rectangle((x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.X, (y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.Y, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize), GameColors.Colors[CurrentTiles[y][x].Color].Color*.5f);
            
        }
        

        public void DrawParticle(int x, int y)
        {
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            SpriteBatch.Draw(Textures.Particle, new Vector2(x, y), null, Color.Lerp(GameColors.Colors[CurrentPlayer.Color].Color, GameColors.Colors[PreviousPlayer.Color].Color, _colorLerp.Value), _rotation, new Vector2(16, 16), 1f, SpriteEffects.None, 1);
            SpriteBatch.Draw(Textures.Particle, new Vector2(x, y), null, Color.Lerp(GameColors.Colors[CurrentPlayer.Color].Color, GameColors.Colors[PreviousPlayer.Color].Color, _colorLerp.Value), -_rotation, new Vector2(16, 16), 1f, SpriteEffects.None, 1);
            SpriteBatch.End();


        }

        public void DrawTiles()
        {

            SpriteBatch.Begin();
            for (int y = 0; y < CurrentTiles.Count(); y++)
            {
                for (int x = 0; x < CurrentTiles[y].Count(); x++)
                {

                    DrawTile(x, y);
                }
            }
            SpriteBatch.End();
            for (int y = 0; y < Game.GameData.Data.GameGridsVsMode[GameType.Grid].YTiles + 1; y++)
            {
                for (int x = 0; x < Game.GameData.Data.GameGridsVsMode[GameType.Grid].XTiles + 1; x++)
                {
                    DrawParticle((x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.X, (y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.Y);
                }
            }

            switch (_tileSelection)
            {
                case TileSelection.X:
                    SpriteBatch.Begin();
                    SpriteBatch.Draw(Textures.Left, new Rectangle((_x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.X, (_y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.Y, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize), null, Color.Lerp(Color.White, Color.Lerp(GameColors.Colors[CurrentPlayer.Color].Color, GameColors.Colors[PreviousPlayer.Color].Color, _colorLerp.Value), .5f));
                    SpriteBatch.Draw(Textures.Right, new Rectangle(((_x - 1) * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.X, (_y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.Y, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize), null, Color.Lerp(Color.White, Color.Lerp(GameColors.Colors[CurrentPlayer.Color].Color, GameColors.Colors[PreviousPlayer.Color].Color, _colorLerp.Value), .5f));
                    SpriteBatch.End();
                    break;

                case TileSelection.Y:
                    SpriteBatch.Begin();
                    SpriteBatch.Draw(Textures.Top, new Rectangle((_x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.X, (_y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.Y, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize), null, Color.Lerp(Color.White, Color.Lerp(GameColors.Colors[CurrentPlayer.Color].Color, GameColors.Colors[PreviousPlayer.Color].Color, _colorLerp.Value), .5f));
                    SpriteBatch.Draw(Textures.Bottom, new Rectangle((_x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.X, ((_y - 1) * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + (int)_offset.Y, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize, Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize), null, Color.Lerp(Color.White, Color.Lerp(GameColors.Colors[CurrentPlayer.Color].Color, GameColors.Colors[PreviousPlayer.Color].Color, _colorLerp.Value), .5f));
                    SpriteBatch.End();

                    break;
            }

        }

        private void DoAI()
        {
            if (_inAI)
            {
                if (_cpuPlayerTween.IsComplete)
                {
                    _inAI = false;
                    if (!IsAndAddScore())
                    {
                        _multiplier = 1;
                        _colorLerp.Reset();
                        _currentPlayer++;
                        if (_currentPlayer >= _players.Count())
                            _currentPlayer = 0;
                    }
                    else
                    {
                        _multiplier += 0.5f;
                    }
                }

            }
            else
            {
                _inAI = true;
                _cpuPlayerTween.Reset();
                switch (GameType.Difficulty)
                {
                    case Difficulty.Easy:
                        AI.AIEasy(this);
                        break;
                    case Difficulty.Normal:
                        AI.AINormal(this);
                        break;
                    case Difficulty.Hard:
                        AI.AIHard(this);
                        break;
                }
                Game.Audio.Play(Cues.Move01);
            }

        }



        public override IComponent Update(GameTime gameTime, BaseGame game)
        {
            if (NextComponent != null)
                return NextComponent;

            _cpuPlayerTween.Update(gameTime.ElapsedGameTime);
            _rotation += 0.05f;
            _rotation = MathHelper.WrapAngle(_rotation);
            _colorLerp.Update(new TimeSpan(50L));

            #region Human
            if (CurrentPlayer.Type == PlayerType.Human && !_gameOver)
            {
                var positions = new List<Vector2>();

                for (var y = 0; y < Game.GameData.Data.GameGridsVsMode[GameType.Grid].YTiles + 1; y++)
                {
                    for (var x = 0; x < Game.GameData.Data.GameGridsVsMode[GameType.Grid].XTiles + 1; x++)
                    {
                        positions.Add(new Vector2((x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + _offset.X,
                                                  (y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + _offset.Y));
                    }
                }
                if (!Game.Mouse.Hidden || Game.Touch.IsTouched)
                {
                    Vector2 loc = Game.Touch.IsTouched
                        ? Game.Touch.Location
                        : Game.Mouse.Location;
                    Vector2? closest1 = null;
                    var closestDistance1 = float.MaxValue;
                    foreach (var position in positions)
                    {
                        var distance = Vector2.DistanceSquared(position, loc);
                        if (closest1.HasValue && distance >= closestDistance1) continue;
                        closest1 = position;
                        closestDistance1 = distance;
                    }
                    _cm1 = closest1 ?? Vector2.Zero;
                    Vector2? closest2 = null;
                    var closestDistance2 = float.MaxValue;
                    foreach (var position in positions)
                    {
                        if (position == _cm1) continue;
                        var distance = Vector2.DistanceSquared(position, loc);
                        if (closest2.HasValue && (distance >= closestDistance2)) continue;
                        closest2 = position;
                        closestDistance2 = distance;
                    }
                    _cm2 = closest2 ?? Vector2.Zero;

                    _mx1 = (int)(((_cm1.X - _offset.X) / Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize));
                    _my1 = (int)(((_cm1.Y - _offset.Y) / Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize));
                    _mx2 = (int)(((_cm2.X - _offset.X) / Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize));
                    _my2 = (int)(((_cm2.Y - _offset.Y) / Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize));
                    if (_mx2 != _mx1)
                    {
                        var nx = Math.Min(_mx1, _mx2);
                        var ny = Math.Min(_my1, _my2);
                        if (_x != nx || _y != ny)
                            Game.Audio.Play(Cues.Move02);
                        _x = nx;
                        _y = ny;
                        _tileSelection = TileSelection.Y;

                    }
                    else
                    {
                        var nx = Math.Min(_mx1, _mx2);
                        var ny = Math.Min(_my1, _my2);
                        if (_x != nx || _y != ny)
                            Game.Audio.Play(Cues.Move01);
                        _x = nx;
                        _y = ny;
                        _tileSelection = TileSelection.X;
                    }


                    var xy = new Vector2((_x * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + _offset.X,
                                         (_y * Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize) + _offset.Y);
                    var d = Vector2.Distance(xy, loc);

                    if ((Game.Mouse.LeftSingleClick || TapLoaction != Vector2.Zero) &&
                        (d <= (Game.GameData.Data.GameGridsVsMode[GameType.Grid].TileSize * .75)))
                    {
                        Game.Audio.Play(Cues.Move02);
                        Game.Mouse.ComponentChange();
                        TapLoaction = Vector2.Zero;
                        if (EndTurn(false))
                        {
                            //do something
                        }
                    }
                }
                if (Game.KeyboardInput.TypedKey(Keys.Enter) || Game.KeyboardInput.TypedKey(Keys.Space))
                {
                    Game.Audio.Play(Cues.Move02);
                    if (EndTurn(false))
                    {
                        //do something
                    }

                }
                if (Game.KeyboardInput.TypedKey(Keys.Up))
                {
                    Game.Audio.Play(Cues.Move02);
                    MoveUp();
                }
                if (Game.KeyboardInput.TypedKey(Keys.Down))
                {
                    Game.Audio.Play(Cues.Move02);
                    MoveDown();
                }
                if (Game.KeyboardInput.TypedKey(Keys.Left))
                {
                    Game.Audio.Play(Cues.Move01);
                    MoveLeft();
                }
                if (Game.KeyboardInput.TypedKey(Keys.Right))
                {
                    Game.Audio.Play(Cues.Move01);
                    MoveRight();
                }

            }
            #endregion

            else if (!_gameOver)
            {
                DoAI();
            }
            if (_gameOver)
            {
                _gameOverFade.Update(gameTime.ElapsedGameTime);
                _gameOverScale.Update(gameTime.ElapsedGameTime);
            }
            else
            {
                _gameOver = true;
                for (var i = 0; i < CurrentTiles.Count(); i++)
                {
                    for (var j = 0; j < CurrentTiles[i].Count(); j++)
                    {
                        if (CurrentTiles[i][j].IsClosed) continue;
                        _gameOver = false;
                        break;
                    }
                }

                if (_gameOver & !_scored)
                {

                    var hs = _players.Max(ps => ps.Score);

                    for (var index = 0; index < _players.Length; index++)
                    {
                        var p = _players[index];
                        if (p.Score == hs) _currentPlayer = index;
                    }
                    var s = 5;
                    for (var i = 0; i < CurrentTiles.Count(); i++)
                    {
                        for (var j = 0; j < CurrentTiles[i].Count(); j++)
                        {
                            if (CurrentTiles[i][j].Color != CurrentPlayer.Color) continue;
                            AddScoreParticle(j, i, s);
                            _players[_currentPlayer].Score += s;
                            s += 5;
                        }
                    }
                    if (CurrentPlayer.Type == PlayerType.Human)
                    {
                        _unlock = true;
                    }
                    _scored = true;
                }
            }
            foreach (var scoreParticle in _scoreParticles)
            {
                scoreParticle.Update(gameTime);
            }
            _scoreParticles.RemoveAll(p => p.IsDead());
            if (_gameOverFade.IsComplete)
            {
                Back();
            }
            return this;
        }

        public override void Draw(GameTime gameTime, BaseGame game)
        {
            DrawTiles();
            SpriteBatch.Begin();
            foreach (var scoreParticle in _scoreParticles)
            {
                scoreParticle.Draw(SpriteBatch);
            }
            SpriteBatch.End();
            if (_gameOver)
            {
                var textSize = Game.Font.MeasureString(Strings.GameOver);
                var textCenter = new Vector2(Center.X + (textSize.X / 2), Center.Y);
                var hs = _players.Max(ps => ps.Score);
                var winner = CurrentPlayer;
                foreach (var p in _players)
                {
                    if (p.Score == hs) winner = p;
                }
                var textSize2 = Game.Font.MeasureString(winner.Name.ToUpperInvariant() + Strings.Space + Strings.Wins);
                var textCenter2 = new Vector2(Center.X + (textSize2.X / 2), Center.Y);

                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);



                SpriteBatch.DrawString(Game.Font, Strings.GameOver, textCenter - (textSize / 2), Color.White * _gameOverFade.Value, 0, textSize / 2, _gameOverScale.Value, SpriteEffects.None, 1f);
                SpriteBatch.DrawString(Game.Font, winner.Name.ToUpperInvariant() + Strings.Space + Strings.Wins, (textCenter2 - (textSize2 / 2)) + new Vector2(0, 60), Color.White * _gameOverFade.Value, 0, textSize2 / 2, _gameOverScale.Value, SpriteEffects.None, 1f);
                SpriteBatch.End();
            }
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Game.Font, CurrentPlayer.Name.ToUpper(), new Vector2(10, 100), GameColors.Colors[CurrentPlayer.Color].Color);
            SpriteBatch.DrawString(Game.Font, Strings.Score, new Vector2(10, 160), GameColors.Colors[CurrentPlayer.Color].Color);
            SpriteBatch.DrawString(Game.Font, CurrentPlayer.Score.ToString(), new Vector2(10, 220), GameColors.Colors[CurrentPlayer.Color].Color);
            var z = 280;
            for (var i = 0; i < _players.Count(); i++)
            {
                if (_currentPlayer == i) continue;
                SpriteBatch.DrawString(Game.Font, _players[i].Name.ToUpper(), new Vector2(10, z), GameColors.Colors[_players[i].Color].Color, 0f, Vector2.Zero, .5f, SpriteEffects.None, 1f);
                SpriteBatch.DrawString(Game.Font, _players[i].Score.ToString(), new Vector2(10, z + 30), GameColors.Colors[_players[i].Color].Color, 0f, Vector2.Zero, .5f, SpriteEffects.None, 1f);
                z += 60;
            }

            SpriteBatch.End();
        }

        public override void Back()
        {
            if (_gameOver)
            {
                foreach (var player in _players)
                {
                    Game.GameData.Data.HighScores.Add(new HighScore { Color = player.Color, Name = player.Name, Score = player.Score });
                }
                Game.GameData.Data.HighScores = new List<HighScore>(Game.GameData.Data.HighScores.OrderByDescending(p => p.Score));
                if (Game.GameData.Data.HighScores.Count >= 100)
                {
                    Game.GameData.Data.HighScores.RemoveRange(100, Game.GameData.Data.HighScores.Count - 100);
                }
                NextComponent = new HighScoreState(Game, new MainMenu(Game,null));
                try
                {
                    if (_unlock)
                        Game.GameData.Data.GameGridsVsMode[GameType.Grid + 1] = new GameGrid
                        {
                            GameMode =
                                Game.GameData.Data.GameGridsVsMode[
                                    GameType.Grid + 1].GameMode,
                            Locked = false,
                            TileSize =
                                Game.GameData.Data.GameGridsVsMode[
                                    GameType.Grid + 1].TileSize,
                            XTiles =
                                Game.GameData.Data.GameGridsVsMode[
                                    GameType.Grid + 1].XTiles,
                            YTiles =
                                Game.GameData.Data.GameGridsVsMode[
                                    GameType.Grid + 1].YTiles
                        };
                }
                catch
                {
                    //Finished Game.
                }

            }
            else
            {
                NextComponent = new InGameMenuState(Game, this);
            }
        }
    }
}
