using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game2.GameClasses
{
    class Item
    {
        #region FieldsAndProps

        protected Texture2D texture;
        protected Rectangle rectangle;
        protected SoundEffect sound;

        public Rectangle Rectangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }
        }
        protected Vector2 position;

        protected bool isPicked;


        public bool IsPicked
        {
            get { return isPicked; }
            private set {  isPicked = value; }
        }


        protected bool canPicked;
        protected bool Dynamic;

        public bool CanPicked
        {
            get { return canPicked; }
            set { canPicked = value; }
        }


        public Vector2 Position
        {
            get { return position; }
            protected set { position = value; }
        }


        #endregion
        public void Update(Rectangle player)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
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
            if (Dynamic)
            {

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isPicked == false)
                spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }

    class Jug : Item
    {
        public Jug(Texture2D texture, Vector2 position, SoundEffect sound)
        {
            this.texture = texture;
            this.position = position;
            this.sound = sound;
            Dynamic = false;
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            isPicked = false;
            canPicked = true;
        }
    }

    class Health : Item
    {
        public Health(Texture2D texture, Vector2 position, SoundEffect sound)
        {
            this.texture = texture;
            this.position = position;
            this.sound = sound;
            Dynamic = true;
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            isPicked = false;
            canPicked = true;
        }
    }
    
    class Armor : Item
    {
        public Armor(Texture2D texture, Vector2 position, SoundEffect sound)
        {
            this.texture = texture;
            this.position = position;
            this.sound = sound;
            Dynamic = true;
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            isPicked = false;
            canPicked = true;
        }
    }
}
