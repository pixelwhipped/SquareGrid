using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGrid.ParticleSystem.ParticleModifiers
{
    public class FountainModifier : IParticleModifier
    {
        private float _g;
        private readonly float _f;
        public FountainModifier(float f)
        {
            _f = f;
        }
        public void Update(GameTime gameTime, Particle p)
        {
            p.Velocity = new Vector2(p.Velocity.X, p.Velocity.Y + (_g * _g));//- .01F);
            _g += _f;
        }
    }
}
