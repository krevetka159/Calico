using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public enum Color
    {
        None = 0,

        Yellow = 1,
        Green = 2, 
        Cyan = 3, 
        Blue = 4, 
        Purple = 5, 
        Pink = 6,
    }

    public enum Pattern
    {
        None = 0,
        Dots = 1,
        Stripes = 2,
        Fern = 3,
        Flowers = 4,
        Vines = 5,
        Quatrefoil = 6,
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
            // init for Empty and Blocked

            Type = t;
            Color = Color.None;
            Pattern = Pattern.None;

            if (t == Type.Empty) Print = " -- ";
            else if (t == Type.Blocked) Print = " XX ";
        }

        public GamePiece(Color c, Pattern p) 
        {
            // init for PatchTile
            Color = c;
            Pattern = p;
            Type = Type.PatchTile;
            Print = $" {(int)Color}{(char)(64 + (int)Pattern)} ";
        }
    }
}
