using System;
using System.Runtime.CompilerServices;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        private static void NewGame()
        {
            Console.WriteLine("CALICO");
            Console.WriteLine("");
            Console.Write("Počet hráčů: ");

            string numOfPlayers = Console.ReadLine();

            if (numOfPlayers == "1") { Console.WriteLine("ANO"); }
            else { Console.WriteLine("NE"); }


            while (true) {
                string a = Console.ReadLine();
                Console.WriteLine(" 1a | 2b | 3c |");
                Console.WriteLine("| 1a | 2a | 3a");
                Console.WriteLine(" 5s | 4a | 3d |");
            }

        }
        static void Main(string[] args)
        {

            NewGame();
        }
    }
}