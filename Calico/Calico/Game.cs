using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Calico.AgentComplete;
using System.Collections;

namespace Calico
{
    public class Game
    {
        public Bag bag;
        public Player player;
        public Agent agent;
        private List<Agent> multiAgents;
        public GamePiece[] Opts = new GamePiece[3];
        public Scoring scoring;
        public GameStatePrinter gameStatePrinter;
        public GameStats Stats;

        private List<(int,string)> AgentDescription = new List<(int, string)>()
        {
            (1, " Kompletně náhodný agent "),
            (2, " Nejlepší umístění vzhledek k barvám "),
            (3, " Nejlepší umístění vzhledem ke vzorům "),
            (4, " Nejlepší umístění "),
            (5, " Nejlepší umístění s malou náhodou "),
            (6, " Nejlepší umístění náhodného dílku "),
            (7, " Utility fce"),
            (8, " Minimax"),
            (9, " MC"),
            (10, "EvolParams")
        };

        public Game()
        {
            bag = new Bag();
            for (int i = 0; i < 3; i++)
            {
                Opts[i] = bag.Next();
            }

            scoring = new Scoring();

            gameStatePrinter = new GameStatePrinter(scoring);
        }

        public Game(int agentType, bool withPrint)
        {
            bag = new Bag();
            for (int i = 0; i < 3; i++)
            {
                Opts[i] = bag.Next();
            }

            scoring = new Scoring();

            gameStatePrinter = new GameStatePrinter(scoring);

            agent = UseAgent(agentType);
            agent.ChooseTaskPieces();
        }
        

    #region SinglePlayer

        public void SinglePlayer()
        {
            for (int i = 0; i < 3; i++)
            {
                Opts[i] = bag.Next();
            }
            player = new Player(scoring);

            //print empty
            gameStatePrinter.PrintTaskChoosing(player, Opts);
            player.ChooseTaskPieces();

            gameStatePrinter.PrintStateSingle(player, Opts);


            for (int i = 0; i < 22; i++)
            {
                MakeMove(player);
                // update points
                gameStatePrinter.PrintStateSingle(player, Opts);

            }
            gameStatePrinter.PrintStats(player);
            gameStatePrinter.PrintDetailedStatsToCSV(player);
        }
        #endregion

    #region MultiPlayer
        public void MultiPlayer() 
        {
            player = new Player(scoring);
            agent = new AgentComplete(scoring);

            //print empty
            gameStatePrinter.PrintStateMulti(player, agent, Opts);

            for (int i = 0; i < 22; i++)
            {
                MakeMove(player);

                gameStatePrinter.PrintStateMulti(player, agent, Opts);

                MakeMove(agent);
                gameStatePrinter.PrintStateMulti(player, agent, Opts);

            }

            gameStatePrinter.PrintStats(player);
            gameStatePrinter.PrintStats(agent);
        }

        #endregion

    #region Testing

        #region AgentInit
        public Agent UseAgent(int agentOption)
        {
            switch (agentOption)
            {
                case 1:
                    {
                        return new Agent(scoring);
                    }
                case 2:
                    {
                        return new AgentColor(scoring);
                    }
                case 3:
                    {
                        return new AgentPattern(scoring);
                    }
                case 4:
                    {
                        return new AgentComplete(scoring);
                    }
                case 5:
                    {
                        return new AgentCompleteWithProb(scoring);
                    }
                case 6:
                    {
                        return new RandomPatchTileAgent(scoring);
                    }
                case 7:
                    {
                        return new AgentCompleteWithUtility(scoring);
                    }
                case 8:
                    {
                        return new MinimaxAgent(scoring);
                    }
                case 9:
                    {
                        return new MonteCarloAgent(scoring, bag);
                    }
                default:
                    {
                        return new Agent(scoring);
                    }
            }
        }

        public Agent UseEvolParamsAgent(double b, (double,double,double) c, double[] t)
        {
            return new AgentCompleteWithUtilityParams(scoring, b, c, t);
        }

        public Agent UseAgent(int agentOption, int boardId)
        {
            switch (agentOption)
            {
                case 1:
                    {
                        return new Agent(scoring, boardId);
                    }
                case 2:
                    {
                        return new AgentColor(scoring, boardId);
                    }
                case 3:
                    {
                        return new AgentPattern(scoring, boardId);
                    }
                case 4:
                    {
                        return new AgentComplete(scoring, boardId);
                    }
                case 5:
                    {
                        return new AgentCompleteWithProb(scoring, boardId);
                    }
                case 6:
                    {
                        return new RandomPatchTileAgent(scoring, boardId);
                    }
                case 7:
                    {
                        return new AgentCompleteWithUtility(scoring, boardId);
                    }
                case 8:
                    {
                        return new MinimaxAgent(scoring,boardId);
                    }
                default:
                    {
                        return new Agent(scoring, boardId);
                    }
            }
        }

