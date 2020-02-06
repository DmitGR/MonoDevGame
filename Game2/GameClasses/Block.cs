using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR.GameClasses
{
    class Block : GameObject
    {
        protected Texture2D texture;
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

        protected float drawLayer;

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, texture.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, drawLayer);
        }
    }
}