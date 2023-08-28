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

        }
        static void Main(string[] args)
        {

            NewGame();
        }
    }
}