using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RGR.GameClasses;

namespace RGR.GameObjects.Enemies
{
    class Mud : Enemy
    {
        public Mud(Vector2 position)
        {
            IsAlive = true;
            CanFly = false;
            this.position = position;
            speed = 1.2f;
            healthPoint = 5;
            Score = 200;
        }

        public override void Load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("spriteFont");
            texture = new Animation(Content.Load<Texture2D>("Textures/Enemys/mud"), 28, 0.3f, true);
            damageSound = Content.Load<SoundEffect>("Sounds/miscDamage2");

            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.FrameWidth, texture.FrameHeight);
        }
    }
}