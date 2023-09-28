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
        public int Score { get; private set; }

        private UnionFindWithArray<GamePiece> colorUF;
        private UnionFindWithArray<GamePiece> patternUF;

        private Scoring scoring;

        public ScoreCounter(Scoring scoring) 
        {
            
            Score = 0;
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
                    Score -= (colorUF.Count(n) / scoring.ColorClusterSize) * scoring.ColorClusterScore;
                    Score -= (colorUF.Count(p) / scoring.ColorClusterSize) * scoring.ColorClusterScore;

                    colorUF.Union(n, p);

                    Score += (colorUF.Count(n) / scoring.ColorClusterSize) * scoring.ColorClusterScore;
                }

            }
            if (p.Pattern == n.Pattern)
            {
                if (!patternUF.Find(p, n))
                {
                    Score -= (patternUF.Count(n) / scoring.PatternClusterSizes[n.Pattern]) * scoring.PatternClusterScores[n.Pattern];
                    Score -= (patternUF.Count(p) / scoring.PatternClusterSizes[p.Pattern]) * scoring.PatternClusterScores[p.Pattern];

                    patternUF.Union(n, p);

                    Score += (patternUF.Count(n) / scoring.PatternClusterSizes[n.Pattern]) * scoring.PatternClusterScores[n.Pattern];

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
            return (colorUF.Count(p) /scoring.ColorClusterSize) * scoring.ColorClusterScore;
        }

        public int GetPatternCount(GamePiece p)
        {
            return patternUF.Count(p);
        }

        public int GetPatternScore(GamePiece p)
        {
            return (patternUF.Count(p) / scoring.PatternClusterSizes[p.Pattern]) * scoring.PatternClusterScores[p.Pattern];
        }

        public int CountColorScore(int count)
        {
            return (count / scoring.ColorClusterSize) * scoring.ColorClusterScore;
        }

        public int CountPatternScore(int count, Pattern p) 
        {
            return (count / scoring.PatternClusterSizes[p]) * scoring.PatternClusterScores[p];
        }


        public int GetScore()
        {
            return Score;
        }
        

    }
}
