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
                        //TestTopAgents();
                        break;
                    }
                case 5:
                    {
                        TestMultiPlayer();
                        break;
                    }
                case 6:
                    {
                        TestTasks();
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
                    Console.WriteLine("    9. 8 but with probability to do sth random");
                    Console.WriteLine("    10. random patch tile, best position");
                    Console.WriteLine("    11. Counting scores with utility");
                    Console.WriteLine("    12. twoplayer");
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
                        return new AgentCompleteWithUtility(scoring);
                    }
                case 12:
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
        private GameStats TestGame(bool withPrint, bool allResults, int agentType, int iterations)
        {
            int sum = 0;
            int max = 0;
            int min = -1;
            int score;
            int buttons = 0;
            (int, int, int) cats = (0,0,0);
            int maxButtons = 0;
            (int, int, int) maxCats = (0, 0, 0);

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

                agent.ChooseTaskPieces();
                if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);



                for (int i = 0; i < 22; i++)
                {
                    MakeMove(agent);

                    if (withPrint) gameStatePrinter.PrintStateSingle(agent, Opts);

                }

                if(allResults) gameStatePrinter.PrintStats(agent);
                score = agent.Board.ScoreCounter.GetScore();
                sum += score;
                if (score > max)
                {
                    max = score;
                    maxButtons = agent.Board.ScoreCounter.GetButtonsCount();
                    var maxCatsTemp = agent.Board.ScoreCounter.GetCatsCount();
                    maxCats.Item1 = maxCatsTemp.Item1;
                    maxCats.Item2 = maxCatsTemp.Item2;
                    maxCats.Item3 = maxCatsTemp.Item3;
                }
                if (score < min || min == -1) min = score;

                buttons += agent.Board.ScoreCounter.GetButtonsCount();
                var catsTemp = agent.Board.ScoreCounter.GetCatsCount();
                cats.Item1 += catsTemp.Item1;
                cats.Item2 += catsTemp.Item2;
                cats.Item3 += catsTemp.Item3;
            }
            Console.WriteLine();
            if (iterations > 1)
            {
                Console.WriteLine(" Mean: " + (Convert.ToDouble(sum) / Convert.ToDouble(iterations)));
                Console.WriteLine(" Average number of buttons: " + (Convert.ToDouble(buttons) / Convert.ToDouble(iterations)));
                Console.WriteLine(" Average number of cats: " + (Convert.ToDouble(cats.Item1) / Convert.ToDouble(iterations)) + "; "+ (Convert.ToDouble(cats.Item2) / Convert.ToDouble(iterations)) + "; " + (Convert.ToDouble(cats.Item3) / Convert.ToDouble(iterations)));
                Console.WriteLine(" Best score: " + max + "; buttons: " + Convert.ToDouble(maxButtons)+
                    "; cats: " + Convert.ToDouble(maxCats.Item1) + "; " + Convert.ToDouble(maxCats.Item2) + "; " + Convert.ToDouble(maxCats.Item3)) ;
                Console.WriteLine(" Lowest score: " + min);
            }

            return new GameStats(agentType, Convert.ToDouble(sum) / Convert.ToDouble(iterations), Convert.ToDouble(buttons) / Convert.ToDouble(iterations), (Convert.ToDouble(cats.Item1) / Convert.ToDouble(iterations), Convert.ToDouble(cats.Item2) / Convert.ToDouble(iterations), Convert.ToDouble(cats.Item3) / Convert.ToDouble(iterations)), max, min);


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
            List<GameStats> stats = new List<GameStats>();

            Console.WriteLine(" Agents: ");
            Console.WriteLine("    1. Random");
            Console.WriteLine("    2. Easy color half-random agent");
            Console.WriteLine("    3. Easy pattern half-random agent");
            Console.WriteLine("    4. Easy half-random agent");
            Console.WriteLine("    5. Random position, count scores");
            Console.WriteLine("    6. Counting color scores");
            Console.WriteLine("    7. Counting pattern scores");
            Console.WriteLine("    8. Counting scores");
            Console.WriteLine("    9. 8 but with probability to do sth random");
            Console.WriteLine("    10. random patch tile, best position");
            Console.WriteLine("    11. Counting scores with utility");

            for (int i = 1;i <= 11;i++)
            {
                Console.WriteLine(" " + i + ": ");
                stats.Add(TestGame(false, false,i, 100));
                Console.WriteLine();
            }

            // Set a variable to the Documents path.
            //string docPath =
            //  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter("./testAll.csv"))
            {
                outputFile.WriteLine("agent;averageScore;buttons;cats;best;lowest");
                foreach (GameStats gs in stats)
                    outputFile.WriteLine($"{gs.AgentType};{gs.AvgScore};{gs.AvgButtons};{gs.AvgCats.Item1}/{gs.AvgCats.Item2}/{gs.AvgCats.Item3};{gs.BestScore};{gs.LowestScore}");
            }

        }

        private void TestTopAgents()
        {
            Console.WriteLine("    8. Counting scores");
            Console.WriteLine("    9. 8 but with probability to do sth random");
            Console.WriteLine("    11. Counting scores with utility");


            foreach (int i in new List<int>(){ 8, 9, 11 })
            {
                Console.WriteLine(" " + i + ": ");
                TestGame(false, false, i, 1000);
                Console.WriteLine();
            }
        }

        private GameStats TestTask(int agentType, int iterations, (int,int,int) tasks)
        {
            int sum = 0;
            int max = 0;
            int min = -1;
            int score;
            int buttons = 0;
            (int, int, int) cats = (0, 0, 0);
            int maxButtons = 0;
            (int, int, int) maxCats = (0, 0, 0);

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

                agent.AddTaskPieces(tasks.Item1,tasks.Item2,tasks.Item3);

                for (int i = 0; i < 22; i++)
                {
                    MakeMove(agent);
                }

                score = agent.Board.ScoreCounter.GetScore();
                sum += score;
                if (score > max)
                {
                    max = score;
                    maxButtons = agent.Board.ScoreCounter.GetButtonsCount();
                    var maxCatsTemp = agent.Board.ScoreCounter.GetCatsCount();
                    maxCats.Item1 = maxCatsTemp.Item1;
                    maxCats.Item2 = maxCatsTemp.Item2;
                    maxCats.Item3 = maxCatsTemp.Item3;
                }
                if (score < min || min == -1) min = score;

                buttons += agent.Board.ScoreCounter.GetButtonsCount();
                var catsTemp = agent.Board.ScoreCounter.GetCatsCount();
                cats.Item1 += catsTemp.Item1;
                cats.Item2 += catsTemp.Item2;
                cats.Item3 += catsTemp.Item3;
            }

            if (iterations > 1)
            {
                Console.WriteLine(" Mean: " + (Convert.ToDouble(sum) / Convert.ToDouble(iterations)));
                //Console.WriteLine(" Average number of buttons: " + (Convert.ToDouble(buttons) / Convert.ToDouble(iterations)));
                //Console.WriteLine(" Average number of cats: " + (Convert.ToDouble(cats.Item1) / Convert.ToDouble(iterations)) + "; " + (Convert.ToDouble(cats.Item2) / Convert.ToDouble(iterations)) + "; " + (Convert.ToDouble(cats.Item3) / Convert.ToDouble(iterations)));
                //Console.WriteLine(" Best score: " + max + "; buttons: " + Convert.ToDouble(maxButtons) +
                //    "; cats: " + Convert.ToDouble(maxCats.Item1) + "; " + Convert.ToDouble(maxCats.Item2) + "; " + Convert.ToDouble(maxCats.Item3));
                //Console.WriteLine(" Lowest score: " + min);
            }

            return new GameStats(agentType, Convert.ToDouble(sum) / Convert.ToDouble(iterations), Convert.ToDouble(buttons) / Convert.ToDouble(iterations), (Convert.ToDouble(cats.Item1) / Convert.ToDouble(iterations), Convert.ToDouble(cats.Item2) / Convert.ToDouble(iterations), Convert.ToDouble(cats.Item3) / Convert.ToDouble(iterations)), max, min);

        }

        private void TestTasks()
        {
            Dictionary<(int,int,int),List<GameStats>> statsDict = new Dictionary<(int, int, int), List<GameStats>>();

            for (int i = 1;  i <= 6; i++) 
            {
                for (int j = i + 1; j <= 6; j++)
                {
                    for (int k = j + 1; k <= 6; k++)
                    {
                        statsDict[(i,j,k)] = new List<GameStats>();

                        Console.WriteLine($" {i},{j},{k}: ");
                        statsDict[(i,j,k)].Add(TestTask(11, 1000, (i, j, k)));
                        Console.WriteLine();

                        Console.WriteLine($" {i},{k},{j}: ");
                        statsDict[(i, j, k)].Add(TestTask(11, 1000, (i, k, j)));
                        Console.WriteLine();

                        Console.WriteLine($" {j},{i},{k}: ");
                        statsDict[(i, j, k)].Add(TestTask(11, 1000, (j, i, k)));
                        Console.WriteLine();

                        Console.WriteLine($" {j},{k},{i}: ");
                        statsDict[(i, j, k)].Add(TestTask(11, 1000, (j, k, i)));
                        Console.WriteLine();

                        Console.WriteLine($" {k},{i},{j}: ");
                        statsDict[(i, j, k)].Add(TestTask(11, 1000, (k, i, j)));
                        Console.WriteLine();

                        Console.WriteLine($" {k},{j},{i}: ");
                        statsDict[(i, j, k)].Add(TestTask(11, 1000, (k, j, i)));
                        Console.WriteLine();
                    }
                }

                //if (i != 4)
                //{
                //    Console.WriteLine($" {i},{4},{6}: ");
                //    TestTask(11, 1000, (i, 4, 6));
                //    Console.WriteLine();
                //    Console.WriteLine($" {i},{6},{4}: ");
                //    TestTask(11, 1000, (i, 6, 4));
                //    Console.WriteLine();
                //    Console.WriteLine($" {4},{i},{6}: ");
                //    TestTask(11, 1000, (4, i, 6));
                //    Console.WriteLine();
                //    Console.WriteLine($" {6},{4},{i}: ");
                //    TestTask(11, 1000, (6, 4, i));
                //    Console.WriteLine();
                //    Console.WriteLine($" {6},{i},{4}: ");
                //    TestTask(11, 1000, (6, i, 4));
                //    Console.WriteLine();
                //    Console.WriteLine($" 4,6,{i}: ");
                //    TestTask(11, 1000, (4, 6, i));
                //    Console.WriteLine();
                //}
            }

            // objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));

            using (StreamWriter outputFile = new StreamWriter("./testTasks.csv"))
            {
                outputFile.WriteLine("tasks;averageScore");
                foreach (KeyValuePair<(int,int,int),List<GameStats>> ds  in statsDict)
                {
                    outputFile.WriteLine($"{ds.Key.Item1},{ds.Key.Item2},{ds.Key.Item3};{ds.Value.Average(item => item.AvgScore)}");

                }
            }
            using (StreamWriter outputFile = new StreamWriter("./testTasks123.csv"))
            {
                outputFile.WriteLine("tasks;score");

                outputFile.WriteLine($"1,2,3;{statsDict[(1, 2, 3)][0].AvgScore}");
                outputFile.WriteLine($"1,3,2;{statsDict[(1, 2, 3)][1].AvgScore}");
                outputFile.WriteLine($"2,1,3;{statsDict[(1, 2, 3)][2].AvgScore}");
                outputFile.WriteLine($"2,3,1;{statsDict[(1, 2, 3)][3].AvgScore}");
                outputFile.WriteLine($"3,1,3;{statsDict[(1, 2, 3)][4].AvgScore}");
                outputFile.WriteLine($"3,2,1;{statsDict[(1, 2, 3)][5].AvgScore}");
            }
            using (StreamWriter outputFile = new StreamWriter("./testTasks235.csv"))
            {
                outputFile.WriteLine("tasks;score");

                outputFile.WriteLine($"2,3,5;{statsDict[(2, 3, 5)][0].AvgScore}");
                outputFile.WriteLine($"2,5,3;{statsDict[(2, 3, 5)][1].AvgScore}");
                outputFile.WriteLine($"3,2,5;{statsDict[(2, 3, 5)][2].AvgScore}");
                outputFile.WriteLine($"3,5,2;{statsDict[(2, 3, 5)][3].AvgScore}");
                outputFile.WriteLine($"5,2,3;{statsDict[(2, 3, 5)][4].AvgScore}");
                outputFile.WriteLine($"5,3,2;{statsDict[(2, 3, 5)][5].AvgScore}");
            }
            using (StreamWriter outputFile = new StreamWriter("./testTasks146.csv"))
            {
                outputFile.WriteLine("tasks;score");

                outputFile.WriteLine($"1,4,6;{statsDict[(1, 4, 6)][0].AvgScore}");
                outputFile.WriteLine($"1,6,4;{statsDict[(1, 4, 6)][1].AvgScore}");
                outputFile.WriteLine($"4,1,6;{statsDict[(1, 4, 6)][2].AvgScore}");
                outputFile.WriteLine($"4,6,1;{statsDict[(1, 4, 6)][3].AvgScore}");
                outputFile.WriteLine($"6,1,4;{statsDict[(1, 4, 6)][4].AvgScore}");
                outputFile.WriteLine($"6,4,1;{statsDict[(1, 4, 6)][5].AvgScore}");
            }
            using (StreamWriter outputFile = new StreamWriter("./testTasks456.csv"))
            {
                outputFile.WriteLine("tasks;score");

                outputFile.WriteLine($"4,5,6;{statsDict[(4, 5, 6)][0].AvgScore}");
                outputFile.WriteLine($"4,6,5;{statsDict[(4, 5, 6)][1].AvgScore}");
                outputFile.WriteLine($"5,4,6;{statsDict[(4, 5, 6)][2].AvgScore}");
                outputFile.WriteLine($"5,6,4;{statsDict[(4, 5, 6)][3].AvgScore}");
                outputFile.WriteLine($"6,4,5;{statsDict[(4, 5, 6)][4].AvgScore}");
                outputFile.WriteLine($"6,5,4;{statsDict[(4, 5, 6)][5].AvgScore}");
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
