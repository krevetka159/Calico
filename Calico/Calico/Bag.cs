using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class Bag

    {
        private List<GamePiece> Pieces;
        private Random random;

        private static int NumOfPatterns = 6;
        private static int NumOfColors = 6;

        public Bag()
        {
            random = new Random();
            Pieces = new List<GamePiece>();
            for (int i = 1; i <= NumOfColors; i++)
            {
                for (int j = 1; j <= NumOfPatterns; j++)
                {
                    Pieces.Add(new GamePiece((Color)i, (Pattern)j));
                    Pieces.Add(new GamePiece((Color)i, (Pattern)j));
                    Pieces.Add(new GamePiece((Color)i, (Pattern)j));
                }
            }
        }

        public GamePiece Next()
        {
            int randInt = random.Next(0, Pieces.Count());
            GamePiece next = Pieces[randInt];
            Pieces.RemoveAt(randInt);

            return next;
        }
    }

}
