using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR.MenuComponents
{
    public static class MenuState
    {
        public enum GameState
        {
            NewGame,
            Playing,
            Options,
            LevelComplete,
            MainMenu,
            GameOver,
            Pause,
            Exit,
        }
    }
}
