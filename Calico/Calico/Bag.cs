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

        public Bag()
        {
            random = new Random();
            Pieces = new List<GamePiece>();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Pieces.Add(new GamePiece(i+1, j+1));
                    Pieces.Add(new GamePiece(i+1, j+1));
                    Pieces.Add(new GamePiece(i+1, j+1));
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
