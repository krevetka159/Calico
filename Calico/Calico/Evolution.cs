using Calico.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    /// <summary>
    /// Evolutionary algorithm for finding optimal weights for the agent
    /// </summary>
    internal class Evolution
    {
        private int population_size = 200;
        private int generations = 200;
        private double mutation_prob = 1;
        private double tournament_prob = 0.8;


        private (int, int, int) tasks;
        private bool mixed;
        private int iterations;

        private string outputFileName;

        private Random random;

        public Evolution((int,int,int) tasks, bool mixed, string outputFileName)
        {
            random = new Random();

            this.tasks = tasks;
            this.mixed = mixed;

            if (mixed)
            {
                iterations = 1200;
            }
            else
            {
                iterations = 1000;
            }

            this.outputFileName = outputFileName;

            EvolutionRun();
        }

        /// <summary>
        /// Run of the evolutionary algorithm
        /// </summary>
        private void EvolutionRun()
        {
            Weights[] population = InitPopulation();
            double[] fitness = new double[population_size];

            double[] avg = new double[generations];
            double[] max = new double[generations];

            Console.WriteLine($"{tasks.Item1}{tasks.Item2}{tasks.Item3}");

            for (int g = 0; g < generations - 1; g++)
            {
                Console.WriteLine($" Generation {g + 1}");

                fitness = PopulationFitness(population);

                population = Mutation(Selection(population, fitness));

                Console.WriteLine(fitness.Average());
                avg[g] = fitness.Average();
                Console.WriteLine(fitness.Max());
                max[g] = fitness.Max();
            }

            Console.WriteLine($" Generation {generations}");

            fitness = PopulationFitness(population);
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

        /// <summary>
        /// Inicialization of the population
        /// </summary>
        /// <returns></returns>
        private Weights[] InitPopulation()
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

        /// <summary>
        /// Selection of the best individuals
        /// </summary>
        /// <param name="population"></param>
        /// <param name="fitness"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Mutation of the population
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        private Weights[] Mutation(Weights[] population)
        {
            for (int i = 0; i < population_size; i++)
            {
                Weights e = new Weights(population[i].ButtonW, population[i].CatsW, population[i].TaskW);

                if (random.NextDouble() < mutation_prob)
                {
                    e.ButtonW += SampleGaussian(0, 0.01);

                    e.CatsW.Item1 += SampleGaussian(0, 0.01);
                    e.CatsW.Item2 += SampleGaussian(0, 0.01);
                    e.CatsW.Item3 += SampleGaussian(0, 0.01);

                    e.TaskW[tasks.Item1 - 1] += SampleGaussian(0, 0.01);
                    e.TaskW[tasks.Item2 - 1] += SampleGaussian(0, 0.01);
                    e.TaskW[tasks.Item3 - 1] += SampleGaussian(0, 0.01);
                }

                population[i] = e; 
            }

            return population;
        }

        /// <summary>
        /// Fitness evaluation for the whole population
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        private double[] PopulationFitness (Weights[] population)
        {
            double[] fitness = new double[population_size];

            Parallel.For(0, population_size, i =>
            {
                fitness[i] = IndividualFitness(population[i]);
            });
            return fitness;
        }

        /// <summary>
        /// Fitness evaluation for individual
        /// </summary>
        /// <param name="weights"></param>
        /// <returns></returns>
        private double IndividualFitness (Weights weights)
        {
            double[] stats = new double[iterations];

            if (mixed)
            {
                int part = iterations / 6;
                for (int j = 0; j < part; j++)
                {
                    EvolutionGame g = new EvolutionGame(weights,tasks);
                    stats[j] = g.Game();
                }
                for (int j = 0; j < part; j++)
                {
                    EvolutionGame g = new EvolutionGame(weights, tasks);
                    stats[part + j] = g.Game();
                }
                for (int j = 0; j < part; j++)
                {
                    EvolutionGame g = new EvolutionGame(weights, tasks);
                    stats[2*part + j] = g.Game();
                }
                for (int j = 0; j < part; j++)
                {
                    EvolutionGame g = new EvolutionGame(weights, tasks);
                    stats[3*part + j] = g.Game();
                }
                for (int j = 0; j < part; j++)
                {
                    EvolutionGame g = new EvolutionGame(weights, tasks);
                    stats[4*part + j] = g.Game();
                }
                for (int j = 0; j < part; j++)
                {
                    EvolutionGame g = new EvolutionGame(weights, tasks);
                    stats[5*part + j] = g.Game();
                }
            }
            else
            {
                for (int j = 0; j < iterations; j++)
                {
                    EvolutionGame g = new EvolutionGame(weights, tasks);
                    stats[j] = g.Game();
                }
            }

            return stats.Average();
        }

        /// <summary>
        /// Sample from Gaussian distribution
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="stddev"></param>
        /// <returns></returns>
        public double SampleGaussian(double mean, double stddev)
        {
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 * stddev + mean;
        }
    }

    /// <summary>
    /// Game for evolutionary algorithm
    /// </summary>
    public class EvolutionGame : Game
    {
        public EvolutionGame(Weights weights, (int, int, int) tasks) : base()
        {

            agent = new AgentWeightedAdvanced(scoring, weights);
            agent.AddTaskPieces(tasks.Item1, tasks.Item2, tasks.Item3);
        }
        public double Game()
        {
            for (int i = 0; i < agent.Board.EmptySpotsCount; i++)
            {
                MakeMove(agent);
            }

            return agent.Board.ScoreCounter.Score;
        }
    }
}
