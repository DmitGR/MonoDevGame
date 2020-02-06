using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.GameClasses
{
    class Animation
    {
        Texture2D texture;
        public Texture2D Texture { get { return texture; } }
        public int FrameWidth;
        public int FrameHeight {get { return Texture.Height; } }

        float frameTime;
        public float FrameTime { get { return frameTime; } }

        public int FrameCount;

        bool isLooping;
        public bool IsLooping { get { return isLooping; } }

        //Rectangle rect;
        //Vector2 position;
        //Vector2 origin;
        //Vector2 velocity;

        //int currentFrame;

        //int frameHeight;

        //float timer;

        // float interval;

        public Animation (Texture2D texture,int FrameWidth, float frameTime,bool isLooping)
        {
            this.texture = texture;
            this.FrameWidth = FrameWidth;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
            FrameCount = Texture.Width / FrameWidth;
        }
        //public void Update(GameTime gametime)
        //{
        //    rect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
        //    origin = new Vector2(rect.Width / 2, rect.Height / 2);
        //    position = position + velocity;
        //}
        //public void Animate(GameTime gameTime)
        //{
        //    tim
        //}
        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(texture, position,rect, Color.White,0f,origin,1.0f,SpriteEffects.FlipVertically,0);
        //}
    }
}
