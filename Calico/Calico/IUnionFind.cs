using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    interface IUnionFind<T> //implementace podle Marešova průvodce
    {
        void Add(T x);
        bool Find(T x, T y);
        void Union(T x, T y);
    }

    public class UnionFind<T> : IUnionFind<T>
    {
        private int lastCluster;
        private Dictionary<T, int> clusters = new();

        public UnionFind()
        {
        }

        public UnionFind(UnionFind<T> uf)
        {
            lastCluster = uf.lastCluster;
            clusters = new Dictionary<T,int>(uf.clusters);
        }

        public void Add(T x)
        {
            clusters.Add(x, ++lastCluster);
        }

        public bool Find(T x, T y)
        {
            return
                clusters.ContainsKey(x) &&
                clusters.ContainsKey(y) &&
                clusters[x] == clusters[y];
        }

        public bool Contains(T x)
        {
            return clusters.ContainsKey(x);
        }

        public int Count(T x)
        {
            int xCluster = clusters[x];
            int cnt = 0;
            foreach ((T z, int zCluster) in clusters)
            {
                if (zCluster == xCluster)
                {
                    cnt++;
                }
            }

            return cnt;
        }

        public List<T> GetCluster(T x)
        {
            int xCluster = clusters[x];
            List<T> cluster = new List<T>();
            foreach ((T z, int zCluster) in clusters)
            {
                if (zCluster == xCluster)
                {
                    cluster.Add(z);
                }
            }
            return cluster;
        }

        public void Union(T x, T y)
        {
            if (!clusters.ContainsKey(x)) throw new ArgumentException($"{x} not found", nameof(x));
            if (!clusters.ContainsKey(y)) throw new ArgumentException($"{y} not found", nameof(y));


            int xCluster = clusters[x];
            int yCluster = clusters[y];


            foreach ((T z, int zCluster) in clusters)
            {
                if (zCluster == yCluster)
                {
                    clusters[z] = xCluster;
                }
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new();
            foreach ((T z, int zColor) in clusters)
            {
                sb.Append($"{z}: {zColor}");
                sb.Append("\n");
            }
            return sb.ToString();
        }


    }


  
}
