using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game2.GameClasses
{
    class CheckPoint
    {
        AnimationSprite animationSprite;
        Animation idle,animation;
        Vector2 position;
        SoundEffect sound;
        public Vector2 Position { get { return position; } }
        ContentManager Content;
        Rectangle rectangle;
        public Rectangle Rectangle { get { return rectangle; } }

        bool isUsed;
        public bool IsUsed { get{ return isUsed; } set { isUsed = value; }  }

        public CheckPoint (Vector2 position, ContentManager Content)
        {
            this.position = position;
            this.Content = Content;
            isUsed = false;
            idle = new Animation(Content.Load<Texture2D>("Textures/Items/checkpoint0"), 48, 1f, false);
            animation = new Animation(Content.Load<Texture2D>("Textures/Items/checkpoint1"), 48, 0.12f, true);
            rectangle = new Rectangle((int)Position.X, (int)Position.Y, idle.FrameWidth, idle.FrameHeight);
            sound = Content.Load<SoundEffect>("Sounds/CheckPoinActivated");
        }
        public void PlaySound()
        {
            sound.Play();
        }
        public void Update()
        {
            if (isUsed)
                animationSprite.PlayAnimation(animation);
            else  animationSprite.PlayAnimation(idle);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationSprite.Draw(gameTime, spriteBatch, position, SpriteEffects.None);
        }

    }
}
