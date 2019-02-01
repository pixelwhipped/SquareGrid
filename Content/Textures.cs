using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquareGrid.Content
{
    public static class Textures
    {
        public static Texture2D ParticleFlowerBurst;
        public static Texture2D ParticleDiamond;
        public static Texture2D ParticleStar;
        public static Texture2D ParticleSmoke;
        public static Texture2D Cursor;
        public static Texture2D Back;
        public static Texture2D Explosion;
        public static Texture2D Particle;
        public static Texture2D Square;
        public static Texture2D G;
        public static Texture2D R;
        public static Texture2D I;
        public static Texture2D D;
        public static Texture2D Cross;
        public static Texture2D Tick;
        public static Texture2D Forward;
        public static Texture2D Lock;
        public static Texture2D Top;
        public static Texture2D Bottom;
        public static Texture2D Left;
        public static Texture2D Right;
        public static Texture2D Center;
        public static Texture2D Help;


        public static void LoadContent(ContentManager content)
        {
            ParticleFlowerBurst = content.Load<Texture2D>(@"Particle\FlowerBurst.png");
            ParticleDiamond = content.Load<Texture2D>(@"Particle\Diamond.png");
            ParticleStar = content.Load<Texture2D>(@"Particle\Star.png");
            ParticleSmoke = content.Load<Texture2D>(@"Particle\Smoke.png");
            Cursor = content.Load<Texture2D>(@"Images\Cursor.png");
            Back = content.Load<Texture2D>(@"Images\Back.png");
            Forward = content.Load<Texture2D>(@"Images\Forward.png");           
            Explosion = content.Load<Texture2D>(@"Images\Explosion.png");
            Particle = content.Load<Texture2D>(@"Images\Particle01.png");
            Square = content.Load<Texture2D>(@"Images\Square.png");
            G = content.Load<Texture2D>(@"Images\G.png");
            R = content.Load<Texture2D>(@"Images\R.png");
            I = content.Load<Texture2D>(@"Images\I.png");
            D = content.Load<Texture2D>(@"Images\D.png");
            Lock = content.Load<Texture2D>(@"Images\Lock.png");
            Cross = content.Load<Texture2D>(@"Images\Cross.png");
            Tick = content.Load<Texture2D>(@"Images\Tick.png");
            Top = content.Load<Texture2D>(@"Images\Top.png");
            Bottom = content.Load<Texture2D>(@"Images\Bottom.png");
            Left = content.Load<Texture2D>(@"Images\Left.png");
            Right = content.Load<Texture2D>(@"Images\Right.png");
            Center = content.Load<Texture2D>(@"Images\Center.png");
            Help = content.Load<Texture2D>(@"Images\HelpL.png");
        }
    }
}
