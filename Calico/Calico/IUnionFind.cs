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

    public class UnionFindWithArray<T> : IUnionFind<T>
    {
        private int _lastCluster;
        private Dictionary<T, int> _clusters = new();

        public UnionFindWithArray()
        {
        }

        public void Add(T x)
        {
            _clusters.Add(x, ++_lastCluster);
        }

        public bool Find(T x, T y)
        {
            return
                _clusters.ContainsKey(x) &&
                _clusters.ContainsKey(y) &&
                _clusters[x] == _clusters[y];
        }

        public int CountScore(T x)
        {
            int xCluster = _clusters[x];
            int cnt = 0;
            foreach ((T z, int zCluster) in _clusters)
            {
                if (zCluster == xCluster)
                {
                    cnt++;
                }
            }

            return cnt;
        }

        public int CountScore(int xCluster)
        {
            int cnt = 0;
            foreach ((T z, int zCluster) in _clusters)
            {
                if (zCluster == xCluster)
                {
                    cnt++;
                }
            }

            return cnt;
        }

        public void Union(T x, T y)
        {
            if (!_clusters.ContainsKey(x)) throw new ArgumentException($"{x} not found", nameof(x));
            if (!_clusters.ContainsKey(y)) throw new ArgumentException($"{y} not found", nameof(y));


            int xCluster = _clusters[x];
            int yCluster = _clusters[y];


            foreach ((T z, int zCluster) in _clusters)
            {
                if (zCluster == yCluster)
                {
                    _clusters[z] = xCluster;
                }
            }
        }

        public int GetCluster(T x)
        {
            return _clusters[x];
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach ((T z, int zColor) in _clusters)
            {
                sb.Append($"{z}: {zColor}");
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }


    }

    //internal class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        UnionFindWithArray<string> uf = new();
    //        uf.Add("David");
    //        uf.Add("Dita");
    //        uf.Add("Petr");
    //        uf.Add("Lucka");
    //        uf.Add("Jelinek");
    //        uf.Add("Barca");
    //        uf.Add("Opler");
    //        uf.Add("Balko");
    //        uf.Add("Vojta");
    //        Console.WriteLine(uf.ToString());

    //        uf.Union("Dita", "Vojta");
    //        uf.Union("Vojta", "Lucka");
    //        uf.Union("Dita", "David");
    //        uf.Union("David", "Petr");
    //        Console.WriteLine(uf.ToString());

    //        uf.Union("David", "Balko");
    //        uf.Union("Jelinek", "Barca");
    //        Console.WriteLine(uf.ToString());

    //        //uf.Union("Petr", "Kaja"); // Vyhodim chybu, Kaja neexistuje

    //    }
    //}
}
