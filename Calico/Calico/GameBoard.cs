using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Calico
{
    public class GameBoard
    {
        public List<List<GamePiece>> board;
        //private Random random;
        private static GamePiece empty = new GamePiece(Type.Empty);
        private static GamePiece blocked = new GamePiece(Type.Blocked);
        public int Size = 7;
        public List<(int,int)> TaskPieceSpots = new List<(int,int)>() { (2,3),(3,4),(4,2)};
        public Dictionary<(int,int),TaskPiece> TaskPieces = new Dictionary<(int, int), TaskPiece>();

        private Scoring scoring;
        public ScoreCounter ScoreCounter { get; private set; }

        #region Init
        public GameBoard(Scoring scoring)
        {
            Random random = new Random();

            this.scoring = scoring;
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
            BorderUF();

        }
        /// <summary>
        /// init for MS Tests
        /// </summary>
        /// <param name="scoring"></param>
        /// <param name="id"></param>
        public GameBoard(Scoring scoring, int id)
        {
            //random = new Random();

            ScoreCounter = new ScoreCounter(scoring);

            // generování okrajů boardu podle id - 4 možnosti
            switch (id)
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
            BorderUF();

        }

        private void BorderUF()
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

        public void AddTaskPiece(int taskId,int position )
        {
            (int row, int col) = TaskPieceSpots[position];

            TaskPiece task = new TaskPiece(taskId);
            board[row][col] = task;

            TaskPieces[(row,col)]=task;
        }

        #endregion

        #region AddPiece

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
            AddToTaskNeighbors(piece, row, col);
            
        }

        private void UnionWithNeighbors(GamePiece piece,int row, int col)
        {
            //List<(int, int)> neighbors = GetNeighbors(row, col);
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);
            List<GamePiece> neighborsToEvaluate = new List<GamePiece>();
            foreach ((int r, int c) in n) 
            {
                if (IsOccupied(r, c))
                {
                    //ScoreCounter.EvaluateNew(piece, board[r][c]);
                    neighborsToEvaluate.Add(board[r][c]);

                }
            }
            ScoreCounter.EvaluateNew(piece, neighborsToEvaluate);
        }

        private void AddToTaskNeighbors(GamePiece piece, int row, int col)
        {
            // List<(int, int)> neighbors = GetNeighbors(row, col);
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);
            foreach ((int r, int c) in n)
            {
                if (IsTask(r, c))
                {
                    TaskPieces[(r,c)].AddNeighbor(piece);
                    ScoreCounter.EvaluateTask(TaskPieces[(r, c)]);
                }
            }
        }

        #endregion

        #region GetNeighbors

        /// <summary>
        /// Return a list of neighboring positions
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private List<(int, int)> GetNeighbors(int row, int col)
        {
            List<(int, int)> neighbors = new List<(int, int)>() { (row - 1, col - 1 + (row%2)), (row - 1, col + (row % 2)), (row, col - 1), (row, col + 1), (row + 1, col - 1 + (row % 2) ), (row + 1, col + (row % 2)) };          
            return neighbors;
        }

        private IEnumerable<(int,int)> IterateNeighbors(int row, int col)
        {
            yield return (row - 1, col - 1 + (row % 2));
            yield return (row - 1, col + (row % 2));
            yield return (row, col - 1);
            yield return (row, col + 1);
            yield return (row + 1, col - 1 + (row % 2));
            yield return (row + 1, col + (row % 2));
        }

        #endregion

        #region Check For Similar Neighbors

        /// <summary>
        /// Checks whether a position on a gameboard has a neighbor with a certain color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool CheckNeighborsColor(Color color, int row, int col)
        {
            // List<(int, int)> neighbors = GetNeighbors(row, col);
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);
            foreach ((int r, int c) in n)
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
            //List<(int, int)> neighbors = GetNeighbors(row, col);
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);
            foreach ((int r, int c) in n)
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

            return (CheckNeighborsColor(gp.Color, row, col) || CheckNeighborsPattern(gp.Pattern, row, col));

        }

        #endregion

        #region Basic evaluation

        /// <summary>
        /// Counts how much color score would change after adding gamepiece
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int EvaluateNeighborsColorFixed(GamePiece gp, int row, int col)
        {
            int count = 0;
            //List<(int, int)> neighbors = GetNeighbors(row, col);
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);

            List<int> clusters = new List<int>(new int[6]);

            int i = 0;

            foreach ((int row_i, int col_i) in n )
            {
                if (board[row_i][col_i].Color == gp.Color)
                {
                    if (!clusters.Contains(ScoreCounter.GetColorClusterId(board[row_i][col_i])))
                    {
                        if (ScoreCounter.GetColorCount(board[row_i][col_i]) >= ScoreCounter.Scoring.ColorScoring.ClusterSize) return 0;
                        count += ScoreCounter.GetColorCount(board[row_i][col_i]);

                        clusters[i] = ScoreCounter.GetColorClusterId(board[row_i][col_i]);
                        i++;
                    }
                }

            }

            if (count == 0) return 0;
            else if (count + 1 >= ScoreCounter.Scoring.ColorScoring.ClusterSize)
            {
                if (ScoreCounter.GetsRainbowButton(gp.Color))
                {
                    return ScoreCounter.Scoring.ColorScoring.Points + 1 + 3;
                }
                else
                {
                    return ScoreCounter.Scoring.ColorScoring.Points + 1;
                }
            }
            else return 1;
        }

        /// <summary>
        /// Counts how much pattern score would change after adding gamepiece
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int EvaluateNeighborsPatternFixed(GamePiece gp, int row, int col)
        {
            int count = 0;
            // List<(int, int)> neighbors = GetNeighbors(row, col);

            IEnumerable<(int, int)> n = IterateNeighbors(row, col);

            List<int> clusters = new List<int>(new int[6]);

            int i = 0;

            foreach ((int row_i, int col_i) in n)
            {
                if (board[row_i][col_i].Pattern == gp.Pattern)
                {
                    if (!clusters.Contains(ScoreCounter.GetPatternClusterId(board[row_i][col_i])))
                    {
                        if (ScoreCounter.GetPatternCount(board[row_i][col_i]) >= ScoreCounter.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].ClusterSize) return 0;
                        count += ScoreCounter.GetPatternCount(board[row_i][col_i]);

                        clusters[i] = ScoreCounter.GetPatternClusterId(board[row_i][col_i]);
                        i++;
                    }
                }

            }


            if (count == 0) return 0;
            else if (count + 1 >= ScoreCounter.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].ClusterSize) return ScoreCounter.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].Points + 1;
            else return 1;
        }

        public int EvaluateNeighboringTask(GamePiece gp, int row, int col)
        {
            int score = 0;
            //List<(int, int)> neighbors = GetNeighbors(row, col);
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);
            foreach ((int r, int c) in n)
            {
                if (IsTask(r, c))
                {
                    score += TaskPieces[(r, c)].CheckNeighbours(gp);
                }
            }

            return score;
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
            return EvaluateNeighborsColorFixed(gp, row, col) + EvaluateNeighborsPatternFixed(gp, row, col) + EvaluateNeighboringTask(gp, row,col);
            
        }

        #endregion

        #region Utility evaluation


        public int EvaluateNeighborsColorUtility(GamePiece gp, int row, int col, ScoreCounter sc)
        {
            int count = 0;
            //List<(int, int)> n = GetNeighbors(row, col);

            IEnumerable<(int, int)> n = IterateNeighbors(row, col);

            List<int> clusters = new List<int>();

            int i = 0;

            foreach ((int row_i, int col_i) in n)
            {
                if (board[row_i][col_i].Color == gp.Color)
                {
                    if (! clusters.Contains(sc.GetColorClusterId(board[row_i][col_i])))
                    {
                        if (sc.GetColorCount(board[row_i][col_i]) >= sc.Scoring.ColorScoring.ClusterSize) return 0;
                        count += sc.GetColorCount(board[row_i][col_i]);

                        clusters.Add( sc.GetColorClusterId(board[row_i][col_i]));
                        i++;
                    }
                }

            }

            if (count + 1 >= sc.Scoring.ColorClusterSize)
            {
                if (sc.GetsRainbowButton(gp.Color))
                {
                    return sc.Scoring.ColorScoring.Points + 1 + 3;
                }
                else
                {
                    return sc.Scoring.ColorScoring.Points + 1;
                }
            }
            else return count;
        }

        public int EvaluateNeighborsColorUtility(GamePiece gp, int row, int col, ScoreCounter sc, List<GamePiece> neighbors)
        {
            int count = 0;

            for (int i = 0; i < neighbors.Count; i++)
            {

                bool separate = true;

                if (neighbors[i].Color == gp.Color)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (sc.CheckColorUnion(neighbors[i], neighbors[j]))
                        {
                            separate = false;
                        };
                    }

                    if (separate)
                    {
                        if (sc.GetColorCount(neighbors[i]) >= sc.Scoring.ColorScoring.ClusterSize) return 0;
                        count += sc.GetColorCount(neighbors[i]);
                    }
                }
            }

            if (count + 1 >= sc.Scoring.ColorClusterSize)
            {
                if (sc.GetsRainbowButton(gp.Color))
                {
                    return sc.Scoring.ColorScoring.Points + 1 + 3;
                }
                else
                {
                    return sc.Scoring.ColorScoring.Points + 1;
                }
            }
            else return count;
        }

        public int EvaluateNeighborsPatternUtility(GamePiece gp, int row, int col, ScoreCounter sc)
        {
            int count = 0;
            //List<(int, int)> neighbors = GetNeighbors(row, col);

            IEnumerable<(int, int)> n = IterateNeighbors(row, col);

            List<int> clusters = new List<int>(new int[6]);

            int i = 0;

            foreach ((int row_i, int col_i) in n)
            {
                if (board[row_i][col_i].Pattern == gp.Pattern)
                {
                    clusters[i] = sc.GetPatternClusterId(board[row_i][col_i]);
                    i++;
                    //if (! clusters.Contains(sc.GetPatternClusterId(board[row_i][col_i])))
                    //{
                    //    if (sc.GetPatternCount(board[row_i][col_i]) >= sc.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].ClusterSize) return 0;
                    //    count += sc.GetPatternCount(board[row_i][col_i]);

                    //    clusters[i] =  sc.GetPatternClusterId(board[row_i][col_i]);
                    //    i++;
                    //}
                }
            }

            foreach (int c in clusters.Distinct())
            {
                if (sc.GetPatternCount(c) >= sc.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].ClusterSize) return 0;
                count += sc.GetPatternCount(c);
            }

            if (count + 1 >= sc.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].ClusterSize) return sc.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].Points + 1;
            else return count;
        }

        public int EvaluateNeighborsPatternUtility(GamePiece gp, int row, int col, ScoreCounter sc, List<GamePiece> neighbors)
        {
            int count = 0;

            for (int i = 0; i < neighbors.Count; i++)
            {

                bool separate = true;

                if (neighbors[i].Pattern == gp.Pattern)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (sc.CheckPatternUnion(neighbors[i], neighbors[j]))
                        {
                            separate = false;
                        };
                    }

                    if (separate)
                    {
                        if (sc.GetPatternCount(neighbors[i]) >= sc.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].ClusterSize) return 0;
                        count += sc.GetPatternCount(neighbors[i]);
                    }
                }

            }

            if (count + 1 >= sc.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].ClusterSize) return sc.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].Points + 1;
            else return count;
        }

        public double EvaluateNeighboringTaskUtility(GamePiece gp, int row, int col)
        {
            double score = 0;
            //List<(int, int)> neighbors = GetNeighbors(row, col);
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);
            foreach ((int r, int c) in n)
            {
                if (IsTask(r, c))
                {
                    score += TaskPieces[(r, c)].CheckNeighboursUtility(gp);
                }
            }

            return score;
        }

        /// <summary>
        /// Counts how much score would change after adding gamepiece
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public double EvaluateNeighborsUtility(GamePiece gp, int row, int col)
        {
            return EvaluateNeighborsColorUtility(gp, row, col, ScoreCounter) + EvaluateNeighborsPatternUtility(gp, row, col, ScoreCounter) + EvaluateNeighboringTaskUtility(gp, row, col);

        }

        #endregion

        #region Evolution Evaluation

        public double EvaluateNeighborsPatternUtility(GamePiece gp, int row, int col, ScoreCounter sc, EvolutionGameProps e)
        {
            int count = 0;
            //List<(int, int)> n = GetNeighbors(row, col);

            IEnumerable<(int, int)> n = IterateNeighbors(row, col);

            List<int> clusters = new List<int>(new int[6]);

            int i = 0;

            foreach ((int row_i, int col_i) in n)
            {
                if (board[row_i][col_i].Pattern == gp.Pattern)
                {
                    //clusters[i] = ScoreCounter.GetPatternClusterId(board[row_i][col_i]);
                    //i++;
                    if (!clusters.Contains(ScoreCounter.GetPatternClusterId(board[row_i][col_i])))
                    {
                        if (ScoreCounter.GetPatternCount(board[row_i][col_i]) >= ScoreCounter.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].ClusterSize) return 0;
                        count += ScoreCounter.GetPatternCount(board[row_i][col_i]);

                        clusters[i] = ScoreCounter.GetPatternClusterId(board[row_i][col_i]);
                        i++;
                    }
                }

            }

            //foreach (int c in clusters.Distinct())
            //{
            //    if (sc.GetPatternCount(c) >= sc.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].ClusterSize) return 0;
            //    count += sc.GetPatternCount(c);
            //}



            double evol_konst = 0;
            switch (sc.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].Id)
            {
                case 1:
                    evol_konst = e.CatsConst.Item1;
                    break;
                case 2:
                    evol_konst = e.CatsConst.Item2;
                    break;
                case 3:
                    evol_konst = e.CatsConst.Item3;
                    break;
            }

            if (count + 1 >= sc.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].ClusterSize)
            {
                return (sc.Scoring.PatternScoring.PatternScoringDict[gp.Pattern].Points + 1)*evol_konst;
            }
            else return count*evol_konst;
        }

        public double EvaluateNeighboringTaskUtility(GamePiece gp, int row, int col, EvolutionGameProps e)
        {
            double score = 0;
            //List<(int, int)> n = GetNeighbors(row, col);
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);

            double evol_const = 0;
            foreach ((int r, int c) in n)
            {
                if (IsTask(r, c))
                {
                    switch (TaskPieceSpots.IndexOf((r, c)))
                    {
                        case 0:
                            evol_const = e.TaskConst.Item1;
                            break;
                        case 1:
                            evol_const = e.TaskConst.Item2;
                            break;
                        case 2:
                            evol_const = e.TaskConst.Item3;
                            break;

                    }
                    score += (TaskPieces[(r, c)].CheckNeighboursUtility(gp))*evol_const;
                }
            }

            return score;
        }

        public double EvaluateNeighborsEvolution(GamePiece gp, int row, int col, EvolutionGameProps e)
        {
            return e.ButtonConst*EvaluateNeighborsColorUtility(gp, row, col, ScoreCounter) + EvaluateNeighborsPatternUtility(gp, row, col, ScoreCounter,e) + EvaluateNeighboringTaskUtility(gp, row, col,e);

        }

        #endregion

        #region Few Steps Ahead Evaluation
        public double EvaluateMinimax(List<(GamePiece, (int,int))> gamePieces)
        {
            if (gamePieces.Count == 1)
            {
                return EvaluateNeighborsUtility(gamePieces[0].Item1, gamePieces[0].Item2.Item1, gamePieces[0].Item2.Item2);
            }

            ScoreCounter mockSC = new ScoreCounter(scoring);
            mockSC.SetProps(ScoreCounter.buttons, ScoreCounter.rainbowButtons, ScoreCounter.cats);

            Dictionary<(int, int), List<GamePiece>> taskNeighbors = new Dictionary<(int, int), List<GamePiece>>();

            foreach((int,int) t in TaskPieces.Keys)
            {
                taskNeighbors[t] = new List<GamePiece>();
            }

            foreach((GamePiece _, (int row, int col)) in gamePieces)
            {
                //List<(int, int)> neighbors = GetNeighbors(r, c);
                IEnumerable<(int, int)> n = IterateNeighbors(row, col);

                foreach ((int row_i, int col_i) in n)
                {
                    if (IsOccupied(row_i,col_i))
                    {
                        if (!mockSC.PatternUFContains(board[row_i][col_i]))
                        {
                            mockSC.AddToPatternUF(ScoreCounter.GetPatternCluster(board[row_i][col_i]));
                        }
                        if (!mockSC.ColorUFContains(board[row_i][col_i]))
                        {
                            mockSC.AddToColorUF(ScoreCounter.GetColorCluster(board[row_i][col_i]));
                        }
                    }
                }
            }

            double score = 0;
            int div = 1;

            for( int i = 0; i < gamePieces.Count; i++)
            {
                (GamePiece gp, (int r, int c)) = gamePieces[i];

                List<(int, int)> neighborsPositions = GetNeighbors(r, c);
                //IEnumerable<(int, int)> n = IterateNeighbors(row, col);
                List<GamePiece> neighbors = new List<GamePiece>(); // co tady actually dělám s tímhle?
                for (int j = 0; j < i; j++)
                {
                    if (neighborsPositions.Contains(gamePieces[j].Item2))
                    {
                        neighbors.Add(gamePieces[j].Item1);
                    }
                }

                foreach((int nr, int nc) in neighborsPositions)
                {
                    if (IsOccupied(nr, nc)) 
                    {
                        neighbors.Add(board[nr][nc]);
                    }
                    if (IsTask(nr, nc))
                    {
                        taskNeighbors[(nr, nc)].Add(gp);
                        score += Convert.ToDouble(TaskPieces[(nr, nc)].CheckNeighboursUtilityMinimax(taskNeighbors[(nr, nc)]))/div;
                    }
                }

                score += Convert.ToDouble(EvaluateNeighborsColorUtility(gp, r, c, mockSC, neighbors))/div + Convert.ToDouble(EvaluateNeighborsPatternUtility(gp, r, c, mockSC, neighbors))/div;

                mockSC.AddToUF(gp);

                mockSC.EvaluateNew(gp, neighbors);

                div *= 4;
            }

            return score;

        }

        #endregion




        #region Check Position
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

        /// <summary>
        /// Checks whether the position is occupied by a tasktile
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool IsTask(int row, int col)
        {
            return (board[row][col].Type == Type.Task);
        }

        #endregion

        public Dictionary<int, int> TasksCompleted()
        {
            Dictionary<int, int> tc = new Dictionary<int, int>();

            for (int i = 1; i <= 6; i++)
            {
                tc[i] = -1;
            }

            foreach(TaskPiece t in TaskPieces.Values)
            {
                tc[t.Id] = t.Completed();
            }

            return tc;
        }

    }
}
