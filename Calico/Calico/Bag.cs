using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class Bag

        // TODO random generátor dalšího dílku
    {
        private List<GamePiece> pieces;

        public Bag()
        {
            pieces = new List<GamePiece>();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    pieces.Add(new GamePiece(i, j));
                    pieces.Add(new GamePiece(i, j));
                    pieces.Add(new GamePiece(i, j));
                }
            }
        }
    }
}
