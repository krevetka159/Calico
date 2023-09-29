using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Calico.AgentComplet;

namespace Calico
{
    internal class Game
    {
        private Bag bag;
        private Player player;
        private Agent agent;
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
            agent = new AgentComplet(scoring);

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

        private void Testing()
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
                    Console.Write(" Pick agent: ");

                    agentOption = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();

                    if (1<= agentOption && agentOption <= 9)
                    {
                        
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
                        break;
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
        private void TestGame(bool withPrint, bool allResults, int agentType, int iterations)
        {
            int sum = 0;

            for (int j = 0; j < iterations; j++)
            {


                bag = new Bag();

                scoring = new Scoring();

                gameStatePrinter = new GameStatePrinter(scoring);

                for (int i = 0; i < 3; i++)
                {
                    Opts[i] = bag.Next();
                }
                switch (agentType)
                {
                    case 1:
                        {
                            agent = new Agent(scoring);
                            break;
                        }
                    case 2:
                        {
                            agent = new RandomAgentColor(scoring);
                            break;
                        }
                    case 3:
                        {
                            agent = new RandomAgentPattern(scoring);
                            break;
                        }
                    case 4:
                        {
                            agent = new RandomAgentComplet(scoring);
                            break;
                        }
                    case 5:
                        {
                            agent = new RandomPositionAgent(scoring);
                            break;
                        }
                    case 6:
                        {
                            agent = new AgentColor(scoring);
                            break;
                        }
                    case 7:
                        {
                            agent = new AgentPattern(scoring);
                            break;
                        }
                    case 8:
                        {
                            agent = new AgentComplet(scoring);
                            break;
                        }
                    case 9:
                        {
                            agent = new AgentCompletWithProb(scoring);
                            break;
                        }
                }
                
                if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);



                for (int i = 0; i < 22; i++)
                {
                    MakeMove(agent);

                    if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);

                }

                if(allResults) gameStatePrinter.PrintStats(agent);
                sum += agent.Board.ScoreCounter.GetScore();
            }
            Console.WriteLine();
            if (iterations > 1) Console.WriteLine(" Mean: " + (sum / iterations));
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

            for (int i = 1;i <= 9;i++)
            {
                Console.WriteLine(" " + i + ": ");
                TestGame(false, false,i, 100);
                Console.WriteLine();
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
