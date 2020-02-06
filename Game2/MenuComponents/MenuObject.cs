using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.MenuComponents
{
    class MenuObject
    {
        Rectangle rectangle;
        public Rectangle Rectangle { get { return rectangle; } }
        Vector2 position;
        SpriteFont font;
        string objString;
        public string ObjString { get { return objString; } set { objString = value; } }
        public bool Selected { get; set; }
        public bool Activ { get; set; }

        public Color Color { get; set; }


        public MenuObject(Rectangle rectangle, string str)
        {
            position.X = rectangle.X;
            position.Y = rectangle.Y;
            Activ = true;
            this.rectangle = rectangle;
            this.objString = str;
        }

        public void Load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Hud");
            Color = new Color(255, 255, 255, 255);
        }


        public void Update()
        {
            if (Selected)
                Color = Color.Red;

            else Color = new Color(100, 200, 100, 255);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, objString, position + new Vector2(1.0f, 1.0f), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, objString, position, Color, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

        }
    }
}
