using Calico;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace CalicoTests
{
    [TestClass]
    public class UnionFindTests
    {
        [TestMethod]
        public void Add()
        {
            UnionFindWithArray<string> uf = new();
            uf.Add("A");
            uf.Add("B");
            string output = uf.ToString();

            string expected = "A: 1\nB: 2\n";

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void Union()
        {
            UnionFindWithArray<string> uf = new();
            uf.Add("A");
            uf.Add("B");
            uf.Union("A", "B");
            string output = uf.ToString();

            string expected = "A: 1\nB: 1\n";

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void FindConnected()
        {
            UnionFindWithArray<string> uf = new();
            uf.Add("A");
            uf.Add("B");
            uf.Union("A", "B");

            Assert.IsTrue(uf.Find("A", "B"));

        }

        [TestMethod]
        public void FindNonexistent()
        {
            UnionFindWithArray<string> uf = new();
            uf.Add("A");

            Assert.IsTrue(!(uf.Find("A", "B")));
        }
        public void FindNotConnected()
        {
            UnionFindWithArray<string> uf = new();
            uf.Add("A");
            uf.Add("B");

            Assert.IsTrue(!(uf.Find("A", "B")));
        }

        [TestMethod]
        public void Count()
        {
            UnionFindWithArray<int> uf = new();
            for (int i = 0; i < 10; i++)
            {
                uf.Add(i);
            }
            for (int i = 1; i < 10; i++)
            {
                uf.Union(0,i);
            }
            Assert.AreEqual(uf.Count(0), 10);
        }
    }
}