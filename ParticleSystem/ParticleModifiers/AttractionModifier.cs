using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGrid.ParticleSystem.ParticleModifiers
{
    public class AttractionModifier : IParticleModifier
    {

        private Vector2 Attractor { get; set; }
        public AttractionModifier(Vector2 attractor)
        {
            Attractor = attractor;
        }
        public void Update(GameTime gameTime, Particle p)
        {
            var dx = p.Position - Attractor;
            p.Velocity += (-dx / dx.LengthSquared());
        }
    }
}
