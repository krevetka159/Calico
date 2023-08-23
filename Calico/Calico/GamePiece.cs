using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class GamePiece
    {
        private int Color;
        private int Pattern;
        private int ID;

        public GamePiece(int color, int pattern)
        {
            Color = color;
            Pattern = pattern;
            //ID = id;
        }
    }
}
