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
        private GamePiece[] Opts = new GamePiece[3];
        //private (GamePiece, GamePiece, GamePiece) PossiblePieces;

        public Game(int numOfPlayers) 
        { 
            Bag = new Bag();
            Player = new Player();
            for (int i = 0; i < 3; i++)
            {
                Opts[i] = Bag.Next();
            }
            //for (int i = 0; i < numOfPlayers; i++)
            //{

            //}
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
            // dílky volné k použití -> print v gamepiece fci?
            // přehled kočiček
            // skore
            // board
        }

        private void GetCommand()
        {
            // TODO legit checking
            int next = 0;
            int row = -1;
            int col = -1;
            do
            {
                Console.Write("Choose gamepiece to add: ");
                next = Convert.ToInt32(Console.ReadLine());
                Console.Write("Choose row: ");
                row = Convert.ToInt32(Console.ReadLine());
                Console.Write("Choose column: ");
                col = Convert.ToInt32(Console.ReadLine());

            } while (! Player.board.IsEmpty(row, col));

            // TODO potřebuju mít seznam možných dílků
            Player.MakeMove(Opts[next - 1], row, col);

            Opts[next - 1] = Bag.Next();
        }

        private void PrintStats()
        {
            // finální výsledky
        }
    }
}
