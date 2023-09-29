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

        private UnionFind<GamePiece> colorUF;
        private UnionFind<GamePiece> patternUF;

        private Scoring scoring;

        public ScoreCounter(Scoring scoring) 
        {
            
            Score = 0;
            this.scoring = scoring;
            colorUF = new UnionFind<GamePiece>();
            patternUF = new UnionFind<GamePiece>();

        }

        /// <summary>
        /// Adds patchtile to color and pattern UnionFinds
        /// </summary>
        /// <param name="piece"></param>
        public void AddToUF(GamePiece piece)
        {
            patternUF.Add(piece);
            colorUF.Add(piece);
        }

        /// <summary>
        /// Checks if 2 patchtiles have the same color/pattern and unites them in UnionFind if needed
        /// </summary>
        /// <param name="p"></param>
        /// <param name="n"></param>
        public void EvaluateNew(GamePiece p, GamePiece n)
        {
            if (p.Color == n.Color)
            {

                if (!colorUF.Find(p, n))
                {
                    Score -= GetColorScore(n);
                    Score -= GetColorScore(p);

                    colorUF.Union(n, p);

                    Score += GetColorScore(n);
                }

            }
            if (p.Pattern == n.Pattern)
            {
                if (!patternUF.Find(p, n))
                {
                    Score -= GetPatternScore(n);
                    Score -= GetPatternScore(p);

                    patternUF.Union(n, p);

                    Score += GetPatternScore(n);
                }

            }

        }
        /// <summary>
        /// Checks whether two patchtiles are in the same color cluster
        /// </summary>
        /// <param name="p"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool CheckColorUnion(GamePiece p, GamePiece n)
        {
            return colorUF.Find(p, n);
        }

        /// <summary>
        /// Checks whether two patchtiles are in the same color cluster
        /// </summary>
        /// <param name="p"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool CheckPatternUnion(GamePiece p, GamePiece n)
        {
            return patternUF.Find(p, n);
        }


        /// <summary>
        /// Returns size of color cluster that the patchtile is a part of
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int GetColorCount(GamePiece p)
        {
            return colorUF.Count(p);
        }

        /// <summary>
        /// Returns scoring of color cluster that the patchtile is a part of
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int GetColorScore(GamePiece p)
        {
            return (colorUF.Count(p) /scoring.ColorClusterSize) * scoring.ColorClusterScore;
        }

        /// <summary>
        /// Returns size of pattern cluster that the patchtile is a part of
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int GetPatternCount(GamePiece p)
        {
            return patternUF.Count(p);
        }

        /// <summary>
        /// Returns scoring of pattern cluster that the patchtile is a part of
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int GetPatternScore(GamePiece p)
        {
            return (patternUF.Count(p) / scoring.PatternClusterSizes[p.Pattern]) * scoring.PatternClusterScores[p.Pattern];
        }

        /// <summary>
        /// Count color score from the number of tiles in a cluster
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public int CountColorScore(int count)
        {
            return (count / scoring.ColorClusterSize) * scoring.ColorClusterScore;
        }

        /// <summary>
        /// Count pattern score from the number of tiles in a cluster
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public int CountPatternScore(int count, Pattern p) 
        {
            return (count / scoring.PatternClusterSizes[p]) * scoring.PatternClusterScores[p];
        }


        /// <summary>
        /// Returns the score value
        /// </summary>
        /// <returns></returns>
        public int GetScore()
        {
            return Score;
        }
        

    }
}
