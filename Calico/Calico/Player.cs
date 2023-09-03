using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class Player

        // TODO unionfind na knoflíky a kočičky
    {
        //private int id;
        public GameBoard board;
        public int score;


        public Player()
        {
            board = new GameBoard();
            score = 0;
        }

        public void MakeMove(GamePiece gamePiece, int x, int y)
        {
            board.AddPiece(gamePiece, x, y); //TODO vyřešit ty exit kódy
            //spočítat body
            
        }
    }
}
