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
    public class GamePiece
    {
        public Color Color { get; private set; }
        public Pattern Pattern { get; private set; }
        public Type Type { get; private set; }
        public string Print { get; private set; }


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
