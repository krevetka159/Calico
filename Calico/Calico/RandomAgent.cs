using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class RandomAgent : Player
    {
        Random random = new Random();

        public RandomAgent(Scoring scoring) : base(scoring)
        {
        }

        public override int ChooseGamePiece(GamePiece[] Opts)
        {
            return random.Next(1, Opts.Length);
        }

        public override (int, int) ChoosePosition()
        {
            int row = random.Next(0, board.size - 1);
            int col = random.Next(0, board.size - 1);

            while (!board.IsEmpty(row, col))
            {
                row = random.Next(0, board.size - 1);
                col = random.Next(0, board.size - 1);
            }

            return (row +1, col +1);
        }
    }

    public class RandomAgent2 : Player
    {
        Random random = new Random();

        public RandomAgent2(Scoring scoring) : base(scoring)
        {
        }

        public override int ChooseGamePiece(GamePiece[] Opts)
        {
            return random.Next(1, Opts.Length);
        }

        public override (int, int) ChoosePosition(GamePiece gp)
        {
            for (int i = 1;i<board.size-1;i++)
            {
                for (int j = 1; j < board.size - 1; j++)
                {
                    if (board.CheckNeighbors(gp.Color, i, j))
                    {
                        return (i+1, j+1);
                    }
                }
            }

            int row = random.Next(0, board.size - 1);
            int col = random.Next(0, board.size - 1);

            while (!board.IsEmpty(row, col))
            {
                row = random.Next(0, board.size - 1);
                col = random.Next(0, board.size - 1);
            }

            return (row + 1, col + 1);
        }
    }
    public class RandomAgent3 : Player
    {
        Random random = new Random();

        public RandomAgent3(Scoring scoring) : base(scoring)
        {
        }

        public override int ChooseGamePiece(GamePiece[] Opts)
        {
            return random.Next(1, Opts.Length);
        }

        public override (int, int) ChoosePosition(GamePiece gp)
        {
            for (int i = 1; i < board.size - 1; i++)
            {
                for (int j = 1; j < board.size - 1; j++)
                {
                    if (board.CheckNeighbors(gp.Pattern, i, j))
                    {
                        return (i + 1, j + 1);
                    }
                }
            }

            int row = random.Next(0, board.size - 1);
            int col = random.Next(0, board.size - 1);

            while (!board.IsEmpty(row, col))
            {
                row = random.Next(0, board.size - 1);
                col = random.Next(0, board.size - 1);
            }

            return (row + 1, col + 1);
        }
    }

    public class RandomAgent4 : Player
    {
        Random random = new Random();

        public RandomAgent4(Scoring scoring) : base(scoring)
        {
        }

        public override int ChooseGamePiece(GamePiece[] Opts)
        {
            return random.Next(1, Opts.Length);
        }

        public override (int, int) ChoosePosition(GamePiece gp)
        {
            for (int i = 1; i < board.size - 1; i++)
            {
                for (int j = 1; j < board.size - 1; j++)
                {
                    if (board.CheckNeighbors(gp, i, j))
                    {
                        return (i + 1, j + 1);
                    }
                }
            }

            int row = random.Next(0, board.size - 1);
            int col = random.Next(0, board.size - 1);

            while (!board.IsEmpty(row, col))
            {
                row = random.Next(0, board.size - 1);
                col = random.Next(0, board.size - 1);
            }

            return (row + 1, col + 1);
        }
    }

    public class RandomAgent21 : Player
    {
        Random random = new Random();

        public RandomAgent21(Scoring scoring) : base(scoring)
        {
        }

        public override int ChooseGamePiece(GamePiece[] Opts)
        {
            return random.Next(1, Opts.Length);
        }

        public override (int, int) ChoosePosition(GamePiece gp)
        {
            int max = 0;
            (int, int) maxPosition = (2,2) ;

            for (int i = 1; i < board.size - 1; i++)
            {
                for (int j = 1; j < board.size - 1; j++)
                {
                    if (board.EvaluateNeighborsColor(gp, i, j) > max)
                    {
                        max = board.EvaluateNeighborsColor(gp, i, j);
                        maxPosition = (i + 1, j + 1);
                    }
                }
            }

            if (max > 0) return maxPosition;

            int row = random.Next(0, board.size - 1);
            int col = random.Next(0, board.size - 1);

            while (!board.IsEmpty(row, col))
            {
                row = random.Next(0, board.size - 1);
                col = random.Next(0, board.size - 1);
            }

            return (row + 1, col + 1);
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            int maxPieceIndex = 0;
            int max = 0;
            (int, int) maxPosition = (2, 2);

            for (int o = 0; o < Opts.Length;o++)
            {
                GamePiece gp = Opts[o];
                for (int i = 1; i < board.size - 1; i++)
                {
                    for (int j = 1; j < board.size - 1; j++)
                    {
                        if (board.EvaluateNeighborsColor(gp, i, j) > max)
                        {
                            maxPieceIndex = o;
                            max = board.EvaluateNeighborsColor(gp, i, j);
                            maxPosition = (i + 1, j + 1);
                        }
                    }
                }
            }

            if (max > 0) return (maxPieceIndex,maxPosition);

            int row = random.Next(0, board.size - 1);
            int col = random.Next(0, board.size - 1);

            while (!board.IsEmpty(row, col))
            {
                row = random.Next(0, board.size - 1);
                col = random.Next(0, board.size - 1);
            }

            return (maxPieceIndex,(row + 1, col + 1));
            
        }
    }
    public class RandomAgent31 : Player
    {
        Random random = new Random();

        public RandomAgent31(Scoring scoring) : base(scoring)
        {
        }

        public override int ChooseGamePiece(GamePiece[] Opts)
        {
            return random.Next(1, Opts.Length);
        }

        public override (int, int) ChoosePosition(GamePiece gp)
        {
            int max = 0;
            (int, int) maxPosition = (2, 2);

            for (int i = 1; i < board.size - 1; i++)
            {
                for (int j = 1; j < board.size - 1; j++)
                {
                    if (board.EvaluateNeighborsPattern(gp, i, j) > max)
                    {
                        max = board.EvaluateNeighborsPattern(gp, i, j);
                        maxPosition = (i + 1, j + 1);
                    }
                }
            }

            if (max > 0) return maxPosition;

            int row = random.Next(0, board.size - 1);
            int col = random.Next(0, board.size - 1);

            while (!board.IsEmpty(row, col))
            {
                row = random.Next(0, board.size - 1);
                col = random.Next(0, board.size - 1);
            }

            return (row + 1, col + 1);
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            int maxPieceIndex = 0;
            int max = 0;
            (int, int) maxPosition = (2, 2);

            for (int o = 0; o < Opts.Length; o++)
            {
                GamePiece gp = Opts[o];
                for (int i = 1; i < board.size - 1; i++)
                {
                    for (int j = 1; j < board.size - 1; j++)
                    {
                        if (board.EvaluateNeighborsPattern(gp, i, j) > max)
                        {
                            maxPieceIndex = o;
                            max = board.EvaluateNeighborsPattern(gp, i, j);
                            maxPosition = (i + 1, j + 1);
                        }
                    }
                }
            }

            if (max > 0) return (maxPieceIndex, maxPosition);

            int row = random.Next(0, board.size - 1);
            int col = random.Next(0, board.size - 1);

            while (!board.IsEmpty(row, col))
            {
                row = random.Next(0, board.size - 1);
                col = random.Next(0, board.size - 1);
            }

            return (maxPieceIndex, (row + 1, col + 1));

        }
    }

    public class RandomAgent41 : Player
    {
        Random random = new Random();

        public RandomAgent41(Scoring scoring) : base(scoring)
        {
        }

        public override int ChooseGamePiece(GamePiece[] Opts)
        {
            return random.Next(1, Opts.Length);
        }

        public override (int, int) ChoosePosition(GamePiece gp)
        {
            for (int i = 1; i < board.size - 1; i++)
            {
                for (int j = 1; j < board.size - 1; j++)
                {
                    if (board.CheckNeighbors(gp, i, j))
                    {
                        return (i + 1, j + 1);
                    }
                }
            }

            int row = random.Next(0, board.size - 1);
            int col = random.Next(0, board.size - 1);

            while (!board.IsEmpty(row, col))
            {
                row = random.Next(0, board.size - 1);
                col = random.Next(0, board.size - 1);
            }

            return (row + 1, col + 1);
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            int maxPieceIndex = 0;
            int max = 0;
            (int, int) maxPosition = (2, 2);

            for (int o = 0; o < Opts.Length; o++)
            {
                GamePiece gp = Opts[o];
                for (int i = 1; i < board.size - 1; i++)
                {
                    for (int j = 1; j < board.size - 1; j++)
                    {
                        if (board.EvaluateNeighbors(gp, i, j) > max)
                        {
                            maxPieceIndex = o;
                            max = board.EvaluateNeighbors(gp, i, j);
                            maxPosition = (i + 1, j + 1);
                        }
                    }
                }
            }

            if (max > 0) return (maxPieceIndex, maxPosition);

            int row = random.Next(0, board.size - 1);
            int col = random.Next(0, board.size - 1);

            while (!board.IsEmpty(row, col))
            {
                row = random.Next(0, board.size - 1);
                col = random.Next(0, board.size - 1);
            }

            return (maxPieceIndex, (row + 1, col + 1));

        }
    }


}
