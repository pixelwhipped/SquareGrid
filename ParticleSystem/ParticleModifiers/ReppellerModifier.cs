using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGrid.ParticleSystem.ParticleModifiers
{
    public class ReppellerModifier : IParticleModifier
    {

        private Vector2 Reppeller { get; set; }
        public ReppellerModifier(Vector2 reppeller)
        {
            Reppeller = reppeller;
        }
        public void Update(GameTime gameTime, Particle p)
        {

            var dx = Reppeller - p.Position;
            p.Velocity += (-dx / dx.LengthSquared());

        }
    }
}
