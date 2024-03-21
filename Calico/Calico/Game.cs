using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Calico.AgentComplete;

namespace Calico
{
    // git test
    internal class Game
    {
        private Bag bag;
        private Player player;
        private Agent agent;
        private Agent agent2;
        private List<Agent> multiAgents;
        private GamePiece[] Opts = new GamePiece[3];
        private Scoring scoring;
        private GameStatePrinter gameStatePrinter;


        public Game(int mode) 
        {
            
            switch (mode)
            {
                case 1:
                    {
                        SinglePlayer();
                        break;
                    }
                case 2:
                    {
                        MultiPlayer();
                        break;
                    }
                case 3:
                    {
                        Testing();
                        break;
                    }
                case 4:
                    {
                        TestAll();
                        break;
                    }
                case 5:
                    {
                        TestMultiPlayer();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            
        }

// ----------------------------------------------- SINGLEPLAYER ------------------------------------------------------------
        private void SinglePlayer()
        {
            bag = new Bag();

            scoring = new Scoring();

            gameStatePrinter = new GameStatePrinter(scoring);

            for (int i = 0; i < 3; i++)
            {
                Opts[i] = bag.Next();
            }
            player = new Player(scoring);

            //print empty
            gameStatePrinter.PrintStateSingle(player, Opts);

            for (int i = 0; i < 22; i++)
            {
                MakeMove(player);
                // update points
                gameStatePrinter.PrintStateSingle(player, Opts);

            }

            gameStatePrinter.PrintStats(player);
        }

// ----------------------------------------------- MULTIPLAYER ------------------------------------------------------------
        private void MultiPlayer() 
        {
            bag = new Bag();

            scoring = new Scoring();

            gameStatePrinter = new GameStatePrinter(scoring);

            for (int i = 0; i < 3; i++)
            {
                Opts[i] = bag.Next();
            }

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

        // ----------------------------------------------- TESTING ------------------------------------------------------------

        private int PickAgent(bool multiPlayer)
        {
            int agentOption;

            while (true)
            {
                try
                {
                    Console.WriteLine(" Agent options: ");
                    Console.WriteLine("    1. Random");
                    Console.WriteLine("    2. Easy color half-random agent");
                    Console.WriteLine("    3. Easy pattern half-random agent");
                    Console.WriteLine("    4. Easy half-random agent");
                    Console.WriteLine("    5. Random position, count scores");
                    Console.WriteLine("    6. Counting color scores");
                    Console.WriteLine("    7. Counting pattern scores");
                    Console.WriteLine("    8. Counting scores");
                    Console.WriteLine("    9. 7 but with probability to do sth random");
                    Console.WriteLine("    10. random patch tile, best position");
                    Console.WriteLine("    11. twoplayer");
                    Console.Write(" Pick agent: ");

                    agentOption = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();

                    if (1 <= agentOption && agentOption <= 11)
                    {
                        return agentOption;
                    }
                    else
                    {
                        Console.WriteLine(" " + agentOption + " is not a mode option");
                    }

                }
                catch
                {
                    Console.WriteLine(" Must be an integer.");
                }
            }
        }
        private Agent UseAgent(int agentOption)
        {
            switch (agentOption)
            {
                case 1:
                    {
                        return new Agent(scoring);
                    }
                case 2:
                    {
                        return new RandomAgentColor(scoring);
                    }
                case 3:
                    {
                        return new RandomAgentPattern(scoring);
                    }
                case 4:
                    {
                        return new RandomAgentComplete(scoring);
                    }
                case 5:
                    {
                        return new RandomPositionAgent(scoring);
                    }
                case 6:
                    {
                        return new AgentColor(scoring);
                    }
                case 7:
                    {
                        return new AgentPattern(scoring);
                    }
                case 8:
                    {
                        return new AgentComplete(scoring);
                    }
                case 9:
                    {
                        return new AgentCompleteWithProb(scoring);
                    }
                case 10:
                    {
                        return new RandomPatchTileAgent(scoring);
                    }
                case 11:
                    {
                        return new TwoPlayerAgent(scoring);
                    }
                default:
                    {
                        return new Agent(scoring);
                    }
            }
        }
        private void Testing()
        {
            int agentOption = PickAgent(false);
            
            while (true)
            {
                            
                Console.Write(" Print progress (y/n): ");
                string newGame = Console.ReadLine();

                if (newGame == "n")
                {
                    TestGame(false, true, agentOption, 50);
                    break;
                }
                else if (newGame == "y") 
                {
                    TestGame(true, true, agentOption, 1);
                    break; 
                }
                else
                {
                    Console.WriteLine(" Invalid expression");
                }
            }
        }
        private void TestGame(bool withPrint, bool allResults, int agentType, int iterations)
        {
            int sum = 0;
            int max = 0;
            int min = -1;
            int score;

            for (int j = 0; j < iterations; j++)
            {


                bag = new Bag();

                scoring = new Scoring();

                gameStatePrinter = new GameStatePrinter(scoring);

                for (int i = 0; i < 3; i++)
                {
                    Opts[i] = bag.Next();
                }

                agent = UseAgent(agentType);
                
                if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);



                for (int i = 0; i < 22; i++)
                {
                    MakeMove(agent);

                    if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);

                }

                if(allResults) gameStatePrinter.PrintStats(agent);
                score = agent.Board.ScoreCounter.GetScore();
                sum += score;
                if (score > max) max = score;
                if (score < min || min == -1) min = score;
            }
            Console.WriteLine();
            if (iterations > 1)
            {
                Console.WriteLine(" Mean: " + (Convert.ToDouble(sum) / Convert.ToDouble(iterations)));
                Console.WriteLine(" Best score: " + max);
                Console.WriteLine(" Lowest score: " + min);
            }
        }

        private void TestMultiPlayerGame(int numOfPlayers, bool withPrint, bool allResults, int iterations)
        {
            List<int> sum = new List<int>();
            List<int> max = new List<int>();
            List<int> min = new List<int>();
            List<int> wins = new List<int>();
            List<int> score = new List<int>();
            List<int> agentOptions = new List<int>();

            multiAgents = new List<Agent>();

            for (int i = 0; i < numOfPlayers; i++)
            {
                int a = PickAgent(true);
                agentOptions.Add(a);
                sum.Add(0);
                max.Add(0);
                min.Add(-1);
                wins.Add(0);
                score.Add(0);
                multiAgents.Add(UseAgent(a));
            }

            for (int j = 0; j < iterations; j++)
            {

                bag = new Bag();

                scoring = new Scoring();

                gameStatePrinter = new GameStatePrinter(scoring);

                for (int i = 0; i < 3; i++)
                {
                    Opts[i] = bag.Next();
                }
                for (int i = 0; i < numOfPlayers; i++)
                {
                    multiAgents[i] = UseAgent(agentOptions[i]);
                }

                for (int i = 0; i < numOfPlayers; i++)
                {
                    List<Agent> ops = new List<Agent>();
                    for (int k = 0; k < numOfPlayers; k++)
                    {
                        if (i != k)
                        {
                            ops.Add(multiAgents[k]);
                        }
                    }
                    multiAgents[i].SetOpponent(ref ops);
                }

                //print empty
                if (withPrint) gameStatePrinter.PrintStateTestMulti(multiAgents[0], multiAgents[1], Opts);

                for (int i = 0; i < 22; i++)
                {
                    //MakeMove(agent);

                    //MakeMove(agent2);
                    //if (withPrint) gameStatePrinter.PrintStateTestMulti(agent, agent2, Opts);

                    for (int k = 0; k < numOfPlayers; k++)
                    {
                        MakeMove(multiAgents[k]);
                        if (withPrint) gameStatePrinter.PrintStateTestMulti(multiAgents[0], multiAgents[1], Opts);
                    }

                }

                for (int i = 0;i < numOfPlayers; i++)
                {
                    score[i] = multiAgents[i].Board.ScoreCounter.GetScore();
                    if (allResults) gameStatePrinter.PrintStats(multiAgents[i]);

                    sum[i] += score[i];
                    if (score[i] > max[i]) max[i] = score[i];
                    if (score[i] < min[i] || min[i] == -1) min[i] = score[i];
                }

                int max_game_score = score.Max();
                for (int i = 0; i < numOfPlayers; i++)
                {
                    if (score[i] == max_game_score)
                    {
                        wins[i] += 1;
                    }
                }

            }
            Console.WriteLine();
            if (iterations > 1) 
            {
                for (int i = 0; i < numOfPlayers; i++)
                {
                    Console.WriteLine(" Mean " + i + ": " + (Convert.ToDouble(sum[i]) / Convert.ToDouble(iterations)));
                    Console.WriteLine(" Best score " + i + ": " + max[i]);
                    Console.WriteLine(" Lowest score " + i +": " + min[i]);
                    Console.WriteLine(" Wins " + i + ": " + wins[i]);
                }
            }
        }

        private void TestAll()
        {
            Console.WriteLine(" Agents: ");
            Console.WriteLine("    1. Random");
            Console.WriteLine("    2. Easy color half-random agent");
            Console.WriteLine("    3. Easy pattern half-random agent");
            Console.WriteLine("    4. Easy half-random agent");
            Console.WriteLine("    5. Random position, count scores");
            Console.WriteLine("    6. Counting color scores");
            Console.WriteLine("    7. Counting pattern scores");
            Console.WriteLine("    8. Counting scores");
            Console.WriteLine("    9. 7 but with probability to do sth random");
            Console.WriteLine("    10. random patch tile, best position");

            for (int i = 1;i <= 10;i++)
            {
                Console.WriteLine(" " + i + ": ");
                TestGame(false, false,i, 100);
                Console.WriteLine();
            }

        }

        private void TestMultiPlayer()
        {
            while (true)
            {
                Console.Write("Number of players (2-4): ");
                try
                {
                    int numOfPlayers = Convert.ToInt32(Console.ReadLine());

                    if (numOfPlayers == 2) 
                    {
                        while (true)
                        {
                            Console.Write(" Print progress (y/n): ");
                            string newGame = Console.ReadLine();

                            if (newGame == "n")
                            {
                                TestMultiPlayerGame(numOfPlayers, false, false, 1000);
                                break;
                            }
                            else if (newGame == "y")
                            {
                                TestMultiPlayerGame(numOfPlayers, true, true, 1);
                                break;
                            }
                            else
                            {
                                Console.WriteLine(" Invalid expression");
                            }
                        }
                    }
                    else if ( numOfPlayers == 3 || numOfPlayers == 4)
                    {
                        TestMultiPlayerGame(numOfPlayers, false, false, 1000);
                        break;
                    }
                    else
                    {
                        Console.WriteLine(" Invalid number of players");
                    }
                }
                catch
                {
                    Console.WriteLine(" Invalid expression");
                }

            }
        }

// ----------------------------------------------- GET COMMAND ------------------------------------------------------------


        private void MakeMove(Player p)
        {
            
            (int next, (int row, int col)) = p.ChooseNextMove(Opts);

            p.MakeMove(Opts[next], row, col);

            Opts[next] = bag.Next();
        }

    }
}
