using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR.GameClasses
{
    class SplashHud : GameObject
    {
        Texture2D texture, potion;

        SpriteFont font;
        SoundEffect sound;
        public bool IsActiv { get; set; }
        public bool IsPicked { get; set; }
        public float Points { get; set; }
        public SplashHud() { }
        const float drawLayer = 1f;
        public override void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/Misc/splashHud");
            potion = Content.Load<Texture2D>("Textures/Items/potion");
            font = Content.Load<SpriteFont>("spriteFont");
            sound = Content.Load<SoundEffect>("Sounds/potionOff");

            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width , texture.Height);

        }

        public void Update(Camera camera, int ScreenWidth, int ScreenHeight)
        {
            if (IsPicked || IsActiv)
            {
                position.X = camera.Center.X - ScreenWidth / 2 + texture.Width /2;
                position.Y = camera.Center.Y - ScreenHeight / 2 + texture.Height ;
            }

            if (Points >= 1)
            {
                IsActiv = true;
                Points--;

                rectangle.Width = (int)Points;
            }

            if (Points <= 0)
                IsActiv = false;

            if (Points == 1)
                sound.Play();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActiv && Points > 1)
                spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            if (IsPicked )
            {
                spriteBatch.Draw(potion, new Rectangle((int)position.X - texture.Width / 2, (int)position.Y + potion.Height * 2, potion.Width, potion.Height), potion.Bounds , Color.White , 0f , Vector2.Zero,SpriteEffects.None, drawLayer);

                spriteBatch.DrawString(font, "Press K", new Vector2 ((int)position.X - potion.Width*6, (int)position.Y + potion.Height * 3 + potion.Height / 6) + new Vector2(1.0f, 1.0f), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, drawLayer-0.1f);
                spriteBatch.DrawString(font, "Press K", new Vector2((int)position.X - potion.Width* 6, (int)position.Y + potion.Height * 3 + potion.Height / 6), Color.CornflowerBlue, 0f, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);

            }

        }
    }
}
