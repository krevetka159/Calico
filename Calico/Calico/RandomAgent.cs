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

    public class ColorAgent : Player
    {
        public ColorAgent(Scoring scoring) : base(scoring)
        {
        }
        public override int ChooseGamePiece(GamePiece[] Opts)
        {
            return 0;
        }

        public override (int, int) ChoosePosition()
        { 

            return (0,0);
        }
    }
}
