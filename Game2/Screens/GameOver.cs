using RGR.GameClasses;
using RGR.MenuComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace RGR.Screens
{
    class GameOver
    {

        List<MenuObject> objects = new List<MenuObject>(2);

        Texture2D gameOver;
        Backround lessHp;
        Backround BackOver;
        Rectangle SelecRect;
        Vector2 size;
        KeyboardState state;
        KeyboardState oldstate;
        GraphicsDeviceManager graphicsDevice;
        SoundEffect select, push, back;
        SoundEffect pause;
        Song OverTheme;
        bool CanChange;
        bool over, overSound;
        public bool Over { get { return over; } }
        public GraphicsDeviceManager GraphicsDevice
        {
            set { graphicsDevice = value; }
        }

        public GameOver(GraphicsDeviceManager graphicsDevice) { this.graphicsDevice = graphicsDevice; ; }

        public void Load(ContentManager Content)
        {
            gameOver = Content.Load<Texture2D>("Textures/Misc/gameOver");
            lessHp = new Backround(Content.Load<Texture2D>("Textures/Backrounds/Backround3"), new Rectangle(0, 0, graphicsDevice.PreferredBackBufferWidth, graphicsDevice.PreferredBackBufferHeight));
            BackOver = new Backround(Content.Load<Texture2D>("Textures/Backrounds/BackroundOver"), new Rectangle(0, 0, graphicsDevice.PreferredBackBufferWidth, graphicsDevice.PreferredBackBufferHeight));
            overSound = true;

            select = Content.Load<SoundEffect>("Sounds/MenuSelect");
            push = Content.Load<SoundEffect>("Sounds/MenuPush");
            back = Content.Load<SoundEffect>("Sounds/MenuBack");
            pause = Content.Load<SoundEffect>("Sounds/MenuPause");
            OverTheme = Content.Load<Song>("Sounds/gameOver");

            size = new Vector2(graphicsDevice.PreferredBackBufferWidth / 16, graphicsDevice.PreferredBackBufferHeight / 16);
            Vector2 center = new Vector2(graphicsDevice.PreferredBackBufferWidth / 2, graphicsDevice.PreferredBackBufferHeight / 2);

            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X*2), (int)(center.Y - size.Y * 3), (int)size.X, (int)size.Y), ""));
            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X), (int)(center.Y - size.Y ), (int)size.X, (int)size.Y), "Continue"));
            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X), (int)(center.Y ), (int)size.X, (int)size.Y), "Main Menu"));
            foreach (MenuObject obj in objects)
            {
                obj.Load(Content);
            }
            CanChange = true;
            SelecRect = new Rectangle((int)(center.X - size.X), (int)(center.Y - size.Y * 2), (int)size.X, (int)size.Y);
        }

        public MenuState.GameState Update(HealthBar health, bool HasDamaged)
        {
            state = Keyboard.GetState();
            if (!Over)
            {
                MediaPlayer.Pause();
                foreach (MenuObject obj in objects)
                {

                    if (obj.Rectangle.Intersects(SelecRect))
                    {
                        obj.Selected = true;
                    }
                    else obj.Selected = false;
                    obj.Update();
                }

                if (state.IsKeyDown(Keys.Down) && oldstate != state)
                {
                    select.Play();
                    SelecRect.Y += (int)size.Y;
                }
                if (state.IsKeyDown(Keys.Up) && oldstate != state)
                {
                    select.Play();
                    SelecRect.Y -= (int)size.Y;
                }
            }
            if (health.Points <= 0)
            {
                objects[1].Color = Color.Gray;
                SelecRect.Y = objects[objects.Count - 1].Rectangle.Y;

            }
            if (SelecRect.Y >= objects[objects.Count - 1].Rectangle.Y)
                SelecRect.Y = objects[objects.Count - 1].Rectangle.Y;
            if (SelecRect.Y <= objects[1].Rectangle.Y)
                SelecRect.Y = objects[1].Rectangle.Y;
            if (objects[1].Rectangle.Intersects(SelecRect) && state.IsKeyDown(Keys.Enter) && oldstate != state && CanChange)
            {
                CanChange = false;
                back.Play();
                MediaPlayer.Resume();
                return MenuState.GameState.Playing;
            }
            if (objects[2].Rectangle.Intersects(SelecRect) && state.IsKeyDown(Keys.Enter) && oldstate != state && CanChange)
            {
                CanChange = false;
                back.Play();
                MediaPlayer.Play(MainMenu.MainMenuTheme);
                return MenuState.GameState.MainMenu;
            }
            objects[0].Color = Color.Gainsboro;

            if (health.Points == 0)
            {
                objects[0].ObjString = "";
                over = true;
            }
            else
                objects[0].ObjString = "You have: " + health.Points + " HealthPoints ";
            if (over && overSound)
            {
                MediaPlayer.Play(OverTheme);
                overSound = false;
            }
            oldstate = state;
            CanChange = true;
            return MenuState.GameState.GameOver;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Over)
                lessHp.Draw(spriteBatch);

            foreach (MenuObject obj in objects)
            {
                obj.Draw(spriteBatch);
            }

            if (Over)
            {
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                BackOver.Draw(spriteBatch);
            }
        }

    }
}
