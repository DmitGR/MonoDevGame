using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR.GameClasses
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

        public Animation (Texture2D texture,int FrameWidth, float frameTime,bool isLooping)
        {
            this.texture = texture;
            this.FrameWidth = FrameWidth;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
            FrameCount = Texture.Width / FrameWidth;
        }
    }
}
