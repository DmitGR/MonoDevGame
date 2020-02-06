using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game2.GameClasses
{
    struct AnimationSprite
    {

        Animation animation;
        public Animation Animation
        {
            get { return animation; }
        }
        int frameIndex;
        public int FrameIndex
        {
            get { return frameIndex; }
            private set { frameIndex = value; }
        }

       private float timer;

        public Vector2 Origin
        {
            get { return new Vector2(animation.FrameWidth , animation.FrameHeight); }
        }

        public void PlayAnimation(Animation animation)
        {
            if (this.animation != animation)
            {
                this.animation = animation;
                frameIndex = 0;
                timer = 0;
            }
        }
        public void Draw(GameTime gameTime,SpriteBatch spriteBatch,Vector2 position ,SpriteEffects spriteEffects)
        {
            if (Animation == null)
                throw new NotSupportedException("no animation");

            position = new Vector2((position.X + Animation.FrameWidth  ), (position.Y + Animation.FrameHeight + 1));
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (timer >= animation.FrameTime)
            {
                timer -= animation.FrameTime;
                if (animation.IsLooping)
                    frameIndex = (frameIndex + 1) % animation.FrameCount;
                else
                    frameIndex = Math.Min(frameIndex + 1, animation.FrameCount - 1);
            }

            Rectangle rect = new Rectangle(frameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.FrameHeight);
            spriteBatch.Draw(Animation.Texture, position, rect, Color.White, 0f, Origin, 1f, spriteEffects, 0f);
        }
    }
}
