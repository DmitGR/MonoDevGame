using Game2.GameClasses;
using Game2.MenuComponents;
using Game2.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace Game2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
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

        int Score;
        int Level;
        int HealthPoint;

        Texture2D back;
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

        Backround test;


        MouseState mouseState;



        MenuState.GameState CurrentGameState = MenuState.GameState.MainMenu;




        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ScreenWidth = graphics.PreferredBackBufferWidth = 1280; // ширина
            ScreenHeight = graphics.PreferredBackBufferHeight = 720; // высота
            graphics.IsFullScreen = false; // полноэкранный режим
            

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
            // camera = new Camera2D();
            MediaPlayer.Volume = 0f;
            options = new Options(graphics);
            gameOver = new GameOver(graphics);
            levelComplete = new LevelComplete(graphics);
            map = new Map(graphics.GraphicsDevice.Viewport);
            player = new Player(Vector2.Zero);
            Level = 2;
            HealthPoint = 3;
            player.Health = new HealthBar(5);
            player.ArmorHud = new ArmorHud();
            mainmenu = new MainMenu(graphics);
            pause = new Pause(graphics);
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
            levelComplete = new LevelComplete(graphics);
            mainmenu = new MainMenu(graphics);
            pause = new Pause(graphics);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            IsMouseVisible = true;
            menuBatch = spriteBatch;
            pause.Load(Content);
            player.Health.Load(Content);
            player.ArmorHud.Load(Content);
            Score = 0;
            player.Load(Content);
            mainmenu.Load(Content);
            options.Load(Content);
            levelComplete.Load(Content);
            gameOver.Load(Content);
            hudFont = Content.Load<SpriteFont>("Hud");
            Map.Content = Content;
            BlockSize = new Vector2(48, 32);
            death = new Animation(Content.Load<Texture2D>("Textures/Hero/death"), 65, 0.8f, false);



            GameTheme = Content.Load<Song>("Sounds/GameTheme");
            camera = new Camera(GraphicsDevice.Viewport);
            backpos = new Vector2 (camera.Center.X,camera.Center.Y);


            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

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
                    // Initialize();
                    StartGame(Level, HealthPoint);
                    // options.Level = Level;
                    Level = options.Level;
                    options.HealthPoint = HealthPoint;
                    // map.Generate(level, BlockSize);
                    MediaPlayer.Stop();
                    CurrentGameState = MenuState.GameState.Playing;
                    MediaPlayer.Play(GameTheme);

                    break;
                case MenuState.GameState.Playing:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        CurrentGameState = MenuState.GameState.Pause;
                    if (player.HasDamaged || Keyboard.GetState().IsKeyDown(Keys.H))
                        CurrentGameState = MenuState.GameState.GameOver;
                    pauseSound = true;

                    MediaPlayer.IsRepeating = true;

                    break;
                case MenuState.GameState.Options:
                    CurrentGameState = options.Update();
                    Level = options.Level;
                    HealthPoint = options.HealthPoint;
                    break;
                case MenuState.GameState.LevelSelect:
                    break;
                case MenuState.GameState.LevelComplete:
                    CurrentGameState = levelComplete.Update(Score);
                    break;
                case MenuState.GameState.MainMenu:
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

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                CurrentGameState = MenuState.GameState.Playing;
            if (CurrentGameState == MenuState.GameState.Playing)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.P))
                    CurrentGameState = MenuState.GameState.Pause;
                mouseState = Mouse.GetState();

                #region Map Collision
                foreach (Block block in map.Blocks)
                {
                    if (player.Rectangle.X >= camera.Center.X - graphics.GraphicsDevice.Viewport.Width && player.Rectangle.X <= camera.Center.X + graphics.GraphicsDevice.Viewport.Width
                        && player.Rectangle.Y <= camera.Center.Y + graphics.GraphicsDevice.Viewport.Height && player.Rectangle.Y >= camera.Center.Y - graphics.GraphicsDevice.Viewport.Height)
                    {
                        player.Collision(block, map.Width, map.Height);
                        if (block is Exit && player.Rectangle.Intersects(block.Rectangle))
                        {
                            if (Score < 50)
                            { spriteBatch.Begin(); spriteBatch.DrawString(hudFont, "Not Yet!", new Vector2(block.Rectangle.Center.X, block.Rectangle.Top), Color.DarkRed); spriteBatch.End(); }
                            else
                            {
                                Level++;
                                MediaPlayer.Pause();
                                levelComplete.PlaySound();
                                CurrentGameState = MenuState.GameState.LevelComplete;
                            }
                        }
                    }
                }
                    foreach (Enemy enemy in map.Enemys)
                {
                    if (enemy.Rectangle.X >= camera.Center.X - graphics.GraphicsDevice.Viewport.Width && enemy.Rectangle.X <= camera.Center.X + graphics.GraphicsDevice.Viewport.Width 
                        && enemy.Rectangle.Y <= camera.Center.Y + graphics.GraphicsDevice.Viewport.Height && enemy.Rectangle.Y >= camera.Center.Y - graphics.GraphicsDevice.Viewport.Height)
                    {
                        enemy.Update();
                        player.Collision(enemy, gameTime);
                        if (enemy.CanGetScore)
                            Score += enemy.Score;

                    }


                }
                foreach (Item item in map.Items)
                {
                    item.Update(player.Rectangle);
                    if (item.IsPicked && item.CanPicked)
                    {
                        if (item is Health)
                            player.Health.Points++;                      
                        if (item is Jug)
                            Score += 100;
                        if (item is Armor) { 
                            player.GetArmored = true;}
                        item.CanPicked = false;
                    }
                }

                foreach (CheckPoint checkPoint in map.ChekPoints)
                {
                    checkPoint.Update();
                    player.Collision(checkPoint);
                }
                map.EnemyCollision(camera.Center);
                #endregion

                camera.Update(player.Position, map.Width, map.Height);
                player.Health.Update(camera, ScreenWidth, ScreenHeight);
                player.ArmorHud.Update(camera, ScreenWidth, ScreenHeight);
                player.Update(gameTime);
                player.mouseDebug(new Vector2((camera.Center.X - ScreenWidth / 2 + mouseState.X), (camera.Center.Y - ScreenHeight / 2 + mouseState.Y)));
            }

            //camera.Update(player.Position);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {     
           // GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.Black);
          
            #region MenuDraw

            switch (CurrentGameState)
            {
                case MenuState.GameState.Playing:
                    
                    break;
                case MenuState.GameState.Options:
                    options.Draw(spriteBatch);
                    break;
                case MenuState.GameState.LevelSelect:
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
                    animationSprite.Draw(gameTime, spriteBatch, new Vector2(ScreenWidth/2- player.Rectangle.Width*4, ScreenHeight/2 - player.Rectangle.Height/2), SpriteEffects.None);

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
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
                map.Draw(spriteBatch, gameTime, camera.Center);
                player.Draw(gameTime, spriteBatch);
                player.Health.Draw(spriteBatch);
                player.ArmorHud.Draw(spriteBatch);
                DrawHud();
                spriteBatch.End();
          
            }
            base.Draw(gameTime);
        }



         private void DrawHud()
        {
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            Vector2 center = new Vector2(ScreenWidth / 2, ScreenHeight / 2);

            string ScoreString = "Score: " + Score;
            Vector2 ScorePos = new Vector2(camera.Center.X - ScreenWidth / 2 + 10, camera.Center.Y - ScreenHeight / 2 + 10);
            DrawShadowedString(hudFont,ScoreString, ScorePos,Color.White );



            string timeString = "Pos: " + (int)player.Position.X  + " : " + (int)player.Position.Y;
            string camtpos = "Camera Pos: " + camera.Center.X + " : " + camera.Center.Y;
            string rectpos = "Rect Pos: " + (int)player.Rectangle.X + " : " + player.Rectangle.Y;
            string texttpos = "Info X : " + (camera.Center.X - ScreenWidth/2  + mouseState.X) + " Y: " + (camera.Center.Y - ScreenHeight/2 + mouseState.Y );
            var strpos = new Vector2(camera.Center.X - ScreenWidth/2 , camera.Center.Y + ScreenHeight/2-20);




            DrawShadowedString(hudFont, timeString, strpos, Color.Red);

            DrawShadowedString(hudFont, camtpos, strpos - new Vector2(0,20), Color.Red);
            DrawShadowedString(hudFont, rectpos, strpos - new Vector2(0, 40), Color.Red);
            DrawShadowedString(hudFont, texttpos, strpos - new Vector2(0, 60), Color.Red);
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }


    }
}
