using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RGR.GameClasses
{
    class CheckPoint : GameObject
    {
        AnimationSprite animationSprite;
        Animation idle,animation;
        SoundEffect sound;
        public Vector2 Position { get { return position; } }
        ContentManager Content;
        public Rectangle Rectangle { get { return rectangle; } }

        bool isUsed;
        public bool IsUsed { get{ return isUsed; } set { isUsed = value; }  }

        const float drawLayer = 0.2f;

        public CheckPoint (Vector2 position, ContentManager Content)
        {
            this.position = position;
            this.Content = Content;
            isUsed = false;
        }

        public override void Load(ContentManager Content)
        {
            idle = new Animation(Content.Load<Texture2D>("Textures/Items/checkpoint0"), 48, 1f, false);
            animation = new Animation(Content.Load<Texture2D>("Textures/Items/checkpoint1"), 48, 0.12f, true);
            sound = Content.Load<SoundEffect>("Sounds/CheckPointActivated");

            rectangle = new Rectangle((int)Position.X, (int)Position.Y, idle.FrameWidth, idle.FrameHeight);
        }
        public void PlaySound()
        {
            sound.Play();
        }
        public override void Update()
        {
            if (isUsed)
                animationSprite.PlayAnimation(animation);
            else  animationSprite.PlayAnimation(idle);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationSprite.Draw(gameTime, spriteBatch, position, SpriteEffects.None, drawLayer);
        }

    }
}
