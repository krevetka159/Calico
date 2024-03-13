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
    internal class Game
    {
        private Bag bag;
        private Player player;
        private Agent agent;
        private Agent agent2;
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
                            agent = new RandomAgentComplete(scoring);
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
                            agent = new AgentComplete(scoring);
                            break;
                        }
                    case 9:
                        {
                            agent = new AgentCompleteWithProb(scoring);
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

        private void TestMultiPlayerGame(bool withPrint, bool allResults, int iterations)
        {
            int sumA1 = 0;
            int sumA2 = 0;

            int maxA1 = 0;
            int maxA2 = 0;

            int minA1 = -1;
            int minA2 = -1;

            int winsA1 = 0;
            int winsA2 = 0;

            int scoreA1;
            int scoreA2;

            for (int j = 0; j < iterations; j++)
            {

                bag = new Bag();

                scoring = new Scoring();

                gameStatePrinter = new GameStatePrinter(scoring);

                for (int i = 0; i < 3; i++)
                {
                    Opts[i] = bag.Next();
                }

                agent = new AgentComplete(scoring);
                agent2 = new TwoPlayerAgent(scoring, ref agent);

                //print empty
                if (withPrint) gameStatePrinter.PrintStateTestMulti(agent, agent2, Opts);

                for (int i = 0; i < 22; i++)
                {
                    MakeMove(agent);

                    if (withPrint) gameStatePrinter.PrintStateTestMulti(agent, agent2, Opts);

                    MakeMove(agent2);
                    if (withPrint) gameStatePrinter.PrintStateTestMulti(agent, agent2, Opts);

                }

                scoreA1 = agent.Board.ScoreCounter.GetScore();
                scoreA2 = agent2.Board.ScoreCounter.GetScore();

                if (allResults) gameStatePrinter.PrintStats(agent, agent2);
                sumA1 += scoreA1;
                sumA2 += scoreA2;
                if (scoreA1 > maxA1) maxA1 = scoreA1;
                if (scoreA2 > maxA2) maxA2 = scoreA2;
                if (scoreA1 < minA1 || minA1 == -1) minA1 = scoreA1;
                if (scoreA2 < minA2 || minA2 == -1) minA2 = scoreA2;
                if (scoreA1 > scoreA2) winsA1++;
                else if (scoreA2 > scoreA1) winsA2++;
            }
            Console.WriteLine();
            if (iterations > 1) 
            { 
                Console.WriteLine(" Mean A1: " + (Convert.ToDouble(sumA1) / Convert.ToDouble(iterations)));
                Console.WriteLine(" Mean A2: " + (Convert.ToDouble(sumA2) / Convert.ToDouble(iterations)));
                Console.WriteLine();
                Console.WriteLine(" Best score A1: " + maxA1);
                Console.WriteLine(" Best score A2: " + maxA2);
                Console.WriteLine(" Lowest score A1: " + minA1);
                Console.WriteLine(" Lowest score A2: " + minA2);
                Console.WriteLine(" Wins A1: " + winsA1);
                Console.WriteLine(" Wins A2: " + winsA2);

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

            for (int i = 1;i <= 9;i++)
            {
                Console.WriteLine(" " + i + ": ");
                TestGame(false, false,i, 100);
                Console.WriteLine();
            }

        }

        private void TestMultiPlayer()
        {
            //int agentOption;

            while (true)
            {
                try
                {
                    //Console.WriteLine(" Agent options: ");
                    //Console.WriteLine("    1. Random");
                    //Console.WriteLine("    2. Easy color half-random agent");
                    //Console.WriteLine("    3. Easy pattern half-random agent");
                    //Console.WriteLine("    4. Easy half-random agent");
                    //Console.WriteLine("    5. Random position, count scores");
                    //Console.WriteLine("    6. Counting color scores");
                    //Console.WriteLine("    7. Counting pattern scores");
                    //Console.WriteLine("    8. Counting scores");
                    //Console.WriteLine("    9. 7 but with probability to do sth random");
                    //Console.Write(" Pick agent: ");

                    //agentOption = Convert.ToInt32(Console.ReadLine());
                    //Console.WriteLine();

                    //if (1 <= agentOption && agentOption <= 9)
                    //{

                        while (true)
                        {

                            Console.Write(" Print progress (y/n): ");
                            string newGame = Console.ReadLine();

                            if (newGame == "n")
                            {
                                TestMultiPlayerGame(false, false, 1000);
                                break;
                            }
                            else if (newGame == "y")
                            {
                                TestMultiPlayerGame(true,true, 1);
                                break;
                            }
                            else
                            {
                                Console.WriteLine(" Invalid expression");
                            }
                        }
                        break;
                    //}
                    //else
                    //{
                    //    Console.WriteLine(" " + agentOption + " is not a mode option");
                    //}

                }
                catch
                {
                    Console.WriteLine(" Must be an integer.");
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
