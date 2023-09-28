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

        public bool CheckColorUnion(GamePiece p, GamePiece n)
        {
            return _colorUF.Find(p, n);
        }

        public bool CheckPatternUnion(GamePiece p, GamePiece n)
        {
            return _patternUF.Find(p, n);
        }

        public int GetColorCount(GamePiece p)
        {
            return _colorUF.CountScore(p);
        }

        public int GetColorScore(GamePiece p)
        {
            return (_colorUF.CountScore(p) /_scoring._colorClusterSize) * _scoring._colorClusterScore;
        }

        public int GetPatternCount(GamePiece p)
        {
            return _patternUF.CountScore(p);
        }

        public int GetPatternScore(GamePiece p)
        {
            return (_patternUF.CountScore(p) / _scoring._patternClusterSizes[p.Pattern]) * _scoring._patternClusterScores[p.Pattern];
        }

        public int CountColorScore(int count)
        {
            return (count / _scoring._colorClusterSize) * _scoring._colorClusterScore;
        }

        public int CountPatternScore(int count, Pattern p) 
        {
            return (count / _scoring._patternClusterSizes[p]) * _scoring._patternClusterScores[p];
        }


        public int GetScore()
        {
            return _score;
        }
        

    }
}
