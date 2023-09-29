using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class GameStatePrinter
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
            Console.WriteLine();

            Console.Write(" Patch tiles to use: ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($" {i + 1}: |{Opts[i].Print}| ");
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(scoring.PatternScoring);

            Console.WriteLine(" Score: " + p.Board.ScoreCounter.GetScore());
            Console.WriteLine();
            PrintBoard(p);
            Console.WriteLine();
        }

        /// <summary>
        /// Print game state for a game against an agent
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <param name="Opts"></param>
        public void PrintStateMulti(Player p, Agent a, GamePiece[] Opts)
        {
            Console.WriteLine();

            Console.Write(" Patch tiles to use: ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($" {i + 1}: |{Opts[i].Print}| ");
            }

            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine(scoring.PatternScoring);

            Console.WriteLine(" Player score: " + p.Board.ScoreCounter.GetScore());

            Console.WriteLine(" Agent score: " + a.Board.ScoreCounter.GetScore());

            Console.WriteLine();

            PrintBoards(p, a);

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
                if (i % 2 == 0) Console.Write("  ");
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
        /// Print 2 boards next to each other
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        private void PrintBoards(Player p, Agent a)
        {
            Console.WriteLine("                  Player                                          Agent");

            Console.WriteLine("       1    2    3    4    5    6    7" + "          " + "      1    2    3    4    5    6    7");
            Console.WriteLine("    ------------------------------------" + "          " + "   ------------------------------------");
            for (int i = 0; i < p.Board.Size; i++)
            {

                Console.Write(" " + (i + 1));
                if (i % 2 == 0) Console.Write("  ");
                Console.Write(" |");
                for (int j = 0; j < p.Board.Size; j++)
                {
                    GamePiece gp = p.Board.board[i][j];
                    Console.Write($"{gp.Print}|");
                }
                Console.Write("       ");
                if (i % 2 == 1) Console.Write("  ");
                Console.Write(i + 1);
                if (i % 2 == 0) Console.Write("  ");
                Console.Write(" |");
                for (int j = 0; j < a.Board.Size; j++)
                {
                    GamePiece gp = a.Board.board[i][j];
                    Console.Write($"{gp.Print}|");
                }
                Console.Write("\n");
                Console.WriteLine("    ------------------------------------" + "          " + "   ------------------------------------");
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


    }

}
