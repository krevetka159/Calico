using System;
using System.Collections.Generic;
using System.Linq;
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

        private Random random;

        public Evolution()
        {
            random = new Random();

            EvolveAllSettings();
        }

        private void EvolutionRun()
        {
            UtilityConsts[] population = RandomPopulation();
            double[] fitness = new double[population_size];

            double[] avg = new double[generations];
            double[] max = new double[generations];

            for (int g = 0; g < generations - 1; g++)
            {
                Console.WriteLine($" Generation {g + 1}");

                fitness = EvolutionGames(population);

                //for (int i = 0; i < population_size; i++)
                //{
                //    Console.WriteLine($" {population[i].ButtonConst}, {population[i].CatsConst}, {population[i].TaskConst} : {fitness[i]}");
                //}
                //for (int i = 0; i < population_size; i++)
                //{
                //    Console.WriteLine($" {population[i].TaskConst[0]}, {population[i].TaskConst[1]}, {population[i].TaskConst[2]} : {fitness[i]}");
                //}

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

            List<(double, UtilityConsts)> results = new List<(double, UtilityConsts)>();
            for (int i = 0; i < population_size; i++)
            {
                results.Add((fitness[i], population[i]));
            }
            results.Sort((x, y) => y.Item1.CompareTo(x.Item1));

            Directory.CreateDirectory("./Evol");
            using (StreamWriter outputFile = new StreamWriter($"./Evol/finalGen_mixed.csv"))
            {
                outputFile.WriteLine("fitness;buttons;catsA;catsB;catsC;taskA;taskB;taskC");
                foreach ((double, UtilityConsts) res in results)
                {
                    double normSum =
                        (res.Item2.ButtonConst +
                        res.Item2.CatsConst.Item1 +
                        res.Item2.CatsConst.Item2 +
                        res.Item2.CatsConst.Item3 +
                        res.Item2.TaskConst.Sum())/10;
                    outputFile.WriteLine(
                        $"{Math.Round(res.Item1, 3, MidpointRounding.AwayFromZero).ToString("0.000")};" +
                        $"{Math.Round(res.Item2.ButtonConst / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.CatsConst.Item1 / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.CatsConst.Item2 / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.CatsConst.Item3 / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.TaskConst[0] / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.TaskConst[1] / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.TaskConst[2] / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.TaskConst[3] / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.TaskConst[4] / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(res.Item2.TaskConst[5] / normSum, 5, MidpointRounding.AwayFromZero).ToString("0.00000")}");
                }
            }
            using (StreamWriter outputFile = new StreamWriter($"./Evol/progress_mixed.csv"))
            {
                outputFile.WriteLine("generation;avg;max");
                for(int g = 0; g<generations;g++)
                {

                    outputFile.WriteLine(
                        $"{g};" +
                        $"{Math.Round(avg[g], 5, MidpointRounding.AwayFromZero).ToString("0.00000")};" +
                        $"{Math.Round(max[g], 3, MidpointRounding.AwayFromZero).ToString("0.00000")}");

                }
            }
        }

        private void EvolveAllSettings()
        {
            //Dictionary<(int, int, int), List<AverageGameStats>> statsDict = new Dictionary<(int, int, int), List<AverageGameStats>>();

            //int i = 1;
            //int j = 2;
            //int k = 3;


            //statsDict[(i, j, k)] = new List<AverageGameStats>();

            //Console.WriteLine($" {i},{j},{k}: ");
            //EvolutionRun((i, j, k));
            //Console.WriteLine();

            //Console.WriteLine($" {i},{k},{j}: ");
            //EvolutionRun((i, k, j));
            //Console.WriteLine();

            //Console.WriteLine($" {j},{i},{k}: ");
            //EvolutionRun((j, i, k));
            //Console.WriteLine();

            //Console.WriteLine($" {j},{k},{i}: ");
            //EvolutionRun((j, k, i));
            //Console.WriteLine();

            //Console.WriteLine($"´{k},{i},{j}: ");
            //EvolutionRun((k, i, j));
            //Console.WriteLine();

            //Console.WriteLine($" {k},{j},{i}: ");
            //EvolutionRun((k, j, i));
            //Console.WriteLine();

            EvolutionRun();

        }


        private UtilityConsts[] RandomPopulation()
        {
            UtilityConsts[] population = new UtilityConsts[population_size];

            for (int i = 0; i < population_size/2; i++)
            {
                population[i] = new UtilityConsts(
                    1 + SampleGaussian(0,0.1),
                    (1 + SampleGaussian(0, 0.1), 1 + SampleGaussian(0, 0.1), 1 + SampleGaussian(0, 0.1)), 
                    new double[] { 1 + SampleGaussian(0, 0.1), 1 + SampleGaussian(0, 0.1), 1 + SampleGaussian(0, 0.1), 1 + SampleGaussian(0, 0.1), 1 + SampleGaussian(0, 0.1), 1 + SampleGaussian(0, 0.1) }
                    );
            }
            for (int i = 0; i < population_size / 2; i++)
            {
                population[(population_size/2) + i] = new UtilityConsts(
                    random.NextDouble(),
                    (random.NextDouble(), random.NextDouble(), random.NextDouble()),
                    new double[] { random.NextDouble(), random.NextDouble(), random.NextDouble(), random.NextDouble(), random.NextDouble(), random.NextDouble() }
                    );
            }

            return population;
        }

        private UtilityConsts[] Selection(UtilityConsts[] population, double[] fitness)
        {
            UtilityConsts[] newPopulation = new UtilityConsts[population_size];

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

        private UtilityConsts[] Mutation(UtilityConsts[] population)
        {
            for (int i = 0; i < population_size; i++)
            {
                //Console.WriteLine($" {population[i].ButtonConst}, {population[i].CatsConst}, {population[i].TaskConst}");
                UtilityConsts e = new UtilityConsts(population[i].ButtonConst, population[i].CatsConst, population[i].TaskConst);

                if (random.NextDouble() < mutation_prob)
                {
                    // pro každou proměnnou přidat číslo z normálního rozdělelní
                    e.ButtonConst += SampleGaussian(0, 0.01);
                    e.CatsConst.Item1 += SampleGaussian(0, 0.01);
                    e.CatsConst.Item2 += SampleGaussian(0, 0.01);
                    e.CatsConst.Item3 += SampleGaussian(0, 0.01);
                    e.TaskConst[0] += SampleGaussian(0, 0.01);
                    e.TaskConst[1] += SampleGaussian(0, 0.01);
                    e.TaskConst[2] += SampleGaussian(0, 0.01);
                    e.TaskConst[3] += SampleGaussian(0, 0.01);
                    e.TaskConst[4] += SampleGaussian(0, 0.01);
                    e.TaskConst[5] += SampleGaussian(0, 0.01);
                }

                population[i] = e; 
                //Console.WriteLine($" {newPopulation[i].ButtonConst}, {newPopulation[i].CatsConst}, {newPopulation[i].TaskConst}");
            }

            return population;
        }

        private double[] EvolutionGames (UtilityConsts[] population)
        {
            double[] fitness = new double[population_size];

            Parallel.For(0, population_size, i =>
            {
                AverageGameStats gs = Game(population[i], 1000 );
                fitness[i] = gs.AvgScore;
            });
            return fitness;
        }

        private AverageGameStats Game (UtilityConsts e, int iterations)
        {
            List<GameStats> stats = new List<GameStats>(new GameStats[iterations]);

            for ( int j = 0; j < iterations; j++)
            {
                Game g = new Game();
                g.EvolutionGame(false, e);
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

    //public class UtilityConsts
    //{
    //    public double ButtonConst;
    //    public (double, double, double) CatsConst;
    //    public (double, double, double) TaskConst;

    //    public UtilityConsts(double b, (double, double, double) c, (double, double, double) t)
    //    {
    //        ButtonConst = b;
    //        CatsConst = c;
    //        TaskConst = t;
    //    }
    //}

    public class UtilityConsts
    {
        public double ButtonConst;
        public (double, double, double) CatsConst;
        public double[] TaskConst;

        public UtilityConsts(double b, (double, double, double) c, double[] t)
        {
            ButtonConst = b;
            CatsConst = c;
            TaskConst = new double[] { t[0], t[1], t[2], t[3], t[4], t[5] };
        }
    }
}
