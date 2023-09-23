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


                int numOfPlayers = GetNumOfPlayers();

                // TODO verze pro 2 hráče

                Game game = new Game(1);

            }

        }

        private static int GetNumOfPlayers()
        {
            int numOfPlayers;
            while (true)
            {
                try
                {
                    Console.Write("Number of players: ");
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
                        default:
                            {
                                Console.WriteLine("1 or 2");
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