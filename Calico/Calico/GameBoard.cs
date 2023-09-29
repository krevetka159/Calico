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
        public int Size = 7;

        public ScoreCounter ScoreCounter { get; private set; }

// ----------------------------------------------- INIT ------------------------------------------------------------
        public GameBoard(Scoring scoring)
        {
            random = new Random();

            ScoreCounter = new ScoreCounter(scoring);

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
        private void CornerUF()
        {
            //Add corners to unionfind
            for (int row = 0; row < Size; row++)
            {
                if (row == 0 || row == Size - 1)
                {
                    for (int col = 0; col < Size; col++)
                    {
                        ScoreCounter.AddToUF(board[row][col]);

                    }
                }
                else
                {
                    ScoreCounter.AddToUF(board[row][0]);

                    ScoreCounter.AddToUF(board[row][Size - 1]);
                }
            }
        }

// ----------------------------------------------- ADD PIECE ------------------------------------------------------------

        /// <summary>
        /// Add patchtile to gameboard and to unionfind
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void AddPiece(GamePiece piece, int row, int col)
        {
            board[row][col] = piece;
            ScoreCounter.AddToUF(piece);
            UnionWithNeighbors(piece, row, col);
            
        }

        private void UnionWithNeighbors(GamePiece piece,int row, int col)
        {
            List<(int, int)> neighbors = GetNeighbors(row, col);
            foreach ((int r, int c) in neighbors) 
            {
                if (IsOccupied(r, c))
                {
                    ScoreCounter.EvaluateNew(piece, board[r][c]);

                }
            }
        }

// ----------------------------------------------- CHECK FOR SIMILAR NEIGHBORS ------------------------------------------------------------

        /// <summary>
        /// Checks whether a position on a gameboard has a neighbor with a certain color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool CheckNeighborsColor(Color color, int row, int col)
        {
            List<(int, int)> neighbors = GetNeighbors(row, col);
            foreach ((int r, int c) in neighbors)
            {
                if (board[r][c].Color == color)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether a position on a gameboard has a neighbor with a certain pattern
        /// </summary>
        /// <param name="color"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool CheckNeighborsPattern(Pattern pattern, int row, int col)
        {
            List<(int, int)> neighbors = GetNeighbors(row, col);
            foreach ((int r, int c) in neighbors)
            {
                if (board[r][c].Pattern == pattern)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether a position on a gameboard has a neighbor with the same pattern or color as gamepiece
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool CheckNeighbors(GamePiece gp, int row, int col)
        {
            List<(int, int)> neighbors = GetNeighbors(row, col);

            return (CheckNeighborsColor(gp.Color, row, col) || CheckNeighborsPattern(gp.Pattern, row, col));

        }

// ----------------------------------------------- EVALUATE NEIGHBORS -----------------------------------------------------

        /// <summary>
        /// Counts how much color score would change after adding gamepiece
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int EvaluateNeighborsColor(GamePiece gp, int row, int col)
        {

            int score = 0;
            int count = 0;
            List<(int, int)> neighbors = GetNeighbors(row, col);

            for (int i = 0; i < neighbors.Count; i++)
            {
                
                bool separate = true;

                (int row_i, int col_i) = neighbors[i];

                if (board[row_i][col_i].Color == gp.Color)
                {
                    for (int j = 0; j < i; j++)
                    {
                        (int row_j, int col_j) = neighbors[j];
                        if (ScoreCounter.CheckColorUnion(board[row_i][col_i], board[row_j][col_j]))
                        {
                            separate = false;
                        };
                    }

                    if (separate)
                    {
                        count += ScoreCounter.GetColorCount(board[row_i][col_i]);
                        score += ScoreCounter.GetColorScore(board[row_i][col_i]);
                    }
                }

            }

            int newScore = ScoreCounter.CountColorScore(count + 1);

            
            if (count == 0) return 0;


            return (newScore - score + 1);
        }

        /// <summary>
        /// Counts how much pattern score would change after adding gamepiece
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int EvaluateNeighborsPattern(GamePiece gp, int row, int col)
        {
            int score = 0;
            int count = 0;
            List<(int, int)> neighbors = GetNeighbors(row, col);

            for (int i = 0; i < neighbors.Count; i++)
            {

                bool separate = true;

                (int row_i, int col_i) = neighbors[i];

                if (board[row_i][col_i].Pattern == gp.Pattern)
                {
                    for (int j = 0; j < i; j++)
                    {
                        (int row_j, int col_j) = neighbors[j];
                        if (ScoreCounter.CheckPatternUnion(board[row_i][col_i], board[row_j][col_j]))
                        {
                            separate = false;
                        };
                    }

                    if (separate)
                    {
                        count += ScoreCounter.GetPatternCount(board[row_i][col_i]);
                        score += ScoreCounter.GetPatternScore(board[row_i][col_i]);
                    }
                }

            }

            int newScore = ScoreCounter.CountPatternScore(count + 1, gp.Pattern);


            if (count == 0) return 0;


            return (newScore - score + 1);
        }

        /// <summary>
        /// Counts how much score would change after adding gamepiece
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int EvaluateNeighbors(GamePiece gp, int row, int col)
        {
            return EvaluateNeighborsColor(gp, row, col) + EvaluateNeighborsPattern(gp, row, col);
            
        }

// ----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Return a list of neighboring positions
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private List<(int,int)> GetNeighbors(int row, int col)
        {
            List<(int, int)> neighbors;
            if (row % 2 == 1)
            {
                neighbors = new List<(int, int)>() { (row - 1, col - 1), (row - 1, col), (row, col - 1), (row, col + 1), (row + 1, col - 1), (row + 1, col) };
            }
            else
            {
                neighbors = new List<(int, int)>() { (row - 1, col), (row - 1, col + 1), (row, col - 1), (row, col + 1), (row + 1, col), (row + 1, col + 1) };
            }
            return neighbors;
        }


// ----------------------------------------------- CHECK POSITION ------------------------------------------------------------
       /// <summary>
       /// Checks whether the position is empty
       /// </summary>
       /// <param name="row"></param>
       /// <param name="col"></param>
       /// <returns></returns>
        public bool IsEmpty(int row, int col)
        {
            return (board[row][col].Type == Type.Empty);
        }
        /// <summary>
        /// Checks whether the position is occupied by a patchtile
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool IsOccupied(int row, int col)
        {
            return (board[row][col].Type == Type.PatchTile);
        }
    }
}
