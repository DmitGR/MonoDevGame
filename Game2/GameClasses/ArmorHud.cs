using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RGR.GameClasses
{
    class ArmorHud : GameObject
    {
        Texture2D texture;
        SoundEffect sound;
        Vector2 Origin;
        public float Points { get; set; }
        const int MinPoints = 8;
        const int SoundPoint =2;
        const float drawLayer = 1f;
        public ArmorHud() { }

        public override void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/Misc/ArmorHud");
            sound = Content.Load<SoundEffect>("Sounds/armorOff");
            rectangle = new Rectangle((int)position.X, (int)position.Y , texture.Width , texture.Height);
            Origin = new Vector2(texture.Width, texture.Height );
        }
        
        public void Update(Camera camera, int ScreenWidth, int ScreenHeight)
        {
            if (Points > 0 && rectangle.Height <= texture.Height)
            {
                rectangle.Height = (int)Points / MinPoints;
                Origin = new Vector2(rectangle.Width, rectangle.Height);
            }
            if( Points == SoundPoint)
                sound.Play();

            position.X = camera.Center.X - ScreenWidth / 2 + texture.Width * 2  ;
            position.Y = camera.Center.Y - ScreenHeight / 2 + texture.Height * 3;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Points > MinPoints)
                spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, Origin, 1f, SpriteEffects.FlipVertically, drawLayer);
        }
    }
}
