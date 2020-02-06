using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RGR.GameClasses;
using RGR.GameObjects.Blocks;
using RGR.GameObjects.Enemies;
using RGR.GameObjects.Items;
using RGR.MenuComponents;

namespace RGR
{
    class Logic
    {

        public int ScreenWidth { get; }
        public int ScreenHeight { get; }

        AnimationSprite animationSprite;
        Animation death;
        public int Score { get; private set; }
        public int Level { get; private set; }
        const int CollisionDist = 2;

        SoundEffect Levelcomplete;

        private bool PlayerTakeDamage;
        private bool CanCompleteLevel;


        public Logic(GraphicsDevice g)
        {
            ScreenWidth = g.Viewport.Width;
            ScreenHeight = g.Viewport.Height;
        }
        public void Load(ContentManager Content)
        {
            death = new Animation(Content.Load<Texture2D>("Textures/Hero/death"), 65, 0.8f, false);
            Levelcomplete = Content.Load<SoundEffect>("Sounds/LevelComplete");
            Score = 0;
        }
        public void Update(Map map, Player player, Camera camera, GameTime gameTime)
        {
            HudUpdate(map, player, camera);
            player.Update(gameTime);
            Control.Input(player, gameTime);

            foreach (GameObject obj in map.GameObjects)
                if (obj.rectangle.X + obj.rectangle.Width >= camera.Center.X - ScreenWidth / 2 && obj.rectangle.X - obj.rectangle.Width <= camera.Center.X + ScreenWidth / 2
                             && obj.rectangle.Y + obj.rectangle.Height >= camera.Center.Y - ScreenHeight / 2 && obj.rectangle.Y - obj.rectangle.Height <= camera.Center.Y + ScreenHeight / 2)
                {
                    obj.Update(gameTime);
                    obj.Update();
                }

            MapCollision(map, player, camera, gameTime);
        }

        public MenuState.GameState GameState(MenuState.GameState CurrentGameState)
        {
            if (CurrentGameState == MenuState.GameState.Playing)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    CurrentGameState = MenuState.GameState.Pause;
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad1) && Keyboard.GetState().IsKeyDown(Keys.NumPad4) && Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                    Score = 0;

