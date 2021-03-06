﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR.GameClasses
{
    public class Camera
    {
        Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
        }

        Vector2 center;
        public Vector2 Center { get { return center; } }
        Viewport viewport;

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(Vector2 position, int xOffset, int yOffset)
        {

            if (position.X < viewport.Width / 2 )
                center.X = viewport.Width / 2 ;
            else if (position.X > xOffset - (viewport.Width / 2))
                center.X = xOffset - (viewport.Width / 2);
            else center.X = position.X;

            if (position.Y < viewport.Height / 2)
                center.Y = viewport.Height / 2;
            else if (position.Y > yOffset - (viewport.Height / 2))
                center.Y = yOffset - (viewport.Height / 2);
            else center.Y = position.Y;



            transform = Matrix.CreateTranslation(new Vector3(-center.X + (viewport.Width / 2),
                                                             -center.Y + (viewport.Height / 2), 0));

        }
    }
}
