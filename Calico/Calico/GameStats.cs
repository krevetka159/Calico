using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    /// <summary>
    /// Score and other statistics of a game
    /// </summary>
    public class GameStats
    {
        public int Score;
        public int Buttons;
        public (int, int, int) Cats;
        public int[] Tasks;

        public GameStats(int score, int buttons, (int, int, int) cats, int[] tasks)
        {
            Score = score;
            Buttons = buttons;
            Cats = cats;
            Tasks = tasks;
        }
    }
}
