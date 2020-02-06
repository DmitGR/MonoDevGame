using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RGR.GameClasses;

namespace RGR.GameObjects.Enemies
{
    class Wolf : Enemy
    {
        public Wolf(Vector2 position)
        {
            IsAlive = true;
            CanFly = false;
            this.position = position;
            speed = 3.6f;
            healthPoint = 3;
            Score = 300;
        }

        public override void Load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("spriteFont");
            texture = new Animation(Content.Load<Texture2D>("Textures/Enemys/wolf"), 64, 0.14f, true);
            damageSound = Content.Load<SoundEffect>("Sounds/miscDamage3");

            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.FrameWidth, texture.FrameHeight);
        }
    }
}