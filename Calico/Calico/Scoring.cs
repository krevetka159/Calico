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

        public int _colorClusterScore = 3;
        public Dictionary<Pattern, int> _patternClusterScores;

        public int _colorClusterSize = 3;
        public Dictionary<Pattern, int> _patternClusterSizes;

        public StringBuilder patternScoring;

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

            _patternClusterScores = new Dictionary<Pattern, int>();
            _patternClusterSizes = new Dictionary<Pattern, int>();

            patternScoring = new StringBuilder();

            foreach ((int size, int score) in patternProps)
            {
                patternScoring.Append(" " + size + " : ");
                for (int i = 0; i < 2; i++)
                {
                    int randInt = random.Next(patterns.Count);
                    _patternClusterSizes[patterns[randInt]] = size;
                    _patternClusterScores[patterns[randInt]] = score;

                    patternScoring.Append((char)(64+(int)patterns[randInt]));
                    patternScoring.Append(", ");
                    
                    patterns.RemoveAt(randInt);
                }
                patternScoring.Append("\n");
            }



        }
    }
}
