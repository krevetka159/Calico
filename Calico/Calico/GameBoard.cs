using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Calico
{
    /// <summary>
    /// Game board representation
    /// </summary>
    public class GameBoard
    {
        public List<List<GamePiece>> board;
        private static GamePiece empty = new GamePiece(Type.Empty);
        private static GamePiece taskSpot = new GamePiece(Type.TaskSpot);
        public int Size = 7;
        public List<(int,int)> TaskPieceSpots = new List<(int,int)>() { (2,3),(3,4),(4,2)};
        public Dictionary<(int,int),TaskPiece> TaskPieces = new Dictionary<(int, int), TaskPiece>();

        private Scoring scoring;
        public ScoreCounter ScoreCounter { get; private set; }

        public int EmptySpotsCount { get; private set; }

        #region Init
        public GameBoard(Scoring scoring)
        {
            Random random = new Random();

            this.scoring = scoring;
            ScoreCounter = new ScoreCounter(scoring);

            int randId = random.Next(1, 5);
            // generate board based on id - 4 options (different borders)
            switch (randId)
            {
                case 1:
                    board = new List<List<GamePiece>>() { 
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Vines), new GamePiece(Color.Yellow, Pattern.Stripes), new GamePiece(Color.Cyan, Pattern.Vines), new GamePiece(Color.Pink, Pattern.Fern), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Yellow, Pattern.Flowers), new GamePiece(Color.Green, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Flowers), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Quatrefoil), empty, empty, taskSpot, empty, empty, new GamePiece(Color.Purple, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Fern), empty, empty, empty, taskSpot, empty, new GamePiece(Color.Yellow, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Stripes), empty, taskSpot, empty, empty, empty, new GamePiece(Color.Green, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Flowers), new GamePiece(Color.Cyan, Pattern.Quatrefoil), new GamePiece(Color.Pink, Pattern.Flowers), new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Green, Pattern.Vines), new GamePiece(Color.Pink, Pattern.Dots), new GamePiece(Color.Cyan, Pattern.Stripes) }
                    };
                    break;
                case 2:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Vines), new GamePiece(Color.Green, Pattern.Fern), new GamePiece(Color.Blue, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Yellow, Pattern.Dots), new GamePiece(Color.Cyan, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Quatrefoil), empty, empty, taskSpot, empty, empty, new GamePiece(Color.Purple, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Stripes), empty, empty, empty, taskSpot, empty, new GamePiece(Color.Yellow, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Flowers), empty, taskSpot, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Vines), empty, empty, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Stripes), new GamePiece(Color.Blue, Pattern.Dots), new GamePiece(Color.Green, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Stripes), new GamePiece(Color.Cyan, Pattern.Fern), new GamePiece(Color.Blue, Pattern.Flowers), new GamePiece(Color.Green, Pattern.Vines) }
                    };
                    break;
                case 3:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Flowers), new GamePiece(Color.Purple, Pattern.Dots), new GamePiece(Color.Pink, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Fern), new GamePiece(Color.Cyan, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Green, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Quatrefoil), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Stripes), empty, empty, taskSpot, empty, empty, new GamePiece(Color.Cyan, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Fern), empty, empty, empty, taskSpot, empty, new GamePiece(Color.Pink, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Vines), empty, taskSpot, empty, empty, empty, new GamePiece(Color.Green, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Stripes), new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Green, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Dots) }
                    };
                    break;
                case 4:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Quatrefoil), new GamePiece(Color.Blue, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Dots), new GamePiece(Color.Blue, Pattern.Vines), new GamePiece(Color.Green, Pattern.Fern), new GamePiece(Color.Pink, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Fern), empty, empty, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Vines), empty, empty, taskSpot, empty, empty, new GamePiece(Color.Blue, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Dots), empty, empty, empty, taskSpot, empty, new GamePiece(Color.Green, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Flowers), empty, taskSpot, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Quatrefoil), empty, empty, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Vines), new GamePiece(Color.Purple, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Vines), new GamePiece(Color.Cyan, Pattern.Dots), new GamePiece(Color.Pink, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Quatrefoil) }
                    };
                    break;
                default:
                    board = new List<List<GamePiece>>();
                    break;
            }
            BorderUF();

            EmptySpotsCount = 22;

        }
        /// <summary>
        /// init for MS Tests
        /// </summary>
        /// <param name="scoring"></param>
        /// <param name="id"></param>
        public GameBoard(Scoring scoring, int id)
        {

            ScoreCounter = new ScoreCounter(scoring);

            // generate with borders based on id - 4 options
            switch (id)
            {
                case 1:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Vines), new GamePiece(Color.Yellow, Pattern.Stripes), new GamePiece(Color.Cyan, Pattern.Vines), new GamePiece(Color.Pink, Pattern.Fern), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Yellow, Pattern.Flowers), new GamePiece(Color.Green, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Flowers), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Quatrefoil), empty, empty, taskSpot, empty, empty, new GamePiece(Color.Purple, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Fern), empty, empty, empty, taskSpot, empty, new GamePiece(Color.Yellow, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Stripes), empty, taskSpot, empty, empty, empty, new GamePiece(Color.Green, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Flowers), new GamePiece(Color.Cyan, Pattern.Quatrefoil), new GamePiece(Color.Pink, Pattern.Flowers), new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Green, Pattern.Vines), new GamePiece(Color.Pink, Pattern.Dots), new GamePiece(Color.Cyan, Pattern.Stripes) }
                    };
                    break;
                case 2:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Vines), new GamePiece(Color.Green, Pattern.Fern), new GamePiece(Color.Blue, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Yellow, Pattern.Dots), new GamePiece(Color.Cyan, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Quatrefoil), empty, empty, taskSpot, empty, empty, new GamePiece(Color.Purple, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Stripes), empty, empty, empty, taskSpot, empty, new GamePiece(Color.Yellow, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Flowers), empty, taskSpot, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Vines), empty, empty, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Stripes), new GamePiece(Color.Blue, Pattern.Dots), new GamePiece(Color.Green, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Stripes), new GamePiece(Color.Cyan, Pattern.Fern), new GamePiece(Color.Blue, Pattern.Flowers), new GamePiece(Color.Green, Pattern.Vines) }
                    };
                    break;
                case 3:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Flowers), new GamePiece(Color.Purple, Pattern.Dots), new GamePiece(Color.Pink, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Fern), new GamePiece(Color.Cyan, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Green, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Quatrefoil), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Stripes), empty, empty, taskSpot, empty, empty, new GamePiece(Color.Cyan, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Fern), empty, empty, empty, taskSpot, empty, new GamePiece(Color.Pink, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Vines), empty, taskSpot, empty, empty, empty, new GamePiece(Color.Green, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Stripes), new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Green, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Dots) }
                    };
                    break;
                case 4:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Quatrefoil), new GamePiece(Color.Blue, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Dots), new GamePiece(Color.Blue, Pattern.Vines), new GamePiece(Color.Green, Pattern.Fern), new GamePiece(Color.Pink, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Fern), empty, empty, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Vines), empty, empty, taskSpot, empty, empty, new GamePiece(Color.Blue, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Dots), empty, empty, empty, taskSpot, empty, new GamePiece(Color.Green, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Flowers), empty, taskSpot, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Quatrefoil), empty, empty, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Vines), new GamePiece(Color.Purple, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Vines), new GamePiece(Color.Cyan, Pattern.Dots), new GamePiece(Color.Pink, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Quatrefoil) }
                    };
                    break;
                default:
                    board = new List<List<GamePiece>>();
                    break;
            }
            BorderUF();

            EmptySpotsCount = 22;


        }

        public GameBoard(GameBoard b) //copy constructor for simulations
        {
            scoring = b.scoring;
            ScoreCounter = new ScoreCounter(b.ScoreCounter);
            board = (b.board).Select(row => row.Select(gp => gp).ToList()).ToList();
            //task pieces copy
            foreach ((int,int)pos in TaskPieceSpots)
            {
                TaskPieces[pos] = new TaskPiece(b.TaskPieces[pos]);
            }
            EmptySpotsCount = b.EmptySpotsCount;
        }
        /// <summary>
        /// Add border pieces from board to unionfind
        /// </summary>
        private void BorderUF()
        {
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
        /// <summary>
        /// Put task on board
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="position"></param>
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
            EmptySpotsCount--;
        }

        /// <summary>
        /// Union new patchtile with neighboring pieces
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void UnionWithNeighbors(GamePiece piece,int row, int col)
        {
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);
            List<GamePiece> neighborsToEvaluate = new List<GamePiece>();
            foreach ((int r, int c) in n) 
            {
                if (IsOccupied(r, c))
                {
                    neighborsToEvaluate.Add(board[r][c]);
                }
            }
            ScoreCounter.EvaluateNew(piece, neighborsToEvaluate);
        }

        /// <summary>
        /// Add new patchtile to neighbors of task
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void AddToTaskNeighbors(GamePiece piece, int row, int col)
        {
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

        #region Basic evaluation

        /// <summary>
        /// Basic evaluation function - color
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int EvaluatePositionBasicColor(GamePiece gp, int row, int col)
        {
            int count = 0;
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
                    return 2*ScoreCounter.Scoring.ColorScoring.Points;
                }
                else
                {
                    return ScoreCounter.Scoring.ColorScoring.Points;
                }
            }
            else return 1;
        }

        /// <summary>
        /// Basic evaluation function - pattern
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int EvaluatePositionBasicPattern(GamePiece gp, int row, int col)
        {
            int count = 0;
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);

            List<int> clusters = new List<int>(new int[6]);

            int i = 0;

            foreach ((int row_i, int col_i) in n)
            {
                if (board[row_i][col_i].Pattern == gp.Pattern)
                {
                    if (!clusters.Contains(ScoreCounter.GetPatternClusterId(board[row_i][col_i])))
                    {
                        if (ScoreCounter.GetPatternCount(board[row_i][col_i]) >= ScoreCounter.Scoring.PatternScoring.PatternScoringDict[(int)gp.Pattern-1].ClusterSize) return 0;
                        count += ScoreCounter.GetPatternCount(board[row_i][col_i]);

                        clusters[i] = ScoreCounter.GetPatternClusterId(board[row_i][col_i]);
                        i++;
                    }
                }

            }


            if (count == 0) return 0;
            else if (count + 1 >= ScoreCounter.Scoring.PatternScoring.PatternScoringDict[(int)gp.Pattern-1].ClusterSize) return ScoreCounter.Scoring.PatternScoring.PatternScoringDict[(int)gp.Pattern - 1].Points;
            else return 1;
        }

        /// <summary>
        /// Basuc evaluation function - tasks
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int EvaluateNeighboringTask(GamePiece gp, int row, int col)
        {
            int score = 0;
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);
            foreach ((int r, int c) in n)
            {
                if (IsTask(r, c))
                {
                    score += TaskPieces[(r, c)].EvaluateBasic(gp);
                }
            }

            return score;
        }

        /// <summary>
        /// Basic evaluation function
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int EvaluatePositionBasic(GamePiece gp, int row, int col)
        {
            return EvaluatePositionBasicColor(gp, row, col) + EvaluatePositionBasicPattern(gp, row, col) + EvaluateNeighboringTask(gp, row,col);
            
        }

        #endregion

        #region Advanced evaluation

        /// <summary>
        /// Part of advanced evaluation function - color
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int EvaluatePositionAdvancedColor(GamePiece gp, int row, int col)
        {
            int count = 0;

            IEnumerable<(int, int)> n = IterateNeighbors(row, col);

            List<int> clusters = new List<int>();

            int i = 0;

            foreach ((int row_i, int col_i) in n)
            {
                if (board[row_i][col_i].Color == gp.Color)
                {
                    if (! clusters.Contains(ScoreCounter.GetColorClusterId(board[row_i][col_i])))
                    {
                        if (ScoreCounter.GetColorCount(board[row_i][col_i]) >= ScoreCounter.Scoring.ColorScoring.ClusterSize) return 0;
                        count += ScoreCounter.GetColorCount(board[row_i][col_i]);

                        clusters.Add( ScoreCounter.GetColorClusterId(board[row_i][col_i]));
                        i++;
                    }
                }

            }

            if (count + 1 >= ScoreCounter.Scoring.ColorClusterSize)
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
            else return count;
        }

        /// <summary>
        /// Part of advanced evaluation function - pattern
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int EvaluatePositionAdvancedPattern(GamePiece gp, int row, int col)
        {
            int count = 0;

            IEnumerable<(int, int)> n = IterateNeighbors(row, col);

            List<int> clusters = new List<int>(new int[6]);

            int i = 0;

            foreach ((int row_i, int col_i) in n)
            {
                if (board[row_i][col_i].Pattern == gp.Pattern)
                {
                    clusters[i] = ScoreCounter.GetPatternClusterId(board[row_i][col_i]);
                    i++;
                }
            }

            foreach (int c in clusters.Distinct())
            {
                if (ScoreCounter.GetPatternCount(c) >= ScoreCounter.Scoring.PatternScoring.PatternScoringDict[(int)gp.Pattern - 1].ClusterSize) return 0;
                count += ScoreCounter.GetPatternCount(c);
            }

            if (count + 1 >= ScoreCounter.Scoring.PatternScoring.PatternScoringDict[(int)gp.Pattern - 1].ClusterSize) return ScoreCounter.Scoring.PatternScoring.PatternScoringDict[(int)gp.Pattern - 1].Points + 1;
            else return count;
        }

        /// <summary>
        /// Part of advanced evaluation function - tasks
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public double EvaluateNeighboringTaskAdvanced(GamePiece gp, int row, int col)
        {
            double score = 0;
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);
            foreach ((int r, int c) in n)
            {
                if (IsTask(r, c))
                {
                    score += TaskPieces[(r, c)].EvaluateAdvanced(gp);
                }
            }

            return score;
        }

        /// <summary>
        /// Advanced evaluation function
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public double EvaluatePositionAdvanced(GamePiece gp, int row, int col)
        {
            return EvaluatePositionAdvancedColor(gp, row, col) + EvaluatePositionAdvancedPattern(gp, row, col) + EvaluateNeighboringTaskAdvanced(gp, row, col);

        }

        /// <summary>
        /// Part od weighted evaluation function - pattern
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="sc"></param>
        /// <param name="weights"></param>
        /// <returns></returns>
        public double EvaluatePositionWeightedAdvancedPattern(GamePiece gp, int row, int col, Weights weights)
        {
            int count = 0;

            IEnumerable<(int, int)> n = IterateNeighbors(row, col);

            List<int> clusters = new List<int>(new int[6]);

            int i = 0;

            foreach ((int row_i, int col_i) in n)
            {
                if (board[row_i][col_i].Pattern == gp.Pattern)
                {
                    if (!clusters.Contains(ScoreCounter.GetPatternClusterId(board[row_i][col_i])))
                    {
                        if (ScoreCounter.GetPatternCount(board[row_i][col_i]) >= ScoreCounter.Scoring.PatternScoring.PatternScoringDict[(int)gp.Pattern - 1].ClusterSize) return 0;
                        count += ScoreCounter.GetPatternCount(board[row_i][col_i]);

                        clusters[i] = ScoreCounter.GetPatternClusterId(board[row_i][col_i]);
                        i++;
                    }
                }

            }

            double evol_konst = 0;
            switch (ScoreCounter.Scoring.PatternScoring.PatternScoringDict[(int)gp.Pattern - 1].Id)
            {
                case 0:
                    evol_konst = weights.CatsW.Item1;
                    break;
                case 1:
                    evol_konst = weights.CatsW.Item2;
                    break;
                case 2:
                    evol_konst = weights.CatsW.Item3;
                    break;
            }

            if (count + 1 >= ScoreCounter.Scoring.PatternScoring.PatternScoringDict[(int)gp.Pattern - 1].ClusterSize)
            {
                return (ScoreCounter.Scoring.PatternScoring.PatternScoringDict[(int)gp.Pattern - 1].Points + 1) * evol_konst;
            }
            else return count * evol_konst;
        }
        /// <summary>
        /// Part of weighted advanced evaluation function - tasks
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="weights"></param>
        /// <returns></returns>
        public double EvaluateNeighboringWeightedAdvancedTask(GamePiece gp, int row, int col, Weights weights)
        {
            double score = 0;
            IEnumerable<(int, int)> n = IterateNeighbors(row, col);

            foreach ((int r, int c) in n)
            {
                if (IsTask(r, c))
                {
                    var task_weight = weights.TaskW[TaskPieces[(r,c)].Id - 1];
                    
                    score += (TaskPieces[(r, c)].EvaluateAdvanced(gp)) * task_weight;
                }
            }

            return score;
        }

        /// <summary>
        /// Weighted advanced evaluation function
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="weights"></param>
        /// <returns></returns>
        public double EvaluatePositionWeightedAdvanced(GamePiece gp, int row, int col, Weights weights)
        {
            return EvaluatePositionAdvancedColor(gp, row, col)*weights.ButtonW + EvaluatePositionWeightedAdvancedPattern(gp, row, col,weights) + EvaluateNeighboringWeightedAdvancedTask(gp, row, col,weights);
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

        /// <summary>
        /// Overview, which tasks were completed
        /// -1 = not chosen 
        /// 0 = not completed
        /// 1 = completed partially
        /// 2 = completed
        /// </summary>
        /// <returns></returns>
        public int[] TasksCompleted()
        {
            int[] tc = new int[6] {-1,-1,-1,-1,-1,-1};

            foreach(TaskPiece t in TaskPieces.Values)
            {
                tc[t.Id-1] = t.Completed();
            }
            return tc;
        }

    }
}
