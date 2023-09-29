using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
// ----------------------------------------------- RANDOM AGENT ------------------------------------------------------------

    public class Agent : Player
    {
        // basic random agent

        public Random random = new Random();

        public Agent(Scoring scoring) : base(scoring)
        {
        }

        public int RandomGamePiece(GamePiece[] Opts)
        {
            return random.Next(1, Opts.Length);
        }

        public (int, int) RandomPosition()
        {
            int row = random.Next(0, Board.Size - 1);
            int col = random.Next(0, Board.Size - 1);

            while (!Board.IsEmpty(row, col))
            {
                row = random.Next(0, Board.Size - 1);
                col = random.Next(0, Board.Size - 1);
            }

            return (row, col);
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            return (RandomGamePiece(Opts), RandomPosition());
        }
    }

    public class RandomPositionAgent : Agent
    {
        // random position, then looks for best possible patch tile
        public RandomPositionAgent(Scoring scoring) : base(scoring)
        {
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            (int x, int y) = RandomPosition();
            int opt = RandomGamePiece(Opts);

            int max = 0;

            for (int i = 0;i<Opts.Length; i++)
            {
                if (Board.IsEmpty(x, y))
                {
                    if (Board.EvaluateNeighbors(Opts[i], x, y) > max)
                    {
                        max = Board.EvaluateNeighbors(Opts[i], x, y);
                        opt = i;
                    }
                }
                
            }

            return (opt, (x, y)); 
        }
    }

    // ----------------------------------------------- FIND NEIGHBOR ------------------------------------------------------------

    public class RandomAgentColor : Agent
    {
        // random patch tile, finds first position with the same color neighbor
        public RandomAgentColor(Scoring scoring) : base(scoring)
        {
        }

        public (int, int) ChoosePosition(GamePiece gp)
        {
            for (int i = 1;i<Board.Size-1;i++)
            {
                for (int j = 1; j < Board.Size - 1; j++)
                {
                    if (Board.IsEmpty(i, j))
                    {
                        if (Board.CheckNeighborsColor(gp.Color, i, j))
                        {
                            return (i, j);
                        }
                    }
                    
                }
            }

            return RandomPosition();
        }
        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            
            int opt = RandomGamePiece(Opts);

            return (RandomGamePiece(Opts), ChoosePosition(Opts[opt]));
        }
    }
    public class RandomAgentPattern : Agent
    {
        // random patch tile, finds first position with the same pattern neighbor
        public RandomAgentPattern(Scoring scoring) : base(scoring)
        {
        }

        public (int, int) ChoosePosition(GamePiece gp)
        {
            for (int i = 1; i < Board.Size - 1; i++)
            {
                for (int j = 1; j < Board.Size - 1; j++)
                {
                    if (Board.IsEmpty(i, j))
                    {
                        if (Board.CheckNeighborsPattern(gp.Pattern, i, j))
                        {
                            return (i, j);
                        }
                    }
                    
                }
            }

            return (RandomPosition());
        }
        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            
            int opt = RandomGamePiece(Opts);

            return (RandomGamePiece(Opts), ChoosePosition(Opts[opt]));
        }
    }

    public class RandomAgentComplet : Agent
    {
        // random patch tile, finds first position with the same color or pattern neighbor
        public RandomAgentComplet(Scoring scoring) : base(scoring)
        {
        }


        public (int, int) ChoosePosition(GamePiece gp)
        {
            for (int i = 1; i < Board.Size - 1; i++)
            {
                for (int j = 1; j < Board.Size - 1; j++)
                {
                    if (Board.IsEmpty(i, j))
                    {
                        if (Board.CheckNeighbors(gp, i, j))
                        {
                            return (i, j);
                        }
                    }
                    
                }
            }

            return (RandomPosition());
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
           
            int opt = RandomGamePiece(Opts);

            return (RandomGamePiece(Opts), ChoosePosition(Opts[opt]));
        }
    }

