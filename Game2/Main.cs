using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RGR.GameClasses;
using RGR.MenuComponents;
using RGR.Screens;

namespace RGR
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {

        #region Fields

        Logic logic;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont hudFont;
        SpriteBatch menuBatch;

        AnimationSprite animationSprite;
        Animation death;

        // Screen
        public int ScreenWidth { get; }         
        public int ScreenHeight { get; }



        //GameWrold
        Player player;
        Camera camera;
        Map map;

        int Level;
        int HealthPoint;

        Vector2 backpos;

        Song GameTheme;

        Vector2 BlockSize;

        // Menu
        MainMenu mainmenu;
        Pause pause;
        bool pauseSound;
        Options options;
        GameOver gameOver;
        LevelComplete levelComplete;
        MenuState.GameState CurrentGameState;

        MouseState mouseState;

        bool Info;

        private bool CanCompleteLevel;

        const int BlockWidth = 48;
        const int BlockHeight = 32;
        const int ScorePosSet = 10;
        #endregion

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ScreenWidth = graphics.PreferredBackBufferWidth = 1280; // Width
            ScreenHeight = graphics.PreferredBackBufferHeight = 720; // Height
            graphics.IsFullScreen = false; //SceenMode
            CurrentGameState = MenuState.GameState.MainMenu;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 
        protected override void Initialize()
        {
            MediaPlayer.Volume = 0.01f;
            Info = false;
            options = new Options(graphics);
            gameOver = new GameOver(graphics);
            levelComplete = new LevelComplete(graphics);
            map = new Map(graphics.GraphicsDevice.Viewport);
            player = new Player(Vector2.Zero);
            Level = 1;
            HealthPoint = 3;
            player.Health = new HealthBar(HealthPoint);
            player.ArmorHud = new ArmorHud();
            player.SplashHud = new SplashHud();
            mainmenu = new MainMenu(graphics);
            pause = new Pause(graphics);
            logic = new Logic(graphics.GraphicsDevice);

            base.Initialize();
        }

        protected void StartGame(int Level,int HP)
        {
            options = new Options(graphics);
            gameOver = new GameOver(graphics);
            map = new Map(graphics.GraphicsDevice.Viewport);
            map.Generate(Level, BlockSize);
            player = new Player(map.Start);
            player.Health = new HealthBar(HP);
            player.ArmorHud = new ArmorHud();
            player.SplashHud = new SplashHud();
            levelComplete = new LevelComplete(graphics);
            mainmenu = new MainMenu(graphics);
            pause = new Pause(graphics);
            logic = new Logic(graphics.GraphicsDevice);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            if(Info)
            IsMouseVisible = true;
            menuBatch = spriteBatch;
            pause.Load(Content);
            player.Load(Content);
            mainmenu.Load(Content);
            options.Load(Content);
            levelComplete.Load(Content);
            gameOver.Load(Content);
            hudFont = Content.Load<SpriteFont>("Hud");
            Map.Content = Content;
            BlockSize = new Vector2(BlockWidth, BlockHeight);
            death = new Animation(Content.Load<Texture2D>("Textures/Hero/death"), 65, 0.8f, false);
            CanCompleteLevel = true;

            GameTheme = Content.Load<Song>("Sounds/GameTheme");
            camera = new Camera(GraphicsDevice.Viewport);
            backpos = new Vector2 (camera.Center.X,camera.Center.Y);


            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            logic.Load(Content);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            #region MenuUpdate
            switch (CurrentGameState)
            {
                case MenuState.GameState.NewGame:
                    Level = options.Level;
                    StartGame(Level, options.HealthPoint);
                    options.HealthPoint = HealthPoint;
                    MediaPlayer.Stop();
                    CurrentGameState = MenuState.GameState.Playing;
                    MediaPlayer.Play(GameTheme);
                    if (CurrentGameState == MenuState.GameState.Playing)
                        logic.Update(map, player, camera, gameTime);
                    break;
                case MenuState.GameState.Playing:
                    CurrentGameState = logic.GameState(CurrentGameState);
                    logic.Update(map, player, camera, gameTime);
                    pauseSound = true;
                    break;
                case MenuState.GameState.Options:
                    CurrentGameState = options.Update();
                    Level = options.Level;
                    HealthPoint = options.HealthPoint;
                    break;
                case MenuState.GameState.LevelComplete:
                    CurrentGameState = levelComplete.Update(logic.Score);
                    options.Level = logic.Level;
                    break;
                case MenuState.GameState.MainMenu:
                    options.Level = Level;
                    pauseSound = true;
                    CurrentGameState = mainmenu.Update();
                    break;
                case MenuState.GameState.GameOver:
                    CurrentGameState = gameOver.Update(player.Health, player.HasDamaged);
                    break;
                case MenuState.GameState.Pause:
                    MediaPlayer.Pause();
                    CurrentGameState = pause.Update();
                    if (pauseSound)
                    {
                        pause.PlaySound();
                        pauseSound = false;
                    }
                    break;
                case MenuState.GameState.Exit:
                    Exit();
                    break;
                default:
                    break;
            }
   
        if (CurrentGameState == MenuState.GameState.GameOver && !gameOver.Over)
           {
               animationSprite.PlayAnimation(death);
           }
           else animationSprite.PlayAnimation(null);
            #endregion
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {     
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.Black);
          
            #region MenuDraw

            switch (CurrentGameState)
            {
                case MenuState.GameState.Options:
                    options.Draw(spriteBatch);
                    break;
                case MenuState.GameState.LevelComplete:
                    levelComplete.Draw(spriteBatch);
                    break;
                case MenuState.GameState.MainMenu:
                    mainmenu.Draw(spriteBatch);
                    break;
                case MenuState.GameState.GameOver:            
                    gameOver.Draw(spriteBatch);
                    if(!gameOver.Over)
                    animationSprite.Draw(gameTime, spriteBatch, new Vector2(ScreenWidth/2- player.Rectangle.Width*4, ScreenHeight/2 - player.Rectangle.Height/2), SpriteEffects.None,0);
                    break;
                case MenuState.GameState.Pause:                
                    pause.Draw(spriteBatch);
                    break;
                case MenuState.GameState.Exit:
                    break;
                default:
                    break;
            }
            spriteBatch.End();
            #endregion


            if (CurrentGameState == MenuState.GameState.Playing)
            {
                GraphicsDevice.Clear(Color.Cyan);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
                map.Draw(spriteBatch, gameTime, camera.Center);
                player.Draw(gameTime, spriteBatch);

                DrawHud();
                if (!CanCompleteLevel)
                {
                    spriteBatch.DrawString(hudFont, "Not Enough Score!", new Vector2(camera.Center.X - ScreenWidth / 10, camera.Center.Y - ScreenHeight / 2 + ScreenHeight / 9), Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(hudFont, "Not Enough Score!", new Vector2(camera.Center.X - ScreenWidth / 10 + 2f, camera.Center.Y - ScreenHeight / 2 + ScreenHeight / 9 + 2f), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

                }
                spriteBatch.End();
          
            }
            base.Draw(gameTime);
        }

         private void DrawHud()
        {
            player.Health.Draw(spriteBatch);
            player.ArmorHud.Draw(spriteBatch);
            player.SplashHud.Draw(spriteBatch);
            Vector2 center = new Vector2(ScreenWidth / 2, ScreenHeight / 2);

            string ScoreString = "Score: " + logic.Score;
            Vector2 ScorePos = new Vector2(camera.Center.X - ScreenWidth / 2 + ScorePosSet, camera.Center.Y - ScreenHeight / 2 + ScorePosSet);
            DrawShadowedString(hudFont,ScoreString, ScorePos,Color.White );
            #region debug
            if (Info)
            {

                string timeString = "Pos: " + (int)player.Position.X + " : " + (int)player.Position.Y;
                string camtpos = "Camera Pos: " + camera.Center.X + " : " + camera.Center.Y;
                string rectpos = "Rect Pos: " + (int)player.Rectangle.X + " : " + player.Rectangle.Y;
                string texttpos = "Info X : " + (camera.Center.X - ScreenWidth / 2 + mouseState.X) + " Y: " + (camera.Center.Y - ScreenHeight / 2 + mouseState.Y);
                var strpos = new Vector2(camera.Center.X - ScreenWidth / 2, camera.Center.Y + ScreenHeight / 2 - 20);

                DrawShadowedString(hudFont, timeString, strpos, Color.Red);

                DrawShadowedString(hudFont, camtpos, strpos - new Vector2(0, 20), Color.Red);
                DrawShadowedString(hudFont, rectpos, strpos - new Vector2(0, 40), Color.Red);
                DrawShadowedString(hudFont, texttpos, strpos - new Vector2(0, 60), Color.Red);
            }
            #endregion
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.2f, 1.2f), Color.Black , 0f, Vector2.Zero , 1f, SpriteEffects.None, 0.9f);
            spriteBatch.DrawString(font, value, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }
    }
}
