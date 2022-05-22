using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLogic;
using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TestRunCalc
    {
        
        [TestMethod]
        public void ValidIfHandUnused()
        {
            var testTileSet=new TestTileSet();
            var baseUnused=new[] {
                "1Th", "3Bh", "3Rh", "6Bb", "6Yb", "9Bh", "9Rh", "ABb", "AYb"
            }.Select(x=>testTileSet.MakeFastCalcTile(x)).ToArray();
            var unusedCalc = new[] {//                                   V valid because would be left over, but originally came from the hand
                "4Bb", "4Rb", "4Th", "4Yh", "5Bb", "5Rb", "5Yh", "BBb", "BRh", "BYb", "CYb*",
                "9Bh", "9Bh", "9Bh", "9Bh", "9Bh",
            }.Select(x=>testTileSet.MakeFastCalcTile(x)).ToArray();
            var arr = new UnusedFastCalcArray
            {
                Set = unusedCalc,
                Count = 11
            };
            int currentScore = RunScorer.Score(baseUnused, ref arr);
            Assert.AreEqual(4, currentScore, "score should be 4");
        }
        [TestMethod]
        public void ValidIfBoardUnusedWithHandEquivalent()
        {
            var testTileSet=new TestTileSet();
            var baseUnused=new[] {
                "1Th", "3Bh", "3Rh", "6Bb", "6Yb", "9Bh", "9Rh", "ABb", "AYb"
            }.Select(x=>testTileSet.MakeFastCalcTile(x)).ToArray();
            var unusedCalc = new[] {//                                   V? valid because would be left over but can be subbed for one in the hand
                "4Bb", "4Rb", "4Th", "4Yh", "5Bb", "5Rb", "5Yh", "BBb", "BRb*", "BYb", "CYb*",
                "9Bh", "9Bh", "9Bh", "9Bh", "9Bh",
            }.Select(x=>testTileSet.MakeFastCalcTile(x)).ToArray();
            var arr = new UnusedFastCalcArray
            {
                Set = unusedCalc,
                Count = 11
            };
            int currentScore = RunScorer.Score(baseUnused, ref arr);
            Assert.AreEqual(4, currentScore, "score should be 4");
        }
        [TestMethod]
        public void InvalidIfBoardUnused()
        {
            var testTileSet=new TestTileSet();
            var baseUnused=new[] {
                "1Th", "3Bh", "3Rh", "6Bb", "6Yb", "9Bh", "9Rh", "ABb", "AYb"
            }.Select(x=>testTileSet.MakeFastCalcTile(x)).ToArray();
            var unusedCalc = new[] {//                                   V invalid because would be left over, and originally came from the board
                "4Bb", "4Rb", "4Th", "4Yh", "5Bb", "5Rb", "5Yh", "BBb", "BRb", "BYb", "CYb*",
                "9Bh", "9Bh", "9Bh", "9Bh", "9Bh",
            }.Select(x=>testTileSet.MakeFastCalcTile(x)).ToArray();
            var arr = new UnusedFastCalcArray
            {
                Set = unusedCalc,
                Count = 11
            };
            int currentScore = RunScorer.Score(baseUnused, ref arr);
            Assert.AreEqual(int.MaxValue, currentScore, "score should be max int because it leaves unused board tiles");

        }
        [TestMethod]
        public void TestThatOneCaseThatFailed()
        {
            var testTileSet=new TestTileSet();
            var baseUnused = new[] {
                "1Th",  "3Bh", "3Rh", "6Bb", "6Yb", "9Bh", "9Rh", "ABb", "AYb"
            }.Select(x => testTileSet.MakeFastCalcTile(x)).ToArray();
            var unusedCalc = new[] {
                "1Rh", "2Bb", "2Rb", "2Yh", "3Bb*", "3Rb*", "3Yh",
                "4Bb", "4Bb", "4Rb", "4Th", "4Yh", "5Bb", "5Yh",
                "7Bh", "7Th", "7Yb", "8Bb", "8Rh", "8Yb",
                "9Bb*", "9Rh", "9Yb", "CYb*",

                "9Bb*", "9Rh", "9Yb", "CYb*"
            }.Select(x => testTileSet.MakeFastCalcTile(x)).ToArray();
            var arr = new UnusedFastCalcArray
            {
                Set = unusedCalc,
                Count = 24
            };
            int currentScore = RunScorer.Score(baseUnused, ref arr);
            Assert.AreEqual(15, currentScore, "score should be 15");
        }
    }
}
