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
            PrintState();

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
            Console.WriteLine();
            Console.Write("Dílky k použití: ");
            for (int i = 0;i < 3;i++)
            {
                Console.Write($" {i + 1}: |{Opts[i].Print}| ");
            }

            Console.WriteLine();

            // přehled kočiček
            
            Console.WriteLine("Skóre: " + Player.score);
            Player.board.PrintBoard();
            Console.WriteLine();
        }

        private void GetCommand()
        {
            // TODO legit checking
            int next;
            int row;
            int col;
            
            Console.Write("Choose gamepiece to add: ");
            next = Convert.ToInt32(Console.ReadLine());
            
            Console.Write("Choose row: ");
            row = Convert.ToInt32(Console.ReadLine());
            Console.Write("Choose column: ");
            col = Convert.ToInt32(Console.ReadLine());

            while (!Player.board.IsEmpty(row - 1, col - 1)) 
            {
                Console.WriteLine("Na vybranou pozici dílek nelze umístit, vyberte prosím jinou pozici.");

                Console.Write("Choose row: ");
                row = Convert.ToInt32(Console.ReadLine());
                Console.Write("Choose column: ");
                col = Convert.ToInt32(Console.ReadLine());

            }

            Player.MakeMove(Opts[next - 1], row-1, col-1);

            Opts[next - 1] = Bag.Next();
        }

        private void PrintStats()
        {
            // finální výsledky
            Console.WriteLine("Finální skóre: " + Player.score);
        }
    }
}
