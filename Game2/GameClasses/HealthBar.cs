using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.GameClasses
{
    class HealthBar
    {
        Texture2D texture;
        Rectangle rectangle;
        //  Rectangle draw;
        Vector2 Origin;
        Vector2 position;
        int points;
        public int Points{ get { return points; }
            set
            { if (value >= 0 && value <= 5)
                    points = value;}}

        public HealthBar(int Points)
        {
            this.Points = Points;
        }
        public void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/Misc/healthbar");
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width / 5 * Points, texture.Height);
            Origin = new Vector2(texture.Width / 5, texture.Height);
        }

        public void Update(Camera camera,int ScreenWidth, int ScreenHeight)
        {
            rectangle.Width = texture.Width / 5 * Points;

            position.X = camera.Center.X - ScreenWidth / 2 + texture.Width/5;
            position.Y = camera.Center.Y - ScreenHeight / 2 + texture.Height*2;          
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(draw, rectangle, Color.White);
            spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, Origin, 1f,SpriteEffects.None, 0f);
        }
    }
}
