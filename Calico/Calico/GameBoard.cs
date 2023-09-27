using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Calico
{
    public class GameBoard
    {
        public List<List<GamePiece>> board;
        private Random random;
        private static GamePiece empty = new GamePiece(Type.Empty);
        private static GamePiece blocked = new GamePiece(Type.Blocked);
        public int size = 7;

        public ScoreCounter _scoreCounter;

// ----------------------------------------------- INIT ------------------------------------------------------------
        public GameBoard(Scoring scoring)
        {
            random = new Random();

            _scoreCounter = new ScoreCounter(scoring);

            int randId = random.Next(0, 4);
            // generování okrajů boardu podle id - 4 možnosti
            switch (randId)
            {
                case 0:
                    board = new List<List<GamePiece>>() { 
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Vines), new GamePiece(Color.Yellow, Pattern.Stripes), new GamePiece(Color.Cyan, Pattern.Vines), new GamePiece(Color.Pink, Pattern.Fern), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Yellow, Pattern.Flowers), new GamePiece(Color.Green, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Flowers), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Quatrefoil), empty, empty, blocked, empty, empty, new GamePiece(Color.Purple, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Fern), empty, empty, empty, blocked, empty, new GamePiece(Color.Yellow, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Stripes), empty, blocked, empty, empty, empty, new GamePiece(Color.Green, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Flowers), new GamePiece(Color.Cyan, Pattern.Quatrefoil), new GamePiece(Color.Pink, Pattern.Flowers), new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Green, Pattern.Vines), new GamePiece(Color.Pink, Pattern.Dots), new GamePiece(Color.Cyan, Pattern.Stripes) }
                    };
                    break;
                case 1:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Vines), new GamePiece(Color.Green, Pattern.Fern), new GamePiece(Color.Blue, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Yellow, Pattern.Dots), new GamePiece(Color.Cyan, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Quatrefoil), empty, empty, blocked, empty, empty, new GamePiece(Color.Purple, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Stripes), empty, empty, empty, blocked, empty, new GamePiece(Color.Yellow, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Flowers), empty, blocked, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Vines), empty, empty, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Stripes), new GamePiece(Color.Blue, Pattern.Dots), new GamePiece(Color.Green, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Stripes), new GamePiece(Color.Cyan, Pattern.Fern), new GamePiece(Color.Blue, Pattern.Flowers), new GamePiece(Color.Green, Pattern.Vines) }
                    };
                    break;
                case 2:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Flowers), new GamePiece(Color.Purple, Pattern.Dots), new GamePiece(Color.Pink, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Fern), new GamePiece(Color.Cyan, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Green, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Quatrefoil), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Stripes), empty, empty, blocked, empty, empty, new GamePiece(Color.Cyan, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Fern), empty, empty, empty, blocked, empty, new GamePiece(Color.Pink, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Vines), empty, blocked, empty, empty, empty, new GamePiece(Color.Green, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Stripes), new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Green, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Dots) }
                    };
                    break;
                case 3:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Quatrefoil), new GamePiece(Color.Blue, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Dots), new GamePiece(Color.Blue, Pattern.Vines), new GamePiece(Color.Green, Pattern.Fern), new GamePiece(Color.Pink, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Fern), empty, empty, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Vines), empty, empty, blocked, empty, empty, new GamePiece(Color.Blue, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Dots), empty, empty, empty, blocked, empty, new GamePiece(Color.Green, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Flowers), empty, blocked, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Quatrefoil), empty, empty, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Vines), new GamePiece(Color.Purple, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Vines), new GamePiece(Color.Cyan, Pattern.Dots), new GamePiece(Color.Pink, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Quatrefoil) }
                    };
                    break;
                default:
                    board = new List<List<GamePiece>>();
                    break;
            }
            CornerUF();

        }
        public void CornerUF()
        {
            for (int row = 0; row < size; row++)
            {
                if (row == 0 || row == size - 1)
                {
                    for (int col = 0; col < size; col++)
                    {
                        _scoreCounter.AddToUF(board[row][col]);

                    }
                }
                else
                {
                    _scoreCounter.AddToUF(board[row][0]);

                    _scoreCounter.AddToUF(board[row][size - 1]);
                }
            }
        }

// ----------------------------------------------- ADD PIECE ------------------------------------------------------------

        public void AddPiece(GamePiece piece, int x, int y)
        {
            board[x][y] = piece;
            _scoreCounter.AddToUF(piece);
            UnionWithNeighbors(piece, x, y);
            
        }

        public void UnionWithNeighbors(GamePiece piece,int row, int col)
        {
            List<(int, int)> neighbors = new List<(int, int)>() { (row - 1, col - 1), (row - 1, col), (row, col - 1), (row, col + 1), (row + 1, col - 1), (row + 1, col) };

            foreach ((int r, int c) in neighbors) 
            {
                if (IsOccupied(r, c))
                {
                    _scoreCounter.EvaluateNew(piece, board[r][c]);

                }
            }
        }

