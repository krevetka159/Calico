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


        public GameBoard()
        {
            board = new List<List<GamePiece>>();
        }
    }
}
