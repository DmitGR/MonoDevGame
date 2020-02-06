using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RGR.GameClasses;

namespace RGR.GameObjects.Enemies
{
    class RedSkeleton : Enemy
    {
        public RedSkeleton(Vector2 position)
        {
            IsAlive = true;
            CanFly = false;
            this.position = position;
            speed = 1.6f;
            healthPoint = 6;
            Score = 450;
        }

        public override void Load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("spriteFont");
            texture = new Animation(Content.Load<Texture2D>("Textures/Enemys/RedSkeleton"), 32, 0.2f, true);
            damageSound = Content.Load<SoundEffect>("Sounds/miscDamage4");

            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.FrameWidth, texture.FrameHeight);

        }
    }
}