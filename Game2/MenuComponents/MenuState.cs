using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.MenuComponents
{
    public static class MenuState
    {
        public enum GameState
        {
            NewGame,
            Playing,
            Options,
            LevelSelect,
            LevelComplete,
            MainMenu,
            GameOver,
            Pause,
            Exit,
        }
    }
}
