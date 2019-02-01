using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareGrid.Utilities;

namespace SquareGrid.ParticleSystem.ParticleModifiers
{
    public class ColorFader : IParticleModifier
    {
        private readonly Tween tween;
        public ColorFader(TimeSpan time, float start, float finish)
        {
            tween = new Tween(time, start, finish);
        }
        public void Update(GameTime gameTime, Particle p)
        {
            tween.Update(gameTime.ElapsedGameTime);
            p.Fade = tween;
        }
    }
}
