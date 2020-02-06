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
    class LevelComplete
    {

        List<MenuObject> objects = new List<MenuObject>(2);

        Backround backround;
        Rectangle SelecRect;
        Vector2 size;
        KeyboardState state;
        KeyboardState oldstate;
        GraphicsDeviceManager graphicsDevice;
        SoundEffect select, push, back;
        bool CanChange;
        int FinalScore;

        public GraphicsDeviceManager GraphicsDevice
        {
            set { graphicsDevice = value; }
        }


        public LevelComplete(GraphicsDeviceManager graphicsDevice) { this.graphicsDevice = graphicsDevice; ; }

        public void Load(ContentManager Content)
        {
            backround = new Backround(Content.Load<Texture2D>("Textures/Backrounds/BackroundComplete"), new Rectangle(0, 0, graphicsDevice.PreferredBackBufferWidth, graphicsDevice.PreferredBackBufferHeight));

            select = Content.Load<SoundEffect>("Sounds/MenuSelect");
            push = Content.Load<SoundEffect>("Sounds/MenuPush");
            back = Content.Load<SoundEffect>("Sounds/MenuBack");
            FinalScore = 0;

            size = new Vector2(graphicsDevice.PreferredBackBufferWidth / 16, graphicsDevice.PreferredBackBufferHeight / 16);
            Vector2 center = new Vector2(graphicsDevice.PreferredBackBufferWidth / 2, graphicsDevice.PreferredBackBufferHeight / 2);
      
            objects.Add(new MenuObject(new Rectangle((int)(center.X - graphicsDevice.PreferredBackBufferWidth/10), (int)(center.Y - graphicsDevice.PreferredBackBufferHeight / 6), (int)size.X, (int)size.Y), "Score "));
            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X), (int)(center.Y ), (int)size.X, (int)size.Y), "Continue"));
            objects.Add(new MenuObject(new Rectangle((int)(center.X - size.X), (int)(center.Y + size.Y ), (int)size.X, (int)size.Y), "Main Menu"));
            foreach (MenuObject obj in objects)
            {
                obj.Load(Content);
            }
            CanChange = true;
            SelecRect = new Rectangle((int)(center.X - size.X), (int)(center.Y - size.Y * 2), (int)size.X, (int)size.Y);
        }

        public MenuState.GameState Update(int Score)
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
            
            if (SelecRect.Y >= objects[objects.Count - 1].Rectangle.Y)
                SelecRect.Y = objects[objects.Count - 1].Rectangle.Y;
            if (SelecRect.Y <= objects[1].Rectangle.Y)
                SelecRect.Y = objects[1].Rectangle.Y;
            if (objects[1].Rectangle.Intersects(SelecRect) && state.IsKeyDown(Keys.Enter) && oldstate != state && CanChange)
            {
                CanChange = false;
                back.Play();
                MediaPlayer.Resume();
                return MenuState.GameState.NewGame;
            }
            if (objects[2].Rectangle.Intersects(SelecRect) && state.IsKeyDown(Keys.Enter) && oldstate != state && CanChange)
            {
                CanChange = false;
                back.Play();
                MediaPlayer.Play(MainMenu.MainMenuTheme);
                return MenuState.GameState.MainMenu;
            }
            objects[0].Color = Color.MonoGameOrange;
            oldstate = state;
            CanChange = true;
            if (FinalScore < Score)
            {
                if (Score - FinalScore < Score / 5)
                    FinalScore += 10;
                else FinalScore += 50;
            }

            objects[0].ObjString = "Score : " + FinalScore;

            return MenuState.GameState.LevelComplete;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backround.Draw(spriteBatch);
            foreach (MenuObject obj in objects)
            {
                obj.Draw(spriteBatch);
            }
        }

    }
}
