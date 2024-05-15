using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class Bag

    {
        private List<GamePiece> pieces;
        private Random random;

        public static int NumOfPatterns = 6;
        public static int NumOfColors = 6;

        public Bag()
        {
            random = new Random();
            pieces = new List<GamePiece>();

            // 3 patchtiles for each color-pattern combination
            for (int i = 1; i <= NumOfColors; i++)
            {
                for (int j = 1; j <= NumOfPatterns; j++)
                {
                    pieces.Add(new GamePiece((Color)i, (Pattern)j));
                    pieces.Add(new GamePiece((Color)i, (Pattern)j));
                    pieces.Add(new GamePiece((Color)i, (Pattern)j));
                }
            }
        }
        public Bag(Bag b)
        {
            random = new Random();
            pieces = new List<GamePiece>(b.pieces);
        }
        /// <summary>
        /// Randomly picks a patchtile from bag
        /// </summary>
        /// <returns></returns>
        public GamePiece Next()
        {
            int randInt = random.Next(0, pieces.Count());
            GamePiece next = pieces[randInt];
            pieces.RemoveAt(randInt);

            return next;
        }
    }

}
