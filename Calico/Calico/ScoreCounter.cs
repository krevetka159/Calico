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
        public int score;


        private UnionFindWithArray<GamePiece> colorUF;
        private UnionFindWithArray<GamePiece> patternUF;


        private Scoring scoring;

        public ScoreCounter(Scoring scoring) 
        {
            
            score = 0;
            this.scoring = scoring;
            colorUF = new UnionFindWithArray<GamePiece>();
            patternUF = new UnionFindWithArray<GamePiece>();

        }

        public void AddToUF(GamePiece piece)
        {
            patternUF.Add(piece);
            colorUF.Add(piece);
        }

        public void EvaluateNew(GamePiece p, GamePiece n)
        {
            if (p.Color == n.Color)
            {

                if (!colorUF.Find(p, n))
                {
                    score -= (colorUF.Count(n) / scoring._colorClusterSize) * scoring._colorClusterScore;
                    score -= (colorUF.Count(p) / scoring._colorClusterSize) * scoring._colorClusterScore;

                    colorUF.Union(n, p);

                    score += (colorUF.Count(n) / scoring._colorClusterSize) * scoring._colorClusterScore;
                }

            }
            if (p.Pattern == n.Pattern)
            {
                if (!patternUF.Find(p, n))
                {
                    score -= (patternUF.Count(n) / scoring.patternClusterSizes[n.Pattern]) * scoring.patternClusterScores[n.Pattern];
                    score -= (patternUF.Count(p) / scoring.patternClusterSizes[p.Pattern]) * scoring.patternClusterScores[p.Pattern];

                    patternUF.Union(n, p);

                    score += (patternUF.Count(n) / scoring.patternClusterSizes[n.Pattern]) * scoring.patternClusterScores[n.Pattern];

                }

            }

        }

        public bool CheckColorUnion(GamePiece p, GamePiece n)
        {
            return colorUF.Find(p, n);
        }

        public bool CheckPatternUnion(GamePiece p, GamePiece n)
        {
            return patternUF.Find(p, n);
        }

        public int GetColorCount(GamePiece p)
        {
            return colorUF.Count(p);
        }

        public int GetColorScore(GamePiece p)
        {
            return (colorUF.Count(p) /scoring._colorClusterSize) * scoring._colorClusterScore;
        }

        public int GetPatternCount(GamePiece p)
        {
            return patternUF.Count(p);
        }

        public int GetPatternScore(GamePiece p)
        {
            return (patternUF.Count(p) / scoring.patternClusterSizes[p.Pattern]) * scoring.patternClusterScores[p.Pattern];
        }

        public int CountColorScore(int count)
        {
            return (count / scoring._colorClusterSize) * scoring._colorClusterScore;
        }

        public int CountPatternScore(int count, Pattern p) 
        {
            return (count / scoring.patternClusterSizes[p]) * scoring.patternClusterScores[p];
        }


        public int GetScore()
        {
            return score;
        }
        

    }
}