// ----------------------------------------------- CHECK FOR SIMILAR NEIGHBORS ------------------------------------------------------------

        public bool CheckNeighbors(Color color, int row, int col)
        {
            List<(int, int)> neighbors = new List<(int, int)>() { (row - 1, col - 1), (row - 1, col), (row, col - 1), (row, col + 1), (row + 1, col - 1), (row + 1, col) };

            foreach ((int r, int c) in neighbors)
            {
                if (board[r][c].Color == color)
                {
                    return true;
                }
            }

            return false;
        }
        public bool CheckNeighbors(Pattern pattern, int row, int col)
        {
            List<(int, int)> neighbors = new List<(int, int)>() { (row - 1, col - 1), (row - 1, col), (row, col - 1), (row, col + 1), (row + 1, col - 1), (row + 1, col) };

            foreach ((int r, int c) in neighbors)
            {
                if (board[r][c].Pattern == pattern)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckNeighbors(GamePiece gp, int row, int col)
        {
            List<(int, int)> neighbors = new List<(int, int)>() { (row - 1, col - 1), (row - 1, col), (row, col - 1), (row, col + 1), (row + 1, col - 1), (row + 1, col) };

            foreach ((int r, int c) in neighbors)
            {
                if (board[r][c].Color == gp.Color || board[r][c].Pattern == gp.Pattern)
                {
                    return true;
                }
            }

            return false;
        }

// ----------------------------------------------- COUNT SIMILAR NEIGHBORS -----------------------------------------------------

        public int EvaluateNeighborsColor(GamePiece gp, int row, int col)
        {

            int score = 0;
            int count = 0;
            List<(int, int)> neighbors = new List<(int, int)>() { (row - 1, col - 1), (row - 1, col), (row, col - 1), (row, col + 1), (row + 1, col - 1), (row + 1, col) };

            for (int i = 0; i < neighbors.Count; i++)
            {
                
                bool separate = true;

                (int row_i, int col_i) = neighbors[i];

                if (board[row_i][col_i].Color == gp.Color)
                {
                    for (int j = 0; j < i; j++)
                    {
                        (int row_j, int col_j) = neighbors[j];
                        if (_scoreCounter.CheckColorUnion(board[row_i][col_i], board[row_j][col_j]))
                        {
                            separate = false;
                        };
                    }

                    if (separate)
                    {
                        count += _scoreCounter.GetColorCount(board[row_i][col_i]);
                        score += (_scoreCounter.GetColorCount(board[row_i][col_i]) / 3) * 3;
                    }
                }

            }

            int newScore = ((count + 1) / 3) * 3;

            
            if (count == 0) return 0;


            return (newScore - score + 1);
        }
        public int EvaluateNeighborsPattern(Pattern pattern, int row, int col)
        {
            int sum = 0;
            List<(int, int)> neighbors = new List<(int, int)>() { (row - 1, col - 1), (row - 1, col), (row, col - 1), (row, col + 1), (row + 1, col - 1), (row + 1, col) };

            foreach ((int r, int c) in neighbors)
            {
                if (board[r][c].Pattern == pattern)
                {
                    sum++;
                }
            }

            return sum;
        }

        public int EvaluateNeighbors(GamePiece gp, int row, int col)
        {
            int sum = 0;
            List<(int, int)> neighbors = new List<(int, int)>() { (row - 1, col - 1), (row - 1, col), (row, col - 1), (row, col + 1), (row + 1, col - 1), (row + 1, col) };

            foreach ((int r, int c) in neighbors)
            {
                if (board[r][c].Color == gp.Color || board[r][c].Pattern == gp.Pattern)
                {
                    sum++;  
                }
            }

            return sum;
        }


 // ----------------------------------------------- PRINT ------------------------------------------------------------

        public void PrintBoard()
        {
            Console.WriteLine("       1    2    3    4    5    6    7");
            Console.WriteLine("    ------------------------------------");
            for (int i = 0; i < size; i++) 
            {

                Console.Write(" " + (i+1));
                if (i % 2 == 0) Console.Write("  ");
                Console.Write(" |");
                for (int j = 0; j < size; j++)
                {
                    GamePiece p = board[i][j];
                    Console.Write($"{p.Print}|");
                }
                Console.Write("\n");
                Console.WriteLine("    ------------------------------------");
            }

        }

// ----------------------------------------------- CHECK POSITION ------------------------------------------------------------
        public bool IsEmpty(int row, int col)
        {
            return (board[row][col].Type == Type.Empty);
        }
        public bool IsOccupied(int row, int col)
        {
            return (board[row][col].Type == Type.PatchTile);
        }
    }
}
