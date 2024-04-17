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
    }

    public class TaskPiece : GamePiece
    {
        public int ScoreCompletedFully;
        public int ScoreCompletedPartly;
        public List<int> Task;
        public Dictionary<Color, int> NeighboringColors = new Dictionary<Color, int>(){
                { Color.Yellow, 0 },
                { Color.Green, 0 },
                { Color.Cyan, 0 },
                { Color.Blue, 0 },
                { Color.Purple, 0 },
                { Color.Pink, 0 }};
        public Dictionary<Pattern, int> NeighboringPatterns = new Dictionary<Pattern, int>(){
                { Pattern.Dots, 0 },
                { Pattern.Stripes, 0 },
                { Pattern.Fern, 0 },
                { Pattern.Flowers, 0 },
                { Pattern.Vines, 0 },
                { Pattern.Quatrefoil, 0 } };
        public string Description;

        public TaskPiece(int TaskId) : base(TaskId) 
        {
            switch (TaskId)
            {
                case 1:
                    ScoreCompletedFully = 15;
                    ScoreCompletedPartly = 10;
                    Task = new List<int>() { 1, 1, 1, 1, 1, 1 };
                    Description = " all different, 10/15 ";
                    break;
                case 2:
                    ScoreCompletedFully = 14;
                    ScoreCompletedPartly = 7;
                    Task = new List<int>() { 4, 2, 0, 0, 0, 0 };
                    Description = " 4:2, 7/14 ";
                    break;
                case 3:
                    ScoreCompletedFully = 13;
                    ScoreCompletedPartly = 7;
                    Task = new List<int>() { 3, 3, 0, 0, 0, 0 };
                    Description = " 3:3, 7/13 ";
                    break;
                case 4:
                    ScoreCompletedFully = 11;
                    ScoreCompletedPartly = 7;
                    Task = new List<int>() { 3, 2, 1, 0, 0, 0 };
                    Description = " 3:2:1, 7/11 ";
                    break;
                case 5:
                    ScoreCompletedFully = 11;
                    ScoreCompletedPartly = 7;
                    Task = new List<int>() { 2, 2, 2, 0, 0, 0 };
                    Description = " 2:2:21 7/11 ";
                    break;
                case 6:
                    ScoreCompletedFully = 7;
                    ScoreCompletedPartly = 5;
                    Task = new List<int>() { 2, 2, 1, 1, 0, 0 };
                    Description = " 2:2:1:1, 5/7 ";
                    break;
            }
        }

        /// <summary>
        /// Evaluates neighboring patchtiles
        /// </summary>
        /// <returns> Score </returns>
        public int Evaluate()
        {
            bool colorsFailed = false; ;
            bool patternsFailed = false;
            List<int> colors = NeighboringColors.Values.ToList();
            List<int> patterns = NeighboringPatterns.Values.ToList();

            colors.Sort((a, b) => b.CompareTo(a));
            patterns.Sort((a, b) => b.CompareTo(a));

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

        public int CheckNeighbours(GamePiece gp)
        {
            bool colorsFailed = false; ;
            bool patternsFailed = false;
            Dictionary<Color, int> colorN = new Dictionary<Color,int>(NeighboringColors);
            Dictionary<Pattern,int> patternsN = new Dictionary<Pattern,int>(NeighboringPatterns);
            colorN[gp.Color] += 1;
            patternsN[gp.Pattern] += 1;

            List<int> colors = colorN.Values.ToList();
            List<int> patterns = patternsN.Values.ToList();

            colors.Sort((a, b) => b.CompareTo(a));
            patterns.Sort((a, b) => b.CompareTo(a));

            // if (colors.Sum() == 1) return 0;

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
            bool colorsFailed = false; ;
            bool patternsFailed = false;
            Dictionary<Color, int> colorN = new Dictionary<Color, int>(NeighboringColors);
            Dictionary<Pattern, int> patternsN = new Dictionary<Pattern, int>(NeighboringPatterns);
            colorN[gp.Color] += 1;
            patternsN[gp.Pattern] += 1;

            List<int> colors = colorN.Values.ToList();
            List<int> patterns = patternsN.Values.ToList();

            colors.Sort((a, b) => b.CompareTo(a));
            patterns.Sort((a, b) => b.CompareTo(a));

            // if (colors.Sum() == 1) return 0;

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
            Dictionary<Color, int> colorN = new Dictionary<Color, int>(NeighboringColors);
            Dictionary<Pattern, int> patternsN = new Dictionary<Pattern, int>(NeighboringPatterns);

            foreach (GamePiece gp in gamePieces)
            {
                colorN[gp.Color] += 1;
                patternsN[gp.Pattern] += 1;
            }

            List<int> colors = colorN.Values.ToList();
            List<int> patterns = patternsN.Values.ToList();

            colors.Sort((a, b) => b.CompareTo(a));
            patterns.Sort((a, b) => b.CompareTo(a));

            // if (colors.Sum() == 1) return 0;

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
            NeighboringColors[piece.Color] += 1;
            NeighboringPatterns[piece.Pattern] += 1;
        }
    }
}
