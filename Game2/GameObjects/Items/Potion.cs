using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RGR.GameClasses;

namespace RGR.GameObjects.Items
{
    class Potion : Item
    {
        public Potion(Vector2 position)
        {
            this.position = position;
            isPicked = false;
            canPicked = true;
        }
        public override void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/Items/potion");
            sound = Content.Load<SoundEffect>("Sounds/ItemPickUp");
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
    }
}