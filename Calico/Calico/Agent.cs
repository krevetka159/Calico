namespace Calico
{
    #region Random Agent

    public class Agent : Player
    {
        // basic random agent
        public List<Agent> Opponents;
        public void SetOpponent(ref List<Agent> opponents) // for multiplayer 
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
    #endregion

    #region Basic Evaluation

    public class AgentBasicColor : Agent
    {
        /// <summary>
        ///  picks patchtile and position that increases color score the most
        /// </summary>
        /// <param name="scoring"></param>
        public AgentBasicColor(Scoring scoring) : base(scoring)
        {
        }
        public AgentBasicColor(Scoring scoring, int boardId) : base(scoring, boardId)
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
                            if (Board.EvaluatePositionBasicColor(gp, i, j) > max)
                            {
                                maxPieceIndex = o;
                                max = Board.EvaluatePositionBasicColor(gp, i, j);
                                maxPosition = (i, j);
                            }
                        }
                        
                    }
                }
            }

            return (maxPieceIndex,maxPosition);
            
        }
    }
    public class AgentBasicPattern : Agent
    {

        /// <summary>
        ///  picks patchtile and position that increases pattern score the most
        /// </summary>
        /// <param name="scoring"></param>
        public AgentBasicPattern(Scoring scoring) : base(scoring)
        {
        }
        public AgentBasicPattern(Scoring scoring, int boardId) : base(scoring, boardId)
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
                            if (Board.EvaluatePositionBasicPattern(gp, i, j) > max)
                            {
                                maxPieceIndex = o;
                                max = Board.EvaluatePositionBasicPattern(gp, i, j);
                                maxPosition = (i, j);
                            }

                        }
                        
                    }
                }
            }

            return (maxPieceIndex, maxPosition);

        }
    }

    public class AgentBasic : Agent
    {
        /// <summary>
        /// picks patchtile and position that increases score the most
        /// </summary>
        /// <param name="scoring"></param>
        public AgentBasic(Scoring scoring) : base(scoring)
        {
        }
        public AgentBasic(Scoring scoring, int boardId) : base(scoring, boardId)
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
                            if (Board.EvaluatePositionBasic(gp, i, j) > max)
                            {
                                maxPieceIndex = o;
                                max = Board.EvaluatePositionBasic(gp, i, j);
                                maxPosition = (i, j);
                            }
                        }

                    }
                }
            }
            return (maxPieceIndex, maxPosition);


        }
    }

    public class AgentAdvancedRandomness : Agent
    {
        /// <summary>
        /// picks patchtile and position that increases score the most, but with a small probability make random move
        /// </summary>
        /// <param name="scoring"></param>

        public AgentAdvancedRandomness(Scoring scoring) : base(scoring)
        {
        }
        public AgentAdvancedRandomness(Scoring scoring,int boardId) : base(scoring, boardId)
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
                            if (Board.EvaluatePositionAdvanced(gp, i, j) > max)
                            {
                                maxPieceIndex = o;
                                max = Board.EvaluatePositionAdvanced(gp, i, j);
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

    #region Advanced Evaluation
    public class AgentAdvanced : Agent
    {
        /// <summary>
        /// picks patchtile and position that increases score the most
        /// </summary>
        /// <param name="scoring"></param>
        public AgentAdvanced(Scoring scoring) : base(scoring)
        {
        }
        public AgentAdvanced(Scoring scoring, int boardId) : base(scoring,boardId)
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
                            if (Board.EvaluatePositionAdvanced(gp, i, j) > max)
                            {
                                maxPieceIndex = o;
                                max = Board.EvaluatePositionAdvanced(gp, i, j);
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


    #region Tree Search

    public class AgentTreeSearch : AgentWeightedAdvanced
    {

        private int depth;
        private double discount;
        public AgentTreeSearch(Scoring scoring, Weights w) : base(scoring, w) 
        {
        }
        public AgentTreeSearch(Scoring scoring, Weights w, int boardId) : base(scoring,w,boardId)
        {
        }

        public AgentTreeSearch(Scoring scoring, WeightsDict wd) : base(scoring,wd)
        { 
        }

        public AgentTreeSearch(Scoring scoring, WeightsDict wd, int dpth, double dsc) : base(scoring, wd)
        {
            depth = dpth;
            discount = dsc;
        }

        public List<(int,int)> TopPositions(GamePiece gp, int num, GameBoard board)
        {
            List<((int,int),double)> positions = new List<((int,int),double)>(new ((int, int), double)[board.EmptySpotsCount]);

            int index = 0;
            for (int i = 1; i < Board.Size - 1; i++)
            {
                for (int j = 1; j < Board.Size - 1; j++)
                {
                    if (board.IsEmpty(i, j))
                    {
                        positions[index] = ((i,j), board.EvaluatePositionWeightedAdvanced(gp, i, j, weights));
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

                        Parallel.ForEach(TopPositions(Opts[optPermutation.Item1], 10, Board), pos =>
                        {
                            (int i1, int j1) = pos;

                            if (Board.IsEmpty(i1, j1))
                            {
                                
                                double eval = Board.EvaluatePositionWeightedAdvanced(Opts[optPermutation.Item1], i1, j1, weights);
                                GameBoard gb_new = new GameBoard(Board);
                                gb_new.AddPiece(Opts[optPermutation.Item1], i1, j1);

                                if (Board.EmptySpotsCount > 2 && depth == 3)
                                {
                                    foreach ((int, int) pos2 in TopPositions(Opts[optPermutation.Item2], 10, gb_new))
                                    {
                                        (int i2, int j2) = pos2;

                                        
                                        double eval2 = eval + (gb_new.EvaluatePositionWeightedAdvanced(Opts[optPermutation.Item2], i2, j2, weights) * discount);

                                        GameBoard gb_new2 = new GameBoard(gb_new);
                                        gb_new2.AddPiece(Opts[optPermutation.Item2], i2, j2);

                                        for (int i3 = 1; i3 < Board.Size - 1; i3++)
                                        {
                                            for (int j3 = 1; j3 < Board.Size - 1; j3++)
                                            {
                                                if (Board.IsEmpty(i3, j3) && ((i1, j1) != (i3, j3)) && ((i2, j2) != (i3, j3)))
                                                {
                                                    double eval3 = eval2 + (gb_new2.EvaluatePositionWeightedAdvanced(Opts[optPermutation.Item3], i3, j3, weights) * discount * discount);

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
                                else
                                {
                                    for (int i2 = 1; i2 < Board.Size - 1; i2++)
                                    {
                                        for (int j2 = 1; j2 < Board.Size - 1; j2++)
                                        {
                                            if (gb_new.IsEmpty(i2, j2))
                                            {
                                                double eval2 = eval + (gb_new.EvaluatePositionWeightedAdvanced(Opts[optPermutation.Item2], i2, j2, weights) * discount);
                                                if (eval2 > max)
                                                {
                                                    max = eval2;
                                                    maxPosition = (i1, j1);
                                                    pieceIndex = optPermutation.Item1;
                                                }
                                            }
                                        }
                                    }

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
                                var temp = Board.EvaluatePositionWeightedAdvanced(gp, i, j, weights);
                                if (temp > max)
                                {
                                    pieceIndex = o;
                                    max = temp;
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

    public class AgentMCTS : AgentTreeSearch
    {
        protected Bag Bag { get; set; }
        protected int SimulationsCount;
        public AgentMCTS(Scoring scoring, Weights w, Bag b, int s) : base(scoring, w)
        {
            Bag = b;
            SimulationsCount = s;
        }
        public AgentMCTS(Scoring scoring, WeightsDict wd, Bag b, int s) : base(scoring, wd)
        {
            Bag = b;
            SimulationsCount = s;
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
                        positions[index] = ((i, j), board.EvaluatePositionAdvanced(gp, i, j));
                        index++;
                    }
                }
            }

            positions.Sort((a, b) => b.Item2.CompareTo(a.Item2));

            return positions.Select(item => item.Item1).Take(5).ToList();
        }

        public double Simulation(GameBoard gb, GamePiece[] Opts)
        {
            int[] scores = new int[SimulationsCount];
            Parallel.For(0, SimulationsCount, i =>
            {
                SimulationGame sg = new SimulationGame(new GameBoard(gb), Opts, gb.ScoreCounter.Scoring, Bag);
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

                            GameBoard gb_new = new GameBoard(Board);
                            gb_new.AddPiece(Opts[optPermutation.Item1], i1, j1);

                                
                            Parallel.ForEach(TopPositions(Opts[optPermutation.Item2], gb_new), pos1 =>
                            {
                                (int i2, int j2) = pos1;

                                    if (Board.EmptySpotsCount > 2)
                                    {
                                        GameBoard gb_new2 = new GameBoard(gb_new);
                                        gb_new2.AddPiece(Opts[optPermutation.Item2], i2, j2);

                                        Parallel.ForEach(TopPositions(Opts[optPermutation.Item3], gb_new2), pos2 =>
                                        {
                                            (int i3, int j3) = pos2;
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
                                                    double eval = gb_new2.EvaluatePositionAdvanced(Opts[optPermutation.Item3], i3, j3);
                                                    if (eval > max)
                                                    {
                                                        pieceIndex = optPermutation.Item1;
                                                        max = eval;
                                                        maxPosition = (i1, j1);
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
                                        double eval = gb_new.EvaluatePositionAdvanced(Opts[optPermutation.Item2], i2, j2);
                                        if (eval > max)
                                        {
                                            pieceIndex = optPermutation.Item1;
                                            max = eval;
                                            maxPosition = (i1, j1);
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
                                if (Board.EvaluatePositionAdvanced(gp, i, j) > max)
                                {
                                    pieceIndex = o;
                                    max = Board.EvaluatePositionAdvanced(gp, i, j);
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

    #region Evolution

    public class AgentWeightedAdvanced : Agent
    {
        protected Weights weights;
        public AgentWeightedAdvanced(Scoring scoring, Weights w) : base(scoring)
        {
            weights = w;
        }
        public AgentWeightedAdvanced(Scoring scoring, Weights w, int boardId) : base(scoring, boardId)
        {
            weights = w;
        }

        public AgentWeightedAdvanced(Scoring scoring, WeightsDict wd) : base(scoring)
        {
            ChooseTaskPieces();
            weights = wd.GetWeights((TaskIds[0], TaskIds[1], TaskIds[2]));
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
                            double eval = Board.EvaluatePositionWeightedAdvanced(gp, i, j, weights);
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
