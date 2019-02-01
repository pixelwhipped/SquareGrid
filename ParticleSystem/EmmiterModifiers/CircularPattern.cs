using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGrid.ParticleSystem.EmmiterModifiers
{
    public class CircularPattern : IEmitterModifier
    {
        public float Radius;
        public CircularPattern(float radius)
        {
            Radius = radius;
        }
        public bool IsPattern { get { return true; } }
        public void Update(GameTime gameTime, Emitter e)
        {
            var rads = (float)(BaseGame.Random.NextDouble() * MathHelper.TwoPi);
            var offset = new Vector2((float)Math.Cos(rads) * Radius, (float)Math.Sin(rads) * Radius);
            e.EmissionPoint = Vector2.Add(e.EmitterLocation, offset);
        }
    }
}
