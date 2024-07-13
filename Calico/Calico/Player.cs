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
                    Console.Write(" Choose gamepiece to add: ");
                    gamepiece = Convert.ToInt32(Console.ReadLine());
                    switch (gamepiece)
                    {
                        case 1: return 0;
                        case 2: return 1;
                        case 3: return 2;
                        default:
                            {
                                Console.WriteLine(" Choose of of the three pieces (1/2/3).");
                                break;
                            }
                    }
                }
                catch
                {
                    Console.WriteLine(" Choose of of the three pieces (1/2/3).");
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
                        Console.Write(" Choose row: ");
                        row = Convert.ToInt32(Console.ReadLine());

                        if (1 <= row && row <= Board.Size)
                        {
                            break;
                        }
                        Console.WriteLine(" Row must be an integer between 1 and 7");
                    }
                    catch
                    {
                        Console.WriteLine(" Row must be an integer");
                    }
                }

                while (true)
                {
                    try
                    {
                        Console.Write(" Choose column: ");
                        col = Convert.ToInt32(Console.ReadLine());

                        if (1 <= col && col <= Board.Size)
                        {
                            break;
                        }
                        Console.WriteLine(" Column must be an integer between 1 and 7");
                    }
                    catch
                    {
                        Console.WriteLine(" Column must be an integer");
                    }
                }

                if (Board.IsEmpty(row - 1, col - 1))
                {
                    return (row - 1, col - 1);
                }
                else
                {
                    Console.WriteLine(" This position is occupied, choose another");
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

        private int ChooseTaskPiece(int index)
        {
            bool taskAvailable;
            int gamepiece;
            while (true)
            {
                taskAvailable = true;
                try
                {
                    Console.Write(" Choose TaskPiece for row " + (index + 3) + ": ");
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
                            Console.WriteLine(" Choose one of the task pieces 1-6. ");
                        }
                    }
                    else
                    {
                        Console.WriteLine($" Task {gamepiece} was already selected, please choose different one. ");
                    }
                   
                }
                catch
                {
                    Console.WriteLine(" Choose one of the task pieces 1-6. ");
                }
            }
        }

        public virtual void ChooseTaskPieces()
        {
            Board.AddTaskPiece(ChooseTaskPiece(0), 0);
            Board.AddTaskPiece(ChooseTaskPiece(1), 1);
            Board.AddTaskPiece(ChooseTaskPiece(2), 2);
        }


    }
}
