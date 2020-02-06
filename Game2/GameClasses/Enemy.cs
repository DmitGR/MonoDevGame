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

namespace RGR.GameClasses
{
    class Enemy : GameObject
    {

        #region FieldsAndProps

        protected AnimationSprite animationSprite;

        protected Animation texture;

        private bool RunToRight;
        private bool isAlive;
        protected bool CanFly;
        public int Score { get; protected set; }


       protected SpriteFont font;

       public bool CanDamaged = true;
       public  float timerForDamage;

        public bool CanGetScore
        {
            get;
            set; 
        }

        public bool IsAlive
        {
            get { return isAlive; }
            protected   set { isAlive = value; }
        }

        protected float speed;
        protected int healthPoint;
        public int HealthPoint
        {
            get { return healthPoint; }
            set { if(value >= 0) healthPoint = value; }
        }
        Vector2 velocity;

        public Rectangle Rectangle { get { return rectangle; } }
        Rectangle collisionArea;
        public Rectangle CollisionArea { get { return collisionArea; } private set { collisionArea = value; } }


        protected SoundEffect damageSound;
        public SoundEffect DamageSound { get { return damageSound; } }

        const float damageTimer = 20f;
        public float DamageTimer { get { return damageTimer; } }

        const float Gravity = 0.4f;
        const float drawLayer = 0.3f;
        #endregion

        public override void Update()
        {

             if (isAlive)
            {
                rectangle = new Rectangle((int)position.X, (int)position.Y, texture.FrameWidth, texture.FrameHeight);
                collisionArea = new Rectangle(rectangle.X - rectangle.Width, rectangle.Y - rectangle.Height, rectangle.Width * 3, rectangle.Height * 3);
                position += velocity;
                if (RunToRight)
                    velocity.X = speed;
                else
                    velocity.X = -speed;
                if (!CanFly)
                    velocity.Y += Gravity;

                animationSprite.PlayAnimation(texture);
            }

            if (healthPoint <= 0 && rectangle != Rectangle.Empty && IsAlive)
            {
                CanGetScore = true;
                IsAlive = false;
                rectangle = Rectangle.Empty;
                collisionArea = Rectangle.Empty;
                timerForDamage = 0f;
            }

        }

        public int GetScore(int Score)
        {
            return Score + this.Score;
        }


        public void Collision(Block block)
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


        public override void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                spriteBatch.DrawString(font, healthPoint.ToString() + " HP", new Vector2(rectangle.X, rectangle.Top - rectangle.Height / 2) + new Vector2(1.0f, 1.0f), Color.Black , 0f, Vector2.Zero, 1f, SpriteEffects.None, drawLayer-0.1f);
                spriteBatch.DrawString(font, healthPoint.ToString() + " HP", new Vector2(rectangle.X, rectangle.Top - rectangle.Height / 2), Color.IndianRed, 0f, Vector2.Zero, 1f, SpriteEffects.None, drawLayer );
                SpriteEffects flip = SpriteEffects.None;
                if (velocity.X > 0)
                    flip = SpriteEffects.None;
                else if (velocity.X < 0)
                    flip = SpriteEffects.FlipHorizontally;
                animationSprite.Draw(gameTime, spriteBatch, position, flip, drawLayer);
            }
        }

    }
}
