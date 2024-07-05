using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class GameModeController
    {
        private List<(int, string)> AgentDescription = new List<(int, string)>()
        {
            (1, " Kompletně náhodný agent "), //NA
            (2, " Nejlepší umístění vzhledek k barvám "), //UB
            (3, " Nejlepší umístění vzhledem ke vzorům "), //UV
            (4, " Nejlepší umístění "), //U
            (5, " Nejlepší umístění s malou náhodou "), // UN
            (6, " Nejlepší umístění náhodného dílku "), //
            (7, " Utility fce"), // Albert
            (8, " Minimax"), // Max
            (9, " MC"), // MC agent (Karel)
            (10, "EvolParams"), // Eva
        };

        private AverageGameStats avgStats;

        public GameModeController(int mode)
        {

            switch (mode)
            {
                case 1:
                    {
                        Game g = new Game();
                        g.SinglePlayer();
                        break;
                    }
                case 2:
                    {
                        Game g = new Game();
                        g.MultiPlayer();
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
                       //TestMultiPlayer();
                        break;
                    }
                case 6:
                    {
                        //TestTasks();
                        //TestBoards();
                        TestAllSettings();
                        break;
                    }
                case 7:
                    {
                        Evolution();
                        break;
                    }
                case 8:
                    {
                        EvolParamsTesting();
                        break;
                    }
                case 9:
                    {
                        VarianceTesting(); break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private int PickAgent(bool multiPlayer)
        {
            int agentOption;

            while (true)
            {
                try
                {
                    Console.WriteLine(" Agent options: ");

                    foreach ((int, string) ad in AgentDescription)
                    {
                        Console.WriteLine($"    {ad.Item1}. {ad.Item2}");
                    }
                    Console.Write(" Pick agent: ");

                    agentOption = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();

                    if (1 <= agentOption && agentOption <= AgentDescription.Count())
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

        #region TestingAgents

        private void Testing()
        {

            while (true)
            {

                Console.Write(" Print progress (y/n): ");
                string newGame = Console.ReadLine();

                int agentType = PickAgent(false);

                if (newGame == "n")
                {
                    //List<double> avg = new List<double>();
                    //for (int j = 0; j < 20; j++)
                    //{
                    //    avg.Add(0);
                    //}

                    //Parallel.For(0, 20, j =>
                    //{
                        int iterations = 5000;
                        GameStats[] stats = new GameStats[iterations];


                        for (int i = 0;i < iterations; i++)
                        {
                            Game g = new Game();
                            g.AgentGame(agentType, false);
                            stats[i] = g.Stats;
                            Console.WriteLine($" {i} : {g.Stats.Score}");
                        }


                    //});


                    using (StreamWriter outputFile = new StreamWriter($"./testAgent{agentType}_146_evol1.csv"))
                    {
                        outputFile.WriteLine(stats.Max(item => item.Score));
                        outputFile.WriteLine(stats.Min(item => item.Score));

                        outputFile.WriteLine(
                            $"{Math.Round(stats.Average(item => item.Score), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats.Average(item => item.Buttons), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats.Average(item => item.Cats.Item1), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats.Average(item => item.Cats.Item2), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats.Average(item => item.Cats.Item3), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{GetTaskAverageScore(1,stats)};" +
                            $"{GetTaskAverageScore(2, stats)};" +
                            $"{GetTaskAverageScore(3, stats)};" +
                            $"{GetTaskAverageScore(4, stats)};" +
                            $"{GetTaskAverageScore(5, stats)};" +
                            $"{GetTaskAverageScore(6, stats)};"


                            ) ;
                        outputFile.WriteLine("");

                        outputFile.WriteLine("score;Buttons;C1;C2;C3;T1;T2;T3;T4;T5;T6");

                        foreach (GameStats gs in stats)
                        {
                            outputFile.WriteLine(
                                $"{gs.Score};" +
                                $"{gs.Buttons};" +
                                $"{gs.Cats.Item1};" +
                                $"{gs.Cats.Item2};" +
                                $"{gs.Cats.Item3};" +
                                $"{gs.Tasks[0]};" +
                                $"{gs.Tasks[1]};" +
                                $"{gs.Tasks[2]};" +
                                $"{gs.Tasks[3]};" +
                                $"{gs.Tasks[4]};" +
                                $"{gs.Tasks[5]}"
                                );
                        }
                    }
                    break;
                }
                else if (newGame == "y")
                {
                    Game g = new Game();
                    g.AgentGame(agentType, true);
                }
                else
                {
                    Console.WriteLine(" Invalid expression");
                }
            }
        }

        private void TestAll()
        {
            int iterations = 5000;
            GameStats[][] stats = new GameStats[7][];

            Console.WriteLine(" Agent options: ");

            foreach ((int, string) ad in AgentDescription)
            {
                Console.WriteLine($"    {ad.Item1}. {ad.Item2}");
            }

            for (int i = 1; i <= 7; i++)
            {
                stats[i - 1] = new GameStats[iterations];
                Console.WriteLine(" " + i + ": ");

                Parallel.For(0, iterations, j =>
                {
                    Game g = new Game();
                    g.AgentGame(i, false);
                    stats[i-1][j] = g.Stats;
                });

                Console.WriteLine(stats[i-1].Average(item => item.Score));
                Console.WriteLine();
            }

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter("./testAll.csv"))
            {
                outputFile.WriteLine("agent;averageScore;buttons;c1;c2;c3;t1;t2;t3;t4;t5;t6;best;lowest");
                for (int i = 0; i < 7; i++)
                    outputFile.WriteLine(
                            $"{i+1};"+
                            $"{Math.Round(stats[i].Average(item => item.Score), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats[i].Average(item => item.Buttons), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats[i].Average(item => item.Cats.Item1), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats[i].Average(item => item.Cats.Item2), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats[i].Average(item => item.Cats.Item3), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats[i].Where(item => item.Tasks[0] >= 0).Average(item => item.Tasks[0]), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats[i].Where(item => item.Tasks[1] >= 0).Average(item => item.Tasks[1]), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats[i].Where(item => item.Tasks[2] >= 0).Average(item => item.Tasks[2]), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats[i].Where(item => item.Tasks[3] >= 0).Average(item => item.Tasks[3]), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats[i].Where(item => item.Tasks[4] >= 0).Average(item => item.Tasks[4]), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(stats[i].Where(item => item.Tasks[5] >= 0).Average(item => item.Tasks[5]), 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{stats[i].Max(item => item.Score)};" +
                            $"{stats[i].Min(item => item.Score)}"
                            );
            }

        }

        //private void TestMultiPlayer()
        //{
        //    while (true)
        //    {
        //        Console.Write("Number of players (2-4): ");
        //        try
        //        {
        //            int numOfPlayers = Convert.ToInt32(Console.ReadLine());

        //            if (numOfPlayers == 2)
        //            {
        //                while (true)
        //                {
        //                    Console.Write(" Print progress (y/n): ");
        //                    string newGame = Console.ReadLine();

        //                    if (newGame == "n")
        //                    {
        //                        TestMultiPlayerGame(numOfPlayers, false, false, 500);
        //                        break;
        //                    }
        //                    else if (newGame == "y")
        //                    {
        //                        TestMultiPlayerGame(numOfPlayers, true, true, 1);
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine(" Invalid expression");
        //                    }
        //                }
        //            }
        //            else if (numOfPlayers == 3 || numOfPlayers == 4)
        //            {
        //                TestMultiPlayerGame(numOfPlayers, false, false, 1000);
        //                break;
        //            }
        //            else
        //            {
        //                Console.WriteLine(" Invalid number of players");
        //            }
        //        }
        //        catch
        //        {
        //            Console.WriteLine(" Invalid expression");
        //        }

        //    }
        //}

        #endregion

        #region Settings analysis

        private AverageGameStats TestSetting(int agentType, int iterations, (int, int, int) tasks, int boardId)
        {
            List<GameStats> stats = new List<GameStats>();

            for (int j = 0; j < iterations; j++)
            {
                stats.Add(null);
            }

            Parallel.For(0, iterations, j =>
            {
                Game g = new Game();
                if(boardId == -1)
                {
                    g.AgentGameTaskSettings(7, false, tasks);
                }
                else
                {
                    g.AgentGameSettings(7, false, tasks, boardId);
                }
                stats[j] = g.Stats;
            });

            if (iterations > 1)
            {
                Console.WriteLine(" Mean: " + stats.Average(item => item.Score));
                //Console.WriteLine(" Average number of buttons: " + (Convert.ToDouble(buttons) / Convert.ToDouble(iterations)));
                //Console.WriteLine(" Average number of cats: " + (Convert.ToDouble(cats.Item1) / Convert.ToDouble(iterations)) + "; " + (Convert.ToDouble(cats.Item2) / Convert.ToDouble(iterations)) + "; " + (Convert.ToDouble(cats.Item3) / Convert.ToDouble(iterations)));
                //Console.WriteLine(" Best score: " + max + "; buttons: " + Convert.ToDouble(maxButtons) +
                //    "; cats: " + Convert.ToDouble(maxCats.Item1) + "; " + Convert.ToDouble(maxCats.Item2) + "; " + Convert.ToDouble(maxCats.Item3));
                //Console.WriteLine(" Lowest score: " + min);
            }

            return new AverageGameStats(agentType, stats.Average(item => item.Score), stats.Average(item => item.Buttons), (stats.Average(item => item.Cats.Item1), stats.Average(item => item.Cats.Item2), stats.Average(item => item.Cats.Item3)), stats.Max(item => item.Score), stats.Min(item => item.Score));

        }

        private void TestAllSettings()
        {
            List<string>results = new List<string>();

                
            for (int i = 1; i <= 6; i++)
            {
                for (int j = i + 1; j <= 6; j++)
                {
                    for (int k = j + 1; k <= 6; k++)
                    {

                        Console.WriteLine($" {i},{j},{k}: ");
                        var res = TestSetting(7, 5000, (i, j, k), -1);
                        results.Add($"{i},{j},{k};{Math.Round(res.AvgScore, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");
                        Console.WriteLine();

                        Console.WriteLine($" {i},{k},{j}: ");
                        res = TestSetting(7, 5000, (i, k, j), -1);
                        results.Add($"{i},{k},{j};{Math.Round(res.AvgScore, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");
                        Console.WriteLine();

                        Console.WriteLine($" {j},{i},{k}: ");
                        res = TestSetting(7, 5000, (j, i, k), -1);
                        results.Add($"{j},{i},{k};{Math.Round(res.AvgScore, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");
                        Console.WriteLine();

                        Console.WriteLine($" {j},{k},{i}: ");
                        res = TestSetting(7, 5000, (j, k, i), -1);
                        results.Add($"{j},{k},{i};{Math.Round(res.AvgScore, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");
                        Console.WriteLine();

                        Console.WriteLine($" {k},{i},{j}: ");
                        res = TestSetting(7, 5000, (k, i, j), -1);
                        results.Add($"{k},{i},{j};{Math.Round(res.AvgScore, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");
                        Console.WriteLine();

                        Console.WriteLine($" {k},{j},{i}: ");
                        res = TestSetting(7, 5000, (k, j, i), -1);
                        results.Add($"{k},{j},{i};{Math.Round(res.AvgScore, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");
                        Console.WriteLine();
                    }
                }
            }


            using (StreamWriter outputFile = new StreamWriter($"./resultsBeforeEvol.csv"))
            {
                outputFile.WriteLine("tasks;score");
                foreach (string res in results)
                {
                outputFile.WriteLine(res);
                }
            }
            
        }


            #region Task Analysis

            private AverageGameStats TestTask(int agentType, int iterations, (int, int, int) tasks)
        {
            List<GameStats> stats = new List<GameStats>();

            for (int j = 0; j < iterations; j++)
            {

                Game g = new Game();
                g.AgentGameTaskSettings(7, false, tasks);
                stats.Add( g.Stats);
            }

            if (iterations > 1)
            {
                //Console.WriteLine(" Mean: " + stats.Average(item => item.Score));
                //Console.WriteLine(" Average number of buttons: " + (Convert.ToDouble(buttons) / Convert.ToDouble(iterations)));
                //Console.WriteLine(" Average number of cats: " + (Convert.ToDouble(cats.Item1) / Convert.ToDouble(iterations)) + "; " + (Convert.ToDouble(cats.Item2) / Convert.ToDouble(iterations)) + "; " + (Convert.ToDouble(cats.Item3) / Convert.ToDouble(iterations)));
                //Console.WriteLine(" Best score: " + max + "; buttons: " + Convert.ToDouble(maxButtons) +
                //    "; cats: " + Convert.ToDouble(maxCats.Item1) + "; " + Convert.ToDouble(maxCats.Item2) + "; " + Convert.ToDouble(maxCats.Item3));
                //Console.WriteLine(" Lowest score: " + min);
            }

            return new AverageGameStats(agentType, stats.Average(item => item.Score), stats.Average(item => item.Buttons), (stats.Average(item => item.Cats.Item1), stats.Average(item => item.Cats.Item2), stats.Average(item => item.Cats.Item3)), stats.Max(item => item.Score), stats.Min(item => item.Score));

        }

        private void TestTasks()
        {
            List<AverageGameStats> statsDict = new List<AverageGameStats>();

            //for (int i = 1; i <= 6; i++)
            //{
            //    for (int j = i + 1; j <= 6; j++)
            //    {
            //        for (int k = j + 1; k <= 6; k++)
            //        {
            int i = 2;
            int j = 3;
            int k = 5;
                        statsDict = new List<AverageGameStats>();

                        Console.WriteLine($" {i},{j},{k}: ");
                        statsDict.Add(TestTask(7, 5000, (i, j, k)));
                        Console.WriteLine();

                        Console.WriteLine($" {i},{k},{j}: ");
                        statsDict.Add(TestTask(7, 5000, (i, k, j)));
                        Console.WriteLine();

                        Console.WriteLine($" {j},{i},{k}: ");
                        statsDict.Add(TestTask(7, 5000, (j, i, k)));
                        Console.WriteLine();

                        Console.WriteLine($" {j},{k},{i}: ");
                        statsDict.Add(TestTask(7, 5000, (j, k, i)));
                        Console.WriteLine();

                        Console.WriteLine($" {k},{i},{j}: ");
                        statsDict.Add(TestTask(7, 5000, (k, i, j)));
                        Console.WriteLine();

                        Console.WriteLine($" {k},{j},{i}: ");
                        statsDict.Add(TestTask(7, 5000, (k, j, i)));
                        Console.WriteLine();
                //    }
                //}
            //}

            // objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));

            List<(string, double)> avgScores = new List<(string, double)>();

            avgScores.Add(($"2,3,5", statsDict[0].AvgScore));
            avgScores.Add(($"2,5,3", statsDict[1].AvgScore));
            avgScores.Add(($"3,2,5", statsDict[2].AvgScore));
            avgScores.Add(($"3,5,2", statsDict[3].AvgScore));
            avgScores.Add(($"5,2,3", statsDict[4].AvgScore));
            avgScores.Add(($"5,3,2", statsDict[5].AvgScore));

            avgScores.Sort((x, y) => x.Item2.CompareTo(y.Item2));

            using (StreamWriter outputFile = new StreamWriter("./testTasks235.csv"))
            {
                outputFile.WriteLine("tasks;averageScore");
                foreach ((string, double) ds in avgScores)
                {
                    outputFile.WriteLine($"{ds.Item1};{Math.Round(ds.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");

                }
            }

            //avgScores = new List<(string, double)>();

            //avgScores.Add(($"1,2,3", statsDict[(1, 2, 3)][0].AvgScore));
            //avgScores.Add(($"1,3,2", statsDict[(1, 2, 3)][1].AvgScore));
            //avgScores.Add(($"2,1,3", statsDict[(1, 2, 3)][2].AvgScore));
            //avgScores.Add(($"2,3,1", statsDict[(1, 2, 3)][3].AvgScore));
            //avgScores.Add(($"3,1,2", statsDict[(1, 2, 3)][4].AvgScore));
            //avgScores.Add(($"3,2,1", statsDict[(1, 2, 3)][5].AvgScore));

            //avgScores.Sort((x, y) => x.Item2.CompareTo(y.Item2));

            //using (StreamWriter outputFile = new StreamWriter("./testTasks123.csv"))
            //{
            //    outputFile.WriteLine("tasks;score");

            //    foreach ((string, double) ds in avgScores)
            //    {
            //        outputFile.WriteLine($"{ds.Item1};{Math.Round(ds.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");

            //    }
            //}

            //avgScores = new List<(string, double)>();

            //avgScores.Add(($"2,3,5", statsDict[(2, 3, 5)][0].AvgScore));
            //avgScores.Add(($"2,5,3", statsDict[(2, 3, 5)][1].AvgScore));
            //avgScores.Add(($"3,2,5", statsDict[(2, 3, 5)][2].AvgScore));
            //avgScores.Add(($"3,5,2", statsDict[(2, 3, 5)][3].AvgScore));
            //avgScores.Add(($"5,2,3", statsDict[(2, 3, 5)][4].AvgScore));
            //avgScores.Add(($"5,3,2", statsDict[(2, 3, 5)][5].AvgScore));

            //avgScores.Sort((x, y) => x.Item2.CompareTo(y.Item2));

            //using (StreamWriter outputFile = new StreamWriter("./testTasks235.csv"))
            //{
            //    outputFile.WriteLine("tasks;score");

            //    foreach ((string, double) ds in avgScores)
            //    {
            //        outputFile.WriteLine($"{ds.Item1};{Math.Round(ds.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");

            //    }
            //}
            //avgScores = new List<(string, double)>();

            //avgScores.Add(($"1,4,6", statsDict[(1, 4, 6)][0].AvgScore));
            //avgScores.Add(($"1,6,4", statsDict[(1, 4, 6)][1].AvgScore));
            //avgScores.Add(($"4,1,6", statsDict[(1, 4, 6)][2].AvgScore));
            //avgScores.Add(($"4,6,1", statsDict[(1, 4, 6)][3].AvgScore));
            //avgScores.Add(($"6,1,4", statsDict[(1, 4, 6)][4].AvgScore));
            //avgScores.Add(($"6,4,1", statsDict[(1, 4, 6)][5].AvgScore));

            //avgScores.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            //using (StreamWriter outputFile = new StreamWriter("./testTasks146.csv"))
            //{
            //    outputFile.WriteLine("tasks;score");

            //    foreach ((string, double) ds in avgScores)
            //    {
            //        outputFile.WriteLine($"{ds.Item1};{Math.Round(ds.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");

            //    }
            //}
            //avgScores = new List<(string, double)>();

            //avgScores.Add(($"4,5,6", statsDict[(4, 5, 6)][0].AvgScore));
            //avgScores.Add(($"4,6,5", statsDict[(4, 5, 6)][1].AvgScore));
            //avgScores.Add(($"5,4,6", statsDict[(4, 5, 6)][2].AvgScore));
            //avgScores.Add(($"5,6,4", statsDict[(4, 5, 6)][3].AvgScore));
            //avgScores.Add(($"6,4,5", statsDict[(4, 5, 6)][4].AvgScore));
            //avgScores.Add(($"6,5,4", statsDict[(4, 5, 6)][5].AvgScore));

            //avgScores.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            //using (StreamWriter outputFile = new StreamWriter("./testTasks456.csv"))
            //{
            //    outputFile.WriteLine("tasks;score");

            //    foreach ((string, double) ds in avgScores)
            //    {
            //        outputFile.WriteLine($"{ds.Item1};{Math.Round(ds.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");

            //    }
            //}
        }

        #endregion
        #region Board Analysis

        private AverageGameStats TestBoard(int boardId, int iterations)
        {
            List<GameStats> stats = new List<GameStats>();
            for (int j = 0; j < iterations; j++)
            {

                Game g = new Game();
                g.AgentGameBoardSettings(7, false, boardId);
                stats.Add(g.Stats);
            }

            if (iterations > 1)
            {
                Console.WriteLine(" Mean: " + stats.Average(item => item.Score));
                //Console.WriteLine(" Average number of buttons: " + (Convert.ToDouble(buttons) / Convert.ToDouble(iterations)));
                //Console.WriteLine(" Average number of cats: " + (Convert.ToDouble(cats.Item1) / Convert.ToDouble(iterations)) + "; " + (Convert.ToDouble(cats.Item2) / Convert.ToDouble(iterations)) + "; " + (Convert.ToDouble(cats.Item3) / Convert.ToDouble(iterations)));
                //Console.WriteLine(" Best score: " + max + "; buttons: " + Convert.ToDouble(maxButtons) +
                //    "; cats: " + Convert.ToDouble(maxCats.Item1) + "; " + Convert.ToDouble(maxCats.Item2) + "; " + Convert.ToDouble(maxCats.Item3));
                //Console.WriteLine(" Lowest score: " + min);
            }

            return new AverageGameStats(7, stats.Average(item => item.Score), stats.Average(item => item.Buttons), (stats.Average(item => item.Cats.Item1), stats.Average(item => item.Cats.Item2), stats.Average(item => item.Cats.Item3)), stats.Max(item => item.Score), stats.Min(item => item.Score));

        }


        private void TestBoards()
        {
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(i);
                TestBoard(i, 5000);
            }
        }
        #endregion

        #endregion




        #region Evolution

        private void Evolution()
        {
            //new Evolution((1, 2, 3), true);
            //new Evolution((1, 2, 4), true);
            //new Evolution((1, 2, 5), true);
            //new Evolution((1, 2, 6), true);
            //new Evolution((1, 3, 4), true);
            //new Evolution((1, 3, 5), true);
            //new Evolution((1, 3, 6), true);
            //new Evolution((1, 4, 5), true);
            //new Evolution((1, 4, 6), true);
            //new Evolution((1, 5, 6), true);
            //new Evolution((2, 3, 4), true);
            //new Evolution((2, 3, 5), true);
            //new Evolution((2, 3, 6), true);
            //new Evolution((2, 4, 5), true);
            //new Evolution((2, 4, 6), true);
            //new Evolution((2, 5, 6), true);
            //new Evolution((3, 4, 5), true);
            //new Evolution((3, 4, 6), true);
            //new Evolution((3, 5, 6), true);
            //new Evolution((4, 5, 6), true);
        }

        private void EvolParamsTesting()
        {

            string[] lines = new string[120];
            int lineIndex = 0;


            for (int i = 1; i <= 6; i++)
            {
                for (int j = i + 1; j <= 6; j++)
                {
                    for (int k = j + 1; k <= 6; k++)
                    {
                        foreach ((int, int, int) tasks in new[]{ (i, j, k), (i, k, j), (j, i, k), (j, k, i), (k, i, j), (k, j, i) })
                        {
                            using (var reader = new StreamReader($"Evol/finalGen_{i}{j}{k}(mixed)new.csv"))
                            {
                                var line = reader.ReadLine();
                                var values = line.Split(';'); // columns

                                int iterations = 5000;

                                double maxScoreAverage = 0;

                                double maxConstB = 1;
                                (double,double,double) maxConstC = (1, 1, 1);
                                double[] maxConstT = new double[6] {1,1,1,1,1,1};


                                for (int gen = 0;gen < 200;gen++) 
                                {
                                    if (!reader.EndOfStream)
                                    {
                                       
                                        line = reader.ReadLine();
                                        values = line.Split(';');

                                        double constB = Convert.ToDouble(values[1]);
                                        (double, double, double) constC = (Convert.ToDouble(values[2]), Convert.ToDouble(values[3]), Convert.ToDouble(values[4]));
                                        double[] constT = new double[]
                                                {
                                                    Convert.ToDouble(values[5]),
                                                    Convert.ToDouble(values[6]),
                                                    Convert.ToDouble(values[7]),
                                                    Convert.ToDouble(values[8]),
                                                    Convert.ToDouble(values[9]),
                                                    Convert.ToDouble(values[10]),
                                                };

                                        GameStats[] stats = new GameStats[iterations];
                                        for (int s = 0; s < iterations; s++)
                                        {
                                            Game g = new Game();
                                            g.EvolParamsTestAgent(
                                                constB, 
                                                constC, 
                                                constT,
                                                tasks);
                                            stats[s] = g.Stats;
                                        }
                                        Console.WriteLine($"{tasks.Item1}{tasks.Item2}{tasks.Item3} : {gen} : {stats.Average(item => item.Score)}");
                                        if (stats.Average(item => item.Score) > maxScoreAverage)
                                        {
                                            maxScoreAverage = stats.Average(item => item.Score);
                                            maxConstB = constB;
                                            maxConstC = (constC.Item1, constC.Item2, constC.Item3);

                                            for(int t=0; t<6; t++) 
                                            {
                                                maxConstT[t] = constT[t];
                                            }
                                        }
                                    }
                                }

                                lines[lineIndex] =
                                    $"{tasks.Item1},{tasks.Item2},{tasks.Item3};" +
                                    $"{Math.Round(maxScoreAverage, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                                    $"{Math.Round(maxConstB, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                                    $"{Math.Round(maxConstC.Item1, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                                    $"{Math.Round(maxConstC.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                                    $"{Math.Round(maxConstC.Item3, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                                    $"{Math.Round(maxConstT[0], 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                                    $"{Math.Round(maxConstT[1], 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                                    $"{Math.Round(maxConstT[2], 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                                    $"{Math.Round(maxConstT[3], 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                                    $"{Math.Round(maxConstT[4], 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                                    $"{Math.Round(maxConstT[5], 3, MidpointRounding.AwayFromZero).ToString("0.000")}";

                                Console.WriteLine(lines[lineIndex]);
                                lineIndex++;
                            }

                        }


                    }
                }
            }

            using (StreamWriter outputFile = new StreamWriter("./Evol/allResultsFinal.csv"))
            {
                outputFile.WriteLine("tasks;score;b;c1;c2;c3;t1;t2;t3;t4;t5;t6");
                foreach (var line in lines)
                {
                    outputFile.WriteLine(line);
                }
            }
        }
        #endregion


        #region Rozptyl


        private void VarianceTesting()
        {
            foreach (int numOfGames in new int[] { 100,250,500,750,1000,2500,5000 })
            {
                double[] averageScores = new double[500];
                Console.WriteLine(numOfGames);

                for (int i = 0;i < 500;i++)
                {
                    int[] scores = new int[numOfGames];

                    for (int j = 0; j<numOfGames; j++)
                    {
                        Game g = new Game();
                        g.AgentGame(7, false);
                        scores[j] = g.Stats.Score;
                    }

                    averageScores[i] = scores.Average();
                    Console.WriteLine(i);
                }

                using (StreamWriter outputFile = new StreamWriter($"./variance_{numOfGames}.csv"))
                {
                    for (int i = 0; i < averageScores.Length; i++)
                    {
                        outputFile.WriteLine(averageScores[i]);
                    }
                    
                }

            }
        }

        #endregion



        private string GetTaskAverageScore(int taskId, GameStats[] stats)
        {
            try
            {
                return $"{Math.Round(stats.Where(item => item.Tasks[taskId-1] >= 0).Average(item => item.Tasks[taskId-1]), 3, MidpointRounding.AwayFromZero).ToString("0.000")}";
            }
            catch
            {
                return "0,000";
            }
        }

    }
}
