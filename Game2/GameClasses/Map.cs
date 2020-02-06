using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Game2.GameClasses
{
    class Map
    {
        private List<Block> blocks = new List<Block>();
        public List<Block> Blocks
        {
            get { return blocks; }
        }

        private List<Enemy> enemys = new List<Enemy>();
        public List<Enemy> Enemys
        {
            get { return enemys; }
        }

        private List<Item> items = new List<Item>();
        public List<Item> Items
        {
            get { return items; }
        }

        private List<SpikeBlock> spikes = new List<SpikeBlock>();
        public List<SpikeBlock> Spikes
        {
            get { return spikes; }
        }

        private List<CheckPoint> chekPoints = new List<CheckPoint>();
        public List<CheckPoint> ChekPoints { get { return chekPoints; } }

        private int width, height;
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        Viewport viewport;
        public Map(Viewport viewport) { this.viewport = viewport; }

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

       public  Vector2 Start { get; private set; }



        public void Generate(int level, Vector2 size)
        {
            if (!File.Exists("Content/level" + level + ".txt"))
                level = 1;
            string[] s = File.ReadAllLines("Content/level" + level + ".txt");
            int x = 0, y = 0;

            foreach (string str in s)
            {
                foreach (char c in str)
                {
                    switch (c)
                    {
                        case '@':
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            Start = new Vector2(x, y);
                            break;
                        case 'X':
                            blocks.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'Y':
                            blocks.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block2"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'x':
                            blocks.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block3"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'z':
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("test"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'C':
                            blocks.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block4"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'S':
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            blocks.Add(new SpikeBlock(Content.Load<Texture2D>("Textures/Blocks/SpikeBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case '#':
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            chekPoints.Add((new CheckPoint(new Vector2(x, y), Content)));
                            break;
                        case 'E':
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            blocks.Add(new Exit(Content.Load<Texture2D>("Textures/Blocks/exit"), new Rectangle(x, y, (int)size.X, (int)size.Y*2)));
                            break;
                        case '.':
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case ',':
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock2"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 't':
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscTourch"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'J':
                            items.Add(new Jug(Content.Load<Texture2D>("Textures/Items/jug"), new Vector2(x, y), content.Load<SoundEffect>("Sounds/ItemPickUp")));
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'A':
                            items.Add(new Armor(Content.Load<Texture2D>("Textures/Items/armor"), new Vector2(x, y), content.Load<SoundEffect>("Sounds/armorPickUp")));
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'H':
                            items.Add(new Health(Content.Load<Texture2D>("Textures/Items/health"), new Vector2(x, y), content.Load<SoundEffect>("Sounds/heartCollect")));
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'B':
                            enemys.Add(new Bat(new Vector2(x, y), (new Animation(Content.Load<Texture2D>("Textures/Enemys/bat"), 17, 0.05f, true)), Content.Load<SoundEffect>("Sounds/miscDamage1")));
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'M':
                            enemys.Add(new Mud(new Vector2(x, y), (new Animation(Content.Load<Texture2D>("Textures/Enemys/mud"), 28, 0.3f, true)), Content.Load<SoundEffect>("Sounds/miscDamage2")));
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'W':
                            enemys.Add(new Wolf(new Vector2(x, y), (new Animation(Content.Load<Texture2D>("Textures/Enemys/wolf"), 64, 0.14f, true)), Content.Load<SoundEffect>("Sounds/miscDamage3")));
                            blocks.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        default:
                            break;
                    }
                    x += (int)size.X;
                    width = System.Math.Max(x,width);

                }
                
                x = 0;
                y += (int)size.Y;
                height = System.Math.Max(y, height);

            }
            foreach (Enemy enemy in Enemys)
            {
                enemy.Load(Content);
            }

        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 center)
        {

            foreach (Block block in Blocks)
            {
                if(block.Rectangle.X >= center.X - viewport.Width && block.Rectangle.X <= center.X + viewport.Width)
                block.Draw(spriteBatch);
            }
            foreach (Item item in Items)
            {

                item.Draw(spriteBatch);
            }
            foreach (Enemy enemy in Enemys)
            {
                enemy.Draw(gameTime, spriteBatch);
            }

            foreach (CheckPoint checkPoint in ChekPoints)
            {
                checkPoint.Draw(gameTime, spriteBatch);

            }
        }

        public void EnemyCollision(Vector2 center)
        {
            for (int i = 0; i < Enemys.Count; i++)
            {
                if (Enemys[i].Rectangle.X  >= center.X - viewport.Width && Enemys[i].Rectangle.X <= center.X + viewport.Width &&  Enemys[i].Rectangle.Y <= center.Y  + viewport.Height && Enemys[i].Rectangle.Y >= center.Y - viewport.Height)
                for (int j = 0; j < Blocks.Count; j++)
                    {
                        if (Blocks[j].Rectangle.X >= center.X - viewport.Width && Blocks[j].Rectangle.X <= center.X + viewport.Width && Blocks[j].Rectangle.Y <= center.Y + viewport.Height && Blocks[j].Rectangle.Y >= center.Y - viewport.Height)
                            Enemys[i].Collision(Blocks[j]);
                }
            }
        }
    }
}
