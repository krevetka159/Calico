using Calico;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace CalicoTests
{
    [TestClass]
    public class GameBoardTests
    {
        [TestMethod]
        public void Add()
        {
            GameBoard board = new GameBoard(new Scoring(), 0);
            GamePiece gp1 = new GamePiece(Color.Yellow, Pattern.Stripes);
            GamePiece gp2 = new GamePiece(Color.Yellow, Pattern.Dots);
            board.AddPiece(gp1, 1, 1);

            Assert.IsTrue(board.IsOccupied(1,1));
        }
        [TestMethod]
        public void ColorClusterScore()
        {
            GameBoard board = new GameBoard(new Scoring(), 0);
            GamePiece gp1 = new GamePiece(Color.Yellow, Pattern.Stripes);
            GamePiece gp2 = new GamePiece(Color.Yellow, Pattern.Dots);
            board.AddPiece(gp1, 1, 1);
            board.AddPiece(gp2, 1, 2);
            Assert.AreEqual(board.ScoreCounter.GetScore(), 3);
        }

        [TestMethod]
        public void PatternClusterScore()
        {
            GameBoard board = new GameBoard(new Scoring(), 0);
            Pattern p = Pattern.Flowers;
            
            for (int i = 0; i < board.ScoreCounter.Scoring.pcSizes[(int)p-1] - 1; i++){
                board.AddPiece(new GamePiece((Color)(i+1), p), 1, i+1);
            }
            
            Assert.AreEqual(board.ScoreCounter.GetScore(), board.ScoreCounter.Scoring.pcScores[(int)p-1]);
        }

        [TestMethod]
        public void FindColorNeighbor()
        {
            GameBoard board = new GameBoard(new Scoring(), 0);
            GamePiece gp1 = new GamePiece(Color.Yellow, Pattern.Dots);

            board.AddPiece(gp1, 1, 1);

            Assert.IsTrue(board.CheckNeighborsColor(Color.Yellow,1, 1));
        }
        [TestMethod]
        public void FindPatternNeighbor()
        {
            GameBoard board = new GameBoard(new Scoring(), 0);
            GamePiece gp1 = new GamePiece(Color.Purple, Pattern.Flowers);

            board.AddPiece(gp1, 1, 1);

            Assert.IsTrue(board.CheckNeighborsPattern(Pattern.Flowers, 1, 1));
        }

        [TestMethod]
        public void EvaluateColorNeighbor0()
        {
            GameBoard board = new GameBoard(new Scoring(), 0);
            GamePiece gp1 = new GamePiece(Color.Purple, Pattern.Dots);


            Assert.AreEqual(board.EvaluateNeighborsColor(gp1, 1, 1), 0);
        }

        [TestMethod]
        public void EvaluatePatternNeighbor0()
        {
            GameBoard board = new GameBoard(new Scoring(), 0);
            GamePiece gp1 = new GamePiece(Color.Purple, Pattern.Dots);


            Assert.AreEqual(board.EvaluateNeighborsPattern(gp1, 1, 1), 0);
        }

        [TestMethod]
        public void EvaluateColorNeighbor1()
        {
            GameBoard board = new GameBoard(new Scoring(), 0);
            GamePiece gp1 = new GamePiece(Color.Yellow, Pattern.Dots);


            Assert.AreEqual(board.EvaluateNeighborsColor(gp1, 1, 1), 1);
        }

        [TestMethod]
        public void EvaluatePatternNeighbor1()
        {
            GameBoard board = new GameBoard(new Scoring(), 0);
            GamePiece gp1 = new GamePiece(Color.Purple, Pattern.Flowers);


            Assert.AreEqual(board.EvaluateNeighborsPattern(gp1, 1, 1), 1);
        }

        [TestMethod]
        public void EvaluateColorNeighbor2()
        {
            GameBoard board = new GameBoard(new Scoring(), 0);
            GamePiece gp1 = new GamePiece(Color.Yellow, Pattern.Dots);

            board.AddPiece(new GamePiece(Color.Yellow, Pattern.Flowers), 1, 2);

            Assert.AreEqual(board.EvaluateNeighborsColor(gp1, 1, 1), 4);
        }

        [TestMethod]
        public void EvaluatePatternNeighbor2()
        {
            GameBoard board = new GameBoard(new Scoring(), 0);
            GamePiece gp1 = new GamePiece(Color.Purple, Pattern.Flowers);


            for (int i = 0; i < board.ScoreCounter.Scoring.pcSizes[(int)Pattern.Flowers-1] - 2; i++)
            {
                board.AddPiece(new GamePiece((Color)(i+1), Pattern.Flowers), 1, i + 2);
            }

            Assert.AreEqual(board.EvaluateNeighborsPattern(gp1, 1, 1), board.ScoreCounter.Scoring.pcScores[(int)Pattern.Flowers-1] + 1);
        }
    }
}
