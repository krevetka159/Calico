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

            for (int i = 0; i < 22; i++)
            {
                GetCommand();
                // make move, update points
                PrintState();
                
            }

            PrintStats();
        }

        private void PrintState()
        {
            // dílky volné k použití
            // přehled kočiček
            // skore
            // board
        }

        private void GetCommand()
        {
            //neměl by být void
            // řeknu si o command
            // checknu jestli legit
        }

        private void PrintStats()
        {
            // finální výsledky
        }
    }
}
