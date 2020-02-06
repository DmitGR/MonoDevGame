using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.GameClasses
{
    class Enemy
    {
        protected AnimationSprite animationSprite;

        protected Animation texture;

        private bool RunToRight;
        private bool isAlive;
        protected bool CanFly;
        public int Score { get; protected set; }


        SpriteFont font;

       public bool CanDamaged = true;
       public  float timerForDamage;

        public bool CanGetScore
        {
            get;
            protected set; 
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        protected float speed;
        protected int healthPoint;
        public int HealthPoint
        {
            get { return healthPoint; }
            set { if(value >= 0) healthPoint = value; }
        }
        Vector2 velocity;
        protected Rectangle rectangle;
        public Rectangle Rectangle { get { return rectangle; } }
        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected SoundEffect damage;
        public SoundEffect DamageSound { get { return damage; } }

        public void Load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("spriteFont");
            isAlive = true;

        }
        public void Update()
        {

             if (isAlive)
            {
                rectangle = new Rectangle((int)position.X, (int)position.Y, texture.FrameWidth, texture.FrameHeight);
                position += velocity;
                if (RunToRight)
                {
                    velocity.X = speed;

                }
                else
                {
                    velocity.X = -speed;
                }
                if (!CanFly)
                    velocity.Y += 0.4f;

                animationSprite.PlayAnimation(texture);
            }
            if (CanGetScore)
                CanGetScore = false;

            if (healthPoint <= 0 && rectangle!=Rectangle.Empty)
            {
                CanGetScore = true;
                IsAlive = false;
                rectangle = Rectangle.Empty;
            }

        }


        public void Collision(Block block)
        {
            if (!block.Passable)
            {
                if (rectangle.TouchTopOf(block.Rectangle))
                {
                    rectangle.Y = block.Rectangle.Y - rectangle.Height;
                    position.Y = block.Rectangle.Y - rectangle.Height;
                    velocity.Y = 0f;
                }
                if (rectangle.TouchRightOf(block.Rectangle))
                {
                    rectangle.X = block.Rectangle.Right + rectangle.Width;
                    RunToRight = true;
                }
                if (rectangle.TouchLeftOf(block.Rectangle))
                {
                    rectangle.X = block.Rectangle.Left - rectangle.Width;
                    RunToRight = false;
                }

            }
        }


        public void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
              //  spriteBatch.DrawString(font,healthPoint.ToString() + " HP", new Vector2(rectangle.X, rectangle.Top - rectangle.Height/2)  ,Color.IndianRed);
                spriteBatch.DrawString(font, healthPoint.ToString() + " HP", new Vector2(rectangle.X, rectangle.Top - rectangle.Height / 2) + new Vector2(1.0f, 1.0f), Color.Black);
                spriteBatch.DrawString(font, healthPoint.ToString() + " HP", new Vector2(rectangle.X, rectangle.Top - rectangle.Height / 2), Color.IndianRed);
                SpriteEffects flip = SpriteEffects.None;
                if (velocity.X > 0)
                    flip = SpriteEffects.None;
                else if (velocity.X < 0)
                    flip = SpriteEffects.FlipHorizontally;
                animationSprite.Draw(gameTime, spriteBatch, Position, flip);
            }
        }

    }
    class Bat : Enemy
    {
        public Bat(Vector2 position, Animation texture, SoundEffect damage)
        {
            CanFly = true;
            this.position = position;
            this.texture = texture;
            this.damage = damage;
            speed = 2f;
            healthPoint = 2;
            Score = 50;
        }
    }

    class Mud : Enemy
    {
        public Mud(Vector2 position, Animation texture, SoundEffect damage)
        {
            CanFly = false;
            this.position = position;
            this.texture = texture;
            this.damage = damage;
            speed = 1.2f;
            healthPoint = 5;
            Score = 200;
        }
    }

    class Wolf : Enemy
    {
        public Wolf(Vector2 position, Animation texture, SoundEffect damage)
        {
            CanFly = false;
            this.position = position;
            this.texture = texture;
            this.damage = damage;
            speed = 3.2f;
            healthPoint = 3;
            Score = 300;
        }
    }
}
