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

        public GamePiece(int color, int pattern)
        {
            Color = color;
            Pattern = pattern;
            //ID = id;
        }
    }
}
