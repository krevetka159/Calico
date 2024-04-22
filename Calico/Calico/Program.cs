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

                GameModeController game = new GameModeController(mode);

                // pro publikaci na testování lidmi
                //Game g = new Game();
                //g.SinglePlayer();

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
                        Console.WriteLine(" Invalid expression");
                    }
                }
            
            }

        }

        private static int GetGameMode()
        {
            int gameMode;
           
            while (true)
            {
                try
                {
                    Console.WriteLine(" Mode options: ");
                    Console.WriteLine("   1. Single player");
                    Console.WriteLine("   2. 2 players (vs. computer)");
                    Console.WriteLine("   3. Single agent testing");
                    Console.WriteLine("   4. All agent testing");
                    Console.WriteLine("   5. Multiplayer testing");
                    Console.WriteLine("   6. Task analysis");
                    Console.WriteLine("   7. Evolution");

                    Console.Write(" Choose game mode: ");

                    gameMode = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    

                    switch (gameMode)
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
                        case 5:
                            {
                                return 5;
                            }
                        case 6:
                            {
                                return 6;
                            }
                        case 7:
                            {
                                return 7;
                            }
                        default:
                            {
                                Console.WriteLine(" " + gameMode + " is not a mode option");
                                break;
                            }
                    }

                    
                    
                }
                catch
                {
                    Console.WriteLine(" Game mode must be an integer.");
                }
            }

        }
        static void Main(string[] args)
        {
            NewGame(); 
        }
    }
}