                if (PlayerTakeDamage)
                {
                    CurrentGameState = MenuState.GameState.GameOver;
                    PlayerTakeDamage = false;
                }
                if (CanCompleteLevel)
                {
                    CurrentGameState = MenuState.GameState.LevelComplete;
                    CanCompleteLevel = false;
                    MediaPlayer.Pause();
                    Levelcomplete.Play();
                }
                MediaPlayer.IsRepeating = true;
            }
            return CurrentGameState;
        }
        private void MapCollision(Map map, Player player, Camera camera, GameTime gameTime)
        {
            foreach (Block block in map.Blocks)
            {
              PlayerCollision(block, player, map.Width, map.Height);
                if (player.HasDamaged)
                {
                    animationSprite.PlayAnimation(death);
                    player.HasDamaged = false;
                    PlayerTakeDamage = true;
                }
                if (block is Exit && player.Rectangle.Intersects(block.Rectangle))
                {
                    if (Score < 4000)
                        CanCompleteLevel = false;
                    else
                    {
                        CanCompleteLevel = true;
                        Level++;
                    }
                    if (block is Exit && !player.Rectangle.Intersects(block.Rectangle)) { CanCompleteLevel = false; }

                }
            }
            foreach (Enemy enemy in map.Enemys)
            {
                if (enemy.Rectangle.X + enemy.Rectangle.Width >= camera.Center.X - ScreenWidth && enemy.Rectangle.X - enemy.Rectangle.Width <= camera.Center.X + ScreenWidth
                    && enemy.Rectangle.Y <= camera.Center.Y + ScreenHeight && enemy.Rectangle.Y >= camera.Center.Y - ScreenHeight)
                {
                    PlayerCollision(enemy, player, gameTime);
                    
                    if (player.HasDamaged)
                    {
                        animationSprite.PlayAnimation(death);
                        player.HasDamaged = false;
                        PlayerTakeDamage = true;
                    }
                    for (int j = 0; j < map.Blocks.Count; j++)
                        if (!map.Blocks[j].Passable && map.Blocks[j].Rectangle.Intersects(enemy.CollisionArea))
                            enemy.Collision(map.Blocks[j]);
                }
                if (enemy.CanGetScore)
                {
                    Score += enemy.Score;
                    enemy.CanGetScore = false;
                }
            }

            foreach (Item item in map.Items)
            {
                if (item.Rectangle.Intersects(player.CollisionArea))
                {
                    item.Update(player.Rectangle);
                    if (item.IsPicked && item.CanPicked)
                    {
                        if (item is Health)
                            player.Health.Points++;
                        if (item is Jug)
                            Score += 100;
                        if (item is Armor)
                            player.GetArmored = true;
                        if (item is Potion)
                            player.SplashHud.IsPicked = true;
                    }
                }
            }
            foreach (CheckPoint checkPoint in map.ChekPoints)
            {
                PlayerCollision(checkPoint, player);
            }

        }

        private void HudUpdate(Map map, Player player, Camera camera)
        {
            camera.Update(player.Position, map.Width, map.Height);
            player.Health.Update(camera, ScreenWidth, ScreenHeight);
            player.ArmorHud.Update(camera, ScreenWidth, ScreenHeight);
            player.SplashHud.Update(camera, ScreenWidth, ScreenHeight);
        }

        private void PlayerCollision(Block block, Player player, int xOffset, int yOffset)
        {
            if (!block.Passable && !block.Damaged)
            {
                if (player.rectangle.Intersects(block.Rectangle) && player.wasSit && !player.hasJumped)
                {
                    player.rectangle.Y -= CollisionDist;
                    player.position.X -= player.velocity.X;
                }

                if (player.rectangle.TouchTopOf(block.Rectangle))
                {
                    player.rectangle.Y = block.Rectangle.Y - player.rectangle.Height ;
                    player.position.Y = block.Rectangle.Y - player.rectangle.Height - CollisionDist;

                    if (player.CanJump)
                        player.velocity.Y = 0f;
                    player.hasJumped = false;
                }

                if (player.rectangle.TouchLeftOf(block.Rectangle))
                    player.position.X = block.Rectangle.X - player.rectangle.Width - CollisionDist;

                if (player.rectangle.TouchRightOf(block.Rectangle))
                    player.position.X = block.Rectangle.X + block.Rectangle.Width + CollisionDist;

                if (player.rectangle.TouchBottomOf(block.Rectangle))
                    player.velocity.Y = 1f;

                if (player.position.X < 0)
                    player.position.X = 0;
                if (player.position.X + player.rectangle.Width >= xOffset)
                    player.position.X = xOffset - player.rectangle.Width;
                if (player.position.Y < 0)
                    player.position.Y = 0;
                if (player.position.Y > yOffset - player.rectangle.Width)
                    player.position.Y = yOffset;
            }
            else if (block.Damaged)
            {
                var temp = new Rectangle(block.Rectangle.X + block.Rectangle.Width / 4, block.Rectangle.Y + block.Rectangle.Height / 4, block.Rectangle.Width / 2, block.Rectangle.Height / 2);
                if (player.rectangle.Intersects(temp) && !player.HasDamaged)
                {
                    player.HasDamaged = true;
                    player.velocity.Y = 0f;
                    player.position = player.checkpoint;
                    player.Health.Points--;
                    player.ArmorHud.Points = 0;
                    player.timerForArmor = 0;
                    player.SplashHud.IsActiv = false;
                    player.SplashHud.Points = 0;
                    player.PlaySounds();
                }
            }
            if (player.HasSplash && !block.Passable && player.splashRect.Intersects(block.Rectangle))
            {
                player.HasSplash = false;
                player.splashRect = Rectangle.Empty;
            }
        }

        private void PlayerCollision(Enemy enemy, Player player, GameTime gameTime)
        {

            if (player.armored && player.rectangle.Intersects(enemy.Rectangle) && !(enemy is RedSkeleton) && !(enemy is Smoke))
            {
                player.PlayAnimation();
                enemy.HealthPoint -= enemy.HealthPoint;
                player.armorDamage.Play();
            }
            else if (player.rectangle.Intersects(enemy.Rectangle) && !player.HasDamaged)
            {
                if ((enemy is RedSkeleton || enemy is Smoke) && player.armored)
                    player.timerForArmor -= 4f;
                else
                {
                    player.HasDamaged = true;
                    player.position = player.checkpoint;
                    player.Health.Points--;
                    player.SplashHud.Points = 0;
                    player.SplashHud.IsActiv = false;
                    player.PlaySounds();
                }
            }
            if (player.hasAttack && enemy.timerForDamage <= 0f && enemy.Rectangle.Intersects(player.damageArea) && enemy.CanDamaged || enemy.Rectangle.Intersects(player.splashRect))
            {
                if (!player.HasSplash)
                    enemy.DamageSound.Play();
                else
                    player.splashDamage.Play();

                enemy.HealthPoint--;
                enemy.CanDamaged = false;
                enemy.timerForDamage = enemy.DamageTimer;
                player.splashRect = Rectangle.Empty;
                player.HasSplash = false;
            }

            if (!enemy.CanDamaged)
            {
                enemy.timerForDamage--;
                if (enemy.timerForDamage <= 0f)
                    enemy.CanDamaged = true;
            }
        }

        private void PlayerCollision(CheckPoint checkPoint, Player player)
        {
            Rectangle rect = new Rectangle(checkPoint.Rectangle.Left + 10, checkPoint.Rectangle.Y, checkPoint.Rectangle.Width / 2, checkPoint.Rectangle.Height);

            if (player.rectangle.Intersects(rect) && !checkPoint.IsUsed)
            {
                checkPoint.IsUsed = true;
                player.checkpoint = checkPoint.Position;
                checkPoint.PlaySound();
            }
        }
    }
}