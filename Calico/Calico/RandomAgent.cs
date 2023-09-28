using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
// ----------------------------------------------- RANDOM AGENT ------------------------------------------------------------

    public class RandomAgent : Player
    {
        Random random = new Random();

        public RandomAgent(Scoring scoring) : base(scoring)
        {
        }

        public int RandomGamePiece(GamePiece[] Opts)
        {
            return random.Next(1, Opts.Length);
        }

        public (int, int) RandomPosition()
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

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            return (RandomGamePiece(Opts), RandomPosition());
        }
    }

    public class RandomAgent2 : RandomAgent
    {

        public RandomAgent2(Scoring scoring) : base(scoring)
        {
        }

        public int ChooseGamePiece(int x, int y, GamePiece[]Opts)
        {
            int max = 0;
            int opt = RandomGamePiece(Opts);

            for (int i = 0; i < Opts.Length; i++)
            {
                
                    if (board.EvaluateNeighbors(Opts[i], x - 1, y - 1) > max)
                    {
                        max = board.EvaluateNeighbors(Opts[i], x - 1, y - 1);
                        opt = i;
                    }
                
                
            }

            return opt;
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            (int x, int y) = RandomPosition();
            int opt = RandomGamePiece(Opts);

            int max = 0;

            for (int i = 0;i<Opts.Length; i++)
            {
                if (board.IsEmpty(x - 1, y - 1))
                {
                    if (board.EvaluateNeighbors(Opts[i], x - 1, y - 1) > max)
                    {
                        max = board.EvaluateNeighbors(Opts[i], x - 1, y - 1);
                        opt = i;
                    }
                }
                
            }

            if (max > 0) { return (opt, (x, y)); }
            return (RandomGamePiece(Opts), (x,y));
        }
    }

    // ----------------------------------------------- FIND NEIGHBOR ------------------------------------------------------------

    public class RandomAgentColor : RandomAgent
    {

        public RandomAgentColor(Scoring scoring) : base(scoring)
        {
        }

        public override (int, int) ChoosePosition(GamePiece gp)
        {
            for (int i = 1;i<board.size-1;i++)
            {
                for (int j = 1; j < board.size - 1; j++)
                {
                    if (board.IsEmpty(i, j))
                    {
                        if (board.CheckNeighbors(gp.Color, i, j))
                        {
                            return (i + 1, j + 1);
                        }
                    }
                    
                }
            }

            return (RandomPosition());
        }
        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            // TODO
            int opt = RandomGamePiece(Opts);

            return (RandomGamePiece(Opts), ChoosePosition(Opts[opt]));
        }
    }
    public class RandomAgentPattern : RandomAgent
    {

        public RandomAgentPattern(Scoring scoring) : base(scoring)
        {
        }

        public override (int, int) ChoosePosition(GamePiece gp)
        {
            for (int i = 1; i < board.size - 1; i++)
            {
                for (int j = 1; j < board.size - 1; j++)
                {
                    if (board.IsEmpty(i, j))
                    {
                        if (board.CheckNeighbors(gp.Pattern, i, j))
                        {
                            return (i + 1, j + 1);
                        }
                    }
                    
                }
            }

            return (RandomPosition());
        }
        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            // TODO
            int opt = RandomGamePiece(Opts);

            return (RandomGamePiece(Opts), ChoosePosition(Opts[opt]));
        }
    }

    public class RandomAgentComplet : RandomAgent
    {
        public RandomAgentComplet(Scoring scoring) : base(scoring)
        {
        }


        public override (int, int) ChoosePosition(GamePiece gp)
        {
            for (int i = 1; i < board.size - 1; i++)
            {
                for (int j = 1; j < board.size - 1; j++)
                {
                    if (board.IsEmpty(i, j))
                    {
                        if (board.CheckNeighbors(gp, i, j))
                        {
                            return (i + 1, j + 1);
                        }
                    }
                    
                }
            }

            return (RandomPosition());
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            //TODO
            int opt = RandomGamePiece(Opts);

            return (RandomGamePiece(Opts), ChoosePosition(Opts[opt]));
        }
    }

// ----------------------------------------------- FIND LARGEST SCORE CHANGE ------------------------------------------------------------

    public class AgentColor : Player
    {
        Random random = new Random();

        public AgentColor(Scoring scoring) : base(scoring)
        {
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
                        if (board.IsEmpty(i, j))
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
    public class AgentPattern : Player
    {
        Random random = new Random();

        public AgentPattern(Scoring scoring) : base(scoring)
        {
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
                        if (board.IsEmpty(i, j))
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

    public class AgentComplet : Player
    {
        Random random = new Random();

        public AgentComplet(Scoring scoring) : base(scoring)
        {
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
                        if (board.IsEmpty(i, j))
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
