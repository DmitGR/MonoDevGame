using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.GameClasses
{
    class Camera2D
    {

        Vector2 position;
        Matrix viewMatrix;
        public Matrix ViewMatrix { get;}
        public int ScreenWidth { get { return GraphicsDeviceManager.DefaultBackBufferWidth; } }
        public int ScreenHeight { get { return GraphicsDeviceManager.DefaultBackBufferHeight; } }

        public void Update(Vector2 playerPos)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                position.Y -= 4f;
          else  if (Keyboard.GetState().IsKeyDown(Keys.Down))
                position.Y += 4f;
           else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                position.X -= 4f;
           else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                position.X += 4f;
            else
            position.X = playerPos.X - (ScreenWidth / 2);
            position.Y = playerPos.Y - (ScreenHeight / 2);

            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;



            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }



























        //public Matrix transform;
        //Viewport view;
        //Vector2 center;

        //public Camera2D(Viewport view)
        //{
        //    this.view = view;
        //}

        //public void Update(GameTime gameTime,Hero player)
        //{
        //    center = new Vector2 (player.Position.X + (player.BoundingRectangle.Width/2)-400,0);
        //    transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
        //        Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        //}
    }
}
