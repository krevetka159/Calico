using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class Evolution
    {
        private int population_size = 100;
        private int generations = 200;
        private double mutation_prob = 1;
        private double tournament_prob = 0.8;

        private Random random;

        public Evolution()
        {
            random = new Random();

            EvolveAllSettings();
        }

        private void EvolutionRun((int,int,int) tasks, int boardId)
        {
            EvolutionGameProps[] population = RandomPopulation();
            double[] fitness = new double[population_size];

            for (int g = 0; g < generations - 1; g++)
            {
                Console.WriteLine($" Generation {g+1}");

                fitness = EvolutionGames(population, tasks, boardId);

                //for (int i = 0; i < population_size; i++)
                //{
                //    Console.WriteLine($" {population[i].ButtonConst}, {population[i].CatsConst}, {population[i].TaskConst} : {fitness[i]}");
                //}

                population = Mutation(Selection(population, fitness));

                Console.WriteLine(fitness.Average());
                Console.WriteLine(fitness.Max());
            }

            Console.WriteLine($" Generation {generations}");

            fitness = EvolutionGames(population, tasks, boardId);
            Console.WriteLine(fitness.Average());
            Console.WriteLine(fitness.Max());

            List<(double, EvolutionGameProps)> results = new List<(double, EvolutionGameProps)>();
            for (int i = 0; i < population_size; i++)
            {
                results.Add((fitness[i], population[i]));
            }
            results.Sort((x, y) => y.Item1.CompareTo(x.Item1));

            Directory.CreateDirectory("./Evol");
            using (StreamWriter outputFile = new StreamWriter($"./Evol/test-evol_{boardId}_{tasks.Item1}{tasks.Item2}{tasks.Item3}.csv"))
            {
                outputFile.WriteLine("fitness;buttons;catsA;catsB;catsC;taskA;taskB;taskC");
                foreach ((double, EvolutionGameProps) res in results)
                    outputFile.WriteLine(
                        $"{Math.Round(res.Item1, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                        $"{Math.Round(res.Item2.ButtonConst, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.CatsConst.Item1, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.CatsConst.Item2, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.CatsConst.Item3, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.TaskConst.Item1, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.TaskConst.Item2, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.TaskConst.Item3, 5, MidpointRounding.AwayFromZero).ToString("0.00000")}");
            }
        }

        private void EvolveAllSettings()
        {
            List<Dictionary<(int, int, int), List<AverageGameStats>>> statsDict = new List<Dictionary<(int, int, int), List<AverageGameStats>>>();

            for (int b = 0; b < 4; b++)
            {
                //        statsDict.Add(new Dictionary<(int, int, int), List<AverageGameStats>>());
                //        for (int i = 1; i <= 6; i++)
                //        {
                //            for (int j = i + 1; j <= 6; j++)
                //            {
                //                for (int k = j + 1; k <= 6; k++)
                //                {
                //                    statsDict[b][(i, j, k)] = new List<AverageGameStats>();

                //                    Console.WriteLine($" {b} - {i},{j},{k}: ");
                //                    EvolutionRun((i, j, k), b);
                //                    Console.WriteLine();

                //                    Console.WriteLine($" {b} - {i},{k},{j}: ");
                //                    EvolutionRun((i, k, j), b);
                //                    Console.WriteLine();

                //                    Console.WriteLine($" {b} - {j},{i},{k}: ");
                //                    EvolutionRun((j, i, k), b);
                //                    Console.WriteLine();

                //                    Console.WriteLine($" {b} - {j},{k},{i}: ");
                //                    EvolutionRun((j, k, i), b);
                //                    Console.WriteLine();

                //                    Console.WriteLine($" {b} - {k},{i},{j}: ");
                //                    EvolutionRun((k, i, j), b);
                //                    Console.WriteLine();

                //                    Console.WriteLine($" {b} - {k},{j},{i}: ");
                //                    EvolutionRun((k, j, i), b);
                //                    Console.WriteLine();
                //                }
                //            }
                //        }

                int i = 1;
                int j = 2;
                int k = 3;

                statsDict.Add(new Dictionary<(int, int, int), List<AverageGameStats>>());

                statsDict[b][(i, j, k)] = new List<AverageGameStats>();

                Console.WriteLine($" {b} - {i},{j},{k}: ");
                EvolutionRun((i, j, k), b);
                Console.WriteLine();

                Console.WriteLine($" {b} - {i},{k},{j}: ");
                EvolutionRun((i, k, j), b);
                Console.WriteLine();

                Console.WriteLine($" {b} - {j},{i},{k}: ");
                EvolutionRun((j, i, k), b);
                Console.WriteLine();

                Console.WriteLine($" {b} - {j},{k},{i}: ");
                EvolutionRun((j, k, i), b);
                Console.WriteLine();

                Console.WriteLine($" {b} - {k},{i},{j}: ");
                EvolutionRun((k, i, j), b);
                Console.WriteLine();

                Console.WriteLine($" {b} - {k},{j},{i}: ");
                EvolutionRun((k, j, i), b);
                Console.WriteLine();
            }

            
        }


        private EvolutionGameProps[] RandomPopulation()
        {
            EvolutionGameProps[] population = new EvolutionGameProps[population_size];

            for (int i = 0; i < population_size; i++)
            {
                population[i] = new EvolutionGameProps(
                    random.NextDouble(),
                    (random.NextDouble(), random.NextDouble(), random.NextDouble()), 
                    (random.NextDouble(), random.NextDouble(), random.NextDouble())
                    );
            }

            return population;
        }

        private EvolutionGameProps[] Selection(EvolutionGameProps[] population, double[] fitness)
        {
            EvolutionGameProps[] newPopulation = new EvolutionGameProps[population_size];

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

        private EvolutionGameProps[] Mutation(EvolutionGameProps[] population)
        {
            for (int i = 0; i < population_size; i++)
            {
                //Console.WriteLine($" {population[i].ButtonConst}, {population[i].CatsConst}, {population[i].TaskConst}");
                EvolutionGameProps e = new EvolutionGameProps(population[i].ButtonConst, population[i].CatsConst, population[i].TaskConst);

                if (random.NextDouble() < mutation_prob)
                {
                    // pro každou proměnnou přidat číslo z normálního rozdělelní
                    e.ButtonConst += SampleGaussian(0, 0.01);
                    e.CatsConst.Item1 += SampleGaussian(0, 0.01);
                    e.CatsConst.Item2 += SampleGaussian(0, 0.01);
                    e.CatsConst.Item3 += SampleGaussian(0, 0.01);
                    e.TaskConst.Item1 += SampleGaussian(0, 0.01);
                    e.TaskConst.Item2 += SampleGaussian(0, 0.01);
                    e.TaskConst.Item3 += SampleGaussian(0, 0.01);
                }

                population[i] = e; 
                //Console.WriteLine($" {newPopulation[i].ButtonConst}, {newPopulation[i].CatsConst}, {newPopulation[i].TaskConst}");
            }

            return population;
        }

        private double[] EvolutionGames (EvolutionGameProps[] population, (int,int,int) tasks, int boardId)
        {
            double[] fitness = new double[population_size];

            Parallel.For(0, population_size, i =>
            {
                AverageGameStats gs = Game(population[i], 200, tasks, boardId);
                fitness[i] = gs.AvgScore;
            });
            return fitness;
        }

        private AverageGameStats Game (EvolutionGameProps e, int iterations, (int,int,int) tasks, int boardId)
        {
            List<GameStats> stats = new List<GameStats>(new GameStats[iterations]);

            for ( int j = 0; j < iterations; j++)
            {
                Game g = new Game();
                g.EvolutionGame(false, e, tasks, boardId);
                stats[j] = g.Stats;
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

    public class EvolutionGameProps
    {
        public double ButtonConst;
        public (double,double,double) CatsConst;
        public (double,double,double) TaskConst;

        public EvolutionGameProps(double b, (double,double,double) c, (double,double,double) t) 
        {
            ButtonConst = b;
            CatsConst = c;
            TaskConst = t;
        }
    }
}
