using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RGR.GameClasses;

namespace RGR.GameObjects.Enemies
{
    class Monkey : Enemy
    {
        public Monkey(Vector2 position)
        {
            IsAlive = true;
            CanFly = false;
            this.position = position;
            speed = 3.2f;
            healthPoint = 4;
            Score = 350;
        }

        public override void Load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("spriteFont");
            texture = new Animation(Content.Load<Texture2D>("Textures/Enemys/monkey"), 32, 0.14f, true);
            damageSound = Content.Load<SoundEffect>("Sounds/miscDamage3");

            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.FrameWidth, texture.FrameHeight);
        }
    }
}