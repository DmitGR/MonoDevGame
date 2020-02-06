using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RGR.GameClasses
{
    class Splash : GameObject
    {
        AnimationSprite splashSprite;

        Animation splash;

        SoundEffect splashAttack, splashDamage;

        private bool HasSplah;
        private Vector2 splashStartPos;
        private bool splashRight;

        const float drawLayer = 0.1f;

        public override void Load(ContentManager Content)
        {
            splash = new Animation(Content.Load<Texture2D>("Textures/Misc/splash"), 33, 0.12f, true);
            splashDamage = Content.Load<SoundEffect>("Sounds/splashDamage");
            splashAttack = Content.Load<SoundEffect>("Sounds/splash");

            position = Vector2.Zero;

        }

        public override void Update(GameTime gameTime)
        {
            if (HasSplah)
            {
                rectangle = new Rectangle((int)position.X, (int)position.Y, splash.FrameWidth, splash.FrameHeight);
                if (splashRight)
                    position.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
                else
                    position.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;

                if (position.X >= splashStartPos.X + 500f || position.X <= splashStartPos.X - 500f)
                {
                    position = Vector2.Zero;
                    HasSplah = false;
                }
            }
        }

        public void Collision(Block block)
        {
            if (HasSplah && !block.Passable && rectangle.Intersects(block.Rectangle))
            {
                HasSplah = false;
                rectangle = Rectangle.Empty;
            }
        }

        public void Collision(Enemy enemy)
        {
          if(  enemy.Rectangle.Intersects(rectangle))
            {
                splashDamage.Play();
                enemy.HealthPoint--;
                enemy.CanDamaged = false;
                enemy.timerForDamage = 20f;
                rectangle = Rectangle.Empty;
                HasSplah = false;
            }
        }

        public void StartPosition(Vector2 playerPosition)
        {
            splashStartPos = playerPosition;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (HasSplah)
            {
                if (splashRight)
                    splashSprite.Draw(gameTime, spriteBatch, position, SpriteEffects.None, drawLayer);
                else
                    splashSprite.Draw(gameTime, spriteBatch, position, SpriteEffects.FlipHorizontally,drawLayer);
            }
        }
    }
}
