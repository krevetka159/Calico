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


// ----------------------------------------------- GET COMMAND ------------------------------------------------------------


        private void GetCommand()
        {
   
            int next = ChooseGamePiece() - 1;
            
            (int row, int col) = GetPosition();

            Player.MakeMove(Opts[next], row-1, col-1);

            Opts[next - 1] = Bag.Next();
        }

        private static int ChooseGamePiece()
        {
            int gamepiece;
            while (true)
            {
                try
                {
                    Console.Write("Choose gamepiece to add: ");
                    gamepiece = Convert.ToInt32(Console.ReadLine());
                    switch (gamepiece)
                    {
                        case 1: return 1;
                        case 2: return 2;
                        case 3: return 3;
                        default:
                            {
                                Console.WriteLine("Choose of of the three pieces (1/2/3).");
                                break;
                            }
                    }
                }
                catch
                {
                    Console.WriteLine("Choose of of the three pieces (1/2/3).");
                }
            }
        }


        private (int, int) GetPosition()
        {
            int row;
            int col;
            while (true)
            {
                while (true)
                {
                    try
                    {
                        Console.Write("Choose row: ");
                        row = Convert.ToInt32(Console.ReadLine());

                        if (1 <= row && row <= 7)
                        {
                            break;
                        }
                        Console.WriteLine("Row must be an integer between 1 and 7");
                    }
                    catch
                    {
                        Console.WriteLine("Row must be an integer");
                    }
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Choose column: ");
                        col = Convert.ToInt32(Console.ReadLine());

                        if (1 <= col && col <= 7)
                        {
                            break;
                        }
                        Console.WriteLine("Column must be an integer between 1 and 7");
                    }
                    catch
                    {
                        Console.WriteLine("Column must be an integer");
                    }
                }
                
                if (Player.board.IsEmpty(row - 1, col - 1))
                {
                    return (row , col);
                }
            }

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

            Console.WriteLine("Skóre: " + Player.score);
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
