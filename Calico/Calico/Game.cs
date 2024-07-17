using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Calico.AgentBasic;
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
            (1, " Náhodný agent "), //RAND
            (2, " Základní ohodnocení barev "), //U1B
            (3, " Základní ohodnocení vzorů "), //U1V
            (4, " Základní ohodnocení"), //U1
            (5, " Rozšířená funkce"), // Albert U2
            (6, " Rozšířená funkce s náhodou "), // U2RAND
            (7, " Vážená rozšířená funkce"), // Adalbert
            (8, " Stromové prohledávání"), // Max
            (9, " Stromové prohledávání se simulacemi"), // Karel
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
                        return new AgentBasicColor(scoring);
                    }
                case 3:
                    {
                        return new AgentBasicPattern(scoring);
                    }
                case 4:
                    {
                        return new AgentBasic(scoring);
                    }
                case 5:
                    {
                        return new AgentAdvanced(scoring);
                    }
                case 6:
                    {
                        return new AgentAdvancedRandomness(scoring);
                    }
                default:
                    {
                        return new Agent(scoring);
                    }
            }
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
                        return new AgentBasicColor(scoring, boardId);
                    }
                case 3:
                    {
                        return new AgentBasicPattern(scoring, boardId);
                    }
                case 4:
                    {
                        return new AgentBasic(scoring, boardId);
                    }
                case 5:
                    {
                        return new AgentAdvanced(scoring, boardId);
                    }
                case 6:
                    {
                        return new AgentAdvancedRandomness(scoring, boardId);
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

        public void WeightedAgentGame(WeightsDict wd, bool withPrint)
        {
            agent = new AgentWeightedAdvanced(scoring,wd);
            

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

            Stats = new GameStats(score, buttons, cats, agent.Board.TasksCompleted());
        }

        public void TreeSearchAgentGame(WeightsDict wd, bool withPrint,int depth, double discount)
        {
            agent = new AgentTreeSearch(scoring, wd, depth,discount);


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

            Stats = new GameStats(score, buttons, cats, agent.Board.TasksCompleted());
        }
        public void SimulationTSAgentGame(WeightsDict wd, bool withPrint, int s)
        {
            agent = new AgentMCTS(scoring, wd, bag, s);


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

            Stats = new GameStats(score, buttons, cats, agent.Board.TasksCompleted());
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

        #endregion
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
