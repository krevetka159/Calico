using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class SimulationGame : Game
    {
        public SimulationGame(GameBoard gb, GamePiece[] o, Scoring s, Bag b)
        {
            bag = new Bag(b);
            for (int i = 0; i < o.Length; i++)
            {
                Opts[i] = o[i];
            }
            for (int i = o.Length; i < 3; i++)
            {
                Opts[i] = bag.Next();
            }

            scoring = s;

            agent = UseAgent(7);
            agent.Board = gb;


        }

        public int Game()
        {
            for (int i = 0; i < agent.Board.EmptySpotsCount; i++)
            {
                MakeMove(agent);
            }

            return agent.Board.ScoreCounter.Score;
        }
    }
}
