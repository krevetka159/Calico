﻿using System;
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

        public Agent(Scoring scoring, int boardId) : base(scoring,boardId)
        {
        }
        public Agent(Scoring scoring, Bag b) : base(scoring, b)
        {
        }

        public Agent(Scoring scoring, int boardId, Bag b) : base(scoring, boardId, b)
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
            int taskId = random.Next(1,7);

            Board.AddTaskPiece(taskId, 0);
            TaskIds[0] = taskId;

            while (taskId == TaskIds[0])
            {
                taskId = random.Next(1,7);
            }

            Board.AddTaskPiece(taskId, 1);
            TaskIds[1] = taskId;

            while (taskId == TaskIds[0] || taskId == TaskIds[1])
            {
                taskId = random.Next(1, 7);
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
        public RandomPositionAgent(Scoring scoring,int boardId) : base(scoring,boardId)
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
        public RandomPatchTileAgent(Scoring scoring, int boardId) : base(scoring,boardId)
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
        public RandomAgentColor(Scoring scoring, int boardId) : base(scoring,boardId)
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
        public RandomAgentPattern(Scoring scoring, int boardId) : base(scoring,boardId)
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
        public RandomAgentComplete(Scoring scoring, int boardId) : base(scoring, boardId)
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

    #region Basic Evaluation

    public class AgentColor : Agent
    {
        /// <summary>
        ///  picks patchtile and position that increases color score the most
        /// </summary>
        /// <param name="scoring"></param>
        public AgentColor(Scoring scoring) : base(scoring)
        {
        }
        public AgentColor(Scoring scoring, int boardId) : base(scoring, boardId)
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
                            if (Board.EvaluateNeighborsColorFixed(gp, i, j) > max)
                            {
                                maxPieceIndex = o;
                                max = Board.EvaluateNeighborsColorFixed(gp, i, j);
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
        public AgentPattern(Scoring scoring, int boardId) : base(scoring, boardId)
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
                            if (Board.EvaluateNeighborsPatternFixed(gp, i, j) > max)
                            {
                                maxPieceIndex = o;
                                max = Board.EvaluateNeighborsPatternFixed(gp, i, j);
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
        public AgentComplete(Scoring scoring, int boardId) : base(scoring, boardId)
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

    public class AgentCompleteWithProb : Agent
    {
        /// <summary>
        /// picks patchtile and position that increases score the most, but with a small probability make random move
        /// </summary>
        /// <param name="scoring"></param>

        public AgentCompleteWithProb(Scoring scoring) : base(scoring)
        {
        }
        public AgentCompleteWithProb(Scoring scoring,int boardId) : base(scoring, boardId)
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

    #endregion

    #region Utility Evaluation
    public class AgentCompleteWithUtility : Agent
    {
        /// <summary>
        /// picks patchtile and position that increases score the most
        /// </summary>
        /// <param name="scoring"></param>
        public AgentCompleteWithUtility(Scoring scoring) : base(scoring)
        {
        }
        public AgentCompleteWithUtility(Scoring scoring, int boardId) : base(scoring,boardId)
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
    public class AgentCompleteWithUtilityParams : Agent
    {
        //public UtilityConsts ep = new UtilityConsts(1.137, (1.180, 1.084, 0.985), new[]{ 1.117, 1.039, 1.062, 0.634, 1.019, 0.744 });
        public UtilityConsts ep = new UtilityConsts(1.14298, (1.21723, 1.13056, 1.06412), new[] { 1.09177, 0.91472, 1.02152, 0.51955, 1.05668, 0.84085 });
        /// <summary>
        /// picks patchtile and position that increases score the most
        /// </summary>
        /// <param name="scoring"></param>
        public AgentCompleteWithUtilityParams(Scoring scoring) : base(scoring)
        {
        }
        public AgentCompleteWithUtilityParams(Scoring scoring, int boardId) : base(scoring, boardId)
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
                            var eval = Board.EvaluateNeighborsUtilityBetter(gp, i, j, ep);
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

    #endregion

    #region Opponents best
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

    #endregion


    #region Few Steps Ahead

    public class MinimaxAgent : Agent
    {
        public UtilityConsts uc = new UtilityConsts(1.08074, (1.18975, 0.99833, 0.96349), new[] { 1.27325, 0.98524, 0.96371, 0.69187, 1.00705, 0.84656 });
        public MinimaxAgent(Scoring scoring) : base(scoring) 
        {
        }
        public MinimaxAgent(Scoring scoring, int boardId) : base(scoring,boardId)
        {
        }

        public List<(int,int)> TopPositions(GamePiece gp, int num, GameBoard board)
        {
            List<((int,int),double)> positions = new List<((int,int),double)>(new ((int, int), double)[board.EmptySpotsCount]);

            int index = 0;
            for (int i = 1; i < Board.Size - 1; i++)
            {
                for (int j = 1; j < Board.Size - 1; j++)
                {
                    if (Board.IsEmpty(i, j))
                    {
                        positions[index] = ((i,j), board.EvaluateNeighborsUtilityBetter(gp, i, j, uc));
                        index++;
                    }
                }
            }

            positions.Sort((a, b) => b.Item2.CompareTo(a.Item2));

            return positions.Select(item => item.Item1).Take(Math.Max((board.EmptySpotsCount+1)/2,5)).ToList();
        }


        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            double max = 0;
            (int, int) maxPosition = RandomPosition();
            int pieceIndex = RandomGamePiece(Opts);

            // pro každou permutaci opts udělám následující: pro každý empty spot na desce udělám kopii desky a pošlu ji do dalšího forcyklu

            List<(int, int, int)> perms = new List<(int, int, int)>() { (0, 1, 2), (0, 2, 1), (1, 0, 2), (1, 2, 0), (2, 0, 1), (2, 1, 0) };

            if (Board.EmptySpotsCount > 1)
            {

                foreach ((int, int, int) optPermutation in perms)
                {
                    //Parallel.For(1, Board.Size - 1, i1 =>
                    //{
                    //    Parallel.For(1, Board.Size - 1, j1 =>
                    //    {
                        Parallel.ForEach(TopPositions(Opts[optPermutation.Item1], 10, Board), pos =>
                        {
                            (int i1, int j1) = pos;

                            if (Board.IsEmpty(i1, j1))
                            {
                                GameBoard gb_new = new GameBoard(Board);
                                double eval = gb_new.EvaluateNeighborsUtilityBetter(Opts[optPermutation.Item1], i1, j1, uc);
                                gb_new.AddPiece(Opts[optPermutation.Item1], i1, j1);

                                for (int i2 = 1; i2 < Board.Size - 1; i2++)
                                {
                                    for (int j2 = 1; j2 < Board.Size - 1; j2++)
                                    {
                                        if (Board.IsEmpty(i2, j2) && ((i1, j1) != (i2, j2)))
                                        {

                                            //Parallel.For(1, Board.Size - 1, i3 =>
                                            //{
                                            //    Parallel.For(1, Board.Size - 1, j3 =>
                                            //    {
                                            //        if (Board.IsEmpty(i3, j3) && ((i1, j1) != (i3, j3)) && ((i2, j2) != (i3, j3)))
                                            //        {


                                            GameBoard gb_new2 = new GameBoard(gb_new);
                                            double eval2 = eval + (gb_new2.EvaluateNeighborsUtilityBetter(Opts[optPermutation.Item2], i2, j2, uc) / 4);

                                            //if (eval2 > max)
                                            //{
                                            //    max = eval2;
                                            //    maxPosition = (i1, j1);
                                            //    pieceIndex = optPermutation.Item1;
                                            //}
                                            gb_new2.AddPiece(Opts[optPermutation.Item2], i2, j2);

                                            for (int i3 = 1; i3 < Board.Size - 1; i3++)
                                            {
                                                for (int j3 = 1; j3 < Board.Size - 1; j3++)
                                                {
                                                    if (Board.IsEmpty(i3, j3) && ((i1, j1) != (i3, j3)) && ((i2, j2) != (i3, j3)))
                                                    {

                                                        GameBoard gb_new3 = new GameBoard(gb_new2);
                                                        double eval3 = eval2 + (gb_new3.EvaluateNeighborsUtilityBetter(Opts[optPermutation.Item3], i3, j3,uc) / 16);

                                                        if (eval3 > max)
                                                        {
                                                            max = eval3;
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
                        });
                    //    });
                    //});
                }

            }

            else 
            {
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
                                    pieceIndex = o;
                                    max = Board.EvaluateNeighborsUtility(gp, i, j);
                                    maxPosition = (i, j);
                                }
                            }

                        }
                    }
                }
            }
            return (pieceIndex, maxPosition);
        }
    }

    #endregion

    public class MonteCarloAgent : Agent
    {
        public MonteCarloAgent(Scoring scoring, Bag b) : base(scoring, b)
        {
        }
        public MonteCarloAgent(Scoring scoring, int boardId, Bag b) : base(scoring, boardId, b)
        {
        }

        public List<(int, int)> TopPositions(GamePiece gp, GameBoard board)
        {
            List<((int, int), double)> positions = new List<((int, int), double)>(new ((int, int), double)[board.EmptySpotsCount]);

            int index = 0;
            for (int i = 1; i < board.Size - 1; i++)
            {
                for (int j = 1; j < board.Size - 1; j++)
                {
                    if (board.IsEmpty(i, j))
                    {
                        positions[index] = ((i, j), board.EvaluateNeighborsUtility(gp, i, j));
                        index++;
                    }
                }
            }

            positions.Sort((a, b) => b.Item2.CompareTo(a.Item2));

            return positions.Select(item => item.Item1).Take(5).ToList();
        }

        public double Simulation(GameBoard gb, GamePiece[] Opts)
        {
            int[] scores = new int[100];
            Parallel.For(0, 100, i =>
            {
                SimulationGame sg = new SimulationGame(new GameBoard(gb), Opts, gb.ScoreCounter.Scoring, bag);
                scores[i] = sg.Game();
            });
            return scores.Average();
        }


        public override (int, (int, int)) ChooseNextMove(GamePiece[] Opts)
        {
            double max = 0;
            (int, int) maxPosition = RandomPosition();
            int pieceIndex = RandomGamePiece(Opts);

            // pro každou permutaci opts udělám následující: pro každý empty spot na desce udělám kopii desky a pošlu ji do dalšího forcyklu

            List<(int, int, int)> perms = new List<(int, int, int)>() { (0, 1, 2), (0, 2, 1), (1, 0, 2), (1, 2, 0), (2, 0, 1), (2, 1, 0) };

            if (Board.EmptySpotsCount > 1)
            {
                foreach ((int, int, int) optPermutation in perms)
                {
                    Parallel.ForEach(TopPositions(Opts[optPermutation.Item1], Board), pos =>
                    {
                        (int i1, int j1) = pos;

                        if (Board.IsEmpty(i1, j1))
                        {
                            GameBoard gb_new = new GameBoard(Board);
                            gb_new.AddPiece(Opts[optPermutation.Item1], i1, j1);

                                
                            Parallel.ForEach(TopPositions(Opts[optPermutation.Item2], gb_new), pos1 =>
                            {
                                (int i2, int j2) = pos1;
                                if (Board.IsEmpty(i2, j2) && ((i1, j1) != (i2, j2)))
                                {
                                    if (Board.EmptySpotsCount > 2)
                                    {
                                        GameBoard gb_new2 = new GameBoard(gb_new);
                                        gb_new2.AddPiece(Opts[optPermutation.Item2], i2, j2);

                                        Parallel.ForEach(TopPositions(Opts[optPermutation.Item3], gb_new2), pos2 =>
                                        {
                                            (int i3, int j3) = pos2;
                                            if (Board.IsEmpty(i3, j3) && ((i1, j1) != (i3, j3)) && ((i2, j2) != (i3, j3)))
                                            {
                                                if (Board.EmptySpotsCount > 3)
                                                {
                                                    GameBoard gb_new3 = new GameBoard(gb_new2);
                                                    gb_new3.AddPiece(Opts[optPermutation.Item3], i3, j3);

                                                    // SIMULACE

                                                    double eval = Simulation(gb_new3, new GamePiece[0]);

                                                    if (eval > max)
                                                    {
                                                        max = eval;
                                                        maxPosition = (i1, j1);
                                                        pieceIndex = optPermutation.Item1;
                                                    }
                                                }
                                                else
                                                {
                                                    double eval = gb_new2.EvaluateNeighborsUtility(Opts[optPermutation.Item3], i3, j3);
                                                    if (eval > max)
                                                    {
                                                        pieceIndex = optPermutation.Item1;
                                                        max = eval;
                                                        maxPosition = (i1, j1);
                                                    }
                                                }

                                            }
                                        });

                                        double eval = Simulation(gb_new2, new GamePiece[1] { Opts[optPermutation.Item3] });

                                        if (eval > max)
                                        {
                                            max = eval;
                                            maxPosition = (i1, j1);
                                            pieceIndex = optPermutation.Item1;
                                        }
                                    }

                                    else
                                    {
                                        double eval = gb_new.EvaluateNeighborsUtility(Opts[optPermutation.Item2], i2, j2);
                                        if (eval > max)
                                        {
                                            pieceIndex = optPermutation.Item1;
                                            max = eval;
                                            maxPosition = (i1, j1);
                                        }
                                    }
                                    
                                }
                            });

                            double eval = Simulation(gb_new, new GamePiece[2] { Opts[optPermutation.Item2], Opts[optPermutation.Item3] });

                            if (eval > max)
                            {
                                max = eval;
                                maxPosition = (i1, j1);
                                pieceIndex = optPermutation.Item1;
                            }
                        }
                    });
                }
            }

            else
            {
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
                                    pieceIndex = o;
                                    max = Board.EvaluateNeighborsUtility(gp, i, j);
                                    maxPosition = (i, j);
                                }
                            }

                        }
                    }
                }
            }
            return (pieceIndex, maxPosition);
        }
    }

    #region Evolution

    public class EvolutionAgent : Agent
    {
        UtilityConsts gameProps;
        public EvolutionAgent(Scoring scoring, UtilityConsts e) : base(scoring)
        {
            gameProps = e;
        }
        public EvolutionAgent(Scoring scoring, UtilityConsts e, int boardId) : base(scoring, boardId)
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

    #endregion
}
