﻿using System;
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
        public void CornerUF()
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

// ----------------------------------------------- ADD PIECE ------------------------------------------------------------

        public void AddPiece(GamePiece piece, int x, int y)
        {
            board[x][y] = piece;
            ScoreCounter.AddToUF(piece);
            UnionWithNeighbors(piece, x, y);
            
        }

        public void UnionWithNeighbors(GamePiece piece,int row, int col)
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

        public bool CheckNeighbors(Color color, int row, int col)
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
        public bool CheckNeighbors(Pattern pattern, int row, int col)
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

        public bool CheckNeighbors(GamePiece gp, int row, int col)
        {
            List<(int, int)> neighbors = GetNeighbors(row, col);
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

        public int EvaluateNeighbors(GamePiece gp, int row, int col)
        {
            int score = 0;
            int count = 0;
            List<(int, int)> neighbors = GetNeighbors(row, col);
            

            for (int i = 0; i < neighbors.Count; i++)
            {

                bool separateColor = true;
                bool separatePattern = true;

                (int row_i, int col_i) = neighbors[i];

                if (board[row_i][col_i].Color == gp.Color)
                {
                    for (int j = 0; j < i; j++)
                    {
                        (int row_j, int col_j) = neighbors[j];
                        if (ScoreCounter.CheckColorUnion(board[row_i][col_i], board[row_j][col_j]))
                        {
                            separateColor = false;
                        };
                    }

                    if (separateColor)
                    {
                        count += ScoreCounter.GetColorCount(board[row_i][col_i]);
                        score += ScoreCounter.GetColorScore(board[row_i][col_i]);
                    }
                }

                if (board[row_i][col_i].Pattern == gp.Pattern)
                {
                    for (int j = 0; j < i; j++)
                    {
                        (int row_j, int col_j) = neighbors[j];
                        if (ScoreCounter.CheckPatternUnion(board[row_i][col_i], board[row_j][col_j]))
                        {
                            separatePattern = false;
                        };
                    }

                    if (separatePattern)
                    {
                        count += ScoreCounter.GetPatternCount(board[row_i][col_i]);
                        score += ScoreCounter.GetPatternScore(board[row_i][col_i]);
                    }
                }

            }

            int newScore = ScoreCounter.CountColorScore(count + 1) + ScoreCounter.CountPatternScore(count + 1, gp.Pattern);


            if (count == 0) return 0;


            return (newScore - score + 1);
        }

        public List<(int,int)> GetNeighbors(int row, int col)
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


 // ----------------------------------------------- PRINT ------------------------------------------------------------

        public void PrintBoard()
        {
            Console.WriteLine("       1    2    3    4    5    6    7");
            Console.WriteLine("    ------------------------------------");
            for (int i = 0; i < Size; i++) 
            {

                Console.Write(" " + (i+1));
                if (i % 2 == 0) Console.Write("  ");
                Console.Write(" |");
                for (int j = 0; j < Size; j++)
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
