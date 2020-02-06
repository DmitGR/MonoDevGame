using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RGR.GameClasses;

namespace RGR.GameObjects.Enemies
{
    class Bat : Enemy
    {
        public Bat(Vector2 position)
        {
            IsAlive = true;
            CanFly = true;
            this.position = position;
            speed = 2.6f;
            healthPoint = 2;
            Score = 50;
        }
        public override void Load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("spriteFont");
            texture = new Animation(Content.Load<Texture2D>("Textures/Enemys/bat"), 17, 0.05f, true);
            damageSound = Content.Load<SoundEffect>("Sounds/miscDamage1");

            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.FrameWidth, texture.FrameHeight);
        }
    }
}