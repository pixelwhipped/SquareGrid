using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.Xna.Framework;
using SquareGrid.Utilities;

namespace SquareGrid.ParticleSystem
{
    public class BackgroundParticle
    {
        public float Rotation;
        public float RotationRate;
        public Tween Scale;
        public Color Color;
        public Tween NextFlare;
        public bool FlareUp;
        public bool FlareDown;
        public Vector2 Location;
        private float _upScale, _downScale;
        public static BackgroundParticle CreateParticle(Vector2 location)
        {
            var u = (float)BaseGame.Random.NextDouble()  * 10.5f;
            var d = (float)BaseGame.Random.NextDouble();
            return new BackgroundParticle
            {
                _upScale = u,
                _downScale = d,
                Rotation = 0f,
                RotationRate = (float)BaseGame.Random.NextDouble() * 0.05f,
                Scale =
                    new Tween(new TimeSpan(0, 0, 0, 3), d, u),
                Color =
                    new Color((float)BaseGame.Random.NextDouble(), (float)BaseGame.Random.NextDouble(),
                              (float)BaseGame.Random.NextDouble()),
                NextFlare = new Tween(new TimeSpan(0, 0, 0, BaseGame.Random.Next(40) + 6), 0, 0),
                FlareUp = false,
                FlareDown = false,
                Location = location
            };
        }

        public void Update(GameTime gameTime)
        {
            NextFlare.Update(gameTime.ElapsedGameTime);
            Rotation = MathHelper.WrapAngle(Rotation + RotationRate);
            if (FlareUp)
            {
                Scale.Update(gameTime.ElapsedGameTime);
                if (Scale.IsComplete)
                {
                    FlareUp = false;
                    FlareDown = true;
                    Scale = new Tween(Scale.TotalTime, _upScale, _downScale);
                }
            }
            else if (FlareDown)
            {
                Scale.Update(gameTime.ElapsedGameTime);
                if (Scale.IsComplete)
                {
                    FlareUp = false;
                    FlareDown = false;
                }
            }
            if (!NextFlare.IsComplete) return;
            FlareUp = true;
            Scale = new Tween(new TimeSpan(0, 0, 0, 3), _downScale, _upScale);
            NextFlare =
                new Tween(new TimeSpan(0, 0, 0, BaseGame.Random.Next(40) + 6), 0, 0);
            //return particle;
        }
    }
}
