using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class Scoring
    {
        private Random random;

        public int ColorClusterScore = 3;
        public Dictionary<Pattern, int> PatternClusterScores { get; private set; }

        public int ColorClusterSize = 3;
        public Dictionary<Pattern, int> PatternClusterSizes { get; private set; }

        public StringBuilder patternScoring { get; private set; }

        public Scoring() 
        {
            random = new Random();

            List<(int, int)> patternProps = new List<(int, int)>() { (3, 3), (4, 5), (5, 7) };

            // randomly rozdělit patterny na clustersizes a bodíky

            List<Pattern> patterns = new List<Pattern>();

            foreach (Pattern i in Enum.GetValues(typeof(Pattern)))
            {
                if (i != Pattern.None)
                {
                    patterns.Add(i);
                }
            }

            PatternClusterScores = new Dictionary<Pattern, int>();
            PatternClusterSizes = new Dictionary<Pattern, int>();

            patternScoring = new StringBuilder();

            foreach ((int size, int score) in patternProps)
            {
                patternScoring.Append(" " + size + " : ");
                for (int i = 0; i < 2; i++)
                {
                    int randInt = random.Next(patterns.Count);
                    PatternClusterSizes[patterns[randInt]] = size;
                    PatternClusterScores[patterns[randInt]] = score;

                    patternScoring.Append((char)(64+(int)patterns[randInt]));
                    patternScoring.Append(", ");
                    
                    patterns.RemoveAt(randInt);
                }
                patternScoring.Append("\n");
            }



        }
    }
}
