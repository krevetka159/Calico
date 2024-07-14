using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class Evolution
    {
        private int population_size = 200;
        private int generations = 200;
        private double mutation_prob = 1;
        private double tournament_prob = 0.8;


        private (int, int, int) tasks;
        private bool mixed;

        private string outputFileName;

        private Random random;

        public Evolution((int,int,int) tasks, bool mixed, string outputFileName)
        {
            random = new Random();

            this.tasks = tasks;
            this.mixed = mixed;

            this.outputFileName = outputFileName;

            EvolutionRun();
        }

        private void EvolutionRun()
        {
            Weights[] population = RandomPopulation();
            double[] fitness = new double[population_size];

            double[] avg = new double[generations];
            double[] max = new double[generations];

            Console.WriteLine($"{tasks.Item1}{tasks.Item2}{tasks.Item3}");

            for (int g = 0; g < generations - 1; g++)
            {
                Console.WriteLine($" Generation {g + 1}");

                fitness = EvolutionGames(population);

                population = Mutation(Selection(population, fitness));

                Console.WriteLine(fitness.Average());
                avg[g] = fitness.Average();
                Console.WriteLine(fitness.Max());
                max[g] = fitness.Max();
            }

            Console.WriteLine($" Generation {generations}");

            fitness = EvolutionGames(population);
            Console.WriteLine(fitness.Average());
            avg[generations - 1] = fitness.Average();
            Console.WriteLine(fitness.Max());
            max[generations - 1] = fitness.Max();

            List<(double, Weights)> results = new List<(double, Weights)>();
            for (int i = 0; i < population_size; i++)
            {
                results.Add((fitness[i], population[i]));
            }
            results.Sort((x, y) => y.Item1.CompareTo(x.Item1));

            using (StreamWriter outputFile = new StreamWriter(outputFileName))
            {
                outputFile.WriteLine("fitness;buttons;c1;c2;c3;t1;t2;t3;t4;t5;t6");
                foreach ((double, Weights) res in results)
                {
                    double normSum =
                        (res.Item2.ButtonW +
                        res.Item2.CatsW.Item1 +
                        res.Item2.CatsW.Item2 +
                        res.Item2.CatsW.Item3 +
                        res.Item2.TaskW[tasks.Item1 - 1] +
                        res.Item2.TaskW[tasks.Item2 - 1] +
                        res.Item2.TaskW[tasks.Item3 - 1]
                        )/7;

                    double[] temp = new double[6] {1,1,1,1,1,1};
                    temp[tasks.Item1 - 1] = res.Item2.TaskW[tasks.Item1 - 1] / normSum;
                    temp[tasks.Item2 - 1] = res.Item2.TaskW[tasks.Item2 - 1] / normSum;
                    temp[tasks.Item3 - 1] = res.Item2.TaskW[tasks.Item3 - 1] / normSum;   
                    
                    outputFile.WriteLine(
                        $"{Math.Round(res.Item1, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                        $"{Math.Round(res.Item2.ButtonW / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.CatsW.Item1 / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.CatsW.Item2 / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.CatsW.Item3 / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(temp[0], 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(temp[1], 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(temp[2], 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(temp[3], 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(temp[4], 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(temp[5], 5, MidpointRounding.AwayFromZero).ToString("0.00000")}");
                }
            }
        }


        private Weights[] RandomPopulation()
        {
            Weights[] population = new Weights[population_size];

            for (int i = 0; i < population_size / 2; i++)
            {
                var tasksConst = new double[]
                {
                    1,1,1,1,1,1
                };
                tasksConst[tasks.Item1 - 1] += SampleGaussian(0, 0.1);
                tasksConst[tasks.Item2 - 1] += SampleGaussian(0, 0.1);
                tasksConst[tasks.Item3 - 1] += SampleGaussian(0, 0.1);

                population[i] = new Weights(
                    1 + SampleGaussian(0, 0.1),

                    (1 + SampleGaussian(0, 0.1), 1 + SampleGaussian(0, 0.1), 1 + SampleGaussian(0, 0.1)),

                     tasksConst);
            }
            for (int i = 0; i < population_size / 2; i++)
            {
                var tasksConst = new double[]
                {
                    1,1,1,1,1,1
                };
                tasksConst[tasks.Item1 - 1] = random.NextDouble();
                tasksConst[tasks.Item2 - 1] = random.NextDouble();
                tasksConst[tasks.Item3 - 1] = random.NextDouble();

                population[(population_size / 2) + i] = new Weights(
                    random.NextDouble(),

                    (random.NextDouble(), random.NextDouble(), random.NextDouble()),

                    tasksConst
                    );
            }


            return population;
        }

        private Weights[] Selection(Weights[] population, double[] fitness)
        {
            Weights[] newPopulation = new Weights[population_size];

            for (int i = 0; i < population_size; i++)
            {
                int e1 = random.Next(population_size);
                int e2 = random.Next(population_size);

                if (fitness[e1] > fitness[e2])
                {
                    if (random.NextDouble() < tournament_prob)
                    {
                        newPopulation[i] = population[e1];
                    }
                    else
                    {
                        newPopulation[i] = population[e2];
                    }
                }
                else
                {
                    if (random.NextDouble() < tournament_prob)
                    {
                        newPopulation[i] = population[e2];
                    }
                    else
                    {
                        newPopulation[i] = population[e1];
                    }
                }
            }

            return newPopulation;
        }

        private Weights[] Mutation(Weights[] population)
        {
            for (int i = 0; i < population_size; i++)
            {
                Weights e = new Weights(population[i].ButtonW, population[i].CatsW, population[i].TaskW);

                if (random.NextDouble() < mutation_prob)
                {
                    // pro každou proměnnou přidat číslo z normálního rozdělelní
                    e.ButtonW += SampleGaussian(0, 0.01);

                    e.CatsW.Item1 += SampleGaussian(0, 0.01);
                    e.CatsW.Item2 += SampleGaussian(0, 0.01);
                    e.CatsW.Item3 += SampleGaussian(0, 0.01);

                    e.TaskW[tasks.Item1 - 1] += SampleGaussian(0, 0.01);
                    e.TaskW[tasks.Item2 - 1] += SampleGaussian(0, 0.01);
                    e.TaskW[tasks.Item3 - 1] += SampleGaussian(0, 0.01);
                }

                population[i] = e; 
                //Console.WriteLine($" {newPopulation[i].ButtonConst}, {newPopulation[i].CatsConst}, {newPopulation[i].TaskConst}");
            }

            return population;
        }

        private double[] EvolutionGames (Weights[] population)
        {
            double[] fitness = new double[population_size];

            Parallel.For(0, population_size, i =>
            {
                AverageGameStats gs = Game(population[i], 1000 );
                fitness[i] = gs.AvgScore;
            });
            return fitness;
        }

        private AverageGameStats Game (Weights e, int iterations)
        {
            List<GameStats> stats = new List<GameStats>(new GameStats[iterations]);

            if (mixed)
            {
                int part = iterations / 6;
                for (int j = 0; j < part; j++)
                {
                    Game g = new Game();
                    g.EvolutionGame(false, e, (tasks.Item1, tasks.Item2, tasks.Item3));
                    stats[j] = g.Stats;
                }
                for (int j = 0; j < part; j++)
                {
                    Game g = new Game();
                    g.EvolutionGame(false, e, (tasks.Item1, tasks.Item3, tasks.Item2));
                    stats[part + j] = g.Stats;
                }
                for (int j = 0; j < part; j++)
                {
                    Game g = new Game();
                    g.EvolutionGame(false, e, (tasks.Item2, tasks.Item1, tasks.Item3));
                    stats[2*part + j] = g.Stats;
                }
                for (int j = 0; j < part; j++)
                {
                    Game g = new Game();
                    g.EvolutionGame(false, e, (tasks.Item2, tasks.Item3, tasks.Item1));
                    stats[3*part + j] = g.Stats;
                }
                for (int j = 0; j < part; j++)
                {
                    Game g = new Game();
                    g.EvolutionGame(false, e, (tasks.Item3, tasks.Item1, tasks.Item2));
                    stats[4*part + j] = g.Stats;
                }
                for (int j = 0; j < part; j++)
                {
                    Game g = new Game();
                    g.EvolutionGame(false, e, (tasks.Item3, tasks.Item2, tasks.Item1));
                    stats[5*part + j] = g.Stats;
                }
            }
            else
            {
                for (int j = 0; j < iterations; j++)
                {
                    Game g = new Game();
                    g.EvolutionGame(false, e, (tasks.Item1, tasks.Item2, tasks.Item3));
                    stats[j] = g.Stats;
                }
            }

            return new AverageGameStats(0, stats.Average(item => item.Score), stats.Average(item => item.Buttons), (stats.Average(item => item.Cats.Item1), stats.Average(item => item.Cats.Item2), stats.Average(item => item.Cats.Item3)), stats.Max(item => item.Score), stats.Min(item => item.Score));
        }

        public double SampleGaussian(double mean, double stddev)
        {
            // The method requires sampling from a uniform random of (0,1]
            // but Random.NextDouble() returns a sample of [0,1).
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 * stddev + mean;
        }
    }

    public class Weights
    {
        public double ButtonW;
        public (double, double, double) CatsW;
        public double[] TaskW;

        public Weights(double b, (double, double, double) c, double[] t)
        {
            ButtonW = b;
            CatsW = c;
            TaskW = new double[] { t[0], t[1], t[2], t[3], t[4], t[5] };
        }
    }

    public class WeightsDict
    {
        private Dictionary<(int, int, int), Weights> WDict;

        public WeightsDict()
        {
            WDict = new Dictionary<(int, int, int), Weights>();

            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", "weights.csv");
            string path = @"..\..\..\weights.csv";
            //string path= "C:\\Users\\ditac\\Documents\\Mff\\bakalarka\\Calico\\Calico\\Calico\\weights.csv"

            using (var reader = new StreamReader(path))
            {
                var line = reader.ReadLine();
                var values = line.Split(';'); // columns

                while (!reader.EndOfStream)
                {

                    line = reader.ReadLine();
                    values = line.Split(';');

                    var tasks = values[0].Split(",");

                    double weightB = Convert.ToDouble(values[2]);
                    (double, double, double) weightC = (Convert.ToDouble(values[3]), Convert.ToDouble(values[4]), Convert.ToDouble(values[5]));
                    double[] weightT = new double[]
                            {
                                                
                                                Convert.ToDouble(values[6]),
                                                Convert.ToDouble(values[7]),
                                                Convert.ToDouble(values[8]),
                                                Convert.ToDouble(values[9]),
                                                Convert.ToDouble(values[10]),
                                                Convert.ToDouble(values[11]),
                            };

                    WDict[(Convert.ToInt32(tasks[0]), Convert.ToInt32(tasks[1]), Convert.ToInt32(tasks[2]))] = new Weights(weightB, weightC, weightT);
                }
            }
        }

        public Weights GetWeights((int, int, int) tasks)
        {
            return WDict[tasks];
        }
    }
}
