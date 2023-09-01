using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class GameBoard
    {
        private List<List<GamePiece>> board;
        private Random random;
        private GamePiece empty = new GamePiece(0, 0);
        private GamePiece blocked = new GamePiece(-1, -1);
        private int size = 7;


        public GameBoard()
        {
            random = new Random();

            int randId = random.Next(0, 4);
            // generování okrajů boardu podle id - 4 možnosti

            switch (randId)
            {
                case 0:
                    board = new List<List<GamePiece>>() { 
                        new List<GamePiece>() { new GamePiece(6, 5), new GamePiece(1, 2), new GamePiece(3, 5), new GamePiece(6, 3), new GamePiece(5, 6), new GamePiece(1, 4), new GamePiece(2, 2) },
                        new List<GamePiece>() { new GamePiece(2, 4), empty, empty, empty, empty, empty, new GamePiece(4, 1) },
                        new List<GamePiece>() { new GamePiece(4, 6), empty, empty, blocked, empty, empty, new GamePiece(5, 5) },
                        new List<GamePiece>() { new GamePiece(3, 3), empty, empty, empty, blocked, empty, new GamePiece(1, 3) },
                        new List<GamePiece>() { new GamePiece(5, 2), empty, blocked, empty, empty, empty, new GamePiece(2, 6) },
                        new List<GamePiece>() { new GamePiece(1, 1), empty, empty, empty, empty, empty, new GamePiece(4, 4) },
                        new List<GamePiece>() { new GamePiece(5, 4), new GamePiece(3, 6), new GamePiece(6, 4), new GamePiece(4, 3), new GamePiece(2, 5), new GamePiece(6, 1), new GamePiece(3, 2) }
                    };
                    break;
                case 1:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(4, 3), new GamePiece(1, 5), new GamePiece(2, 3), new GamePiece(4, 2), new GamePiece(5, 6), new GamePiece(1, 1), new GamePiece(3, 5) },
                        new List<GamePiece>() { new GamePiece(5, 1), empty, empty, empty, empty, empty, new GamePiece(6, 4) },
                        new List<GamePiece>() { new GamePiece(6, 6), empty, empty, blocked, empty, empty, new GamePiece(5, 3) },
                        new List<GamePiece>() { new GamePiece(3, 2), empty, empty, empty, blocked, empty, new GamePiece(1, 2) },
                        new List<GamePiece>() { new GamePiece(1, 4), empty, blocked, empty, empty, empty, new GamePiece(3, 6) },
                        new List<GamePiece>() { new GamePiece(5, 2), empty, empty, empty, empty, empty, new GamePiece(6, 1) },
                        new List<GamePiece>() { new GamePiece(2, 2), new GamePiece(4, 1), new GamePiece(2, 6), new GamePiece(5, 2), new GamePiece(3, 3), new GamePiece(4, 4), new GamePiece(2, 5) }
                    };
                    break;
                case 2:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(1, 4), new GamePiece(5, 1), new GamePiece(6, 4), new GamePiece(1, 3), new GamePiece(3, 2), new GamePiece(5, 6), new GamePiece(2, 1) },
                        new List<GamePiece>() { new GamePiece(3, 6), empty, empty, empty, empty, empty, new GamePiece(4, 5) },
                        new List<GamePiece>() { new GamePiece(4, 2), empty, empty, blocked, empty, empty, new GamePiece(3, 4) },
                        new List<GamePiece>() { new GamePiece(2, 3), empty, empty, empty, blocked, empty, new GamePiece(6, 3) },
                        new List<GamePiece>() { new GamePiece(6, 5), empty, blocked, empty, empty, empty, new GamePiece(2, 2) },
                        new List<GamePiece>() { new GamePiece(3, 1), empty, empty, empty, empty, empty, new GamePiece(4, 6) },
                        new List<GamePiece>() { new GamePiece(5, 3), new GamePiece(1, 6), new GamePiece(5, 2), new GamePiece(4, 3), new GamePiece(2, 4), new GamePiece(1, 6), new GamePiece(5, 1) }
                    };
                    break;
                case 3:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(2, 6), new GamePiece(4, 4), new GamePiece(1, 2), new GamePiece(5, 1), new GamePiece(4, 5), new GamePiece(2, 3), new GamePiece(6, 6) },
                        new List<GamePiece>() { new GamePiece(6, 3), empty, empty, empty, empty, empty, new GamePiece(3, 4) },
                        new List<GamePiece>() { new GamePiece(5, 5), empty, empty, blocked, empty, empty, new GamePiece(4, 2) },
                        new List<GamePiece>() { new GamePiece(1, 1), empty, empty, empty, blocked, empty, new GamePiece(2, 1) },
                        new List<GamePiece>() { new GamePiece(2, 4), empty, blocked, empty, empty, empty, new GamePiece(6, 5) },
                        new List<GamePiece>() { new GamePiece(4, 6), empty, empty, empty, empty, empty, new GamePiece(3, 3) },
                        new List<GamePiece>() { new GamePiece(3, 5), new GamePiece(5, 3), new GamePiece(1, 5), new GamePiece(3, 1), new GamePiece(6, 2), new GamePiece(5, 4), new GamePiece(1, 6) }
                    };
                    break;
                default:
                    board = new List<List<GamePiece>>();
                    break;
            }

        }

        public void AddPiece(GamePiece piece, int x, int y)
        {
                board[x][y] = piece;   
            
            
        }

        public void PrintBoard()
        {
            // funkce pro print dílku?
            // všechny switche tu?
            Console.WriteLine("      1    2    3    4    5    6    7");
            Console.WriteLine("   ------------------------------------");
            for (int i = 0; i < size; i++) 
            {

                Console.Write(i+1);
                if (i % 2 == 0) Console.Write("  ");
                Console.Write(" |");
                for (int j = 0; j < size; j++)
                {
                    GamePiece p = board[i][j];
                    switch (p.Type)
                    {
                        case 0:
                            Console.Write(" -- |");
                            break;
                        case -1:
                            Console.Write(" XX |");
                            break;
                        case 1:
                            Console.Write($" {p.Color}{(char)(64+p.Pattern)} |");
                            break;
                        default:
                            break;
                    }
                }
                Console.Write("\n");
                Console.WriteLine("   ------------------------------------");
            }

        }

        public bool IsEmpty(int x, int y)
        {
            return (board[x][y].Type == 0);
        }
    }
}
