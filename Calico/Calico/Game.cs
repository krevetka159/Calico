using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Calico.AgentComplet;

namespace Calico
{
    internal class Game
    {
        private Bag Bag;
        private Player Player;
        private Player Agent;
        private GamePiece[] Opts = new GamePiece[3];
        private Scoring scoring;


        public Game(int mode) 
        { 
            
            switch (mode)
            {
                case 1:
                    {
                        SinglePlayer();
                        break;
                    }
                case 2:
                    {
                        MultiPlayer();
                        break;
                    }
                case 3:
                    {
                        Testing();
                        break;
                    }
                case 4:
                    {
                        TestAll();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            
        }

// ----------------------------------------------- SINGLEPLAYER ------------------------------------------------------------
        public void SinglePlayer()
        {
            Bag = new Bag();

            scoring = new Scoring();

            for (int i = 0; i < 3; i++)
            {
                Opts[i] = Bag.Next();
            }
            Player = new Player(scoring);

            //print empty
            PrintStateSingle(Player);

            for (int i = 0; i < 22; i++)
            {
                MakeMove(Player);
                // update points
                PrintStateSingle(Player);
                
            }

            PrintStats(Player);
        }

// ----------------------------------------------- MULTIPLAYER ------------------------------------------------------------
        public void MultiPlayer() 
        {
            Bag = new Bag();

            scoring = new Scoring();

            for (int i = 0; i < 3; i++)
            {
                Opts[i] = Bag.Next();
            }

            Player = new Player(scoring);
            Agent = new AgentComplet(scoring);

            //print empty
            PrintStateMulti();

            for (int i = 0; i < 22; i++)
            {
                MakeMove(Player);
                
                PrintStateMulti();

                MakeTestMove(Agent);
                PrintStateMulti();

            }

            PrintStats(Player);
            PrintStats(Agent);
        }

        // ----------------------------------------------- TESTING ------------------------------------------------------------

        public void Testing()
        {
            int agentOption;
            //bool choosingAgent = true;
            while (true)
            {
                try
                {
                    Console.WriteLine(" Agent options: ");
                    Console.WriteLine("    1. Random");
                    Console.WriteLine("    2. Easy color half-random agent");
                    Console.WriteLine("    3. Easy pattern half-random agent");
                    Console.WriteLine("    4. Easy half-random agent");
                    Console.WriteLine("    5. Counting color scores");
                    Console.WriteLine("    6. Counting pattern scores");
                    Console.WriteLine("    7. Counting scores");
                    Console.WriteLine("    8. Random position, count scores");
                    Console.WriteLine("    9. 7 but with probability to do sth random");
                    Console.Write(" Pick agent: ");

                    agentOption = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();

                    if (1<= agentOption && agentOption <= 9)
                    {
                        
                        while (true)
                        {
                            
                            Console.Write(" Print progress (y/n): ");
                            string newGame = Console.ReadLine();

                            if (newGame == "n")
                            {
                                TestGame(false, true, agentOption, 50);
                                break;
                            }
                            else if (newGame == "y") 
                            {
                                TestGame(true, true, agentOption, 1);
                                break; 
                            }
                            else
                            {
                                Console.WriteLine(" Invalid expression");
                            }
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine(" " + agentOption + " is not a mode option");
                    }

                }
                catch
                {
                    Console.WriteLine(" Must be an integer.");
                }
            }
        }
        public void TestGame(bool withPrint, bool allResults, int agentType, int iterations)
        {
            int sum = 0;

            for (int j = 0; j < iterations; j++)
            {


                Bag = new Bag();

                scoring = new Scoring();

                for (int i = 0; i < 3; i++)
                {
                    Opts[i] = Bag.Next();
                }
                switch (agentType)
                {
                    case 1:
                        {
                            Agent = new Agent(scoring);
                            break;
                        }
                    case 2:
                        {
                            Agent = new RandomAgentColor(scoring);
                            break;
                        }
                    case 3:
                        {
                            Agent = new RandomAgentPattern(scoring);
                            break;
                        }
                    case 4:
                        {
                            Agent = new RandomAgentComplet(scoring);
                            break;
                        }
                    case 5:
                        {
                            Agent = new AgentColor(scoring);
                            break;
                        }
                    case 6:
                        {
                            Agent = new AgentPattern(scoring);
                            break;
                        }
                    case 7:
                        {
                            Agent = new AgentComplet(scoring);
                            break;
                        }
                    case 8:
                        {
                            Agent = new RandomPositionAgent(scoring);
                            break;
                        }
                    case 9:
                        {
                            Agent = new AgentCompletWithProb(scoring);
                            break;
                        }
                }
                
                if (withPrint) PrintStateSingle(Agent);



                for (int i = 0; i < 22; i++)
                {
                    MakeTestMove(Agent);

                    if (withPrint) PrintStateSingle(Agent);

                }

                if(allResults) PrintStats(Agent);
                sum += Agent.Board.ScoreCounter.GetScore();
            }
            Console.WriteLine();
            if (iterations > 1) Console.WriteLine(" Mean: " + (sum / iterations));
        }

        private void TestAll()
        {
            Console.WriteLine(" Agents: ");
            Console.WriteLine("    1. Random");
            Console.WriteLine("    2. Easy color half-random agent");
            Console.WriteLine("    3. Easy pattern half-random agent");
            Console.WriteLine("    4. Easy half-random agent");
            Console.WriteLine("    5. Counting color scores");
            Console.WriteLine("    6. Counting pattern scores");
            Console.WriteLine("    7. Counting scores");
            Console.WriteLine("    8. Random position, count scores");
            Console.WriteLine("    9. 7 but with probability to do sth random");

            for (int i = 1;i <= 9;i++)
            {
                Console.WriteLine(" " + i + ": ");
                TestGame(false, false,i, 100);
                Console.WriteLine();
            }

        }

// ----------------------------------------------- GET COMMAND ------------------------------------------------------------


        private void MakeMove(Player p)
        {
   
            int next = p.ChooseGamePiece(Opts) - 1;
            
            (int row, int col) = p.ChoosePosition();

            p.MakeMove(Opts[next], row, col);

            Opts[next] = Bag.Next();
        }

        private void MakeTestMove(Player p)
        {

            (int next, (int row, int col)) = p.ChooseNextMove(Opts);

            p.MakeMove(Opts[next], row, col);

            Opts[next] = Bag.Next();
        }



        // ----------------------------------------------- PRINT ------------------------------------------------------------------

        private void PrintStateSingle(Player p)
        {
            Console.WriteLine();
            
            Console.Write(" Patch tiles to use: ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($" {i + 1}: |{Opts[i].Print}| ");
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(scoring.patternScoring);

            Console.WriteLine(" Score: " + p.Board.ScoreCounter.GetScore());
            Console.WriteLine();
            p.Board.PrintBoard();
            Console.WriteLine();
        }

        private void PrintStateMulti()
        {
            Console.WriteLine();

            Console.Write(" Patch tiles to use: ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($" {i + 1}: |{Opts[i].Print}| ");
            }

            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine(scoring.patternScoring);

            Console.WriteLine(" Player score: " + Player.Board.ScoreCounter.GetScore());
            
            Console.WriteLine(" Agent score: " + Agent.Board.ScoreCounter.GetScore());

            Console.WriteLine();
            Console.WriteLine("                   Player                                        Agent");

            Console.WriteLine("       1    2    3    4    5    6    7" + "        " + "      1    2    3    4    5    6    7");
            Console.WriteLine("    ------------------------------------" + "        " + "   ------------------------------------");
            for (int i = 0; i < Player.Board.Size; i++)
            {

                Console.Write(" " + (i + 1));
                if (i % 2 == 0) Console.Write("  ");
                Console.Write(" |");
                for (int j = 0; j < Player.Board.Size; j++)
                {
                    GamePiece p = Player.Board.board[i][j];
                    Console.Write($"{p.Print}|");
                }
                Console.Write("        ");
                Console.Write(i + 1);
                if (i % 2 == 0) Console.Write("  ");
                Console.Write(" |");
                for (int j = 0; j < Agent.Board.Size; j++)
                {
                    GamePiece p = Agent.Board.board[i][j];
                    Console.Write($"{p.Print}|");
                }
                Console.Write("\n");
                Console.WriteLine("    ------------------------------------" + "        " + "   ------------------------------------");
            }

            
            Console.WriteLine();
        }



        private void PrintStats(Player p)
        {
            // finální výsledky
            Console.WriteLine(" Final score: " + p.Board.ScoreCounter.GetScore());
        }
    }
}
