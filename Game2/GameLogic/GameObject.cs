using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RGR.GameClasses
{
    abstract class GameObject
    {
        public Rectangle rectangle;
        public Vector2 position;

        public virtual void Load(ContentManager Content) { }

        public virtual void Update() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}
