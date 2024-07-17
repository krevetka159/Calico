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
        Task,
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

        public GamePiece(int TaskId)
        {
            // init for TaskPiece

            Type = Type.Task;
            Color = Color.None;
            Pattern = Pattern.None;

            Print = $" T{TaskId} ";
        }

        public GamePiece(GamePiece gp)
        {
            // init for TaskPiece

            Type = gp.Type;
            Color = gp.Color;
            Pattern = gp.Pattern;
            Print = gp.Print;
        }
    }

    public class TaskPiece : GamePiece
    {
        public int Id;
        public int ScoreCompletedFully;
        public int ScoreCompletedPartly;
        public int[] Task;
        public int[] Colors = new int[6] { 0, 0, 0, 0, 0, 0 };
        public int[] Patterns = new int[6] { 0, 0, 0, 0, 0, 0 };
        public string Description;

        public TaskPiece(int TaskId) : base(TaskId) 
        {
            Id = TaskId;
            switch (TaskId)
            {
                case 1:
                    ScoreCompletedFully = 15;
                    ScoreCompletedPartly = 10;
                    Task = new int[6] { 1, 1, 1, 1, 1, 1 };
                    Description = " vše různé, 10/15 ";
                    break;
                case 2:
                    ScoreCompletedFully = 14;
                    ScoreCompletedPartly = 7;
                    Task = new int[6] { 4, 2, 0, 0, 0, 0 };
                    Description = " 4:2, 7/14 ";
                    break;
                case 3:
                    ScoreCompletedFully = 13;
                    ScoreCompletedPartly = 7;
                    Task = new int[6] { 3, 3, 0, 0, 0, 0 };
                    Description = " 3:3, 7/13 ";
                    break;
                case 4:
                    ScoreCompletedFully = 11;
                    ScoreCompletedPartly = 7;
                    Task = new int[6] { 3, 2, 1, 0, 0, 0 };
                    Description = " 3:2:1, 7/11 ";
                    break;
                case 5:
                    ScoreCompletedFully = 11;
                    ScoreCompletedPartly = 7;
                    Task = new int[6] { 2, 2, 2, 0, 0, 0 };
                    Description = " 2:2:2 7/11 ";
                    break;
                case 6:
                    ScoreCompletedFully = 7;
                    ScoreCompletedPartly = 5;
                    Task = new int[6] { 2, 2, 1, 1, 0, 0 };
                    Description = " 2:2:1:1, 5/7 ";
                    break;
            }
        }

        public TaskPiece(TaskPiece piece) : base(piece)
        {
            Id = piece.Id;
            ScoreCompletedFully = piece.ScoreCompletedFully;
            ScoreCompletedPartly = piece.ScoreCompletedPartly;
            Task = piece.Task;
            Description = piece.Description;
            Colors = (int[])piece.Colors.Clone();
            Patterns = (int[])piece.Patterns.Clone();
        }

        /// <summary>
        /// Evaluates neighboring patchtiles
        /// </summary>
        /// <returns> Score </returns>
        public int Evaluate()
        {
            bool colorsFailed = false; ;
            bool patternsFailed = false;
            int[] colors = (int[])Colors.Clone();
            int[] patterns = (int[])Patterns.Clone();

            Array.Sort(colors);
            Array.Reverse(colors);
            Array.Sort(patterns);
            Array.Reverse(patterns);

            for (int i = 0; i < Task.Count(); i++)
            {
                if (colors[i] != Task[i])
                {
                    if (patternsFailed) return 0;
                    else colorsFailed = true;
                }
                if (patterns[i] != Task[i])
                {
                    if (colorsFailed) return 0;
                    else patternsFailed = true;
                }
            }

            if (!colorsFailed && !patternsFailed) return ScoreCompletedFully;
            else if (!patternsFailed || !colorsFailed) return ScoreCompletedPartly;
            else return 0;
        }

        public int Completed()
        {
            bool colorsFailed = false; ;
            bool patternsFailed = false;
            int[] colors = (int[])Colors.Clone();
            int[] patterns = (int[])Patterns.Clone();

            Array.Sort(colors);
            Array.Reverse(colors);
            Array.Sort(patterns);
            Array.Reverse(patterns);

            for (int i = 0; i < Task.Count(); i++)
            {
                if (colors[i] != Task[i])
                {
                    if (patternsFailed) return 0;
                    else colorsFailed = true;
                }
                if (patterns[i] != Task[i])
                {
                    if (colorsFailed) return 0;
                    else patternsFailed = true;
                }
            }

            if (!colorsFailed && !patternsFailed) return 2;
            else if (!patternsFailed || !colorsFailed) return 1;
            else return 0;
        }

        public int CheckNeighbours(GamePiece gp)
        {
            bool colorsFailed = false; ;
            bool patternsFailed = false;

            int[] colors = (int[])Colors.Clone();
            colors[(int)gp.Color - 1] += 1;
            int[] patterns = (int[])Patterns.Clone();
            patterns[(int)gp.Pattern-1] += 1;

            Array.Sort(colors);
            Array.Reverse(colors);
            Array.Sort(patterns);
            Array.Reverse(patterns);


            if (colors.Sum() == 6)
            {
                for (int i = 0; i < Task.Count(); i++)
                {
                    if (colors[i] != Task[i])
                    {
                        if (patternsFailed) return 0;
                        else colorsFailed = true;
                    }
                    if (patterns[i] != Task[i])
                    {
                        if (colorsFailed) return 0;
                        else patternsFailed = true;
                    }
                }

                if (!colorsFailed && !patternsFailed) return ScoreCompletedFully;
                else if (!patternsFailed || !colorsFailed) return ScoreCompletedPartly;
                else return 0;
            }
            else
            {

                for (int i = 0; i < Task.Count(); i++)
                {
                    if (colors[i] > Task[i])
                    {
                        if (patternsFailed) return 0;
                        else colorsFailed = true;
                    }
                    if (patterns[i] > Task[i])
                    {
                        if (colorsFailed) return 0;
                        else patternsFailed = true;
                    }
                }

                if (!colorsFailed && !patternsFailed) return 2;
                else if (!patternsFailed || !colorsFailed) return 1;
                else return 0;
            }
        }

        public double CheckNeighboursUtility(GamePiece gp)
        {
            bool colorsFailed = false;
            bool patternsFailed = false;
            int[] colors = (int[])Colors.Clone();
            colors[(int)gp.Color-1] += 1;
            int[] patterns = (int[])Patterns.Clone();
            patterns[(int)gp.Pattern-1] += 1;

            Array.Sort(colors);
            Array.Reverse(colors);
            Array.Sort(patterns);
            Array.Reverse(patterns);


            for (int i = 0; i < Task.Count(); i++)
            {
                if (colors[i] > Task[i]) // for colors.Sum() == 6 nastane když existuje pokud neplatí rovnosti u všeho
                {
                    if (patternsFailed) return 0;
                    else colorsFailed = true;
                }
                if (patterns[i] > Task[i])
                {
                    if (colorsFailed) return 0;
                    else patternsFailed = true;
                }
            }

            if (!colorsFailed && !patternsFailed) return (Convert.ToDouble(ScoreCompletedFully) / 6) * colors.Sum();
            else if (!patternsFailed || !colorsFailed) return (Convert.ToDouble(ScoreCompletedPartly) / 6) * colors.Sum();
            else return 0;
        }

        public double CheckNeighboursUtilityMinimax(List<GamePiece> gamePieces)
        {
            bool colorsFailed = false; ;
            bool patternsFailed = false;

            int[] colors = (int[])Colors.Clone();
            int[] patterns = (int[])Patterns.Clone();

            foreach (GamePiece gp in gamePieces)
            {
                colors[(int)gp.Color-1] += 1;
                patterns[(int)gp.Pattern-1] += 1;
            }

            Array.Sort(colors);
            Array.Reverse(colors);
            Array.Sort(patterns);
            Array.Reverse(patterns);

            for (int i = 0; i < Task.Count(); i++)
            {
                if (colors[i] > Task[i]) // for colors.Sum() == 6 nastane když existuje pokud neplatí rovnosti u všeho
                {
                    if (patternsFailed) return 0;
                    else colorsFailed = true;
                }
                if (patterns[i] > Task[i])
                {
                    if (colorsFailed) return 0;
                    else patternsFailed = true;
                }
            }

            if (!colorsFailed && !patternsFailed) return (Convert.ToDouble(ScoreCompletedFully) / 6) * colors.Sum();
            else if (!patternsFailed || !colorsFailed) return (Convert.ToDouble(ScoreCompletedPartly) / 6) * colors.Sum();
            else return 0;
        }

        public void AddNeighbor(GamePiece piece)
        {
            Colors[(int)piece.Color-1] += 1;
            Patterns[(int)piece.Pattern-1] += 1;
        }
    }
}