        #endregion

        #region Game

        #region Agent Game

        public void AgentGame(int agentType, bool withPrint)
        {
            agent = UseAgent(agentType);
            agent.ChooseTaskPieces();
            //agent.AddTaskPieces(1, 4, 6);

            if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);

            for (int i = 0; i < 22; i++)
            {
                MakeMove(agent);

                if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);
            }

            if(withPrint) gameStatePrinter.PrintStats(agent);

            int score = agent.Board.ScoreCounter.GetScore();

            int buttons = agent.Board.ScoreCounter.GetButtonsCount();
            var catsTemp = agent.Board.ScoreCounter.GetCatsCount();
            (int, int, int) cats = (catsTemp.Item1, catsTemp.Item2, catsTemp.Item3);

            Stats = new GameStats(score,buttons,cats,agent.Board.TasksCompleted());
        }

        #endregion

        #region Agent with Settings

        public void AgentGameTaskSettings(int agentType, bool withPrint, (int,int,int) taskChoice)
        {
            agent = UseAgent(agentType);
            agent.AddTaskPieces(taskChoice.Item1,taskChoice.Item2,taskChoice.Item3);

            if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);

            for (int i = 0; i < 22; i++)
            {
                MakeMove(agent);

                if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);
            }

            if (withPrint) gameStatePrinter.PrintStats(agent);

            int score = agent.Board.ScoreCounter.GetScore();

            int buttons = agent.Board.ScoreCounter.GetButtonsCount();
            var catsTemp = agent.Board.ScoreCounter.GetCatsCount();
            (int, int, int) cats = (catsTemp.Item1, catsTemp.Item2, catsTemp.Item3);

            Stats = new GameStats(score, buttons, cats);
        }

        public void AgentGameBoardSettings(int agentType, bool withPrint, int boardId)
        {
            agent = UseAgent(agentType, boardId);
            agent.ChooseTaskPieces();

            if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);

            for (int i = 0; i < 22; i++)
            {
                MakeMove(agent);

                if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);
            }

            if (withPrint) gameStatePrinter.PrintStats(agent);

            int score = agent.Board.ScoreCounter.GetScore();

            int buttons = agent.Board.ScoreCounter.GetButtonsCount();
            var catsTemp = agent.Board.ScoreCounter.GetCatsCount();
            (int, int, int) cats = (catsTemp.Item1, catsTemp.Item2, catsTemp.Item3);

            Stats = new GameStats(score, buttons, cats);
        }

        public void AgentGameSettings(int agentType, bool withPrint, (int,int,int) taskChoice, int boardId)
        {
            agent = UseAgent(agentType);
            agent.AddTaskPieces(taskChoice.Item1, taskChoice.Item2, taskChoice.Item3);

            if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);

            for (int i = 0; i < 22; i++)
            {
                MakeMove(agent);

                if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);
            }

            if (withPrint) gameStatePrinter.PrintStats(agent);

            int score = agent.Board.ScoreCounter.GetScore();

            int buttons = agent.Board.ScoreCounter.GetButtonsCount();
            var catsTemp = agent.Board.ScoreCounter.GetCatsCount();
            (int, int, int) cats = (catsTemp.Item1, catsTemp.Item2, catsTemp.Item3);

            Stats = new GameStats(score, buttons, cats);
        }

        #endregion


        //private GameStats TestGame(bool withPrint, bool allResults, int agentType, int iterations)
        //{
        //    int sum = 0;
        //    int max = 0;
        //    int min = -1;
        //    int score;
        //    int buttons = 0;
        //    (int, int, int) cats = (0,0,0);
        //    int maxButtons = 0;
        //    (int, int, int) maxCats = (0, 0, 0);

        //    for (int j = 0; j < iterations; j++)
        //    {


        //        bag = new Bag();

        //        scoring = new Scoring();

        //        gameStatePrinter = new GameStatePrinter(scoring);

        //        for (int i = 0; i < 3; i++)
        //        {
        //            Opts[i] = bag.Next();
        //        }

        //        agent = UseAgent(agentType);

        //        //agent.AddTaskPieces(1,6,4); //best option

        //        agent.ChooseTaskPieces();

        //        if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);



        //        for (int i = 0; i < 22; i++)
        //        {
        //            MakeMove(agent);

        //            if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);

        //        }

        //        if (allResults)
        //        {
        //            if (j % 1000 == 0) Console.WriteLine(j);
        //            //gameStatePrinter.PrintStats(agent);
        //        }

        //        score = agent.Board.ScoreCounter.GetScore();
        //        sum += score;
        //        if (score > max)
        //        {
        //            max = score;
        //            maxButtons = agent.Board.ScoreCounter.GetButtonsCount();
        //            var maxCatsTemp = agent.Board.ScoreCounter.GetCatsCount();
        //            maxCats.Item1 = maxCatsTemp.Item1;
        //            maxCats.Item2 = maxCatsTemp.Item2;
        //            maxCats.Item3 = maxCatsTemp.Item3;
        //        }
        //        if (score < min || min == -1) min = score;

        //        buttons += agent.Board.ScoreCounter.GetButtonsCount();
        //        var catsTemp = agent.Board.ScoreCounter.GetCatsCount();
        //        cats.Item1 += catsTemp.Item1;
        //        cats.Item2 += catsTemp.Item2;
        //        cats.Item3 += catsTemp.Item3;
        //    }
        //    Console.WriteLine();
        //    if (iterations > 1)
        //    {
        //        Console.WriteLine(" Mean: " + (Convert.ToDouble(sum) / Convert.ToDouble(iterations)));
        //        Console.WriteLine(" Average number of buttons: " + (Convert.ToDouble(buttons) / Convert.ToDouble(iterations)));
        //        Console.WriteLine(" Average number of cats: " + (Convert.ToDouble(cats.Item1) / Convert.ToDouble(iterations)) + "; "+ (Convert.ToDouble(cats.Item2) / Convert.ToDouble(iterations)) + "; " + (Convert.ToDouble(cats.Item3) / Convert.ToDouble(iterations)));
        //        Console.WriteLine(" Best score: " + max + "; buttons: " + Convert.ToDouble(maxButtons)+
        //            "; cats: " + Convert.ToDouble(maxCats.Item1) + "; " + Convert.ToDouble(maxCats.Item2) + "; " + Convert.ToDouble(maxCats.Item3)) ;
        //        Console.WriteLine(" Lowest score: " + min);
        //    }

        //    return new GameStats(agentType, Convert.ToDouble(sum) / Convert.ToDouble(iterations), Convert.ToDouble(buttons) / Convert.ToDouble(iterations), (Convert.ToDouble(cats.Item1) / Convert.ToDouble(iterations), Convert.ToDouble(cats.Item2) / Convert.ToDouble(iterations), Convert.ToDouble(cats.Item3) / Convert.ToDouble(iterations)), max, min);


        //}

        //private void TestMultiPlayerGame(int numOfPlayers, bool withPrint, bool allResults, int iterations)
        //{
        //    List<int> sum = new List<int>();
        //    List<int> max = new List<int>();
        //    List<int> min = new List<int>();
        //    List<int> wins = new List<int>();
        //    List<int> score = new List<int>();
        //    List<int> agentOptions = new List<int>();

        //    multiAgents = new List<Agent>();

        //    for (int i = 0; i < numOfPlayers; i++)
        //    {
        //        int a = PickAgent(true);
        //        agentOptions.Add(a);
        //        sum.Add(0);
        //        max.Add(0);
        //        min.Add(-1);
        //        wins.Add(0);
        //        score.Add(0);
        //        multiAgents.Add(null);
        //    }

        //    for (int j = 0; j < iterations; j++)
        //    {
        //        Console.WriteLine(j);

        //        bag = new Bag();

        //        scoring = new Scoring();

        //        gameStatePrinter = new GameStatePrinter(scoring);

        //        for (int i = 0; i < 3; i++)
        //        {
        //            Opts[i] = bag.Next();
        //        }
        //        for (int i = 0; i < numOfPlayers; i++)
        //        {
        //            multiAgents[i] = UseAgent(agentOptions[i]);
        //            multiAgents[i].ChooseTaskPieces();
        //        }

        //        for (int i = 0; i < numOfPlayers; i++)
        //        {
        //            List<Agent> ops = new List<Agent>();
        //            for (int k = 0; k < numOfPlayers; k++)
        //            {
        //                if (i != k)
        //                {
        //                    ops.Add(multiAgents[k]);
        //                }
        //            }
        //            multiAgents[i].SetOpponent(ref ops);
        //        }

        //        //print empty
        //        if (withPrint) gameStatePrinter.PrintStateTestMulti(multiAgents[0], multiAgents[1], Opts);

        //        for (int i = 0; i < 22; i++)
        //        {
        //            //MakeMove(agent);

        //            //MakeMove(agent2);
        //            //if (withPrint) gameStatePrinter.PrintStateTestMulti(agent, agent2, Opts);

        //            for (int k = 0; k < numOfPlayers; k++)
        //            {
        //                MakeMove(multiAgents[k]);
        //                if (withPrint) gameStatePrinter.PrintStateTestMulti(multiAgents[0], multiAgents[1], Opts);
        //            }

        //        }

        //        for (int i = 0;i < numOfPlayers; i++)
        //        {
        //            score[i] = multiAgents[i].Board.ScoreCounter.GetScore();
        //            if (allResults) gameStatePrinter.PrintStats(multiAgents[i]);

        //            sum[i] += score[i];
        //            if (score[i] > max[i]) max[i] = score[i];
        //            if (score[i] < min[i] || min[i] == -1) min[i] = score[i];
        //        }

        //        int max_game_score = score.Max();
        //        for (int i = 0; i < numOfPlayers; i++)
        //        {
        //            if (score[i] == max_game_score)
        //            {
        //                wins[i] += 1;
        //            }
        //        }

        //    }
        //    Console.WriteLine();
        //    if (iterations > 1) 
        //    {
        //        for (int i = 0; i < numOfPlayers; i++)
        //        {
        //            Console.WriteLine(" Mean " + i + ": " + (Convert.ToDouble(sum[i]) / Convert.ToDouble(iterations)));
        //            Console.WriteLine(" Best score " + i + ": " + max[i]);
        //            Console.WriteLine(" Lowest score " + i +": " + min[i]);
        //            Console.WriteLine(" Wins " + i + ": " + wins[i]);
        //        }
        //    }
        //}

        #endregion
        #endregion

        #region Evolution

        public void EvolutionGame(bool withPrint, Weights e)
        {
            agent = new EvolutionAgent(scoring,e);
            //agent.ChooseTaskPieces();
            agent.AddTaskPieces(1, 4, 6);

            if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);

            for (int i = 0; i < 22; i++)
            {
                MakeMove(agent);

                if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);
            }

            if (withPrint) gameStatePrinter.PrintStats(agent);

            int score = agent.Board.ScoreCounter.GetScore();

            int buttons = agent.Board.ScoreCounter.GetButtonsCount();
            var catsTemp = agent.Board.ScoreCounter.GetCatsCount();
            (int, int, int) cats = (catsTemp.Item1, catsTemp.Item2, catsTemp.Item3);

            Stats = new GameStats(score, buttons, cats);
        }

        public void EvolutionGame(bool withPrint, Weights e, (int,int,int) tasks)
        {
            agent = new EvolutionAgent(scoring, e);
            //agent.ChooseTaskPieces();
            agent.AddTaskPieces(tasks.Item1, tasks.Item2, tasks.Item3);

            if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);

            for (int i = 0; i < 22; i++)
            {
                MakeMove(agent);

                if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);
            }

            if (withPrint) gameStatePrinter.PrintStats(agent);

            int score = agent.Board.ScoreCounter.GetScore();

            int buttons = agent.Board.ScoreCounter.GetButtonsCount();
            var catsTemp = agent.Board.ScoreCounter.GetCatsCount();
            (int, int, int) cats = (catsTemp.Item1, catsTemp.Item2, catsTemp.Item3);

            Stats = new GameStats(score, buttons, cats);
        }

        public void EvolParamsTestAgent(double b, (double,double,double) c, double[] t, (int,int,int) taskSettings)
        {
            agent = UseEvolParamsAgent(b, c, t);
            agent.AddTaskPieces(taskSettings.Item1, taskSettings.Item2, taskSettings.Item3);

            for (int i = 0; i < 22; i++)
            {
                MakeMove(agent);
            }

            int score = agent.Board.ScoreCounter.GetScore();

            int buttons = agent.Board.ScoreCounter.GetButtonsCount();
            var catsTemp = agent.Board.ScoreCounter.GetCatsCount();
            (int, int, int) cats = (catsTemp.Item1, catsTemp.Item2, catsTemp.Item3);

            Stats = new GameStats(score, buttons, cats, agent.Board.TasksCompleted());
        }

        #endregion

        #region Move


        public void MakeMove(Player p)
        {
            
            (int next, (int row, int col)) = p.ChooseNextMove(Opts);

            p.MakeMove(Opts[next], row, col);

            Opts[next] = bag.Next();
        }
        #endregion

    }
}
