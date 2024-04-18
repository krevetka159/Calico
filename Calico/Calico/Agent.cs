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
        public List<Agent> Opponents;
        public void SetOpponent(ref List<Agent> opponents)
        {
            Opponents = opponents;
        }

        public Random random = new Random();

        /// <summary>
        /// Random patchtile, random position
        /// </summary>
        /// <param name="scoring"></param>
        public Agent(Scoring scoring) : base(scoring)
        {
        }

        /// <summary>
        /// Choose a random patchtile from options
        /// </summary>
        /// <param name="Opts"></param>
        /// <returns></returns>
        public int RandomGamePiece(GamePiece[] Opts)
        {
            return random.Next(0, Opts.Length-1);
        }

        /// <summary>
        /// Choose at random an empty position on the gameboard
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Agent makes a move
        /// </summary>
        /// <param name="Opts"></param>
        /// <returns></returns>
        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            return (RandomGamePiece(Opts), RandomPosition());
        }

        public override void ChooseTaskPieces()
        {
            int taskId = random.Next(1,6);

            Board.AddTaskPiece(taskId, 0);
            TaskIds[0] = taskId;

            while (taskId == TaskIds[0])
            {
                taskId = random.Next(1,6);
            }

            Board.AddTaskPiece(taskId, 1);
            TaskIds[1] = taskId;

            while (taskId == TaskIds[0] || taskId == TaskIds[1])
            {
                taskId = random.Next(1, 6);
            }

            Board.AddTaskPiece(taskId, 2);
            TaskIds[2] = taskId;
        }

        public void AddTaskPieces(int a, int b, int c)
        {
            Board.AddTaskPiece(a, 0);
            Board.AddTaskPiece(b, 1);
            Board.AddTaskPiece(c, 2);
        }
    }


