using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class GameBoard
    {
        private List<List<GamePiece>> board;
        private Random random;
        private static GamePiece empty = new GamePiece(Type.Empty);
        private static GamePiece blocked = new GamePiece(Type.Blocked);
        private int size = 7;


        public GameBoard()
        {
            random = new Random();

            int randId = random.Next(0, 4);
            // generování okrajů boardu podle id - 4 možnosti

            switch (randId)
            {
                case 0:
                    board = new List<List<GamePiece>>() { 
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Vines), new GamePiece(Color.Yellow, Pattern.Stripes), new GamePiece(Color.Cyan, Pattern.Vines), new GamePiece(Color.Pink, Pattern.Fern), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Yellow, Pattern.Flowers), new GamePiece(Color.Green, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Flowers), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Quatrefoil), empty, empty, blocked, empty, empty, new GamePiece(Color.Purple, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Fern), empty, empty, empty, blocked, empty, new GamePiece(Color.Yellow, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Stripes), empty, blocked, empty, empty, empty, new GamePiece(Color.Green, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Flowers), new GamePiece(Color.Cyan, Pattern.Quatrefoil), new GamePiece(Color.Pink, Pattern.Flowers), new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Green, Pattern.Vines), new GamePiece(Color.Pink, Pattern.Dots), new GamePiece(Color.Cyan, Pattern.Stripes) }
                    };
                    break;
                case 1:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Vines), new GamePiece(Color.Green, Pattern.Fern), new GamePiece(Color.Blue, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Yellow, Pattern.Dots), new GamePiece(Color.Cyan, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Quatrefoil), empty, empty, blocked, empty, empty, new GamePiece(Color.Purple, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Stripes), empty, empty, empty, blocked, empty, new GamePiece(Color.Yellow, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Flowers), empty, blocked, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Vines), empty, empty, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Stripes), new GamePiece(Color.Blue, Pattern.Dots), new GamePiece(Color.Green, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Stripes), new GamePiece(Color.Cyan, Pattern.Fern), new GamePiece(Color.Blue, Pattern.Flowers), new GamePiece(Color.Green, Pattern.Vines) }
                    };
                    break;
                case 2:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Flowers), new GamePiece(Color.Purple, Pattern.Dots), new GamePiece(Color.Pink, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Fern), new GamePiece(Color.Cyan, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Quatrefoil), new GamePiece(Color.Green, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Quatrefoil), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Stripes), empty, empty, blocked, empty, empty, new GamePiece(Color.Cyan, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Fern), empty, empty, empty, blocked, empty, new GamePiece(Color.Pink, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Vines), empty, blocked, empty, empty, empty, new GamePiece(Color.Green, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Dots), empty, empty, empty, empty, empty, new GamePiece(Color.Blue, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Stripes), new GamePiece(Color.Blue, Pattern.Fern), new GamePiece(Color.Green, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Quatrefoil), new GamePiece(Color.Purple, Pattern.Dots) }
                    };
                    break;
                case 3:
                    board = new List<List<GamePiece>>() {
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Quatrefoil), new GamePiece(Color.Blue, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Dots), new GamePiece(Color.Blue, Pattern.Vines), new GamePiece(Color.Green, Pattern.Fern), new GamePiece(Color.Pink, Pattern.Quatrefoil) },
                        new List<GamePiece>() { new GamePiece(Color.Pink, Pattern.Fern), empty, empty, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Flowers) },
                        new List<GamePiece>() { new GamePiece(Color.Purple, Pattern.Vines), empty, empty, blocked, empty, empty, new GamePiece(Color.Blue, Pattern.Stripes) },
                        new List<GamePiece>() { new GamePiece(Color.Yellow, Pattern.Dots), empty, empty, empty, blocked, empty, new GamePiece(Color.Green, Pattern.Dots) },
                        new List<GamePiece>() { new GamePiece(Color.Green, Pattern.Flowers), empty, blocked, empty, empty, empty, new GamePiece(Color.Pink, Pattern.Vines) },
                        new List<GamePiece>() { new GamePiece(Color.Blue, Pattern.Quatrefoil), empty, empty, empty, empty, empty, new GamePiece(Color.Cyan, Pattern.Fern) },
                        new List<GamePiece>() { new GamePiece(Color.Cyan, Pattern.Vines), new GamePiece(Color.Purple, Pattern.Fern), new GamePiece(Color.Yellow, Pattern.Vines), new GamePiece(Color.Cyan, Pattern.Dots), new GamePiece(Color.Pink, Pattern.Stripes), new GamePiece(Color.Purple, Pattern.Flowers), new GamePiece(Color.Yellow, Pattern.Quatrefoil) }
                    };
                    break;
                default:
                    board = new List<List<GamePiece>>();
                    break;
            }

        }

        public void AddPiece(GamePiece piece, int x, int y)
        {
                board[x][y] = piece;   
            
            
        }

        public void PrintBoard()
        {
            // funkce pro print dílku?
            // všechny switche tu?
            Console.WriteLine("      1    2    3    4    5    6    7");
            Console.WriteLine("   ------------------------------------");
            for (int i = 0; i < size; i++) 
            {

                Console.Write(i+1);
                if (i % 2 == 0) Console.Write("  ");
                Console.Write(" |");
                for (int j = 0; j < size; j++)
                {
                    GamePiece p = board[i][j];
                    Console.Write($"{p.Print}|");
                }
                Console.Write("\n");
                Console.WriteLine("   ------------------------------------");
            }

        }

        public bool IsEmpty(int x, int y)
        {
            return (board[x][y].Type == 0);
        }
    }
}
