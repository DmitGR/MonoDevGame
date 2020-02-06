using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.GameClasses
{
    class Block
    {
        protected Texture2D texture;
        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }
        }

        protected bool passable;
        public bool Passable
        {
            get { return passable; }
            protected set { passable = value; }
        }

        protected bool damaged;
        public bool Damaged { get { return damaged; } }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
    class ImpassableBlock : Block
    {
        public ImpassableBlock(Texture2D texture,Rectangle rectangle)
        {
            this.texture = texture;
            Rectangle = rectangle;
            passable = false;

        }
    }
    class PassableBlock : Block
    {
        public PassableBlock(Texture2D texture, Rectangle rectangle)
        {
            this.texture = texture;
            Rectangle = rectangle;
            passable = true;

        }
    }
    class SpikeBlock : Block
    {
        public SpikeBlock(Texture2D texture ,Rectangle rectangle)
        {
            this.texture = texture;
            Rectangle = rectangle;
            passable = false;
            damaged = true;
        }
    }
    class Exit : Block
    {
        public Exit(Texture2D texture, Rectangle rectangle)
        {
            this.texture = texture;
            Rectangle = rectangle;
            passable = true;
        }
    }

}