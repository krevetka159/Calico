using System;
using System.Collections.Generic;
using System.Drawing;
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

        public Dictionary<Color, int> buttons;
        public int rainbowButtons { get; private set; }
        public Dictionary<PatternScoringPanel, int> cats; 

        public Scoring Scoring { get; private set; }

        public ScoreCounter(Scoring scoring) 
        {
            
            Score = 0;
            Scoring = scoring;
            colorUF = new UnionFind<GamePiece>();
            patternUF = new UnionFind<GamePiece>();

            buttons = new Dictionary<Color, int>(){ 
                { Color.Yellow, 0 },
                { Color.Green, 0 },
                { Color.Cyan, 0 },
                { Color.Blue, 0 },
                { Color.Purple, 0 },
                { Color.Pink, 0 },
            };
            rainbowButtons = 0;

            cats = new Dictionary<PatternScoringPanel, int>();
            foreach (PatternScoringPanel panel in Scoring.PatternScoring.PatternScoringPanels)
            {
                cats[panel] = 0;
            }
            
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
        /// Checks the sizes of neighbours clusters for both color and pattern
        /// </summary>
        /// <param name="p"></param>
        /// <param name="neighbors"></param>
        public void EvaluateNew(GamePiece p, List<GamePiece> neighbors)
        {
            bool possible_button = true;
            bool possible_cat = true;

            foreach (GamePiece n in neighbors)
            {
                if (p.Color == n.Color)
                {
                    if (GetColorCount(n) >= Scoring.ColorScoring.ClusterSize) 
                    {
                        possible_button = false;
                    }
                }

                if (p.Pattern == n.Pattern)
                {
                    if (GetPatternCount(n) >= Scoring.PatternScoring.PatternScoringDict[n.Pattern].ClusterSize)
                    {
                        possible_cat = false;
                    }
                }
            }

            foreach (GamePiece n in neighbors)
            {
                if (p.Color == n.Color)  colorUF.Union(n, p);
                if (p.Pattern == n.Pattern)  patternUF.Union(n, p);
            }

            if(possible_button && GetColorCount(p) >= Scoring.ColorScoring.ClusterSize)
            {
                Score += Scoring.ColorScoring.Points;
                AddAndUpdateButtons(p.Color);
            }
            if (possible_cat && GetPatternCount(p) >= Scoring.PatternScoring.PatternScoringDict[p.Pattern].ClusterSize)
            {
                Score += Scoring.PatternScoring.PatternScoringDict[p.Pattern].Points;
                AddCat(p.Pattern);
            }

        }

        public void EvaluateTask(TaskPiece task)
        {
            Score += task.Evaluate();
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
            return (colorUF.Count(p) /Scoring.ColorClusterSize) * Scoring.ColorClusterScore;
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
            return (patternUF.Count(p) / Scoring.PatternClusterSizes[p.Pattern]) * Scoring.PatternClusterScores[p.Pattern];
        }

        /// <summary>
        /// Count color score from the number of tiles in a cluster
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public int CountColorScore(int count)
        {
            return (count / Scoring.ColorClusterSize) * Scoring.ColorClusterScore;
        }

        /// <summary>
        /// Count pattern score from the number of tiles in a cluster
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public int CountPatternScore(int count, Pattern p) 
        {
            return (count / Scoring.PatternClusterSizes[p]) * Scoring.PatternClusterScores[p];
        }


        /// <summary>
        /// Returns the score value
        /// </summary>
        /// <returns></returns>
        public int GetScore()
        {
            return Score;
        }

        /// <summary>
        /// Checks whether a button of given color would gain a rainbow button
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool GetsRainbowButton(Color c)
        {
            int min = buttons.Values.Min();
            foreach (KeyValuePair<Color,int> pair in buttons)
            {
                if (pair.Key == c)
                {
                    if (pair.Value != min) return false;
                }
                else
                {
                    if (pair.Value <= min) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Updates buttons dictionary while adding a button of a given color.
        /// </summary>
        /// <param name="c"></param>
        public void AddAndUpdateButtons(Color c)
        {
            if (GetsRainbowButton(c))
            {
                Score += Scoring.ColorClusterScore;
                rainbowButtons += 1;
            }
            buttons[c] += 1;
        }
        
        public void AddCat(Pattern p)
        {
            cats[Scoring.PatternScoring.PatternScoringDict[p]] += 1;
        }

        public int GetButtonsCount()
        {
            return buttons.Values.Sum() + rainbowButtons;
        }

        public (int, int, int) GetCatsCount()
        {
            return (cats[Scoring.PatternScoring.PatternScoringPanels[0]], cats[Scoring.PatternScoring.PatternScoringPanels[1]], cats[Scoring.PatternScoring.PatternScoringPanels[2]]);
        }
    }
}
