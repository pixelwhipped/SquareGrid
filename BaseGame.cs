using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SquareGrid.Audio;
using SquareGrid.Content;
using SquareGrid.Input;
using SquareGrid.Interfaces;
using SquareGrid.ParticleSystem;
using SquareGrid.States;
using SquareGrid.UI;

namespace SquareGrid
{
    public class BaseGame : Game, IComponent
    {        
        public static Random Random = new Random();

        private bool _paused;
        public bool IsPaused
        {
            get
            {
                var p = ApplicationView.Value != ApplicationViewState.FullScreenLandscape || _paused;
                if (ParentInterface != null) ParentInterface.ShowPause(p);
                return p;
            }
            set { _paused = value; }
        }

        public GraphicsDeviceManager Graphics;

        public static Texture2D Pixel;
        public float Width { get { return Graphics.GraphicsDevice.Viewport.Width; } }
        public float Height { get { return Graphics.GraphicsDevice.Viewport.Height; } }
       
        public UnifiedInput UnifiedInput { get; private set; }
        public Vector2 Center { get { return new Vector2(Width / 2f, Height / 2f); } }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    0, 0, (int)Width, (int)Height);
            }
        }
        public AudioFx Audio { get; set; }

        public IParent ParentInterface;
        private Settings _settings;
        public SpriteBatch SpriteBatch { get; private set; }

        public Settings Settings
        {
            get { return _settings ?? (_settings = new Settings(this)); }
        }

        public GamePersistance<GameData> GameData { get; set; }

        private List<BackgroundParticle> _backgroundParticles; 
        public BaseGame Game { get { return this; } }
               
        public bool IsVisible { get; private set; }

        private float _transition;

        public float Transition
        {
            get { return _transition; }
            set { _transition = MathHelper.Clamp(value, 0f, 1f); }
        }

        private bool _fade1Xin;
        private bool _fade2Xin;
        public float FadeX1 { get; protected set; }
        public float FadeX2 { get; protected set; }

        private IComponent _currentComponent;
        private IComponent _previousComponent;

        private RenderTarget2D _currentTarget;
        private RenderTarget2D _previousTarget;


        public TouchInput Touch;
        public KeyboardInput KeyboardInput { get; private set; }
        
        public MouseInput Mouse;

        private List<Vector2> _taps;

        public bool HasPrevious { get { return false; } }
        public IComponent NextComponent { get; set; }

        public FontTexture Font;
        public BaseGame(bool isVisible)
        {
            IsVisible = isVisible;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Branding.BackgroundColor = new Color(0, 0, 32);
            NextComponent = null;
        }

        public BaseGame()
        {
            IsVisible = true;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Branding.BackgroundColor = new Color(0, 0, 32);
            NextComponent = null;
        }

        public static byte[] LoadStream(string path)
        {
            var s = TitleContainer.OpenStream(path);
            using (var ms = new MemoryStream())
            {
                s.CopyTo(ms);
                return ms.ToArray();

            }
        } 
        protected override void Initialize()
        {
            _taps = new List<Vector2>();
            GameData = new GamePersistance<GameData>(this);
            Audio = new AudioFx(this);
            _currentTarget = new RenderTarget2D(Graphics.GraphicsDevice, Graphics.PreferredBackBufferWidth,
                                    Graphics.PreferredBackBufferHeight, false,
                                    SurfaceFormat.Color,
                                   DepthFormat.Depth24,
                                   0,
                                   RenderTargetUsage.PreserveContents);
            _previousTarget = new RenderTarget2D(Graphics.GraphicsDevice, Graphics.PreferredBackBufferWidth,
                                                Graphics.PreferredBackBufferHeight, false,
                                   SurfaceFormat.Color,
                                   DepthFormat.Depth24,
                                   0,
                                   RenderTargetUsage.PreserveContents);
            _backgroundParticles = new List<BackgroundParticle>();
            for (var i = 0; i < 100; i++)
            {
                var v = new Vector2(Random.Next((int)Width), Random.Next((int)Height));
                _backgroundParticles.Add(BackgroundParticle.CreateParticle(v));  //Dual Color Effect
                _backgroundParticles.Add(BackgroundParticle.CreateParticle(v));
            }
            _currentComponent = this;
            FadeX1 = 1f;
            FadeX2 = 1f;
            _fade1Xin = false;
            _fade2Xin = false;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            GameData = new GamePersistance<GameData>(this);
            Pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new[] { Color.White });
            Textures.LoadContent(Content);
            Font = new FontTexture(Content.Load<Texture2D>(@"Images\EditSysLarge.png"), 58, 8, -10, new[]
            {
                26,22,36,36,50,47,40,21,28,28,29,33,26,40,22,47,
                47,33,47,47,47,47,47,47,47,47,26,26,51,40,46,47,
                47,47,47,47,47,47,47,47,47,26,47,47,47,47,47,47,
                47,47,47,47,47,47,47,47,47,47,47,29,47,29,33,47,
                26,31,33,30,33,32,28,33,31,22,26,32,21,40,31,24,
                33,33,27,29,28,31,33,40,32,33,31,33,21,33,33,31
            });
            Mouse = new MouseInput();
            IsMouseVisible = false;

            Touch = new TouchInput();
            UnifiedInput = new UnifiedInput(this);
            KeyboardInput = new KeyboardInput(this);
            UnifiedInput.TapListeners.Add(Tap);
            Branding.BackgroundColor = Color.Black;
            Audio.Play(Cues.Music01,AudioChannels.Music,true);
            _currentComponent = new Intro(this,null);
        }

         
        private void Tap(Vector2 value)
        {
            _taps.Add(value);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected async override void Update(GameTime gameTime)
        {
            if (IsPaused) return;
            Touch.Update(gameTime);
            Mouse.Update(gameTime);
            UnifiedInput.Update(gameTime);
            KeyboardInput.Update(gameTime);
            //TODO Update Code            

            foreach (var backgroundParticle in _backgroundParticles)
            {
                backgroundParticle.Update(gameTime);
            }
            

            if (_fade1Xin)
            {
                FadeX1 += 0.025f;
                if (FadeX1 >= 1f) _fade1Xin = false;
            }
            else
            {
                FadeX1 -= 0.025f;
                if (FadeX1 <= .5f) _fade1Xin = true;
            }
            if (_fade2Xin)
            {
                FadeX2 += 0.05f;
                if (FadeX2 >= 1f) _fade2Xin = false;
            }
            else
            {
                FadeX2 -= 0.05f;
                if (FadeX2 <= 0.5f) _fade2Xin = true;
            }

            if ((_currentComponent ?? this).Transition >= 1f)
                NextComponent = (_currentComponent ?? this).Update(gameTime, this);

            if (_currentComponent != null)
            {
                _currentComponent.Transition += 0.05f;
                if (_currentComponent.HasPrevious && _taps.Any(t => new Rectangle((int)Width - 64, 0, 64, 64).Contains(t)))
                {
                    Audio.Play(Cues.Fail);
                    _currentComponent.Back();
                }


                if (_currentComponent.HasPrevious && KeyboardInput.TypedKey(Keys.Escape))
                {
                    Audio.Play(Cues.Fail);
                    _currentComponent.Back();
                }
            }
            if (_previousComponent != null)
                _previousComponent.Transition -= 0.05f;

            if (NextComponent != null && NextComponent != _currentComponent)
            {
                GameData.Save();
                _previousComponent = _currentComponent;
                _currentComponent = NextComponent;
            }

            _taps.Clear();
            base.Update(gameTime);
        }

        public void BeginRenderTargetDraw(RenderTarget2D target)
        {
            Graphics.GraphicsDevice.SetRenderTarget(target);
        }

        public void EndRenderTargetDraw()
        {
            Graphics.GraphicsDevice.SetRenderTarget(_restoreCurrentTarget);
        }

        private RenderTarget2D _restoreCurrentTarget;       
        protected override void Draw(GameTime gameTime)
        {
            if (IsPaused) return;

            if (_currentComponent != null)
            {
                _restoreCurrentTarget = _currentTarget;
                Graphics.GraphicsDevice.SetRenderTarget(_currentTarget);
                Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Transparent, 1.0f, 0);
                _currentComponent.Draw(gameTime, this);
            }
            if (_previousComponent != null)
            {
                _restoreCurrentTarget = _previousTarget;
                Graphics.GraphicsDevice.SetRenderTarget(_previousTarget);
                Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Transparent, 1.0f, 0);
                _previousComponent.Draw(gameTime, this);
            }
            _restoreCurrentTarget = null;
            Graphics.GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Branding.BackgroundColor);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            foreach (var p in _backgroundParticles)
            {
                SpriteBatch.Draw(Textures.Particle, p.Location, null, p.Color, p.Rotation, new Vector2(Textures.Particle.Width / 2f, Textures.Particle.Height / 2f), p.Scale, SpriteEffects.None, 1f);
                SpriteBatch.Draw(Textures.Particle, p.Location, null, p.Color, -p.Rotation, new Vector2(Textures.Particle.Width / 2f, Textures.Particle.Height / 2f), p.Scale, SpriteEffects.None, 1f);
            }
            SpriteBatch.End();

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                if (_currentComponent != null)
                    SpriteBatch.Draw(_currentTarget, Vector2.Zero, Color.White * _currentComponent.Transition);
                if (_previousComponent != null)
                    SpriteBatch.Draw(_previousTarget, Vector2.Zero, Color.White * _previousComponent.Transition);

                if (_currentComponent!= null && _currentComponent.HasPrevious)
                    SpriteBatch.Draw(Textures.Back, new Rectangle((int)Width - 64, 0, 64, 64), null, Color.White);
                SpriteBatch.Draw(Textures.Cursor, new Rectangle((int)Mouse.X, (int)Mouse.Y, 32, 32), null,
                                     Color.White * Mouse.Fade);
            SpriteBatch.End();
            KeyboardInput.Draw(SpriteBatch);
            base.Draw(gameTime);
        }

        public IComponent Update(GameTime gameTime, BaseGame game)
        {
            return this;
        }

        public void Draw(GameTime gameTime, BaseGame game)
        {

        }

        public void Back()
        {

        }

        public async Task<IUICommand> ShowMessageAsync(string title, string message, Action onOk = null, Action onCancel = null)
        {

            var md = new MessageDialog(message, title);

            md.Commands.Add(new UICommand("Ok", ui => { if (onOk != null) onOk(); }));
            if (onCancel != null)
                md.Commands.Add(new UICommand("Cancel", ui => onCancel()));
            var c = await md.ShowAsync();
            return c;
        }

        public void ShowToast(string toast, string title = "Achievement", TimeSpan? time = null)
        {
            if (ParentInterface != null)
                ParentInterface.ShowToast(toast, title, time ?? TimeSpan.FromSeconds(5));
        }

    }
}
