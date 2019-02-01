using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGrid.ParticleSystem
{
    public static class ParticleBatch
    {
        public static void Draw(this SpriteBatch spriteBatch, ParticleEngine p)
        {
            p.Draw(spriteBatch);
        }

        public static void Draw(this SpriteBatch spriteBatch, Particle p)
        {
            p.Draw(spriteBatch);
        }

        public static void Draw(this SpriteBatch spriteBatch, Emitter p)
        {
            p.Draw(spriteBatch);
        }
    }
}
