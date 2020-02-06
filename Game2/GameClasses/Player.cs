using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2.GameClasses
{
    class Player
    {

        AnimationSprite animationSprite;
        AnimationSprite effectSprite;

        Animation run, idle, jump, attack, death, damaged, jumpAttack, sit, sitAttack, fall, armor;

        Texture2D texture;

        Animation test;

        SoundEffect sword,armorDamage;
         
        Vector2 velocity;
        Rectangle rectangle;
        KeyboardState state;
        KeyboardState oldState;


        public Rectangle Rectangle { get {return rectangle; }  }

        bool LookLeftSide;
        bool hasJumped = false;
        bool CanJump = true;
        bool hasAttack = false;

        public bool GetArmored { get; set; }
        private bool armored;

        bool CanAttack =true;
        float timerForAttack;
        float timerForSit;
        float timerForDamage;
        float timerForArmor;
        Rectangle damageArea;
        bool hasDamaged = false;
        public bool HasDamaged { get { return hasDamaged; }set { hasDamaged = value; } }
        bool isSit;
        bool wasSit = false;

        HealthBar health;
        public HealthBar Health { get { return health; } set { health = value; } }

        ArmorHud armorHud;
        public ArmorHud ArmorHud { get { return armorHud; } set { armorHud = value; } }

        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
        }

        Vector2 checkpoint;
        private bool attacksound =true;


        public Player(Vector2 Start) { checkpoint = position = Start; }

        public void Load(ContentManager Content)
        {

            texture = Content.Load<Texture2D>("test");

            idle = new Animation(Content.Load<Texture2D>("Textures/Hero/idle"), 46, 0.1f, true);
            jump = new Animation(Content.Load<Texture2D>("Textures/Hero/jump"), 50, 0.2f, true);
            fall = new Animation(Content.Load<Texture2D>("Textures/Hero/fall"), 34, 0.1f, false);
            attack = new Animation(Content.Load<Texture2D>("Textures/Hero/attack"), 65, 0.09f, false);
            damaged = new Animation(Content.Load<Texture2D>("Textures/Hero/damaged"), 40, 0.0001f, false);
            death = new Animation(Content.Load<Texture2D>("Textures/Hero/death"), 65, 0.6f, false);
            run = new Animation(Content.Load<Texture2D>("Textures/Hero/run"), 47, 0.15f, true);
            jumpAttack = new Animation(Content.Load<Texture2D>("Textures/Hero/jumpAttack"), 55, 0.08f, false);
            sit = new Animation(Content.Load<Texture2D>("Textures/Hero/sit"), 32, 0.1f, true);
            sitAttack = new Animation(Content.Load<Texture2D>("Textures/Hero/sitAttack"), 64, 0.09f, false);
            sword = Content.Load<SoundEffect>("Sounds/sword");
            armorDamage = Content.Load<SoundEffect>("Sounds/armorDamage");
            //   heal = new Animation(Content.Load<Texture2D>("Textures/Hero/sitAttack"), 64, 0.09f, false);

            armor = new Animation(Content.Load<Texture2D>("armor2"), 64, 0.1f, true);

            timerForAttack = 0f;
            timerForSit = 0f;

        }

        public Animation CurrentAmimation()
        {
            if (hasDamaged)
                return damaged;
            else if (hasJumped && hasAttack)
                return jumpAttack;
            else if (hasJumped)
                return jump;
            else if (isSit && hasAttack)
                return sitAttack;
            else if (hasAttack)
            {
                return attack;
            }
            else if (velocity.Y > 8 && !hasJumped)
                return fall;
            else if (velocity.X != 0)
            {
                //isSit = false;
                wasSit = false;
                return run;
            }
            else if (isSit)
                return sit;
            else
                return idle;
        }

        public Rectangle CurrentRectangle()
        {

            if (hasJumped) 
                return  new Rectangle((int)Position.X, (int)Position.Y + idle.FrameHeight / 7, idle.FrameWidth, idle.FrameHeight);
            else if(isSit)
                return new Rectangle((int)Position.X, (int)Position.Y , idle.FrameWidth, sit.FrameHeight);
            else return new Rectangle((int)Position.X, (int)Position.Y, idle.FrameWidth, idle.FrameHeight );
        }

        public bool PlayAnimation()
        {
            if (armored)
            {
                effectSprite.PlayAnimation(armor);
                return true;
            }
            
            else return false;

        }

        public void PlaySounds()
        {
            if (hasAttack && attacksound)
            {
                attacksound = false;
                sword.Play();

            }
            else if (!hasAttack) attacksound = true;
        }

        void ArmorUpdate(GameTime gameTime)
        {

            if (GetArmored)
                timerForArmor =(float)gameTime.ElapsedGameTime.Milliseconds * 32;

            if (timerForArmor > 0)
            {
                armored = true;
                ArmorHud.Points = timerForArmor;
                timerForArmor--;
                GetArmored = false;
            }
            if (timerForArmor == 0)
                armored = false;

            
        }

        public void Update(GameTime gameTime)
        {
            position += velocity;
            rectangle = CurrentRectangle();

            Input(gameTime);
            PlaySounds();
            ArmorUpdate(gameTime);

            if (velocity.Y < 9)
                velocity.Y += 0.4f;




            if (LookLeftSide && timerForAttack > 0)
                damageArea = new Rectangle(CurrentRectangle().Left - CurrentRectangle().Width, rectangle.Y, CurrentRectangle().Width*2, CurrentRectangle().Height / 2);
            else if(!LookLeftSide && timerForAttack > 0)
                damageArea = new Rectangle(CurrentRectangle().Left , rectangle.Y, CurrentRectangle().Width*2, CurrentRectangle().Height / 2);

            animationSprite.PlayAnimation(CurrentAmimation());
        }

        public void Input(GameTime gameTime)
        {
            
            state = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.D) && !isSit && timerForSit <=0 )
            {
                LookLeftSide = false;
                velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
                wasSit = true;

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A) && !isSit && timerForSit <= 0)
            {
                LookLeftSide = true;
                velocity.X = -(float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
                wasSit = true;
            }
            else velocity.X = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                velocity.Y = -(float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && velocity.X == 0)
            {
                isSit = true;
                wasSit = true;
                timerForSit = 1.5f;

            }
            else
            {
                timerForSit--;
                isSit = false;
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                position = new Vector2(6000, 1084);

            if (Keyboard.GetState().IsKeyDown(Keys.J) && timerForAttack == 0f)
            {
                if (CanAttack)
                {
                    hasAttack = true;
                    timerForAttack = 15f;
                    CanAttack = false;
                }
                else hasAttack = false;

                if (timerForAttack > 0)
                    timerForAttack--;
            }
            else
            {

                if (timerForAttack > 0)
                    timerForAttack--;
                else if (timerForAttack < 1f)
                {
                    hasAttack = false;
                    if (Keyboard.GetState().IsKeyUp(Keys.J))
                        CanAttack = true;
                }
            }


            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                rectangle.Y -= 25;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !hasJumped && CanJump && !isSit)
            {
                CanJump = false;
                position.Y -= 5f;
                velocity.Y = -9f;
                hasJumped = true;
            }
            else
            {
                CanJump = true;
            }
            oldState = state;
        }

        public Texture2D GetTexture()
        {
            return CurrentAmimation().Texture;
        }

        public void mouseDebug(Vector2 mousePos)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                position = mousePos;
            }
        }
        public void Collision(Block block, int xOffset, int yOffset)
        {
            if (!block.Passable && !block.Damaged)
            {
                if (rectangle.Intersects(block.Rectangle) && wasSit && !hasJumped )
                {
                    rectangle.Y -= 2;
                    position.X -= velocity.X;
                }

                if (rectangle.TouchTopOf(block.Rectangle))
                {

                    rectangle.Y = block.Rectangle.Y - rectangle.Height;
                    position.Y = block.Rectangle.Y - rectangle.Height;

                    if (CanJump)
                        velocity.Y = 0f;
                    hasJumped = false;
                }
                if (rectangle.TouchLeftOf(block.Rectangle))
                {
                    position.X = block.Rectangle.X - rectangle.Width - 2;
                }
                if (rectangle.TouchRightOf(block.Rectangle))
                {
                    position.X = block.Rectangle.X + block.Rectangle.Width + 2;
                }
                if (rectangle.TouchBottomOf(block.Rectangle))
                {
                    velocity.Y = 1f;
                    if (velocity.Y <= 2f)
                        velocity.Y += 0.2f;
                }

                if (position.X < 0)
                    position.X = 0;
                if (position.X+rectangle.Width >= xOffset)
                    position.X = xOffset-rectangle.Width;
                if (position.Y < 0)
                    position.Y = 0;
                if (position.Y > yOffset - rectangle.Width)
                    position.Y = yOffset;      
            }
            else if (block.Damaged)
            {
               var temp = new Rectangle(block.Rectangle.X + block.Rectangle.Width / 4, block.Rectangle.Y + block.Rectangle.Height / 4, block.Rectangle.Width / 2, block.Rectangle.Height / 2);
                if (rectangle.Intersects(temp))
                {
                    HasDamaged = true;
                    timerForDamage = 5f;
                    position = checkpoint;
                    health.Points--;
                    armorHud.Points = 0;
                    timerForArmor = 0;
                }
                else if (timerForDamage <=0) hasDamaged = false;
                if (timerForDamage >= 0)
                    timerForDamage-=0.1f;
            }
        }

        public void Collision(Enemy enemy , GameTime gameTime)
        {

            if (armored && rectangle.Intersects(enemy.Rectangle))
            {
               armorDamage.Play();
                enemy.HealthPoint-=enemy.HealthPoint;

            }
            else if (rectangle.Intersects(enemy.Rectangle))
            {
                HasDamaged = true;
                position = checkpoint;
                timerForDamage = 1f;
                health.Points--;

            }
            else if (timerForDamage <= 0) hasDamaged = false;
            if (timerForDamage >= 0)
                timerForDamage -= 0.1f;

            if (hasAttack && enemy.timerForDamage <= 0f && enemy.Rectangle.Intersects(damageArea) && enemy.CanDamaged)
            {
                enemy.DamageSound.Play();
                enemy.HealthPoint--;
                enemy.CanDamaged = false;
                enemy.timerForDamage = 20f;
            }

            if(!enemy.CanDamaged)
            {
                enemy.timerForDamage--;
                if (enemy.timerForDamage <= 0f)
                    enemy.CanDamaged = true;
            }

        }

        public void Collision (CheckPoint checkPoint)
        {
            Rectangle rect = new Rectangle(checkPoint.Rectangle.Left + 10, checkPoint.Rectangle.Y, checkPoint.Rectangle.Width / 2, checkPoint.Rectangle.Height);
            if (rectangle.Intersects(rect) && !checkPoint.IsUsed)
            {
                checkPoint.IsUsed = true;
                checkpoint = checkPoint.Position;
                checkPoint.PlaySound();
            }

        }


        public void Draw(GameTime gameTime , SpriteBatch spriteBatch)
        {
            SpriteEffects flip = SpriteEffects.None;
            if (velocity.X > 0 || !LookLeftSide)
                flip = SpriteEffects.None;
            else if (velocity.X < 0 || LookLeftSide)
                flip = SpriteEffects.FlipHorizontally;
            animationSprite.Draw(gameTime, spriteBatch, Position, flip);
            if (PlayAnimation())
                effectSprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None);
            // spriteBatch.Draw(texture, temp, Color.Green);
            if (hasAttack)
          //  spriteBatch.Draw(texture, damageArea, Color.White);
            if(hasDamaged)
            spriteBatch.Draw(damaged.Texture, rectangle, Color.White);
        }
    }
}