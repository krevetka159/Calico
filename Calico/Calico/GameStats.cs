using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class GameStats
    {
        public int AgentType;
        public int Score;
        public int Buttons;
        public (int, int, int) Cats;
        public int TaskPoints;

        public GameStats(int score, int buttons, (int,int,int) cats)
        {
            Score = score;
            Buttons = buttons;
            Cats = cats;
        }
    }

    public class AverageGameStats
    {
        public int AgentType;
        public double AvgScore;
        public double AvgButtons;
        public (double, double, double) AvgCats;
        public double AvgTaskPoints;
        public int BestScore;
        public int LowestScore;

        public AverageGameStats(int agentType, double avgScore, double avgButtons, (double, double, double) avgCats, int bestScore, int lowestScore)
        {
            AgentType = agentType;
            AvgScore = avgScore;
            AvgButtons = avgButtons;
            AvgCats = avgCats;
            BestScore = bestScore;
            LowestScore = lowestScore;
        }
    }
}
