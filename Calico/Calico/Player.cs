﻿using System;
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

        public int ChooseGamePiece(GamePiece[] Opts)
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

        public (int, int) ChoosePosition()
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

                        if (1 <= row && row <= board.size)
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

                        if (1 <= col && col <= board.size)
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

                if (board.IsEmpty(row - 1, col - 1))
                {
                    return (row, col);
                }
            }

        }
    }
}
