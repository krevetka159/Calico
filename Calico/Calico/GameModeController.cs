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

        public GameModeController()
        {
            WeightsDict = new WeightsDict();
            bool end = false;
            string endGame;
            while (!end)
            {

                Console.WriteLine();
                Console.WriteLine(" CALICO ");
                Console.WriteLine();


                ChooseGameMode();


                while (true)
                {
                    Console.WriteLine();
                    Console.Write(" Přejete si ukončit program? (y/n): ");
                    endGame = Console.ReadLine();

                    if (endGame == "y")
                    {
                        end = true;
                        break;
                    }
                    else if (endGame == "n") { break; }
                    else
                    {
                        Console.WriteLine(" Neznámý příkaz");
                    }
                }
            }
        }

        private void ChooseGameMode()
        {
            int gameMode;

            while (true)
            {
                try
                {
                    Console.WriteLine(" Módy: ");
                    Console.WriteLine("   1. Testování agentů");
                    Console.WriteLine("   2. Testování nastavení hry");
                    Console.WriteLine("   3. Evoluce");
                    Console.WriteLine("   4. Hra");

                    Console.Write(" Vyberte mód: ");

                    gameMode = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();


                    switch (gameMode)
                    {
                        case 4:
                            {
                                Game g = new Game();
                                g.SinglePlayer();
                                return;
                            }
                        case 1:
                            {
                                TestingAgent();
                                return;
                            }
                        case 2:
                            {
                                TestSettingsController();
                                return;
                            }
                        case 3:
                            {
                                ChooseEvolutionMode();
                                return;
                            }
                        default:
                            {
                                Console.WriteLine(" " + gameMode + " není možnost módu");
                                break;
                            }
                    }
                }
                catch
                {
                    Console.WriteLine(" Neznámý příkaz, zadejte číslo");
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
                Console.Write(" Zadejte discount faktor: ");
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

        /// <summary>
        /// Choose settings to test
        /// </summary>
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
                TestSetting(tasks, mixedPlacement, boardId);
            }
            else if (testTasks)
            {
                TestTask(tasks, mixedPlacement);
            }
            else if (testBoards)
            {
                TestBoard(boardId);
            }
            else
            {
                mixedPlacement = GetYesNo(" Testovat kombinace úkolů bez konkrétního umístění? (y/n): ");
                TestAllSettings(mixedPlacement);
            }
        }

        /// <summary>
        /// Průměrné skóre s daným umístěním úkolů na desce
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        private double TestSettingScore((int, int, int) tasks, int boardId)
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
        /// <summary>
        /// Průměrné skóre s daným umístěním úkolů
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        private double TestTaskScore((int, int, int) tasks)
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
        /// <summary>
        /// Průměrné skóre s danou deskou
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        private double TestBoardScore(int boardId)
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

        /// <summary>
        /// Otestování dané kombinace úkolů na desce
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="mixedPlacement"></param>
        /// <param name="boardId"></param>
        private void TestSetting((int,int,int) tasks, bool mixedPlacement, int boardId)
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
                    (TestSettingScore((i, j, k), boardId) +
                    TestSettingScore((i, k, j), boardId) +
                    TestSettingScore((j, i, k), boardId) +
                    TestSettingScore((j, k, i), boardId) +
                    TestSettingScore((k, i, j), boardId) +
                    TestSettingScore((k, j, i), boardId))
                    / 6;
            }
            else
            {
                score = TestSettingScore(tasks, boardId);
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

        /// <summary>
        /// Otestování dané kombinace úkolů
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="mixedPlacement"></param>
        private void TestTask((int, int, int) tasks, bool mixedPlacement)
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
                    (TestTaskScore((i, j, k)) +
                    TestTaskScore((i, k, j)) +
                    TestTaskScore((j, i, k)) +
                    TestTaskScore((j, k, i)) +
                    TestTaskScore((k, i, j)) +
                    TestTaskScore((k, j, i))
                    ) / 6;
            }
            else
            {
                score = TestTaskScore(tasks);
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

        /// <summary>
        /// Otestování dané desky
        /// </summary>
        /// <param name="boardId"></param>
        private void TestBoard(int boardId)
        {
            bool outputToFile = GetYesNo(" Chcete výsledky zapsat do souboru? (y/n): ");
            string fileName = "";

            if (outputToFile)
            {
                fileName = GetOutputFileName();
            }

            double score = TestBoardScore(boardId);

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

        /// <summary>
        /// Otestování všech kombinací úkolů na všech deskách
        /// </summary>
        /// <param name="mixedPlacement"></param>
        private void TestAllSettings(bool mixedPlacement)
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
                                        (TestSettingScore((i, j, k), b) +
                                        TestSettingScore((i, k, j), b) +
                                        TestSettingScore((j, i, k), b) +
                                        TestSettingScore((j, k, i), b) +
                                        TestSettingScore((k, i, j), b) +
                                        TestSettingScore((k, j, i), b))
                                        / 6;
                                    results[index] = $"{b};{i},{j},{k};{Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}";
                                    index++;

                                }
                                else
                                {
                                    foreach((int,int,int) t in new[] { (i, j, k), (i, k, j), (j, i, k), (j, k, i), (k, i, j), (k, j, i) })
                                    {
                                        score = TestSettingScore(t, b);
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
                                        (TestSettingScore((i, j, k), b) +
                                        TestSettingScore((i, k, j), b) +
                                        TestSettingScore((j, i, k), b) +
                                        TestSettingScore((j, k, i), b) +
                                        TestSettingScore((k, i, j), b) +
                                        TestSettingScore((k, j, i), b))
                                        / 6;
                                    Console.WriteLine($" {b} | {i},{j},{k}: {Math.Round(score, 3, MidpointRounding.AwayFromZero).ToString("0.000")}");
                                }
                                else
                                { 
                                    foreach((int,int,int) t in new[] { (i, j, k), (i, k, j), (j, i, k), (j, k, i), (k, i, j), (k, j, i) })
                                    {
                                        score = TestSettingScore(t, b);
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

        private int GetEvolMode()
        {
            int mode;

            Console.WriteLine(" Vyberte jeden z následujících: ");
            Console.WriteLine("   1. Spustit evoluci");
            Console.WriteLine("   2. Získat váhy pro konkrétní umístění");

            while (true)
            {
                Console.Write(" Zadejte vybrané: ");

                try
                {
                    mode = Convert.ToInt32(Console.ReadLine());

                    if (mode == 1 || mode == 2)
                    {
                        return mode;
                    }
                    else
                    {
                        Console.WriteLine(" Zadejte 1 nebo 2");
                    }
                }
                catch
                {
                    Console.WriteLine(" Zadejte 1 nebo 2");
                }
            }
        }

        private void ChooseEvolutionMode()
        {
            // jestli spustím evoluci nebo získání vah z poslední generace
            int mode = GetEvolMode();

            if (mode == 1)
            {
                Evolution();
            }
            else // mode == 2
            {
                EvolParamsTesting();
            }
        }

        /// <summary>
        /// Spuštění evoluce pro kombinaci úkolů
        /// </summary>
        private void Evolution()
        {
            // vybrat tasky
            (int,int, int) tasks = GetTasksInput();
            // vybrat jestli mixed placement
            bool mixedPlacement = GetYesNo(" Chcete testovat kombinace bez konkrétního umístění? (y/n): ");

            // název souboru
            string outputFileName = GetOutputFileName();

            new Evolution(tasks, true, outputFileName);

        }

        /// <summary>
        /// Vybrání nejlepších vah pro dané umístění úkolů z poslední generace evolu pro kombinaci úkolů bez ohledu na umístění
        /// </summary>
        private void EvolParamsTesting()
        {
            // název souboru ze kterého beru
            string inputFileName = GetOutputFileName();
            // název souboru kam ukládám
            string outputFileName = GetOutputFileName();
            // kombinace kterou čtu
            (int i, int j, int k) = GetTasksInput();

            string[] lines = new string[6]; // 6 možností umístění kombinace úkolů
            int lineIndex = 0;

            foreach ((int, int, int) tasks in new[]{ (i, j, k), (i, k, j), (j, i, k), (j, k, i), (k, i, j), (k, j, i) })
            {
                using (var reader = new StreamReader(inputFileName))
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

                            // váha pro knoflíky
                            double constB = Convert.ToDouble(values[1]);
                            // váhy pro žetony koček
                            (double, double, double) constC = (Convert.ToDouble(values[2]), Convert.ToDouble(values[3]), Convert.ToDouble(values[4]));
                            // váhy pro úkoly
                            double[] constT = new double[]
                                    {
                                        Convert.ToDouble(values[5]),
                                        Convert.ToDouble(values[6]),
                                        Convert.ToDouble(values[7]),
                                        Convert.ToDouble(values[8]),
                                        Convert.ToDouble(values[9]),
                                        Convert.ToDouble(values[10]),
                                    };

                            double[] stats = new double[iterations];

                            // test pro dané váhy
                            for (int s = 0; s < iterations; s++) 
                            {
                                EvolutionGame g = new EvolutionGame(new Weights(constB, constC, constT),tasks);
                                stats[s] = g.Game(); 
                            }
                            Console.WriteLine($"{tasks.Item1}{tasks.Item2}{tasks.Item3} : {gen} : {stats.Average()}");

                            // hledání maxima pro dané umístění úkolů
                            if (stats.Average() > maxScoreAverage)
                            {
                                maxScoreAverage = stats.Average();
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

            using (StreamWriter outputFile = new StreamWriter(outputFileName))
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

        /// <summary>
        /// Výpočet průměrného splnění úkolu přes nezáporné hodnoty (pouze přes hry, kdy byl úkol vybrán)
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="stats"></param>
        /// <returns></returns>
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
