using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RGR.GameClasses
{
    class Item : GameObject
    {

        #region FieldsAndProps

        protected Texture2D texture;
        protected SoundEffect sound;

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        protected bool isPicked;


        public bool IsPicked
        {
            get { return isPicked; }
            private set {  isPicked = value; }
        }


        protected bool canPicked;

        public bool CanPicked
        {
            get { return canPicked; }
            set { canPicked = value; }
        }

        const float drawLayer = 0.2f;


        #endregion

        public void Update(Rectangle player)
        {
            player.Width /= 2;
            player.Height /= 2;
            if ((player.Bottom + 1 >= rectangle.Top - 1 &&
                    player.Top <= rectangle.Bottom - (rectangle.Height / 2) &&
                    player.Right >= rectangle.Left &&
                    player.Left <= rectangle.Right - (rectangle.Width / 5)) && canPicked)
            {
                isPicked = true;
                sound.Play();
                rectangle = Rectangle.Empty;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isPicked)
                spriteBatch.Draw(texture, rectangle, texture.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, layerDepth: drawLayer);
        }
    }
}