using Calico;
using System;
using System.Runtime.CompilerServices;

namespace Calico
{
    internal class Program
    {

        private static void NewGame()
        {
            while (true) {

                Console.WriteLine("CALICO");
                Console.WriteLine("");


                int mode = GetGameMode();

                Game game = new Game(mode);
            }

        }

        private static int GetGameMode()
        {
            int numOfPlayers;
            while (true)
            {
                try
                {
                    Console.WriteLine("Mode options: ");
                    Console.WriteLine("   1. Single player");
                    Console.WriteLine("   2. 2 players (vs. computer)");
                    Console.WriteLine("   3. Agent testing (with state print");
                    Console.WriteLine("   4. Agent testing (without state print");

                    Console.Write("Choose game mode: ");

                    numOfPlayers = Convert.ToInt32(Console.ReadLine());
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
                                Console.WriteLine( numOfPlayers + " is not a mode option");
                                break;
                            }
                    }
                }
                catch
                {
                    Console.WriteLine("Number of players must be an integer.");
                }
            }

        }
        static void Main(string[] args)
        {
            NewGame(); 
        }
    }
}