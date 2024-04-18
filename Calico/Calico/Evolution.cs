using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class Evolution : Game
    {
        private int population_size = 500;
        private int generations = 1000;
        private double mutation_switch = 0.2;
        private double mutation_flip = 0.25;
        private double tournament_prob = 0.8;
        private double crossover_prob = 0.7;

        private Random random;

        public Evolution() : base()
        {
            random = new Random();

            List<EvolutionGameProps> population = RandomPopulation();
            List<double> fitness = new List<double>();

            for (int g = 0; g < generations; g++)
            {
                Console.WriteLine($" Generation {g}");
                fitness = EvolutionGames(population);

                List<EvolutionGameProps> newPopulation = Selection(population, fitness);

                population = Mutation(newPopulation);
            }

            for ( int i = 0; i < 10; i++)
            {
                Console.WriteLine($" {population[i].ButtonConst}, {population[i].CatsConst}, {population[i].TaskConst} : {fitness[i]}");
            }

        }

        private List<EvolutionGameProps> RandomPopulation()
        {
            List<EvolutionGameProps> population = new List<EvolutionGameProps>();

            for (int i = 0; i < population_size; i++)
            {
                population.Add(new EvolutionGameProps(random.NextDouble(),random.NextDouble(), random.NextDouble()));
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
                EvolutionGameProps e = population[i];

                if (random.NextDouble() < mutation_switch)
                {
                    int constChange = random.Next(3);

                    switch (constChange)
                    {
                        case 0:
                            e.ButtonConst = random.NextDouble(); break;
                        case 1:
                            e.CatsConst = random.NextDouble(); break;
                        case 2:
                            e.TaskConst = random.NextDouble(); break;
                    }
                }

                newPopulation.Add(e);
            }

            return newPopulation;
        }

        private List<double> EvolutionGames (List<EvolutionGameProps> population)
        {
            List<double> fitness = new List<double>();
            foreach (EvolutionGameProps e in population)
            {
                GameStats gs = Game(e, 50);
                fitness.Add(gs.AvgScore);
            }
            return fitness;
        }

        private GameStats Game (EvolutionGameProps e, int iterations)
        {
            int sum = 0;
            int max = 0;
            int min = -1;
            int score;
            int buttons = 0;
            (int, int, int) cats = (0, 0, 0);

            for (int j = 0; j < iterations; j++)
            {


                bag = new Bag();

                scoring = new Scoring();

                gameStatePrinter = new GameStatePrinter(scoring);

                for (int i = 0; i < 3; i++)
                {
                    Opts[i] = bag.Next();
                }

                agent = new EvolutionAgent(scoring, e);

                //agent.AddTaskPieces(1,4,6); //best option

                agent.ChooseTaskPieces();



                for (int i = 0; i < 22; i++)
                {
                    MakeMove(agent);

                }

                score = agent.Board.ScoreCounter.GetScore();
                sum += score;
                if (score > max)
                {
                    max = score;
                }
                if (score < min || min == -1) min = score;

                buttons += agent.Board.ScoreCounter.GetButtonsCount();
                var catsTemp = agent.Board.ScoreCounter.GetCatsCount();
                cats.Item1 += catsTemp.Item1;
                cats.Item2 += catsTemp.Item2;
                cats.Item3 += catsTemp.Item3;
            }

            return new GameStats(0, Convert.ToDouble(sum) / Convert.ToDouble(iterations), Convert.ToDouble(buttons) / Convert.ToDouble(iterations), (Convert.ToDouble(cats.Item1) / Convert.ToDouble(iterations), Convert.ToDouble(cats.Item2) / Convert.ToDouble(iterations), Convert.ToDouble(cats.Item3) / Convert.ToDouble(iterations)), max, min);

        }
    }

    public class EvolutionGameProps
    {
        public double ButtonConst;
        public double CatsConst;
        public double TaskConst;

        public EvolutionGameProps(double b, double c, double t) 
        {
            ButtonConst = b;
            CatsConst = c;
            TaskConst = t;
        }
    }
}
