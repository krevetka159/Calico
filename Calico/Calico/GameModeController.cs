using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class GameModeController
    {
        private List<(int, string)> AgentDescription = new List<(int, string)>()
        {
            (1, " Kompletně náhodný agent "),
            (2, " Nejlepší umístění vzhledek k barvám "),
            (3, " Nejlepší umístění vzhledem ke vzorům "),
            (4, " Nejlepší umístění "),
            (5, " Nejlepší umístění s malou náhodou "),
            (6, " Nejlepší umístění náhodného dílku "),
            (7, " Utility fce"),
            (8, " Minimax")
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
                    for (int j = 0; j < 10; j++)
                    {
                        Console.WriteLine(j);
                        int iterations = 500;
                        List<GameStats> stats = new List<GameStats>();
                        for (int i = 0; i < iterations; i++)
                        {
                            stats.Add(null);
                        }
                        Parallel.For(0, iterations, i =>
                        {
                            Game g = new Game();
                            g.AgentGame(agentType, false);
                            stats[i] = g.Stats;
                        });

                        Console.WriteLine(stats.Average(item => item.Score));
                    }
                        
                    //});

                    //using (StreamWriter outputFile = new StreamWriter($"./testAgent{agentOption}.csv"))
                    //{
                    //    outputFile.WriteLine("mean;avgButtons;avgC1;avgC2;avgC3;best;lowest");

                    //    foreach (GameStats gs in stats)
                    //    {
                    //        outputFile.WriteLine(
                    //            $"{Math.Round(gs.AvgScore, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                    //            $"{Math.Round(gs.AvgButtons, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                    //            $"{Math.Round(gs.AvgCats.Item1, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                    //            $"{Math.Round(gs.AvgCats.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                    //            $"{Math.Round(gs.AvgCats.Item3, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                    //            $"{gs.BestScore};" +
                    //            $"{gs.LowestScore}"
                    //            );

                    //    }
                    //}

                    break;
                }
                else if (newGame == "y")
                {
                    Game g = new Game();
                    g.AgentGame(7, true);
                }
                else
                {
                    Console.WriteLine(" Invalid expression");
                }
            }
        }

        private void TestAll()
        {
            int iterations = 500;
            List<GameStats> stats = new List<GameStats>();
            for (int j = 0; j < iterations; j++)
            {
                stats.Add(null);
            }

            Console.WriteLine(" Agent options: ");

            foreach ((int, string) ad in AgentDescription)
            {
                Console.WriteLine($"    {ad.Item1}. {ad.Item2}");
            }

            for (int i = 1; i <= AgentDescription.Count(); i++)
            {
                Console.WriteLine(" " + i + ": ");

                Parallel.For(0, iterations, i =>
                {
                    Game g = new Game();
                    g.AgentGame(i, false);
                    stats[i] = g.Stats;
                });

                Console.WriteLine(stats.Average(item => item.Score));
                Console.WriteLine();
            }

            // Write the string array to a new file named "WriteLines.txt".
            //using (StreamWriter outputFile = new StreamWriter("./testAll.csv"))
            //{
            //    outputFile.WriteLine("agent;averageScore;buttons;cats;best;lowest");
            //    foreach (GameStats gs in stats)
            //        outputFile.WriteLine($"{gs.AgentType};" +
            //            $"{Math.Round(gs.AvgScore, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
            //            $"{Math.Round(gs.AvgButtons, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
            //            $"{Math.Round(gs.AvgCats.Item1, 3, MidpointRounding.AwayFromZero).ToString("0.000")}|" +
            //            $"{Math.Round(gs.AvgCats.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")}|" +
            //            $"{Math.Round(gs.AvgCats.Item3, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
            //            $"{gs.BestScore};{gs.LowestScore}");
            //}

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
                g.AgentGameSettings(7, false, tasks, boardId);
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
            List<Dictionary<(int, int, int), List<AverageGameStats>>> statsDict = new List<Dictionary<(int, int, int), List<AverageGameStats>>>();

            for (int b =  0; b < 4; b++)
            {
                statsDict.Add(new Dictionary<(int, int, int), List<AverageGameStats>>());
                for (int i = 1; i <= 6; i++)
                {
                    for (int j = i + 1; j <= 6; j++)
                    {
                        for (int k = j + 1; k <= 6; k++)
                        {
                            
                            statsDict[b][(i, j, k)] = new List<AverageGameStats>();

                            Console.WriteLine($" {b} - {i},{j},{k}: ");
                            statsDict[b][(i, j, k)].Add(TestSetting(7, 5000, (i, j, k), b));
                            Console.WriteLine();

                            Console.WriteLine($" {b} - {i},{k},{j}: ");
                            statsDict[b][(i, j, k)].Add(TestSetting(7, 5000, (i, k, j), b));
                            Console.WriteLine();

                            Console.WriteLine($" {b} - {j},{i},{k}: ");
                            statsDict[b][(i, j, k)].Add(TestSetting(7, 5000, (j, i, k), b));
                            Console.WriteLine();

                            Console.WriteLine($" {b} - {j},{k},{i}: ");
                            statsDict[b][(i, j, k)].Add(TestSetting(7, 5000, (j, k, i), b));
                            Console.WriteLine();

                            Console.WriteLine($" {b} - {k},{i},{j}: ");
                            statsDict[b][(i, j, k)].Add(TestSetting(7, 5000, (k, i, j), b));
                            Console.WriteLine();

                            Console.WriteLine($" {b} - {k},{j},{i}: ");
                            statsDict[b][(i, j, k)].Add(TestSetting(7, 5000, (k, j, i), b));
                            Console.WriteLine();
                        }
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                List<(string, List<double>)> avgScores = new List<(string, List<double>)>();
                foreach ((int, int, int) tasks in statsDict[0].Keys)
                {
                    avgScores.Add(($"{tasks.Item1},{tasks.Item2},{tasks.Item3}",
                        new List<double>() { 
                            statsDict[0][tasks].Average(item => item.AvgScore),
                            statsDict[1][tasks].Average(item => item.AvgScore),
                            statsDict[2][tasks].Average(item => item.AvgScore),
                            statsDict[3][tasks].Average(item => item.AvgScore),
                        }));
                }
                avgScores.Sort((x, y) => x.Item2.Average().CompareTo(y.Item2.Average()));

                using (StreamWriter outputFile = new StreamWriter($"./testSetting.csv"))
                {
                    outputFile.WriteLine("tasks;avg1;avg2;avg3;avg4;avg");
                    foreach ((string, List<double>) ds in avgScores)
                    {
                        outputFile.WriteLine($"{ds.Item1};" +
                            $"{Math.Round(ds.Item2[0], 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(ds.Item2[1], 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(ds.Item2[2], 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(ds.Item2[3], 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                            $"{Math.Round(ds.Item2.Average(), 3, MidpointRounding.AwayFromZero).ToString("0.000")}");
                    }
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
                Console.WriteLine(" Mean: " + stats.Average(item => item.Score));
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
            Dictionary<(int, int, int), List<AverageGameStats>> statsDict = new Dictionary<(int, int, int), List<AverageGameStats>>();

            for (int i = 1; i <= 6; i++)
            {
                for (int j = i + 1; j <= 6; j++)
                {
                    for (int k = j + 1; k <= 6; k++)
                    {
                        statsDict[(i, j, k)] = new List<AverageGameStats>();

                        Console.WriteLine($" {i},{j},{k}: ");
                        statsDict[(i, j, k)].Add(TestTask(11, 1000, (i, j, k)));
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

            List<(string, double)> avgScores = new List<(string, double)>();
            foreach (KeyValuePair<(int, int, int), List<AverageGameStats>> ds in statsDict)
            {
                avgScores.Add(($"{ds.Key.Item1},{ds.Key.Item2},{ds.Key.Item3}", ds.Value.Average(item => item.AvgScore)));

            }
            avgScores.Sort((x, y) => x.Item2.CompareTo(y.Item2));

            using (StreamWriter outputFile = new StreamWriter("./testTasks.csv"))
            {
                outputFile.WriteLine("tasks;averageScore");
                foreach ((string, double) ds in avgScores)
                {
                    outputFile.WriteLine($"{ds.Item1};{Math.Round(ds.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");

                }
            }

            avgScores = new List<(string, double)>();

            avgScores.Add(($"1,2,3", statsDict[(1, 2, 3)][0].AvgScore));
            avgScores.Add(($"1,3,2", statsDict[(1, 2, 3)][1].AvgScore));
            avgScores.Add(($"2,1,3", statsDict[(1, 2, 3)][2].AvgScore));
            avgScores.Add(($"2,3,1", statsDict[(1, 2, 3)][3].AvgScore));
            avgScores.Add(($"3,1,2", statsDict[(1, 2, 3)][4].AvgScore));
            avgScores.Add(($"3,2,1", statsDict[(1, 2, 3)][5].AvgScore));

            avgScores.Sort((x, y) => x.Item2.CompareTo(y.Item2));

            using (StreamWriter outputFile = new StreamWriter("./testTasks123.csv"))
            {
                outputFile.WriteLine("tasks;score");

                foreach ((string, double) ds in avgScores)
                {
                    outputFile.WriteLine($"{ds.Item1};{Math.Round(ds.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");

                }
            }

            avgScores = new List<(string, double)>();

            avgScores.Add(($"2,3,5", statsDict[(2, 3, 5)][0].AvgScore));
            avgScores.Add(($"2,5,3", statsDict[(2, 3, 5)][1].AvgScore));
            avgScores.Add(($"3,2,5", statsDict[(2, 3, 5)][2].AvgScore));
            avgScores.Add(($"3,5,2", statsDict[(2, 3, 5)][3].AvgScore));
            avgScores.Add(($"5,2,3", statsDict[(2, 3, 5)][4].AvgScore));
            avgScores.Add(($"5,3,2", statsDict[(2, 3, 5)][5].AvgScore));

            avgScores.Sort((x, y) => x.Item2.CompareTo(y.Item2));

            using (StreamWriter outputFile = new StreamWriter("./testTasks235.csv"))
            {
                outputFile.WriteLine("tasks;score");

                foreach ((string, double) ds in avgScores)
                {
                    outputFile.WriteLine($"{ds.Item1};{Math.Round(ds.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");

                }
            }
            avgScores = new List<(string, double)>();

            avgScores.Add(($"1,4,6", statsDict[(1, 4, 6)][0].AvgScore));
            avgScores.Add(($"1,6,4", statsDict[(1, 4, 6)][1].AvgScore));
            avgScores.Add(($"4,1,6", statsDict[(1, 4, 6)][2].AvgScore));
            avgScores.Add(($"4,6,1", statsDict[(1, 4, 6)][3].AvgScore));
            avgScores.Add(($"6,1,4", statsDict[(1, 4, 6)][4].AvgScore));
            avgScores.Add(($"6,4,1", statsDict[(1, 4, 6)][5].AvgScore));

            avgScores.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            using (StreamWriter outputFile = new StreamWriter("./testTasks146.csv"))
            {
                outputFile.WriteLine("tasks;score");

                foreach ((string, double) ds in avgScores)
                {
                    outputFile.WriteLine($"{ds.Item1};{Math.Round(ds.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");

                }
            }
            avgScores = new List<(string, double)>();

            avgScores.Add(($"4,5,6", statsDict[(4, 5, 6)][0].AvgScore));
            avgScores.Add(($"4,6,5", statsDict[(4, 5, 6)][1].AvgScore));
            avgScores.Add(($"5,4,6", statsDict[(4, 5, 6)][2].AvgScore));
            avgScores.Add(($"5,6,4", statsDict[(4, 5, 6)][3].AvgScore));
            avgScores.Add(($"6,4,5", statsDict[(4, 5, 6)][4].AvgScore));
            avgScores.Add(($"6,5,4", statsDict[(4, 5, 6)][5].AvgScore));

            avgScores.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            using (StreamWriter outputFile = new StreamWriter("./testTasks456.csv"))
            {
                outputFile.WriteLine("tasks;score");

                foreach ((string, double) ds in avgScores)
                {
                    outputFile.WriteLine($"{ds.Item1};{Math.Round(ds.Item2, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");

                }
            }
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
            new Evolution();
        }
        #endregion

    }
}