// ----------------------------------------------- BEST FOR RANDOM POSITION ------------------------------------------------------------
    public class RandomPositionAgent : Agent
    {
        /// <summary>
        /// random position, then looks for best possible patch tile
        /// </summary>
        /// <param name="scoring"></param>
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

    public class RandomPatchTileAgent : Agent
    {
        /// <summary>
        /// random patch tile, then looks for best possible position
        /// </summary>
        /// <param name="scoring"></param>
        public RandomPatchTileAgent(Scoring scoring) : base(scoring)
        {
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            (int x, int y) = RandomPosition();
            int opt = RandomGamePiece(Opts);

            int max = 0;

            for (int i = 1; i < Board.Size - 1; i++)
            {
                for (int j = 1; j < Board.Size - 1; j++)
                {
                    if (Board.IsEmpty(i, j))
                    {
                        if (Board.EvaluateNeighbors(Opts[opt], i, j) > max)
                        {
                            max = Board.EvaluateNeighbors(Opts[opt], i, j);
                            (x, y) = (i, j);
                        }
                    }

                }
            }

            return (opt, (x, y));
        }
    }

    // ----------------------------------------------- FIND NEIGHBOR ------------------------------------------------------------

    public class RandomAgentColor : Agent
    {
        /// <summary>
        /// random patch tile, finds first position with the same color neighbor
        /// </summary>
        /// <param name="scoring"></param>
        public RandomAgentColor(Scoring scoring) : base(scoring)
        {
        }

        private (int, int) ChoosePosition(GamePiece gp)
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

        /// <summary>
        /// random patch tile, finds first position with the same color neighbor
        /// </summary>
        /// <param name="scoring"></param>
        public RandomAgentPattern(Scoring scoring) : base(scoring)
        {
        }

        private (int, int) ChoosePosition(GamePiece gp)
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

    public class RandomAgentComplete : Agent
    {
        /// <summary>
        /// random patch tile, finds first position with the same color or pattern neighbor
        /// </summary>
        /// <param name="scoring"></param>
        public RandomAgentComplete(Scoring scoring) : base(scoring)
        {
        }


        private (int, int) ChoosePosition(GamePiece gp)
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
        /// <summary>
        ///  picks patchtile and position that increases color score the most
        /// </summary>
        /// <param name="scoring"></param>
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

        /// <summary>
        ///  picks patchtile and position that increases pattern score the most
        /// </summary>
        /// <param name="scoring"></param>
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

    public class AgentComplete : Agent
    {
        /// <summary>
        /// picks patchtile and position that increases score the most
        /// </summary>
        /// <param name="scoring"></param>
        public AgentComplete(Scoring scoring) : base(scoring)
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
    }
    public class AgentCompleteWithUtility : Agent
    {
        /// <summary>
        /// picks patchtile and position that increases score the most
        /// </summary>
        /// <param name="scoring"></param>
        public AgentCompleteWithUtility(Scoring scoring) : base(scoring)
        {
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            int maxPieceIndex = RandomGamePiece(Opts);
            double max = 0;
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
                            if (Board.EvaluateNeighborsUtility(gp, i, j) > max)
                            {
                                maxPieceIndex = o;
                                max = Board.EvaluateNeighborsUtility(gp, i, j);
                                maxPosition = (i, j);
                            }
                        }

                    }
                }
            }
            return (maxPieceIndex, maxPosition);


        }
    }
    public class AgentCompleteWithProb : Agent
        {
            /// <summary>
            /// picks patchtile and position that increases score the most, but with a small probability make random move
            /// </summary>
            /// <param name="scoring"></param>

            public AgentCompleteWithProb(Scoring scoring) : base(scoring)
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

    // ----------------------------------------------- MULTIPLAYER AGENT ------------------------------------------------------------
    public class TwoPlayerAgent : Agent
    {
        /// <summary>
        /// choose the piece that AgentComplete opponent whould choose and pick the position that increases the score the most
        /// </summary>
        /// <param name="scoring"></param>
        public TwoPlayerAgent(Scoring scoring) : base(scoring)
        {
        }

        private int FindOpponentsBestPiece(GamePiece[] Opts)
        {
            int maxPieceIndex = RandomGamePiece(Opts);
            int max = 0;

            for (int o = 0; o < Opts.Length; o++)
            {
                foreach (Agent opponent in Opponents)
                {
                    GamePiece gp = Opts[o];
                    for (int i = 1; i < opponent.Board.Size - 1; i++)
                    {
                        for (int j = 1; j < opponent.Board.Size - 1; j++)
                        {
                            if (opponent.Board.IsEmpty(i, j))
                            {
                                if (opponent.Board.EvaluateNeighbors(gp, i, j) > max)
                                {
                                    maxPieceIndex = o;
                                    max = opponent.Board.EvaluateNeighbors(gp, i, j);
                                }
                            }
                        }
                    }
                }
            }
            return (maxPieceIndex);
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            int max = 0;
            (int, int) maxPosition = RandomPosition();
            int pieceIndex = FindOpponentsBestPiece(Opts);
            GamePiece gp = Opts[pieceIndex];
                for (int i = 1; i < Board.Size - 1; i++)
                {
                    for (int j = 1; j < Board.Size - 1; j++)
                    {
                        if (Board.IsEmpty(i, j))
                        {
                            if (Board.EvaluateNeighbors(gp, i, j) > max)
                            {
                                max = Board.EvaluateNeighbors(gp, i, j);
                                maxPosition = (i, j);
                            }
                        }
                    }
                }
            return (pieceIndex, maxPosition);
        }
    }


    public class MinimaxAgent : Agent
    {
        public MinimaxAgent(Scoring scoring) : base(scoring) 
        {
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            double max = 0;
            (int, int) maxPosition = RandomPosition();
            int pieceIndex = RandomGamePiece(Opts);

            // pro každou permutaci opts udělám následující: pro každý empty spot na desce udělám kopii desky a pošlu ji do dalšího forcyklu

            List<(int, int, int)> perms = new List<(int, int, int)>() { (0, 1, 2), (0, 2, 1), (1, 0, 2), (1, 2, 0), (2, 0, 1), (2, 1, 0) };

            foreach ((int,int,int) optPermutation in perms)
            {
                for (int i1 = 1; i1 < Board.Size - 1; i1++)
                {
                    for (int j1=1; j1 < Board.Size -1; j1++)
                    {
                        if (Board.IsEmpty(i1, j1))
                        {

                            for (int i2 = 1; i2 < Board.Size - 1; i2++)
                            {
                                for (int j2 = 1; j2 < Board.Size - 1; j2++)
                                {
                                    if (Board.IsEmpty(i2, j2) && (i1, j1) != (i2, j2))
                                    {

                                        for (int i3 = 1; i3 < Board.Size - 1; i3++)
                                        {
                                            for (int j3 = 1; j3 < Board.Size - 1; j3++)
                                            {
                                                if (Board.IsEmpty(i3, j3) && (i1, j1) != (i3, j3) && (i2, j2) != (i3, j3))
                                                {
                                                    List<(GamePiece,(int,int))> toAdd = new List<(GamePiece, (int, int))>()
                                                    {
                                                        (Opts[optPermutation.Item1], (i1, j1)),
                                                        (Opts[optPermutation.Item2], (i2, j2)),
                                                        (Opts[optPermutation.Item3], (i3, j3))
                                                    };

                                                    double eval = Board.EvaluateMinimax(toAdd);
                                                    if (eval > max)
                                                    {
                                                        max = eval;
                                                        maxPosition = (i1, j1);
                                                        pieceIndex = optPermutation.Item1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }


            return (pieceIndex, maxPosition);
        }
    }

    public class EvolutionAgent : Agent
    {
        EvolutionGameProps gameProps;
        public EvolutionAgent(Scoring scoring, EvolutionGameProps e) : base(scoring)
        {
            gameProps = e;
        }

        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            int maxPieceIndex = RandomGamePiece(Opts);
            double max = 0;
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
                            double eval = Board.EvaluateNeighborsEvolution(gp, i, j, gameProps);
                            if (eval > max)
                            {
                                maxPieceIndex = o;
                                max = eval;
                                maxPosition = (i, j);
                            }
                        }

                    }
                }
            }
            return (maxPieceIndex, maxPosition);

        }
    }
}
