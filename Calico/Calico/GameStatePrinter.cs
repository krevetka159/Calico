using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class GameStatePrinter
    {
        private Scoring scoring;
        public GameStatePrinter(Scoring scoring) 
        { 
            this.scoring = scoring;
        }

        /// <summary>
        /// Print game state for a single player game
        /// </summary>
        /// <param name="p"></param>
        /// <param name="Opts"></param>
        public void PrintStateSingle(Player p, GamePiece[] Opts)
        {
            Console.WriteLine(" --------------------------------------------------------------------------------------------- ");
            Console.WriteLine();

            foreach (TaskPiece t in p.Board.TaskPieces.Values)
            {
                Console.WriteLine($"{t.Print}: {t.Description}");
            }
            //Console.WriteLine(scoring.PatternScoringToString);
            Console.WriteLine(PrintPartialScoreStats(p));

            Console.WriteLine(" Score: " + p.Board.ScoreCounter.GetScore());
            Console.WriteLine();

            Console.Write(" Patch tiles to use: ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($" {i + 1}: |{Opts[i].Print}| ");
            }

            Console.WriteLine();
            Console.WriteLine();

            PrintBoard(p);
            Console.WriteLine();
        }

        /// <summary>
        /// Print game state for a single player game
        /// </summary>
        /// <param name="p"></param>
        /// <param name="Opts"></param>
        public void PrintTaskChoosing(Player p, GamePiece[] Opts)
        {
            Console.WriteLine();

            Console.WriteLine(" Task piece options: ");
 
            Console.WriteLine("  1: all different, 10/15");
            Console.WriteLine("  2: 4:2, 7/14");
            Console.WriteLine("  3: 3:3, 7/13");
            Console.WriteLine("  4: 3:2:1, 7/11");
            Console.WriteLine("  5: 2:2:2, 7/11");
            Console.WriteLine("  6: 2:2:1:1, 5/7");

            Console.WriteLine();
            PrintBoard(p);
            Console.WriteLine();
        }

        /// <summary>
        /// Print game board
        /// </summary>
        /// <param name="p"></param>
        private void PrintBoard(Player p)
        {
            Console.WriteLine("       1    2    3    4    5    6    7");
            Console.WriteLine("    ------------------------------------");
            for (int i = 0; i < p.Board.Size; i++)
            {

                Console.Write(" " + (i + 1));
                if (i % 2 == 1) Console.Write("  ");
                Console.Write(" |");
                for (int j = 0; j < p.Board.Size; j++)
                {
                    GamePiece gp = p.Board.board[i][j];
                    Console.Write($"{gp.Print}|");
                }
                Console.Write("\n");
                Console.WriteLine("    ------------------------------------");
            }
        }

       

        /// <summary>
        /// Print final score
        /// </summary>
        /// <param name="p"></param>
        public void PrintStats(Player p)
        {
            // finální výsledky
            Console.WriteLine(" Final score: " + p.Board.ScoreCounter.GetScore());
        }
        public void PrintDetailedStatsToCSV(Player p)
        {
            (int, int, int) cats = p.Board.ScoreCounter.GetCatsCount();
            int[] tasks = p.Board.TasksCompleted();
            Console.WriteLine(
                $"{p.Board.ScoreCounter.GetScore()};" +
                $"{p.Board.ScoreCounter.GetButtonsCount()};" +
                $"{cats.Item1};" +
                $"{cats.Item2};" +
                $"{cats.Item3};" +
                $"{tasks[0]};{tasks[1]};{tasks[2]};{tasks[3]};{tasks[4]};{tasks[5]}"
                );

        }

        public string PrintPartialScoreStats(Player p)
        {
            StringBuilder partialScore = new StringBuilder();
            foreach (Color c in Enum.GetValues(typeof(Color)))
            {
                if (c != Color.None)
                {
                    partialScore.Append($" {(int)c} : {p.Board.ScoreCounter.butts[(int)c-1]} | ");
                }
            }
            partialScore.Append($" RB : {p.Board.ScoreCounter.rainbowButtons}");
            partialScore.Append('\n');

            foreach (PatternScoringPanel panel in p.Board.ScoreCounter.Scoring.PatternScoring.ps)
            {
                partialScore.Append($" {(char)(64 + (int)panel.Patterns.Item1)}, {(char)(64 + (int)panel.Patterns.Item2)} : {p.Board.ScoreCounter.catts[panel.Id]} | ");
            }
            partialScore.Append('\n');

            return partialScore.ToString();
        }


    }

}
