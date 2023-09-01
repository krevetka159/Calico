using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class GamePiece
    {
        public int Color;
        public int Pattern;
        private int ID;
        public int Type;

        public GamePiece(int color, int pattern)
        {
            Color = color;
            Pattern = pattern;

            if (pattern == 0 && color == 0) Type = 0;
            else if (pattern == -1 && color == -1) Type = -1;
            else Type = 1;
            // TODO check správného předání color, pattern
            //ID = id;
        }
    }
}
