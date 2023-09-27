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
        public int _score;


        private UnionFindWithArray<GamePiece> _colorUF;
        private UnionFindWithArray<GamePiece> _patternUF;


        private Scoring _scoring;

        public ScoreCounter(Scoring scoring) 
        {
            
            _score = 0;
            _scoring = scoring;
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
