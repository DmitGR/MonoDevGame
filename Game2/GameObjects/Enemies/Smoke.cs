using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RGR.GameClasses;

namespace RGR.GameObjects.Enemies
{
    class Smoke : Enemy
    {
        public Smoke(Vector2 position)
        {
            IsAlive = true;
            CanFly = false;
            this.position = position;
            speed = 4.2f;
            healthPoint = 2;
            Score = 250;
        }

        public override void Load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("spriteFont");
            texture = new Animation(Content.Load<Texture2D>("Textures/Enemys/Smoke"), 32, 0.16f, true);
            damageSound = Content.Load<SoundEffect>("Sounds/miscDamage1");

            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.FrameWidth, texture.FrameHeight);
        }
    }
}