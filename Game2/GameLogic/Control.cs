using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RGR.GameClasses;

namespace RGR
{
    static class Control
    {
        static KeyboardState state, oldState;

        public static void Input(Player player, GameTime gameTime)
        {
            state = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.D) && state == oldState)
                player.WalkRight(gameTime);

            else if (state.IsKeyDown(Keys.A))
                player.WalkLeft(gameTime);

            else player.Stand();


            if (Keyboard.GetState().IsKeyDown(Keys.S) )
                player.Sit();
            else player.StandUp();

            if (Keyboard.GetState().IsKeyDown(Keys.J) && state != oldState)
                player.Attack();
            else player.NonAttack();


            if (Keyboard.GetState().IsKeyDown(Keys.Space) && oldState == state)
            {
                player.Jump();
                if (Keyboard.GetState().IsKeyDown(Keys.J) && state != oldState)
                    player.Attack();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.K) && oldState != state)
                player.AcivatePotion();

                oldState = state;
        }
    }
}
