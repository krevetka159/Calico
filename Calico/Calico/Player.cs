using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class Player
    {
        //private int id;
        public GameBoard Board { get; private set; }

        public Player(Scoring scoring)
        {
            Board = new GameBoard(scoring);
        }

        public void MakeMove(GamePiece gamePiece, int x, int y)
        {
            Board.AddPiece(gamePiece, x, y);
            
        }

        public virtual int ChooseGamePiece(GamePiece[] Opts)
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
                        case 1: return 1;
                        case 2: return 2;
                        case 3: return 3;
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

        public virtual (int, int) ChoosePosition()
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
        public virtual (int, int) ChoosePosition(GamePiece gp)
        {
            return (0, 0);
        }

        public virtual (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            return (ChooseGamePiece(Opts), ChoosePosition());
        }

    }
}
