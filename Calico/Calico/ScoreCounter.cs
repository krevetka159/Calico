using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class ScoreCounter
    {
        private int _colorClusterScore = 3;
        private Dictionary<Pattern, int> _patternClusterScores;
        public int _score;

        private int _colorClusterSize = 3;
        private Dictionary<Pattern, int> _patternClusterSizes;

        private UnionFindWithArray<GamePiece> _colorUF;
        private UnionFindWithArray<GamePiece> _patternUF;

        public ScoreCounter() 
        { 
            _score = 0;
            _colorUF = new UnionFindWithArray<GamePiece>();
            _patternUF = new UnionFindWithArray<GamePiece>();
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
                    _score -= (_colorUF.CountScore(n) / _colorClusterSize) * _colorClusterScore;
                    _score -= (_colorUF.CountScore(p) / _colorClusterSize) * _colorClusterScore;

                    _colorUF.Union(n, p);

                    _score += (_colorUF.CountScore(n) / _colorClusterSize) * _colorClusterScore;
                }

            }
            //if (p.Pattern == n.Pattern)
            //{
            //    if (!_patternUF.Find(p, n))
            //    {
            //        _score -= (_patternUF.CountScore(n) / _patternClusterSizes[n.Pattern]) * _patternClusterScores[n.Pattern];
            //        _score -= (_patternUF.CountScore(p) / _patternClusterSizes[p.Pattern]) * _patternClusterScores[p.Pattern];

            //        _colorUF.Union(n, p);

            //        _score += (_patternUF.CountScore(n) / _patternClusterSizes[n.Pattern]) * _patternClusterScores[n.Pattern];

            //    }

            //}

        }

        public int GetScore()
        {
            return _score;
        }
        

    }
}
