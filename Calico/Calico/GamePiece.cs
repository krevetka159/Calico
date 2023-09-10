using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public enum Color
    {
        None,
        Yellow,
        Green, 
        Cyan, 
        Blue, 
        Purple, 
        Pink
    }

    public enum Pattern
    {
        //to jsem si nevymyslela, to je legit z pravidel
        None,
        Dots,
        Stripes,
        Fern,
        Flowers,
        Vines,
        Quatrefoil
    }

    public enum Type
    {
        Empty,
        Blocked,
        PatchTile
    }
    internal class GamePiece
    {
        public Color Color;
        public Pattern Pattern;
        private int ID;
        public Type Type;
        public string Print;

        //public GamePiece(int color, int pattern)
        //{
        //    Color = color;
        //    Pattern = pattern;

        //    if (pattern == 0 && color == 0) Type = 0;
        //    else if (pattern == -1 && color == -1) Type = -1;
        //    else Type = 1;
        //    // TODO check správného předání color, pattern
        //    //ID = id;
        //}


        public GamePiece(Type t)
        {
            Type = t;
            Color = Color.None;
            Pattern = Pattern.None;

            if (t == Type.Empty) Print = " -- ";
            else if (t == Type.Blocked) Print = " XX ";
        }

        public GamePiece(Color c, Pattern p) 
        {
            Color = c;
            Pattern = p;
            Type = Type.PatchTile;
            Print = $" {(int)Color}{(char)(64 + (int)Pattern)} ";
        }
    }
}
