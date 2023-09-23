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
            
            for (int i = 0; i < 3; i++)
            {
                Opts[i] = Bag.Next();
            }
            

            if (numOfPlayers == 1) { SinglePlayer(); }
            else { MultiplePlayers(numOfPlayers); }
        }


        public void SinglePlayer()
        {
            Player = new Player();

            //print empty
            PrintState();

            for (int i = 0; i < 22; i++)
            {
                MakeMove();
                // update points
                PrintState();
                
            }

            PrintStats();
        }

        public void MultiplePlayers(int numOfPlayers) 
        {
        }


// ----------------------------------------------- GET COMMAND ------------------------------------------------------------


        private void MakeMove()
        {
   
            int next = Player.ChooseGamePiece(Opts) - 1;
            
            (int row, int col) = Player.ChoosePosition();

            Player.MakeMove(Opts[next], row-1, col-1);

            Opts[next] = Bag.Next();
        }



// ----------------------------------------------- PRINT ------------------------------------------------------------------

        private void PrintState()
        {
            Console.WriteLine();
            Console.Write("Dílky k použití: ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($" {i + 1}: |{Opts[i].Print}| ");
            }

            Console.WriteLine();

            // přehled kočiček

            Console.WriteLine("Skóre: " + Player.board._score);
            Player.board.PrintBoard();
            Console.WriteLine();
        }



        private void PrintStats()
        {
            // finální výsledky
            Console.WriteLine("Finální skóre: " + Player.score);
        }
    }
}
