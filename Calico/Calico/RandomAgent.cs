using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class RandomAgent : Player
    {
        Random random = new Random();

        public override int ChooseGamePiece(GamePiece[] Opts)
        {
            return random.Next(1, Opts.Length);
        }

        public override (int, int) ChoosePosition()
        {
            int row = random.Next(0, board.size - 1);
            int col = random.Next(0, board.size - 1);

            while (!board.IsEmpty(row, col))
            {
                row = random.Next(0, board.size - 1);
                col = random.Next(0, board.size - 1);
            }

            return (row +1, col +1);
        }
    }
}
