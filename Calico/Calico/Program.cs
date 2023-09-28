using Calico;
using System;
using System.Runtime.CompilerServices;

namespace Calico
{
    internal class Program
    {

        private static void NewGame()
        {
            bool inGame = true;
            string newGame;
            while (inGame) {

                Console.WriteLine();
                Console.WriteLine(" CALICO ");
                Console.WriteLine();


                int mode = GetGameMode();

                Game game = new Game(mode);

                while (true)
                {
                    Console.WriteLine();
                    Console.Write(" Start a new game (y/n): ");
                    newGame = Console.ReadLine();

                    if (newGame == "n")
                    {
                        inGame = false;
                        break;
                    }
                    else if (newGame == "y") { break; }
                    else
                    {
                        Console.WriteLine(" Neplatný výraz");
                    }
                }
            
            }

        }

        private static int GetGameMode()
        {
            int numOfPlayers;
           
            while (true)
            {
                try
                {
                    Console.WriteLine(" Mode options: ");
                    Console.WriteLine("   1. Single player");
                    Console.WriteLine("   2. 2 players (vs. computer)");
                    Console.WriteLine("   3. Single agent testing");
                    Console.WriteLine("   4. All agent testing");

                    Console.Write(" Choose game mode: ");

                    numOfPlayers = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    

                    switch (numOfPlayers)
                    {
                        case 1:
                            {
                                return 1;
                            }
                        case 2:
                            {
                                return 2;
                            }
                        case 3:
                            {
                                return 3;
                            }
                        case 4:
                            {
                                return 4;
                            }

                        default:
                            {
                                Console.WriteLine(" " + numOfPlayers + " is not a mode option");
                                break;
                            }
                    }

                    
                    
                }
                catch
                {
                    Console.WriteLine(" Number of players must be an integer.");
                }
            }

        }
        static void Main(string[] args)
        {
            NewGame(); 
        }
    }
}