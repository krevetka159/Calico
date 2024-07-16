using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class Player
    {
        public GameBoard Board { get; set; }

        public int[] TaskIds;

        public Player(Scoring scoring)
        {
            Board = new GameBoard(scoring);
            TaskIds = new int[] { 0, 0, 0 };
        }

        public Player(Scoring scoring, int boardId)
        {
            Board = new GameBoard(scoring, boardId);
            TaskIds = new int[] { 0, 0, 0 };
        }

        /// <summary>
        /// Add patchtile to gameboard
        /// </summary>
        /// <param name="gamePiece"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MakeMove(GamePiece gamePiece, int row, int col)
        {
            Board.AddPiece(gamePiece, row, col);
            
        }

        /// <summary>
        /// Let the player choose patchtile to add (gets instructions from console)
        /// </summary>
        /// <param name="Opts"></param>
        /// <returns></returns>
        private int ChooseGamePiece(GamePiece[] Opts)
        {
            int gamepiece;
            while (true)
            {
                try
                {
                    Console.Write(" Vyberte dílek z nabídky (1/2/3): ");
                    gamepiece = Convert.ToInt32(Console.ReadLine());
                    switch (gamepiece)
                    {
                        case 1: return 0;
                        case 2: return 1;
                        case 3: return 2;
                        default:
                            {
                                Console.WriteLine(" Neplatné číslo");
                                break;
                            }
                    }
                }
                catch
                {
                    Console.WriteLine(" Neznámý příkaz");
                }
            }
        }


        /// <summary>
        /// Let the player choose position to add new patchtile to (gets instructions from console)
        /// </summary>
        /// <param name="Opts"></param>
        /// <returns></returns>
        private (int, int) ChoosePosition()
        {
            int row;
            int col;
            while (true)
            {
                while (true)
                {
                    try
                    {
                        Console.Write(" Vyberte řádek: ");
                        row = Convert.ToInt32(Console.ReadLine());

                        if (1 <= row && row <= Board.Size)
                        {
                            break;
                        }
                        Console.WriteLine(" Herní deska obsahuje pouze řádky 1 až 7");
                    }
                    catch
                    {
                        Console.WriteLine(" Neznámý příkaz, zadejte celé číslo");
                    }
                }

                while (true)
                {
                    try
                    {
                        Console.Write(" Vyberte sloupec: ");
                        col = Convert.ToInt32(Console.ReadLine());

                        if (1 <= col && col <= Board.Size)
                        {
                            break;
                        }
                        Console.WriteLine(" Herní deska obsahuje pouze sloupce 1 až 7");
                    }
                    catch
                    {
                        Console.WriteLine(" Neznámý příkaz, zadejte celé číslo");
                    }
                }

                if (Board.IsEmpty(row - 1, col - 1))
                {
                    return (row - 1, col - 1);
                }
                else
                {
                    Console.WriteLine(" Tato pozice je již obsazená, vyberte jinou");
                }
            }

        }
        /// <summary>
        /// Let the player choose his next move (gets input from console)
        /// </summary>
        /// <param name="Opts"></param>
        /// <returns></returns>
        public virtual (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            return (ChooseGamePiece(Opts), ChoosePosition());
        }


        /// <summary>
        /// Choose one task to add on exact position on gameboard (gets instructions from console)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int ChooseTaskPiece(int index)
        {
            bool taskAvailable;
            int gamepiece;
            while (true)
            {
                taskAvailable = true;
                try
                {
                    Console.Write(" Vyberte úkol pro řádek " + (index + 3) + ": ");
                    gamepiece = Convert.ToInt32(Console.ReadLine());

                    for (int i = 0; i < index; i++)
                    {
                        if (TaskIds[i] == gamepiece) taskAvailable = false;
                    }

                    if (taskAvailable)
                    {
                        if ( 1 <= gamepiece && gamepiece <= 6)
                        {
                            TaskIds[index] = gamepiece;
                            return gamepiece;
                        }
                        else
                        {
                            Console.WriteLine(" Vyberte úkol 1-6. ");
                        }
                    }
                    else
                    {
                        Console.WriteLine($" Úkol {gamepiece} již byl zvolen, vyberte jiný. ");
                    }
                   
                }
                catch
                {
                    Console.WriteLine(" Neznámý příkaz, zadejte celé číslo ");
                }
            }
        }

        /// <summary>
        /// Choose all tasks for a game
        /// </summary>
        public virtual void ChooseTaskPieces()
        {
            Board.AddTaskPiece(ChooseTaskPiece(0), 0);
            Board.AddTaskPiece(ChooseTaskPiece(1), 1);
            Board.AddTaskPiece(ChooseTaskPiece(2), 2);
        }
    }
}
