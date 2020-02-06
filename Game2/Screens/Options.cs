using RGR.GameClasses;
using RGR.MenuComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR.Screens
{
    public class Options
    {

        #region Fields And Props



        List<MenuObject> objects = new List<MenuObject>();
        List<MenuObject> subObjects = new List<MenuObject>();
        SoundEffect select, push, back,choose;
        Backround backround;
        Rectangle SelecRect;
        Vector2 size;
        int level;
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        int healthPoint;
        public int HealthPoint
        {
            get { return healthPoint; }
            set { healthPoint = value; }
        }
        KeyboardState state;
        KeyboardState oldstate;
        GraphicsDeviceManager graphicsDevice;
        bool CanChange;
        string ScreenMode;
        public GraphicsDeviceManager GraphicsDevice
        {
            set { graphicsDevice = value; }
        }


        #endregion


        public Options(GraphicsDeviceManager graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }


        public void Load(ContentManager Content)
        {
            backround = new Backround(Content.Load<Texture2D>("Textures/Backrounds/BackroundOptions"), new Rectangle(0, 0, graphicsDevice.PreferredBackBufferWidth, graphicsDevice.PreferredBackBufferHeight));
            if (graphicsDevice.IsFullScreen)
                ScreenMode = "FullScreen";
            else ScreenMode = "Window";

            select = Content.Load<SoundEffect>("Sounds/MenuSelect");
            push = Content.Load<SoundEffect>("Sounds/MenuPush");
            back = Content.Load<SoundEffect>("Sounds/MenuBack");
            choose = Content.Load<SoundEffect>("Sounds/MenuSelect");

            size = new Vector2(graphicsDevice.PreferredBackBufferWidth / 16, graphicsDevice.PreferredBackBufferHeight / 16);
            Vector2 center = new Vector2(graphicsDevice.PreferredBackBufferWidth / 2, graphicsDevice.PreferredBackBufferHeight / 2);
            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X * 2), (int)(center.Y+size.Y*2- size.Y * 5), (int)size.X, (int)size.Y), "Music"));
            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X * 2), (int)(center.Y+size.Y*2- size.Y * 4), (int)size.X, (int)size.Y), "ScreenMode"));
            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X * 2), (int)(center.Y+size.Y*2- size.Y * 3), (int)size.X, (int)size.Y), "Level"));
            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X * 2), (int)(center.Y+size.Y*2- size.Y * 2), (int)size.X, (int)size.Y), "Health"));
            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X * 2), (int)(center.Y+size.Y*2- size.Y * 1), (int)size.X, (int)size.Y), "Back to Menu"));

            //Default
            CanChange = false;
            level = 1;
            healthPoint = 3;

            subObjects.Add(new MenuObject(new Rectangle((int)(center.X + size.X * 2), (int)(center.Y + size.Y * 2 - size.Y * 5), (int)size.X, (int)size.Y), ((int)(MediaPlayer.Volume*100)).ToString() + " % "));
            subObjects.Add(new MenuObject(new Rectangle((int)(center.X + size.X * 2), (int)(center.Y + size.Y * 2 - size.Y * 4), (int)size.X, (int)size.Y), ScreenMode));
            subObjects.Add(new MenuObject(new Rectangle((int)(center.X + size.X * 2), (int)(center.Y + size.Y * 2 - size.Y * 3), (int)size.X, (int)size.Y), level.ToString()));
            subObjects.Add(new MenuObject(new Rectangle((int)(center.X + size.X * 2), (int)(center.Y + size.Y * 2 - size.Y * 2), (int)size.X, (int)size.Y),healthPoint.ToString()));

            foreach (MenuObject obj in objects)
            {
                obj.Load(Content);
            }
            foreach (MenuObject SubObj in subObjects)
            {
                SubObj.Load(Content);
            }
            

            SelecRect = new Rectangle((int)(center.X - size.X*2), (int)(center.Y - size.Y * 5), (int)size.X, (int)size.Y);

        }

        public MenuState.GameState Update()
        {

            state = Keyboard.GetState();
            foreach (MenuObject obj in objects)
            {

                if (obj.Rectangle.Intersects(SelecRect))
                {
                    obj.Selected = true;
                }
                else obj.Selected = false;
                obj.Update();
            }

            if (state.IsKeyDown(Keys.Down) && oldstate != state && CanChange)
            {
                SelecRect.Y += (int)size.Y;
                select.Play();
                CanChange = false;
            }
            if (state.IsKeyDown(Keys.Up) && oldstate != state && CanChange)
            {
                select.Play();
                SelecRect.Y -= (int)size.Y;
                CanChange = false;
            }

            if (state.IsKeyDown(Keys.Escape) && oldstate != state && CanChange)
            {
                back.Play();
                 CanChange = false;
                return MenuState.GameState.MainMenu;
            }

            if (SelecRect.Y >= objects[objects.Count - 1].Rectangle.Y)
                SelecRect.Y = objects[objects.Count - 1].Rectangle.Y;
            if (SelecRect.Y <= objects[0].Rectangle.Y)
                SelecRect.Y = objects[0].Rectangle.Y;

            if (objects[0].Rectangle.Intersects(SelecRect) && state != oldstate)
            {
                if (state.IsKeyDown(Keys.Right) && oldstate != state && CanChange)
                {
                    MediaPlayer.Volume += 0.1f;
                    choose.Play();
                }
                else if (state.IsKeyDown(Keys.Left) && oldstate != state &&  CanChange)
                {
                    choose.Play();
                    if (MediaPlayer.Volume == 1f)
                        MediaPlayer.Volume -= 0.09f;
                    else
                        MediaPlayer.Volume -= 0.1f;         
                }

                CanChange = false;
                subObjects[0].ObjString = ((int)(MediaPlayer.Volume * 100)).ToString() + " % ";
                return MenuState.GameState.Options;
            }

            if (objects[1].Rectangle.Intersects(SelecRect) && state.IsKeyDown(Keys.Enter) && oldstate != state && CanChange)
            {
                if (graphicsDevice.IsFullScreen)
                {
                    push.Play();
                    graphicsDevice.IsFullScreen = false;
                    graphicsDevice.ApplyChanges();
                    ScreenMode = "Window";
                }
                else
                {
                    push.Play();
                    graphicsDevice.IsFullScreen = true;
                    graphicsDevice.ApplyChanges();
                    ScreenMode = "FullScreen";
                }
                CanChange = false;
                return MenuState.GameState.Options;
            }

            if (objects[2].Rectangle.Intersects(SelecRect) && oldstate != state && CanChange)
            {
                if (state.IsKeyDown(Keys.Right) && oldstate != state && CanChange && level < 2)
                {
                    choose.Play();
                    level++;
                }
                else if (state.IsKeyDown(Keys.Left) && oldstate != state && CanChange && level > 1)
                {
                    choose.Play();
                    level--;
                }

                CanChange = false;
                return MenuState.GameState.Options;
            }

            if (objects[3].Rectangle.Intersects(SelecRect)  && oldstate != state && CanChange)
            {
                if (state.IsKeyDown(Keys.Right) && oldstate != state && CanChange && healthPoint < 5)
                {
                    choose.Play();
                    healthPoint++;
                }
                else if (state.IsKeyDown(Keys.Left) && oldstate != state && CanChange && healthPoint > 1)
                {
                    choose.Play();
                    healthPoint--;
                }

                CanChange = false;
                return MenuState.GameState.Options;
            }

            if (objects[objects.Count - 1].Rectangle.Intersects(SelecRect) && state.IsKeyDown(Keys.Enter) && oldstate != state && CanChange)
            {

                CanChange = false;
                back.Play();
                return MenuState.GameState.MainMenu;
            }

            oldstate = state;
            CanChange = true;

            subObjects[1].ObjString = ScreenMode;
            subObjects[2].ObjString = level.ToString();
            subObjects[3].ObjString = healthPoint.ToString();


            return MenuState.GameState.Options;

        }

        public int GetLevel()
        {
            return level;
        }

        public int GetHealthPoint()
        {
            return healthPoint;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backround.Draw(spriteBatch);
            foreach (MenuObject obj in objects)
            {
                obj.Draw(spriteBatch);
            }
            foreach (MenuObject SubObj in subObjects)
            {
                SubObj.Draw(spriteBatch);
            }
        }
    }

}
