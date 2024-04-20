using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class Evolution
    {
        private int population_size = 500;
        private int generations = 100;
        private double mutation_prob = 1;
        private double tournament_prob = 0.8;

        private Random random;

        public Evolution()
        {
            random = new Random();

            List<EvolutionGameProps> population = RandomPopulation();
            List<double> fitness = new List<double>();

            for (int g = 0; g < generations; g++)
            {
                Console.WriteLine($" Generation {g}");

                fitness = EvolutionGames(population);

                //for (int i = 0; i < population_size; i++)
                //{
                //    Console.WriteLine($" {population[i].ButtonConst}, {population[i].CatsConst}, {population[i].TaskConst} : {fitness[i]}");
                //}

                population = new List<EvolutionGameProps>(Mutation(Selection(population, fitness)));

                Console.WriteLine(fitness.Average());
                Console.WriteLine(fitness.Max());
            }

            for (int i = 0; i < population_size; i++)
            {
                Console.WriteLine($" {population[i].ButtonConst}, {population[i].CatsConst}, {population[i].TaskConst} : {fitness[i]}");
            }

        }

        private List<EvolutionGameProps> RandomPopulation()
        {
            List<EvolutionGameProps> population = new List<EvolutionGameProps>();

            for (int i = 0; i < population_size; i++)
            {
                population.Add(new EvolutionGameProps(
                    random.NextDouble(),
                    (random.NextDouble(), random.NextDouble(), random.NextDouble()), 
                    (random.NextDouble(), random.NextDouble(), random.NextDouble())
                    ));
            }

            return population;
        }

        private List<EvolutionGameProps> Selection(List<EvolutionGameProps> population, List<double> fitness)
        {
            List<EvolutionGameProps> newPopulation = new List<EvolutionGameProps>();

            for (int i = 0; i < population_size; i++)
            {
                int e1 = random.Next(population_size);
                int e2 = random.Next(population_size);

                if (fitness[e1] > fitness[e2])
                {
                    if (random.NextDouble() < tournament_prob)
                    {
                        newPopulation.Add(population[e1]);
                    }
                    else
                    {
                        newPopulation.Add(population[e2]);
                    }
                }
                else
                {
                    if (random.NextDouble() < tournament_prob)
                    {
                        newPopulation.Add(population[e2]);
                    }
                    else
                    {
                        newPopulation.Add(population[e1]);
                    }
                }
            }

            return newPopulation;
        }

        private List<EvolutionGameProps> Mutation(List<EvolutionGameProps> population)
        {
            List<EvolutionGameProps> newPopulation = new List<EvolutionGameProps>();


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

                newPopulation.Add(e); 
                //Console.WriteLine($" {newPopulation[i].ButtonConst}, {newPopulation[i].CatsConst}, {newPopulation[i].TaskConst}");
            }

            return newPopulation;
        }

        private List<double> EvolutionGames (List<EvolutionGameProps> population)
        {
            List<double> fitness = new List<double>();
            for (int i = 0; i < population_size; i++)
            {
                fitness.Add(0);
            }
            Parallel.For(0, population_size, i =>
            {
                AverageGameStats gs = Game(population[i], 500);
                fitness[i] = gs.AvgScore;
            });
            return fitness;
        }

        private AverageGameStats Game (EvolutionGameProps e, int iterations)
        {
            List<GameStats> stats = new List<GameStats>();
            for (int i = 0; i < iterations; i++)
            {
                stats.Add(null);
            }

            for (int j = 0; j < iterations; j++)
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
