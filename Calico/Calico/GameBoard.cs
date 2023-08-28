using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class GameBoard
    {
        private List<List<GamePiece>> board;
        private Random random;


        public GameBoard()
        {
            board = new List<List<GamePiece>>();
            random = new Random();

            int randId = random.Next(0, 4);
            // TODO generování okrajů boardu podle id

        }

        public void PrintBoard()
        {

        }
    }
}
