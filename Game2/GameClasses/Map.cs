using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RGR.GameObjects.Blocks;
using RGR.GameObjects.Enemies;
using RGR.GameObjects.Items;
using System.Collections.Generic;
using System.IO;

namespace RGR.GameClasses
{
    class Map
    {
        #region FieldsAndProps}

        private List<GameObject> gameObjects = new List<GameObject>();
        public List<GameObject> GameObjects
        {
            get { return gameObjects; }
        }
        private List<PassableBlock> backround = new List<PassableBlock>();
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

        private List<CheckPoint> chekPoints = new List<CheckPoint>();
        public List<CheckPoint> ChekPoints
        {
            get { return chekPoints; }
        }

        public  Vector2 Start { get; private set; }

        #endregion

        public void Generate(int level, Vector2 size)
        {
            if (!File.Exists("Content/Maps/level" + level + ".txt"))
                level = 1;
            string[] s = File.ReadAllLines("Content/Maps/level" + level + ".txt");
            int x = 0, y = 0;

            foreach (string str in s)
            {
                foreach (char c in str)
                {
                    switch (c)
                    {
                        //Start Position
                        case '@':
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            Start = new Vector2(x, y);
                            break;
                        // Exit
                        case 'E':
                            gameObjects.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            gameObjects.Add(new Exit(Content.Load<Texture2D>("Textures/Blocks/exit"), new Rectangle(x, y, (int)size.X, (int)size.Y * 2)));
                            break;
                        // Impassable Blocks
                        case 'X':
                            gameObjects.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'V':
                            gameObjects.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block5"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'F':
                            gameObjects.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block8"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'x':
                            gameObjects.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block9"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'Z':
                            gameObjects.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block7"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'Y':
                            gameObjects.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block2"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'D':
                            gameObjects.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block3"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'C':
                            gameObjects.Add(new ImpassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block4"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        // Passable Blocks
                        case 'z':
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("test"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case '.':
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case ',':
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock2"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 'o':
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/Block6"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // Tourch
                        case 't':
                            gameObjects.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscTourch"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // Spike Blocks
                        case 'S':
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            gameObjects.Add(new SpikeBlock(Content.Load<Texture2D>("Textures/Blocks/SpikeBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                        case 's':
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            gameObjects.Add(new SpikeBlock(Content.Load<Texture2D>("Textures/Blocks/SpikeBlock2"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // CheckPoint
                        case '#':
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            gameObjects.Add((new CheckPoint(new Vector2(x, y), Content)));
                            break;
                            // Jug
                        case 'J':
                            gameObjects.Add(new Jug(new Vector2(x, y)));
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // Armor
                        case 'A':
                            gameObjects.Add(new Armor(new Vector2(x, y)));
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // HealthPoint
                        case 'H':
                            gameObjects.Add(new Health(new Vector2(x, y)));
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // Potion
                        case '*':
                            gameObjects.Add(new Potion(new Vector2(x, y)));
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // Bat
                        case 'B':
                            gameObjects.Add(new Bat(new Vector2(x, y)));
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // MudMan
                        case 'M':
                            gameObjects.Add(new Mud(new Vector2(x, y)));
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // Wolf
                        case 'W':
                            gameObjects.Add(new Wolf(new Vector2(x, y)));
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // Raven
                        case 'R':
                            gameObjects.Add(new Raven(new Vector2(x, y)));
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // Monke
                        case 'm':
                            gameObjects.Add(new Monkey(new Vector2(x, y)));
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // Smoke
                        case 'K':
                            gameObjects.Add(new Smoke(new Vector2(x, y)));
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
                            break;
                            // RedSkeleton
                        case 'T':
                            gameObjects.Add(new RedSkeleton(new Vector2(x, y)));
                            backround.Add(new PassableBlock(Content.Load<Texture2D>("Textures/Blocks/MiscBlock1"), new Rectangle(x, y, (int)size.X, (int)size.Y)));
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


            foreach (GameObject obj in gameObjects)
            {
                obj.Load(Content);
                if (obj is Block)
                    blocks.Add((Block)obj);
                else if (obj is CheckPoint)
                    chekPoints.Add((CheckPoint)obj);
                else if (obj is Enemy)
                    Enemys.Add((Enemy)obj);
                else if (obj is Item)
                    items.Add((Item)obj);
            }
        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 center)
        {


            foreach (var item in backround)
            {
                item.Draw(spriteBatch);
            }

            foreach (GameObject obj in gameObjects)
               if (obj.rectangle.X + obj.rectangle.Width >= center.X - viewport.Width / 2 && obj.rectangle.X - obj.rectangle.Width <= center.X + viewport.Width / 2
                            && obj.rectangle.Y + obj.rectangle.Height >= center.Y - viewport.Height / 2 && obj.rectangle.Y - obj.rectangle.Height <= center.Y + viewport.Height / 2)
               {
                    obj.Draw(spriteBatch);
                    obj.Draw(gameTime, spriteBatch);
                }
        }
    }
}
