using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Calico
{
    /// <summary>
    /// Scoring and scoring rules for colors and patterns
    /// </summary>
    public class Scoring
    {
        private Random random;

        public int ColorClusterScore = 3;

        public int[] PatternClusterScores {  get; private set; }

        public int ColorClusterSize = 3;
        public int[] PatternClusterSizes { get; private set; }

        public ColorScoring ColorScoring { get; private set; }

        public PatternScoring PatternScoring {  get; private set; }

        public StringBuilder PatternScoringToString { get; private set; }

        public Scoring() 
        {
            random = new Random();

            List<(int, int)> patternProps = new List<(int, int)>() { (3, 3), (4, 5), (5, 7) };

            // randomly assign pattern to different scoring panels
            List<Pattern> patterns = new List<Pattern>();

            foreach (Pattern i in Enum.GetValues(typeof(Pattern)))
            {
                if (i != Pattern.None)
                {
                    patterns.Add(i);
                }
            }

            PatternClusterScores = new int[6];
            PatternClusterSizes = new int[6];

            PatternScoringToString = new StringBuilder();

            foreach ((int size, int score) in patternProps)
            {
                PatternScoringToString.Append($" {size} patchtiles = {score} points for patterns ");
                for (int i = 0; i < 2; i++)
                {
                    int randInt = random.Next(patterns.Count);

                    PatternClusterSizes[(int)patterns[randInt]-1] = size;
                    PatternClusterScores[(int)patterns[randInt]-1] = score;

                    PatternScoringToString.Append((char)(64+(int)patterns[randInt]));
                    PatternScoringToString.Append(", ");
                    
                    patterns.RemoveAt(randInt);
                }
                PatternScoringToString.Append("\n");
            }


            PatternScoring = new PatternScoring();
            ColorScoring = new ColorScoring();
        }
    }

    /// <summary>
    /// Scoring for patterns
    /// </summary>
    public class PatternScoring
    {
        private Random random;
        public PatternScoringPanel[] PatternScoringDict { get; private set; }
        public PatternScoringPanel[] PatternScoringPanels { get; private set; }

        public StringBuilder PatternScoringPrint {  get; private set; }

        public PatternScoring()
        {
            random = new Random();

            List<int> patterns = new List<int>(new int[6] { 1, 2, 3, 4, 5, 6 });

            PatternScoringPrint = new StringBuilder();

            PatternScoringPanels = new PatternScoringPanel[3];
            PatternScoringDict = new PatternScoringPanel[6];

            for (int i = 0; i <= 2; i++)
            {
                int randInt = random.Next(patterns.Count);
                Pattern p1 = (Pattern) patterns[randInt];
                patterns.RemoveAt(randInt);

                randInt = random.Next(patterns.Count);
                Pattern p2 = (Pattern) patterns[randInt];
                patterns.RemoveAt(randInt);

                PatternScoringPanel panel = new PatternScoringPanel(i, p1, p2);

                PatternScoringPanels[i] = panel;
                PatternScoringDict[(int)p1-1] = panel;
                PatternScoringDict[(int)p2 - 1] = panel;
            }

            foreach ( PatternScoringPanel panel in PatternScoringPanels)
            {
                PatternScoringPrint.Append($" {panel.ClusterSize} patchtiles = {panel.Points} points for patterns {panel.PatternsPrint} \n");
            }

        }
    }

    /// <summary>
    /// Scoring panel for patterns
    /// </summary>
    public class PatternScoringPanel
    {
        public int Id;
        public int Points;
        public int ClusterSize;
        public (Pattern, Pattern) Patterns;
        public string PatternsPrint;

        public PatternScoringPanel(int id, Pattern p1, Pattern p2)
        {
            Patterns = (p1,p2);
            Id = id;
            switch (id)
            {
                case 0:
                    Points = 3;
                    ClusterSize = 3;
                    break;
                case 1:
                    Points = 5;
                    ClusterSize = 4;
                    break;
                case 2:
                    Points = 7;
                    ClusterSize = 5;
                    break;
            }

            PatternsPrint = $"{(char)(64 + (int)p1)}, {(char)(64 + (int)p2)}";

        }
    }

    /// <summary>
    /// Scoring for colors
    /// </summary>
    public class ColorScoring
    {
        public int Points;
        public int ClusterSize;

        public ColorScoring()
        {
            Points = 3;
            ClusterSize = 3;
        }
    }
}
