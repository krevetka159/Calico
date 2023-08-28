using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class Game
    {
        private Bag Bag;
        private Player Player;
        //private (GamePiece, GamePiece, GamePiece) PossiblePieces;

        public Game(int numOfPlayers) 
        { 
            Bag = new Bag();
        }


        public void SinglePlay()
        {
            Player = new Player();

            //print empty

            for (int i = 0; i < 25; i++)
            {
                // ask for command
                // make move, update points
                // print board and points
                
            }

            // print stats
        }

    }
}
