using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class ScoreCounter
    {
        //private int _colorClusterScore = 3;
        //private Dictionary<Pattern, int> _patternClusterScores;
        public int _score;

        //private int _colorClusterSize = 3;
        //private Dictionary<Pattern, int> _patternClusterSizes;

        private UnionFindWithArray<GamePiece> _colorUF;
        private UnionFindWithArray<GamePiece> _patternUF;

        private Random random;

        private Scoring _scoring;

        public ScoreCounter(Scoring scoring) 
        {
            //random = new Random();
            _score = 0;
            _scoring = scoring;
            _colorUF = new UnionFindWithArray<GamePiece>();
            _patternUF = new UnionFindWithArray<GamePiece>();

            //List<(int,int)> patternProps = new List<(int, int)>() { (3,3),(4,5),(5,7)};

            //// randomly rozdělit patterny na clustersizes a bodíky

            //List<Pattern> patterns = new List<Pattern>();

            //foreach (Pattern i in Enum.GetValues(typeof(Pattern)))
            //{
            //   if(i != Pattern.None)
            //    {
            //        patterns.Add(i);
            //    }
            //}

            //_patternClusterScores = new Dictionary<Pattern, int>();
            //_patternClusterSizes = new Dictionary<Pattern, int>();

            //foreach ((int size, int score) in patternProps)
            //{
            //    for (int i = 0;  i < 2; i++)
            //    {
            //        int randInt = random.Next(patterns.Count);
            //        _patternClusterSizes[patterns[randInt]] = size;
            //        _patternClusterScores[patterns[randInt]] = score;

            //        Console.WriteLine(patterns[randInt]);
            //        patterns.RemoveAt(randInt);
            //    }
            //}

            //foreach((Pattern p, int size) in _patternClusterSizes)
            //{
            //    Console.WriteLine(p);
            //    Console.WriteLine(size);
            //}
        }

        public void AddToUF(GamePiece piece)
        {
            _patternUF.Add(piece);
            _colorUF.Add(piece);
        }

        public void EvaluateNew(GamePiece p, GamePiece n)
        {
            if (p.Color == n.Color)
            {

                if (!_colorUF.Find(p, n))
                {
                    _score -= (_colorUF.CountScore(n) / _scoring._colorClusterSize) * _scoring._colorClusterScore;
                    _score -= (_colorUF.CountScore(p) / _scoring._colorClusterSize) * _scoring._colorClusterScore;

                    _colorUF.Union(n, p);

                    _score += (_colorUF.CountScore(n) / _scoring._colorClusterSize) * _scoring._colorClusterScore;
                }

            }
            if (p.Pattern == n.Pattern)
            {
                if (!_patternUF.Find(p, n))
                {
                    _score -= (_patternUF.CountScore(n) / _scoring._patternClusterSizes[n.Pattern]) * _scoring._patternClusterScores[n.Pattern];
                    _score -= (_patternUF.CountScore(p) / _scoring._patternClusterSizes[p.Pattern]) * _scoring._patternClusterScores[p.Pattern];

                    _patternUF.Union(n, p);

                    _score += (_patternUF.CountScore(n) / _scoring._patternClusterSizes[n.Pattern]) * _scoring._patternClusterScores[n.Pattern];

                }

            }

        }

        public int GetScore()
        {
            return _score;
        }
        

    }
}
