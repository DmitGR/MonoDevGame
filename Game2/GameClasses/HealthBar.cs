using Microsoft.Xna.Framework;
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
    class HealthBar : GameObject
    {
        Texture2D texture;
        Vector2 Origin;
        int points;
        public int Points{ get { return points; }
            set
            { if (value >= 0 && value <= 5)
                    points = value;}}

        const int MaxPoints = 5;
        const float drawLayer = 1f;
        public HealthBar(int Points)
        {
            this.Points = Points;
        }
        public override void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/Misc/healthbar");
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width / MaxPoints * Points, texture.Height);
            Origin = new Vector2(texture.Width / MaxPoints, texture.Height);
        }

        public void Update(Camera camera,int ScreenWidth, int ScreenHeight)
        {
            rectangle.Width = texture.Width / MaxPoints * Points;

            position.X = camera.Center.X - ScreenWidth / 2 + texture.Width / MaxPoints;
            position.Y = camera.Center.Y - ScreenHeight / 2 + texture.Height * 2;          
        }

        public override void Draw( SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, Origin, 1f,SpriteEffects.None, drawLayer);
        }
    }
}