// ----------------------------------------------- FIND LARGEST SCORE CHANGE ------------------------------------------------------------

    public class AgentColor : Agent
    {
        // picks patchtile and position that increases color score the most
        // -> first that does or first neighbor if it can't increase score
        public AgentColor(Scoring scoring) : base(scoring)
        {
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            int maxPieceIndex = RandomGamePiece(Opts);
            int max = 0;
            (int, int) maxPosition = RandomPosition();

            for (int o = 0; o < Opts.Length;o++)
            {
                GamePiece gp = Opts[o];
                for (int i = 1; i < Board.Size - 1; i++)
                {
                    for (int j = 1; j < Board.Size - 1; j++)
                    {
                        if (Board.IsEmpty(i, j))
                        {
                            if (Board.EvaluateNeighborsColor(gp, i, j) > max)
                            {
                                maxPieceIndex = o;
                                max = Board.EvaluateNeighborsColor(gp, i, j);
                                maxPosition = (i, j);
                            }
                        }
                        
                    }
                }
            }

            return (maxPieceIndex,maxPosition);
            
        }
    }
    public class AgentPattern : Agent
    {
        // picks patchtile and position that increases pattern score the most
        // -> first that does or first neighbor if it can't increase score
        public AgentPattern(Scoring scoring) : base(scoring)
        {
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            int maxPieceIndex = RandomGamePiece(Opts);
            int max = 0;
            (int, int) maxPosition = RandomPosition();

            for (int o = 0; o < Opts.Length; o++)
            {
                GamePiece gp = Opts[o];
                for (int i = 1; i < Board.Size - 1; i++)
                {
                    for (int j = 1; j < Board.Size - 1; j++)
                    {
                        if (Board.IsEmpty(i, j))
                        {
                            if (Board.EvaluateNeighborsPattern(gp, i, j) > max)
                            {
                                maxPieceIndex = o;
                                max = Board.EvaluateNeighborsPattern(gp, i, j);
                                maxPosition = (i, j);
                            }

                        }
                        
                    }
                }
            }

            return (maxPieceIndex, maxPosition);

        }
    }

    public class AgentComplet : Agent
    {
        // picks patchtile and position that increases score the most
        // -> first that does or first neighbor if it can't increase score
        public AgentComplet(Scoring scoring) : base(scoring)
        {
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            int maxPieceIndex = RandomGamePiece(Opts);
            int max = 0;
            (int, int) maxPosition = RandomPosition();

            for (int o = 0; o < Opts.Length; o++)
            {
                GamePiece gp = Opts[o];
                for (int i = 1; i < Board.Size - 1; i++)
                {
                    for (int j = 1; j < Board.Size - 1; j++)
                    {
                        if (Board.IsEmpty(i, j))
                        {
                            if (Board.EvaluateNeighbors(gp, i, j) > max)
                            {
                                maxPieceIndex = o;
                                max = Board.EvaluateNeighbors(gp, i, j);
                                maxPosition = (i, j);
                            }
                        }
                        
                    }
                }
            }
            return (maxPieceIndex, maxPosition);


        }

        public class AgentCompletWithProb : Agent
        {
            // picks patchtile and position that increases score the most
            // -> first that does or first neighbor if it can't increase score
            // with a small probability make random move

            public AgentCompletWithProb(Scoring scoring) : base(scoring)
            {
            }

            public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
            {
                int maxPieceIndex = RandomGamePiece(Opts);
                int max = 0;
                (int, int) maxPosition = RandomPosition();

                for (int o = 0; o < Opts.Length; o++)
                {
                    GamePiece gp = Opts[o];
                    for (int i = 1; i < Board.Size - 1; i++)
                    {
                        for (int j = 1; j < Board.Size - 1; j++)
                        {
                            if (Board.IsEmpty(i, j))
                            {
                                if (Board.EvaluateNeighbors(gp, i, j) > max)
                                {
                                    maxPieceIndex = o;
                                    max = Board.EvaluateNeighbors(gp, i, j);
                                    maxPosition = (i, j);
                                }
                            }

                        }
                    }
                }
                if (random.NextDouble() > 0.05)
                {
                    return (maxPieceIndex, maxPosition);
                }
                return (RandomGamePiece(Opts), RandomPosition());



            }

        }
    }


}
