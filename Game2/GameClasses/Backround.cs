using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.GameClasses
{
    class Backround
    {
        protected Texture2D texture;
        public Texture2D Texture { get { return texture; }protected set {texture = value; } }
        protected Rectangle rect;
        public Rectangle Rect { get { return rect; }  set { rect = value; } }
        public Backround(Texture2D texture, Rectangle rect)
        {
            this.texture = texture;
            this.rect = rect;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.White);
        }

    }
    class Scrolling : Backround
    {
        public Scrolling(Texture2D texture, Rectangle rect) : base (texture,rect)
        {
            this.texture = texture;
            this.rect = rect;
        }
        public void Update()
        {
            rect.X -= 2;
        }
    }
}
