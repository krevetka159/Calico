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


        public GameBoard()
        {
            random = new Random();

            int randId = random.Next(0, 4);
            // TODO generování okrajů boardu podle id

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

        public int AddPiece(GamePiece piece, int x, int y)
        {
            if (board[x][y].Color == 0 && board[x][y].Pattern == 0) //can only add to an empty space
            {
                board[x][y] = piece;
                return 0;
            }
            return 1;
        }

        public void PrintBoard()
        {
            // funkce pro print dílku?
            // všechny switche tu?

        }
    }
}
