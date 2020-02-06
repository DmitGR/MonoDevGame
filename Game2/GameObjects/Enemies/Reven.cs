﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RGR.GameClasses;

namespace RGR.GameObjects.Enemies
{
    class Raven : Enemy
    {
        public Raven(Vector2 position)
        {
            IsAlive = true;
            CanFly = true;
            this.position = position;
            speed = 2f;
            healthPoint = 2;
            Score = 100;
        }

        public override void Load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("spriteFont");
            texture = new Animation(Content.Load<Texture2D>("Textures/Enemys/raven"), 32, 0.14f, true);
            damageSound = Content.Load<SoundEffect>("Sounds/miscDamage1");

            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.FrameWidth, texture.FrameHeight);
        }
    }
}