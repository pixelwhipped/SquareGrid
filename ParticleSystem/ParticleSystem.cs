using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGrid.ParticleSystem
{
    public class ParticleEngine
    {
        private readonly List<Emitter> ParticleSystems;
        public ParticleEngine()
        {
            ParticleSystems = new List<Emitter>();
        }
        public void Add(Emitter e)
        {
            ParticleSystems.Add(e);
        }
        public void Update(GameTime gameTime)
        {
            foreach (var e in ParticleSystems)
            {
                e.Update(gameTime);
            }
            for (var particle = 0; particle < ParticleSystems.Count; particle++)
            {
                ParticleSystems[particle].Update(gameTime);
                if (!ParticleSystems[particle].IsComplete) continue;
                ParticleSystems.RemoveAt(particle);
                particle--;
            }
        }
        public void Draw(SpriteBatch spritebatch)
        {
            foreach (var e in ParticleSystems)
            {
                spritebatch.Draw(e);
            }
        }
    }
}
