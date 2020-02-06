using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RGR.GameClasses
{
    class Player : GameObject
    {
        #region FieldsAndProps

        AnimationSprite animationSprite , armorSprite, splashSprite;

        Animation run, idle, jump, attack, death, damaged, jumpAttack, sit, sitAttack, fall, armor, splash;
        SoundEffect sword, fly, die, potionActiv, splashAttack;
        public SoundEffect armorDamage, splashDamage;

        public Vector2 velocity;

        public Rectangle Rectangle { get { return rectangle; } }
        Rectangle collisionArea;
        public Rectangle CollisionArea { get { return collisionArea; } private set { collisionArea = value; } }

        bool LookLeftSide;
        public bool hasJumped = false;
        public bool CanJump = true;
        public bool hasAttack = false;

        public bool GetArmored { get; set; }
        public bool armored;

        bool CanAttack = true;
        public float timerForAttack;
        public float timerForSit;
        public float timerForArmor;
        public Rectangle damageArea;
        bool hasDamaged;
        public bool HasDamaged { get { return hasDamaged; } set { hasDamaged = value; } }
        bool isSit;
        public bool wasSit = false;

        HealthBar health;
        public HealthBar Health { get { return health; } set { health = value; } }

        ArmorHud armorHud;
        public ArmorHud ArmorHud { get { return armorHud; } set { armorHud = value; } }

        SplashHud splashHud;
        public SplashHud SplashHud { get { return splashHud; } set { splashHud = value; } }

        public Vector2 Position { get { return position; } }

        public Vector2 checkpoint;
        private bool attacksound = true;
        private bool flysound = true;
        private Vector2 splashPos;
        public bool HasSplash;
        private Vector2 splashStartPos;
        public Rectangle splashRect;
        private bool splashRight;


        const float MaxSplashDistance = 500f;
        const float Gravity = 0.4f;
        const float MaxVelocity = 9f;
        const float sitTime = 1.5f;
        const float FullSplash = 600f;
        const float armorPerSecond = 4f;
        const float MoveForce = 0.33f;
        const float attackTime = 15f;
        const float JumpForce = 9f;
        const int CollisionDist = 2;
        const int CollisionBonds = 3;
        const float drawLayer = 0.3f;

        #endregion

        public Player(Vector2 Start) { checkpoint = position = Start; }

        public override void Load(ContentManager Content)
        {


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
            fly = Content.Load<SoundEffect>("Sounds/fly");
            die = Content.Load<SoundEffect>("Sounds/die");
            armor = new Animation(Content.Load<Texture2D>("Textures/Misc/armor"), 64, 0.1f, true);
            splash = new Animation(Content.Load<Texture2D>("Textures/Misc/splash"), 33, 0.12f, true);
            potionActiv = Content.Load<SoundEffect>("Sounds/potionActivated");
            splashDamage = Content.Load<SoundEffect>("Sounds/splashDamage");
            splashAttack = Content.Load<SoundEffect>("Sounds/splash");

            timerForAttack = 0f;
            timerForSit = 0f;
            splashPos =position;

            Health.Load(Content);
            ArmorHud.Load(Content);
            SplashHud.Load(Content);

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
                return attack;
            else if (velocity.Y > MaxVelocity && !hasJumped)
                return fall;
            else if (velocity.X != 0)
            {
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
            {
                return new Rectangle((int)Position.X, (int)Position.Y /*+ idle.FrameHeight / 7*/, idle.FrameWidth, idle.FrameHeight);
            }
            else if (isSit)
                return new Rectangle((int)Position.X, (int)Position.Y, idle.FrameWidth, sit.FrameHeight);
            else if (velocity.Y > MaxVelocity && !hasJumped)
                return new Rectangle((int)Position.X + idle.FrameWidth/4, (int)Position.Y, idle.FrameWidth/2, idle.FrameHeight);
            else return new Rectangle((int)Position.X, (int)Position.Y, idle.FrameWidth, idle.FrameHeight);
        }

        public bool PlayAnimation()
        {
            if (armored && !HasSplash)
            {
                armorSprite.PlayAnimation(armor);
                return true;
            }

            if (HasSplash)
            {
                splashSprite.PlayAnimation(splash);
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

            if (hasJumped && CanJump && flysound)
            {
                flysound = false;
                fly.Play();
            }
            else if (!hasJumped) flysound = true;

            if (hasDamaged)
                die.Play();
        }

        void ArmorUpdate(GameTime gameTime)
        {

            if (GetArmored)
                timerForArmor = (float)gameTime.ElapsedGameTime.Milliseconds * 32;

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

        #region Control
        public void WalkLeft(GameTime gameTime)
        {
            if (!isSit && timerForSit <= 0)
            {
                LookLeftSide = true;
                velocity.X = -(float)gameTime.ElapsedGameTime.TotalMilliseconds * MoveForce;
                wasSit = true;
            }
        }

        public void WalkRight(GameTime gameTime)
        {
            if (!isSit && timerForSit <= 0)
            {
                LookLeftSide = false;
                velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds * MoveForce;
                wasSit = true;

            }
        }

        public void Jump()
        {
            if (!hasJumped && CanJump && !isSit)
            {
                CanJump = false;
                position.Y -= Gravity;
                velocity.Y = -MaxVelocity;
                hasJumped = true;
            }
            else CanJump = true;
        }

        public void Sit()
        {
            if (velocity.X == 0 && !hasJumped)
            {
                isSit = true;
                wasSit = true;
                timerForSit = sitTime;
            }
        }

        public void StandUp()
        {
            timerForSit--;
            isSit = false;
        }

        public void Attack()
        {
            if (timerForAttack == 0f)
            {
                if (CanAttack)
                {
                    hasAttack = true;
                    timerForAttack = attackTime;
                    CanAttack = false;
                }
                else hasAttack = false;

                if (timerForAttack > 0)
                    timerForAttack--;
            }

            if (!HasSplash && splashHud.IsActiv)
            {
                HasSplash = true;
                splashAttack.Play();
                splashStartPos = splashPos = position;
                if (LookLeftSide)
                    splashRight = false;
                else splashRight = true;
            }
        }

        public void NonAttack()
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

        public void AcivatePotion()
        {
            if (splashHud.IsPicked)
            {
                splashHud.IsPicked = false;
                potionActiv.Play();
                splashHud.Points = FullSplash;
            }
        }

        public void Stand() { velocity.X = 0; }

        public void ApplyGravity()
        {
            if (velocity.Y < MaxVelocity)
                velocity.Y += Gravity;
            if (velocity.Y == 0)
                CanJump = true;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            ApplyGravity();
            position += velocity;
            rectangle = CurrentRectangle();
            collisionArea = new Rectangle(rectangle.X - rectangle.Width, rectangle.Y - rectangle.Height, rectangle.Width * CollisionBonds, rectangle.Height * CollisionBonds);
            PlaySounds();
            ArmorUpdate(gameTime);


            if (HasSplash)
            {
                splashRect = new Rectangle((int)splashPos.X, (int)splashPos.Y, splash.FrameWidth, splash.FrameHeight);
                if (splashRight)
                    splashPos.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
                else
                    splashPos.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            }
            if (splashPos.X >= splashStartPos.X + MaxSplashDistance || splashPos.X <= splashStartPos.X - MaxSplashDistance)
            {
                splashPos = Vector2.Zero;
                HasSplash = false;
            }

            if (LookLeftSide && timerForAttack > 0)
                damageArea = new Rectangle(CurrentRectangle().Left - CurrentRectangle().Width, rectangle.Y, CurrentRectangle().Width * 2, CurrentRectangle().Height / 2);
            else if (!LookLeftSide && timerForAttack > 0)
                damageArea = new Rectangle(CurrentRectangle().Left, rectangle.Y, CurrentRectangle().Width * 2, CurrentRectangle().Height / 2);

            animationSprite.PlayAnimation(CurrentAmimation());
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
                rectangle.X = (int)mousePos.X;
                rectangle.Y = (int)mousePos.Y;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects flip = SpriteEffects.None;
            if (velocity.X > 0 || !LookLeftSide)
                flip = SpriteEffects.None;
            else if (velocity.X < 0 || LookLeftSide)
                flip = SpriteEffects.FlipHorizontally;
            animationSprite.Draw(gameTime, spriteBatch, Position, flip, drawLayer);

            if (PlayAnimation())
            {
                if (HasSplash)
                {
                    if (splashRight)
                        splashSprite.Draw(gameTime, spriteBatch, splashPos, SpriteEffects.None, drawLayer);
                    else if (!splashRight)
                        splashSprite.Draw(gameTime, spriteBatch, splashPos, SpriteEffects.FlipHorizontally, drawLayer);
                }
                if (armored)
                    armorSprite.Draw(gameTime, spriteBatch, Position, flip, drawLayer);
            }
        }
    }
}