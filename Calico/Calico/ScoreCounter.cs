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

        //public Dictionary<Color, int> buttons;
        public int[] butts;
        public int rainbowButtons { get; private set; }
        //public Dictionary<PatternScoringPanel, int> cats;
        public int[] catts;

        public Scoring Scoring { get; private set; }

        public ScoreCounter(Scoring scoring) 
        {
            
            Score = 0;
            Scoring = scoring;
            colorUF = new UnionFind<GamePiece>();
            patternUF = new UnionFind<GamePiece>();

            //buttons = new Dictionary<Color, int>(){ 
            //    { Color.Yellow, 0 },
            //    { Color.Green, 0 },
            //    { Color.Cyan, 0 },
            //    { Color.Blue, 0 },
            //    { Color.Purple, 0 },
            //    { Color.Pink, 0 },
            //};

            butts = new int[6] {0,0,0,0,0,0};
            rainbowButtons = 0;

            //cats = new Dictionary<PatternScoringPanel, int>();
            catts = new int[3];
            foreach (PatternScoringPanel panel in Scoring.PatternScoring.ps)
            {
                catts[panel.Id] = 0;
            }
            
        }

        public ScoreCounter(ScoreCounter sc)
        {

            Score = sc.Score;
            Scoring = sc.Scoring;
            colorUF = new UnionFind<GamePiece>(sc.colorUF);
            patternUF = new UnionFind<GamePiece>(sc.patternUF);

            butts = (int[])sc.butts.Clone();
            rainbowButtons = sc.rainbowButtons;

            catts = (int[])sc.catts.Clone();

        }

        public void SetProps(int[] b, int rb, Dictionary<PatternScoringPanel,int> c)
        {
            //foreach(KeyValuePair<Color,int>button in b)
            //{
            //    //buttons[button.Key] = button.Value;
            //    butts[(int)button.Key-1] = button.Value;
            //}
            butts = b;
            rainbowButtons = rb;
            foreach (KeyValuePair<PatternScoringPanel,int> cat in c)
            {
                catts[cat.Key.Id] = cat.Value;
            }
        }

        public bool PatternUFContains(GamePiece gp)
        {
            return patternUF.Contains(gp);
        }

        public bool ColorUFContains(GamePiece gp)
        {
            return colorUF.Contains(gp);
        }

        public List<GamePiece> GetPatternCluster(GamePiece gp)
        {
            return patternUF.GetCluster(gp);
        }

        public List<GamePiece> GetColorCluster(GamePiece gp)
        {
            return colorUF.GetCluster(gp);
        }

        public int GetPatternClusterId(GamePiece gp)
        {
            return patternUF.GetClusterId(gp);
        }

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

        public void AddToPatternUF(List<GamePiece> pieces)
        {
            patternUF.Add(pieces[0]);
            for (int i = 1; i < pieces.Count; i++)
            {
                patternUF.Add(pieces[i]);
                patternUF.Union(pieces[i], pieces[i - 1]);
            }
        }

        public void AddToColorUF(List<GamePiece> pieces)
        {
            colorUF.Add(pieces[0]);
            for (int i = 1; i < pieces.Count; i++)
            {
                colorUF.Add(pieces[i]);
                colorUF.Union(pieces[i], pieces[i - 1]);
            }
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
                    if (GetPatternCount(n) >= Scoring.PatternScoring.psDict[(int)n.Pattern-1].ClusterSize)
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
            if (possible_cat && GetPatternCount(p) >= Scoring.PatternScoring.psDict[(int)p.Pattern-1].ClusterSize)
            {
                Score += Scoring.PatternScoring.psDict[(int)p.Pattern-1].Points;
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

        public int GetColorCount(int p)
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

        public int GetPatternCount(int p)
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
            return (patternUF.Count(p) / Scoring.pcSizes[(int)p.Pattern-1]) * Scoring.pcScores[(int)p.Pattern-1];
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
            return (count / Scoring.pcSizes[(int)p-1]) * Scoring.pcScores[(int)p-1];
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
            int min = butts.Min();
            for (int i = 0; i<butts.Length; i++)
            {
                if (i == (int)c-1)
                {
                    if (butts[i] != min) return false;
                }
                else
                {
                    if (butts[i] <= min) return false;
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
            butts[(int)c-1] += 1;
        }
        
        public void AddCat(Pattern p)
        {
            catts[Scoring.PatternScoring.psDict[(int)p-1].Id] += 1;
        }

        public int GetButtonsCount()
        {
            return butts.Sum() + rainbowButtons;
        }

        public (int, int, int) GetCatsCount()
        {
            return (catts[0], catts[1], catts[2]);
        }
    }
}
