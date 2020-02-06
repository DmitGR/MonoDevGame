using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game2.GameClasses
{
    class ArmorHud
    {
        Texture2D texture;
        Rectangle rectangle;
        //  Rectangle draw;
        Vector2 Origin;
        Vector2 position;
        public float Points { get; set; }
        public ArmorHud()
        {
            Points = 1;
        }
        public void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/Misc/ArmorHud");
            rectangle = new Rectangle((int)position.X, (int)position.Y , texture.Width , texture.Height);
            Origin = new Vector2(texture.Width, texture.Height );
        }
        
        public void Update(Camera camera, int ScreenWidth, int ScreenHeight)
        {
            if (Points > 0 && rectangle.Height <= texture.Height)
            {
                rectangle.Height = (int)Points / 8;
                Origin = new Vector2(rectangle.Width, rectangle.Height);
            }

            position.X = camera.Center.X - ScreenWidth / 2 + texture.Width * 2  ;
            position.Y = camera.Center.Y - ScreenHeight / 2 + texture.Height * 3;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Points > 8)
                spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, Origin, 1f, SpriteEffects.FlipVertically, 0f);
        }
    }
}
