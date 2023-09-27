﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class Game
    {
        private Bag Bag;
        private Player Player;
        private Player Agent;
        private GamePiece[] Opts = new GamePiece[3];
        private Scoring scoring;
        //private (GamePiece, GamePiece, GamePiece) PossiblePieces;



        public Game(int mode) 
        { 
            Bag = new Bag();

            scoring = new Scoring();

            for (int i = 0; i < 3; i++)
            {
                Opts[i] = Bag.Next();
            }
            
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
                        TestGame(false);
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
            Player = new Player(scoring);

            //print empty
            PrintStateSingle();

            for (int i = 0; i < 22; i++)
            {
                MakeMove(Player);
                // update points
                PrintStateSingle();
                
            }

            PrintStats(Player);
        }

// ----------------------------------------------- MULTIPLAYER ------------------------------------------------------------
        public void MultiPlayer() 
        {
            Player = new Player(scoring);
            Agent = new RandomAgent(scoring);

            //print empty
            PrintStateMulti();

            for (int i = 0; i < 22; i++)
            {
                MakeMove(Player);
                
                PrintStateMulti();

                MakeMove(Agent);
                PrintStateMulti();

            }

            PrintStats(Player);
            PrintStats(Agent);
        }

        // ----------------------------------------------- TESTING ------------------------------------------------------------

        public void TestGame(bool withPrint)
        {
            Player = new RandomAgent(scoring);

            if (withPrint) PrintStateSingle();
            
            

            for (int i = 0; i < 22; i++)
            {
                MakeMove(Player);
                
                if (withPrint) PrintStateSingle();

            }

            PrintStats(Player);
        }

        // ----------------------------------------------- GET COMMAND ------------------------------------------------------------


        private void MakeMove(Player p)
        {
   
            int next = p.ChooseGamePiece(Opts) - 1;
            
            (int row, int col) = p.ChoosePosition();

            p.MakeMove(Opts[next], row-1, col-1);

            Opts[next] = Bag.Next();
        }



// ----------------------------------------------- PRINT ------------------------------------------------------------------

        private void PrintStateSingle()
        {
            Console.WriteLine();
            
            Console.Write(" Dílky k použití: ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($" {i + 1}: |{Opts[i].Print}| ");
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(scoring.patternScoring);

            Console.WriteLine(" Skóre: " + Player.board._scoreCounter.GetScore());
            Console.WriteLine();
            Player.board.PrintBoard();
            Console.WriteLine();
        }

        private void PrintStateMulti()
        {
            Console.WriteLine();

            Console.Write(" Dílky k použití: ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($" {i + 1}: |{Opts[i].Print}| ");
            }

            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine(scoring.patternScoring);

            Console.WriteLine(" Hráč skóre: " + Player.board._scoreCounter.GetScore());
            Player.board.PrintBoard();
            Console.WriteLine(" Agent skóre: " + Player.board._scoreCounter.GetScore());
            Agent.board.PrintBoard();


            Console.WriteLine("       1    2    3    4    5    6    7" + "        " + "      1    2    3    4    5    6    7");
            Console.WriteLine("    ------------------------------------" + "        " + "   ------------------------------------");
            for (int i = 0; i < Player.board.size; i++)
            {

                Console.Write(" " + (i + 1));
                if (i % 2 == 0) Console.Write("  ");
                Console.Write(" |");
                for (int j = 0; j < Player.board.size; j++)
                {
                    GamePiece p = Player.board.board[i][j];
                    Console.Write($"{p.Print}|");
                }
                Console.Write("        ");
                Console.Write(i + 1);
                if (i % 2 == 0) Console.Write("  ");
                Console.Write(" |");
                for (int j = 0; j < Agent.board.size; j++)
                {
                    GamePiece p = Agent.board.board[i][j];
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
            Console.WriteLine(" Finální skóre: " + p.board._scoreCounter.GetScore());
        }
    }
}
