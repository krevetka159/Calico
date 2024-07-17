using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    /// <summary>
    /// In game scoring counter
    /// </summary>
    public class ScoreCounter
    {
        public int Score { get; private set; }

        private UnionFind<GamePiece> colorUF;
        private UnionFind<GamePiece> patternUF;

        public int[] ButtonsCount { get; private set; }
        public int RainbowButtonsCount { get; private set; }
        
        public int[] CatsCount { get; private set; }

        public Scoring Scoring { get; private set; }

        public ScoreCounter(Scoring scoring) 
        {
            
            Score = 0;
            Scoring = scoring;
            colorUF = new UnionFind<GamePiece>();
            patternUF = new UnionFind<GamePiece>();

            ButtonsCount = new int[6] {0,0,0,0,0,0};
            RainbowButtonsCount = 0;

            CatsCount = new int[3];
            foreach (PatternScoringPanel panel in Scoring.PatternScoring.PatternScoringPanels)
            {
                CatsCount[panel.Id] = 0;
            }
            
        }

        public ScoreCounter(ScoreCounter sc) // Copy constructor for simulations
        {

            Score = sc.Score;
            Scoring = sc.Scoring;
            colorUF = new UnionFind<GamePiece>(sc.colorUF);
            patternUF = new UnionFind<GamePiece>(sc.patternUF);

            ButtonsCount = (int[])sc.ButtonsCount.Clone();
            RainbowButtonsCount = sc.RainbowButtonsCount;

            CatsCount = (int[])sc.CatsCount.Clone();

        }

        /// <summary>
        /// Gets id of a cluster from pattern UnionFind containing the patchtile
        /// </summary>
        /// <param name="gp"></param>
        /// <returns></returns>
        public int GetPatternClusterId(GamePiece gp)
        {
            return patternUF.GetClusterId(gp);
        }

        /// <summary>
        /// Gets id of a cluster from color UnionFind containing the patchtile
        /// </summary>
        /// <param name="gp"></param>
        /// <returns></returns>
        public int GetColorClusterId(GamePiece gp)
        {
            return colorUF.GetClusterId(gp);
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
                    if (GetPatternCount(n) >= Scoring.PatternScoring.PatternScoringDict[(int)n.Pattern-1].ClusterSize)
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
            if (possible_cat && GetPatternCount(p) >= Scoring.PatternScoring.PatternScoringDict[(int)p.Pattern-1].ClusterSize)
            {
                Score += Scoring.PatternScoring.PatternScoringDict[(int)p.Pattern-1].Points;
                AddCat(p.Pattern);
            }

        }

        /// <summary>
        /// Adds score from a task to the total score
        /// </summary>
        /// <param name="task"></param>
        public void EvaluateTask(TaskPiece task)
        {
            Score += task.Evaluate();
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
        /// Returns size of pattern cluster that the patchtile is a part of
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int GetPatternCount(GamePiece p)
        {
            return patternUF.Count(p);
        }

        /// <summary>
        /// Returns size of pattern cluster with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetPatternCount(int id)
        {
            return patternUF.Count(id);
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
            int min = ButtonsCount.Min();
            for (int i = 0; i<ButtonsCount.Length; i++)
            {
                if (i == (int)c-1)
                {
                    if (ButtonsCount[i] != min) return false;
                }
                else
                {
                    if (ButtonsCount[i] <= min) return false;
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
                RainbowButtonsCount += 1;
            }
            ButtonsCount[(int)c-1] += 1;
        }
        
        /// <summary>
        /// Updates cats dictionary while adding a cat for a given pattern.
        /// </summary>
        /// <param name="p"></param>
        public void AddCat(Pattern p)
        {
            CatsCount[Scoring.PatternScoring.PatternScoringDict[(int)p-1].Id] += 1;
        }

        /// <summary>
        /// Returns the number of buttons
        /// </summary>
        /// <returns></returns>
        public int GetButtonsCount()
        {
            return ButtonsCount.Sum() + RainbowButtonsCount;
        }

        /// <summary>
        /// Returns the number of cat tokens
        /// </summary>
        /// <returns></returns>
        public (int, int, int) GetCatsCount()
        {
            return (CatsCount[0], CatsCount[1], CatsCount[2]);
        }
    }
}
