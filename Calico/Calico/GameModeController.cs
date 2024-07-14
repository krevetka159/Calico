using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Calico
{
    public class GameModeController
    {
        private List<(int, string)> AgentDescription = new List<(int, string)>()
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

        private AverageGameStats avgStats;

        public WeightsDict WeightsDict;

        public GameModeController(int mode)
        {
            WeightsDict = new WeightsDict();

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
                        TestingAgent();
                        break;
                    }
                case 6:
                    {
                        TestSettingsController();
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

        #region GetConsoleInput

        private int PickAgent()
        {
            int agentOption;

            while (true)
            {
                try
                {
                    Console.WriteLine(" Nabídka agentů: ");

                    foreach ((int, string) ad in AgentDescription)
                    {
                        Console.WriteLine($"    {ad.Item1}. {ad.Item2}");
                    }
                    Console.Write(" Vyberte číslo agenta: ");

                    agentOption = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();

                    if (1 <= agentOption && agentOption <= AgentDescription.Count())
                    {
                        return agentOption;
                    }
                    else
                    {
                        Console.WriteLine(" " + agentOption + " není číslo agenta");
                    }

                }
                catch
                {
                    Console.WriteLine(" Zadejte celé číslo.");
                }
            }
        }

        private bool GetYesNo(string message)
        {
            string val;
            while (true)
            {
                Console.Write(message);

                try
                {
                    val = Console.ReadLine().Replace(" ", "");
                    if (val == "n")
                    {
                        return false;
                    }
                    else if (val == "y")
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Neznámý příkaz");
                    }

                }
                catch
                {

                }
            }
        }

        private string GetOutputFileName()
        {
            string output;
            while (true)
            {
                Console.Write(" Zadejte jméno výstupního souboru: ");

                output = Console.ReadLine();
                if (output != null)
                {
                    return output;
                }       
                
            }
        }

        private int getTreeDepth()
        {
            int depth;

            while (true)
            {
                try
                {
                    Console.Write(" Vyberte hloubku stromu (2/3): ");
                    depth = Convert.ToInt32(Console.ReadLine());

                    if (2 <= depth && depth <= 3)
                    {
                        return depth;
                    }
                    else
                    {
                        Console.WriteLine(" Zadejte číslo 2 nebo 3");
                    }
                }
                catch
                {
                    Console.WriteLine(" Zadejte číslo 2 nebo 3");
                }
            }
        }

        private int getSimulationSize()
        {
            int simulationSize;

            while (true)
            {
                try
                {
                    Console.Write(" Zadejte velikost simulace: ");

                    simulationSize = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    return simulationSize;
                }
                catch
                {
                    Console.WriteLine(" Zadejte celé číslo.");
                }
            }
        }
        private double getDiscountFactor()
        {
            double discountFactor;

            while (true)
            {
                Console.Write(" Zadejte velikost simulace: ");
                try
                {
                    discountFactor = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine();
                    return discountFactor;
                }
                catch
                {
                    Console.WriteLine(" Zadejte číslo ");
                }
            }
        }

        private (int, int, int) GetTasksInput()
        {
            int task1, task2, task3;

            while (true)
            {
                try
                {
                    Console.Write(" Zadejte čísla úkolů ve formátu i,j,k: ");

                    string[] tasks = Console.ReadLine().Split(",");

                    task1 = Convert.ToInt32(tasks[0]);
                    task2 = Convert.ToInt32(tasks[1]);
                    task3 = Convert.ToInt32(tasks[2]);

                    if (tasks.Length != 3)
                    {
                        Console.WriteLine(" Zadejte právě tři čísla v danm formátu");
                    }
                    else
                    {
                        if (1 <= task1 && task1 <= 6 && 1 <= task2 && task2 <= 6 && 1 <= task3 && task3 <= 6)
                        {
                            if (task1 == task2 || task1 == task3 || task2 == task3)
                            {
                                Console.WriteLine(" Každý úkol lze vybrat nejvýše jednou");
                            }
                            else
                            {
                                return (task1, task2, task3);
                            }
                        }
                        else
                        {
                            Console.WriteLine(" Zadejte čísla od 1 do 6");
                        }
                    }

                    
                }
                catch
                {
                    Console.WriteLine(" Zadejte čísla od 1 do 6");
                }
            }
        }

        private int GetBoardId()
        {
            int boardId;

            while (true)
            {
                try
                {
                    Console.Write(" Zadejte číslo desky (1-4): ");

                    boardId = Convert.ToInt32(Console.ReadLine());

                    if (1 <= boardId && boardId <= 4)
                    {
                        return boardId;
                    }
                    else
                    {
                        Console.WriteLine(" Zadejte číslo od 1 do 4");
                    }
                }
                catch
                {
                    Console.WriteLine(" Zadejte číslo od 1 do 4");
                }
            }
        }

        #endregion

        #region TestingAgents - fixed

        private void TestingAgent()
        {
            int agentType = PickAgent();

            bool outputToFile = GetYesNo(" Chcete výsledky zapsat do souboru? (y/n): ");
            string fileName = "";

            if (outputToFile)
            {
                fileName = GetOutputFileName();
            }

            int iterations;
            GameStats[] stats = null;


            if(agentType < 7) // rand, základní a rozšířené ohodnocení
            {
                iterations = 5000;
                stats = new GameStats[iterations];

                for (int i = 0; i < iterations; i++)
                {
                    Game g = new Game();
                    g.AgentGame(agentType, false);
                    stats[i] = g.Stats;
                    Console.WriteLine($" {i} : {g.Stats.Score}");
                }
            }
            else if(agentType == 7) // vážená rozšířená funkce
            {
                iterations = 5000;
                stats = new GameStats[iterations];

                for (int i = 0; i < iterations; i++)
                {
                    Game g = new Game();
                    g.WeightedAgentGame(WeightsDict, false);
                    stats[i] = g.Stats;
                    Console.WriteLine($" {i} : {g.Stats.Score}");
                }
            }
            else if(agentType == 8) // stromové prohledávání
            {
                iterations = 5000;
                stats = new GameStats[iterations];

                int depth = getTreeDepth();
                double discountFactor = getDiscountFactor();

                for (int i = 0; i < iterations; i++)
                {
                    Game g = new Game();
                    g.TreeSearchAgentGame(WeightsDict, false, depth, discountFactor);
                    stats[i] = g.Stats;
                    Console.WriteLine($" {i} : {g.Stats.Score}");
                }
            }
            else if (agentType == 9) // MCTS inspired prohledávání
            {
                iterations = 250;
                stats = new GameStats[iterations];

                int simulationSize = getSimulationSize();

                for (int i = 0;i < iterations; i++)
                {
                    Game g = new Game();
                    g.SimulationTSAgentGame(WeightsDict, false, simulationSize);
                    stats[i] = g.Stats;
                    Console.WriteLine($" {i} : {g.Stats.Score}");
                }
            }

            if (outputToFile)
            {
                try
                {
                    using (StreamWriter outputFile = new StreamWriter(fileName))
                    {
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
                }
                catch
                {
                    Console.WriteLine(" Zápis do souboru selhal");
                }
            }
            else
            {
                Console.WriteLine(stats.Max(item => item.Score));
                Console.WriteLine(stats.Min(item => item.Score));
                Console.WriteLine(
                            $"{Math.Round(stats.Average(item => item.Score), 3, MidpointRounding.AwayFromZero).ToString("0.000")}, " +
                            $"{Math.Round(stats.Average(item => item.Buttons), 3, MidpointRounding.AwayFromZero).ToString("0.000")} " +
                            $"{Math.Round(stats.Average(item => item.Cats.Item1), 3, MidpointRounding.AwayFromZero).ToString("0.000")}, " +
                            $"{Math.Round(stats.Average(item => item.Cats.Item2), 3, MidpointRounding.AwayFromZero).ToString("0.000")}, " +
                            $"{Math.Round(stats.Average(item => item.Cats.Item3), 3, MidpointRounding.AwayFromZero).ToString("0.000")}, " +
                            $"{GetTaskAverageScore(1, stats)}, " +
                            $"{GetTaskAverageScore(2, stats)}, " +
                            $"{GetTaskAverageScore(3, stats)}, " +
                            $"{GetTaskAverageScore(4, stats)}, " +
                            $"{GetTaskAverageScore(5, stats)}, " +
                            $"{GetTaskAverageScore(6, stats)} "
                );
            }
                
        }
        #endregion

        #region Settings analysis

        private void TestSettingsController()
        {
            bool testTasks = GetYesNo(" Testování konkrétní kombinace úkolů? (y/n): ");
            (int, int, int) tasks = (0, 0, 0);
            bool mixedPlacement = false;

            if(testTasks)
            {
                tasks = GetTasksInput();
                mixedPlacement = GetYesNo(" Otestovat kombinaci bez konkrétního umístění? (y/n): ");
            }

            bool testBoards = GetYesNo(" Testování konkrétní desky? (y/n): ");
            int boardId = -1;

            if(testBoards)
            {
                boardId = GetBoardId();
            }

            if (testTasks && testBoards)
            {
                TestSettingMock(tasks, mixedPlacement, boardId);
            }
            else if (testTasks)
            {
                TestTaskMock(tasks, mixedPlacement);
            }
            else if (testBoards)
            {
                TestBoardMock(boardId);
            }
            else
            {
                mixedPlacement = GetYesNo(" Testovat kombinace úkolů bez konkrétního umístění? (y/n): ");
                TestAllMock(mixedPlacement);
            }
        }

        private double TestSetting((int, int, int) tasks, int boardId)
        {
            int iterations = 5000;
            GameStats[] stats = new GameStats[iterations];

            for (int i = 0; i < iterations; i++)
            {
                Game g = new Game();
                g.AgentGameSettings(5, false, tasks, boardId);
                stats[i] = g.Stats;
            }

            return stats.Average(item => item.Score);
        }

        private double TestTask((int, int, int) tasks)
        {
            int iterations = 5000;
            GameStats[] stats = new GameStats[iterations];

            for (int i = 0; i < iterations; i++)
            {
                Game g = new Game();
                g.AgentGameTaskSettings(5, false, tasks);
                stats[i] = g.Stats;
            }

            return stats.Average(item => item.Score);
        }

        private double TestBoard(int boardId)
        {
            int iterations = 5000;
            GameStats[] stats = new GameStats[iterations];

            for (int i = 0; i < iterations; i++)
            {
                Game g = new Game();
                g.AgentGameBoardSettings(5, false, boardId);
                stats[i] = g.Stats;
            }

            return stats.Average(item => item.Score);
        }

        private void TestSettingMock((int,int,int) tasks, bool mixedPlacement, int boardId)
        {
            bool outputToFile = GetYesNo(" Chcete výsledky zapsat do souboru? (y/n): ");
            string fileName = "";

            if (outputToFile)
            {
                fileName = GetOutputFileName();
            }

            double score;

            if (mixedPlacement)
            {
                int i = tasks.Item1;
                int j = tasks.Item2;
                int k = tasks.Item3;

                score =
                    (TestSetting((i, j, k), boardId) +
                    TestSetting((i, k, j), boardId) +
                    TestSetting((j, i, k), boardId) +
                    TestSetting((j, k, i), boardId) +
                    TestSetting((k, i, j), boardId) +
                    TestSetting((k, j, i), boardId))
                    / 6;
            }
            else
            {
                score = TestSetting(tasks, boardId);
            }

            if (outputToFile)
            {
                using (StreamWriter outputFile = new StreamWriter(fileName))
                {
                    outputFile.WriteLine("board;tasks;score");
                    outputFile.WriteLine(
                            $"{boardId};{tasks.Item1},{tasks.Item2},{tasks.Item3};" +
                            $"{Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}"
                    );
                }  
            }
            else
            {
                Console.WriteLine(
                    $" {boardId} | {tasks.Item1},{tasks.Item2},{tasks.Item3}: " +
                    $"{Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}"
                );
            }
        }

        private void TestTaskMock((int, int, int) tasks, bool mixedPlacement)
        { 
            bool outputToFile = GetYesNo(" Chcete výsledky zapsat do souboru? (y/n): ");
            string fileName = "";

            if (outputToFile)
            {
                fileName = GetOutputFileName();
            }

            double score;

            if (mixedPlacement)
            {
                int i = tasks.Item1;
                int j = tasks.Item2;
                int k = tasks.Item3;

                score =
                    (TestTask((i, j, k)) +
                    TestTask((i, k, j)) +
                    TestTask((j, i, k)) +
                    TestTask((j, k, i)) +
                    TestTask((k, i, j)) +
                    TestTask((k, j, i))
                    ) / 6;
            }
            else
            {
                score = TestTask(tasks);
            }

            if (outputToFile)
            {
                using (StreamWriter outputFile = new StreamWriter(fileName))
                {
                    outputFile.WriteLine("tasks;score");
                    outputFile.WriteLine(
                            $"{tasks.Item1},{tasks.Item2},{tasks.Item3};" +
                            $"{Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}"
                    );
                }
            }
            else
            {
                Console.WriteLine(
                    $" {tasks.Item1},{tasks.Item2},{tasks.Item3}: " +
                    $"{Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}"
                );
            }
        }

        private void TestBoardMock(int boardId)
        {
            bool outputToFile = GetYesNo(" Chcete výsledky zapsat do souboru? (y/n): ");
            string fileName = "";

            if (outputToFile)
            {
                fileName = GetOutputFileName();
            }

            double score = TestBoard(boardId);

            if (outputToFile)
            {
                using (StreamWriter outputFile = new StreamWriter(fileName))
                {
                    outputFile.WriteLine("board;score");
                    outputFile.WriteLine(
                    $"{boardId};" +
                            $"{Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}"
                    );
                }
            }
            else
            {
                Console.WriteLine(
                $" {boardId}: " +
                    $"{Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}"
                );
            }
        }

        private void TestAllMock(bool mixedPlacement)
        {
            bool outputToFile = GetYesNo(" Chcete výsledky zapsat do souboru? (y/n): ");
            string fileName = "";

            if (outputToFile)
            {
                fileName = GetOutputFileName();
            }

            double score;

            if (outputToFile)
            {
                string[] results;
                if (mixedPlacement)
                {
                    results = new string[4 * 120];
                }
                else
                {
                    results = new string[4 * 20];
                }

                int index = 0;
                for (int b = 0; b < 4; b++)
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        for (int j = i + 1; j <= 6; j++)
                        {
                            for (int k = j + 1; k <= 6; k++)
                            {
                                if (mixedPlacement)
                                {
                                    score = 
                                        (TestSetting((i, j, k), b) +
                                        TestSetting((i, k, j), b) +
                                        TestSetting((j, i, k), b) +
                                        TestSetting((j, k, i), b) +
                                        TestSetting((k, i, j), b) +
                                        TestSetting((k, j, i), b))
                                        / 6;
                                    results[index] = $"{b};{i},{j},{k};{Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}";
                                    index++;

                                }
                                else
                                {
                                    foreach((int,int,int) t in new[] { (i, j, k), (i, k, j), (j, i, k), (j, k, i), (k, i, j), (k, j, i) })
                                    {
                                        score = TestSetting(t, b);
                                        results[index] = $"{b};{t.Item1},{t.Item2},{t.Item3};{Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}";
                                        index++;
                                    }

                                }
                            }
                        }
                    }
                }

                using (StreamWriter outputFile = new StreamWriter(fileName))
                {
                    outputFile.WriteLine("board;tasks;score");
                    foreach (string line in results)
                    {
                        outputFile.WriteLine(line);
                    }
                }

            }
            else
            {
                for (int b = 0; b < 4; b++)
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        for (int j = i + 1; j <= 6; j++)
                        {
                            for (int k = j + 1; k <= 6; k++)
                            {
                                if (mixedPlacement)
                                {
                                    score =
                                        (TestSetting((i, j, k), b) +
                                        TestSetting((i, k, j), b) +
                                        TestSetting((j, i, k), b) +
                                        TestSetting((j, k, i), b) +
                                        TestSetting((k, i, j), b) +
                                        TestSetting((k, j, i), b))
                                        / 6;
                                    Console.WriteLine($" {b} | {i},{j},{k}: {Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");
                                }
                                else
                                { 
                                    foreach((int,int,int) t in new[] { (i, j, k), (i, k, j), (j, i, k), (j, k, i), (k, i, j), (k, j, i) })
                                    {
                                        score = TestSetting(t, b);
                                        Console.WriteLine($"{b} | {t.Item1},{t.Item2},{t.Item3}: {Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

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

            new Evolution((1, 5, 6), false);
            new Evolution((1, 6, 5), false);
            new Evolution((5, 1, 6), false);
            new Evolution((5, 6, 1), false);
            new Evolution((6, 1, 5), false);
            new Evolution((6, 5, 1), false);
        }

        private void EvolParamsTesting()
        {

            string[] lines = new string[6];
            int lineIndex = 0;


            for (int i = 1; i <= 6; i++)
            {
                for (int j = i + 1; j <= 6; j++)
                {
                    for (int k = j + 1; k <= 6; k++)
                    {
                        foreach ((int, int, int) tasks in new[]{ (i, j, k), (i, k, j), (j, i, k), (j, k, i), (k, i, j), (k, j, i) })
                        {
                            using (var reader = new StreamReader($"finalGen_{tasks.Item1}{tasks.Item2}{tasks.Item3}(detail)new.csv"))
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

            using (StreamWriter outputFile = new StreamWriter("156DetailFinal.csv"))
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

        #region Helpers

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

        #endregion

    }
}
