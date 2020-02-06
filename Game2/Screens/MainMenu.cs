using RGR.GameClasses;
using RGR.MenuComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace RGR.Screens
{
    public class MainMenu
    {
        List<MenuObject> objects = new List<MenuObject>(3);

        Rectangle SelecRect;
        Rectangle CirlceRect;
        Vector2 center;
        Vector2 size;
        KeyboardState state;
        KeyboardState oldstate;
        GraphicsDeviceManager graphicsDevice;
        public static Song MainMenuTheme;
        SoundEffect select, push, back;
        bool CanChange;

        Scrolling scroll1;
        Scrolling scroll2;

        Texture2D Circle;

        public GraphicsDeviceManager GraphicsDevice
        {
            set { graphicsDevice = value; }
        }

        public MainMenu(GraphicsDeviceManager graphicsDevice) {  this.graphicsDevice = graphicsDevice;  ; }

        public void Load(ContentManager Content)
        {
            CanChange = false;

            select = Content.Load<SoundEffect>("Sounds/MenuSelect");
            push = Content.Load<SoundEffect>("Sounds/MenuPush");
            back = Content.Load<SoundEffect>("Sounds/MenuBack");

            MainMenuTheme = Content.Load<Song>("Sounds/MainMenuTheme");

            size = new Vector2(graphicsDevice.PreferredBackBufferWidth / 16, graphicsDevice.PreferredBackBufferHeight / 16);
            center = new Vector2(graphicsDevice.PreferredBackBufferWidth /2 , graphicsDevice.PreferredBackBufferHeight / 2);

            scroll1 = new Scrolling (Content.Load<Texture2D>("Textures/Backrounds/Backround1"), new Rectangle(0, 0, graphicsDevice.PreferredBackBufferWidth, graphicsDevice.PreferredBackBufferHeight));
            scroll2 = new Scrolling(Content.Load<Texture2D>("Textures/Backrounds/Backround1"), new Rectangle(graphicsDevice.PreferredBackBufferWidth, 0 , graphicsDevice.PreferredBackBufferWidth, graphicsDevice.PreferredBackBufferHeight));
            Circle = Content.Load<Texture2D>("Textures/Misc/Circle");
            CirlceRect = new Rectangle((int)(center.X - size.X*2) , (int)(center.Y - size.Y*4), 255, 250);


            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X), (int)(center.Y - size.Y * 3), (int)size.X, (int)size.Y ),  "Start"));
            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X), (int)(center.Y - size.Y * 2), (int)size.X, (int)size.Y), "Options"));
            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X), (int)(center.Y - size.Y * 1), (int)size.X, (int)size.Y),  "Exit"));

            foreach (MenuObject obj in objects)
            {
                obj.Load(Content);
            }
            SelecRect = new Rectangle((int)(center.X - size.X), (int)(center.Y - size.Y * 3), (int)size.X, (int)size.Y);
            MediaPlayer.Play(MainMenuTheme);
        }

        public MenuState.GameState Update()
        {
            if (scroll1.Rect.Right <= 2)
                scroll1.Rect = new Rectangle(graphicsDevice.PreferredBackBufferWidth, 0, graphicsDevice.PreferredBackBufferWidth, graphicsDevice.PreferredBackBufferHeight);
            else scroll1.Update();
            if (scroll2.Rect.Right <= 2)
                scroll2.Rect = new Rectangle(graphicsDevice.PreferredBackBufferWidth, 0, graphicsDevice.PreferredBackBufferWidth, graphicsDevice.PreferredBackBufferHeight);
            else scroll2.Update();

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

            if (SelecRect.Y >= objects[2].Rectangle.Y)
                SelecRect.Y = objects[2].Rectangle.Y;
            if (SelecRect.Y <= objects[0].Rectangle.Y)
                SelecRect.Y = objects[0].Rectangle.Y;


            if (objects[0].Rectangle.Intersects(SelecRect) && state.IsKeyDown(Keys.Enter) && oldstate != state && CanChange)
            {

                push.Play();
                MediaPlayer.Stop();
                CanChange = false;
                return MenuState.GameState.NewGame;
            }
            if (objects[1].Rectangle.Intersects(SelecRect) && state.IsKeyDown(Keys.Enter) && oldstate != state && CanChange)
            {
                push.Play();
                CanChange = false;
                return MenuState.GameState.Options;
            }
            if (objects[2].Rectangle.Intersects(SelecRect) && state.IsKeyDown(Keys.Enter))
            {
                back.Play();
                return MenuState.GameState.Exit;
            }


            oldstate = state;
            CanChange = true;

            return MenuState.GameState.MainMenu;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            scroll1.Draw(spriteBatch);
            scroll2.Draw(spriteBatch);

            spriteBatch.Draw(Circle, CirlceRect, new Color(0,0,0,100));

            foreach (MenuObject obj in objects)
            {
                obj.Draw(spriteBatch);
            }
        }
    }

}
