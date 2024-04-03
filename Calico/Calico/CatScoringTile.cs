using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    internal class CatScoringTile
    {
        public int Points;
        public int ClusterSize;
        public List<Pattern> Patterns;

        public CatScoringTile(int id, Pattern p1, Pattern p2) 
        {
            Patterns = new List<Pattern>() { p1, p2 };
            switch (id)
            {
                case 1:
                    Points = 3;
                    ClusterSize = 3;
                    break;
                case 2:
                    Points = 5;
                    ClusterSize = 4;
                    break;
                case 3:
                    Points = 7;
                    ClusterSize = 5;
                    break;
            }

        }
    }
}
