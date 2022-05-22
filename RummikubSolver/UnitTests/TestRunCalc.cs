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
                "1Th_0", "3Bh_0", "3Rh_0", "6Bb_0", "6Yb_0", "9Bh_0", "9Rh_0", "ABb_0", "AYb_0"
            }.Select(x=>testTileSet.MakeFastCalcTile(x)).ToArray();
            var unusedCalc = new[] {//                                                   V? valid because would be left over but came from the hand
                "4Bb_0", "4Rb_0", "4Th_0", "4Yh_0", "5Bb_0", "5Rb_0", "5Yh_0", "BBb_0", "BRh_0", "BYb_0", "CYb*_0",
                "9Bh_0", "9Bh_0", "9Bh_0", "9Bh_0", "9Bh_0",//simulated arbitrary trash from last add try: trimmed out
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
                "1Th_0", "3Bh_0", "3Rh_0", "6Bb_0", "6Yb_0", "9Bh_0", "9Rh_0", "ABb_0", "AYb_0"
            }.Select(x=>testTileSet.MakeFastCalcTile(x)).ToArray();
            var unusedCalc = new[] {//                                                   V? valid because would be left over but can be subbed for one in the hand
                "4Bb_0", "4Rb_0", "4Th_0", "4Yh_0", "5Bb_0", "5Rb_0", "5Yh_0", "BBb_0", "BRb*0", "BYb_0", "CYb*_0",
                "9Bh_0", "9Bh_0", "9Bh_0", "9Bh_0", "9Bh_0",//simulated arbitrary trash from last add try: trimmed out
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
                "1Th_0", "3Bh_0", "3Rh_0", "6Bb_0", "6Yb_0", "9Bh_0", "9Rh_0", "ABb_0", "AYb_0"
            }.Select(x=>testTileSet.MakeFastCalcTile(x)).ToArray();
            var unusedCalc = new[] {//                                                   V invalid because would be left over, and originally came from the board
                "4Bb_0", "4Rb_0", "4Th_0", "4Yh_0", "5Bb_0", "5Rb_0", "5Yh_0", "BBb_0", "BRb_0", "BYb_0", "CYb*_0",
                "9Bh_0", "9Bh_0", "9Bh_0", "9Bh_0", "9Bh_0",//simulated arbitrary trash from last add try: trimmed out
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
                "1Th_0",  "3Bh_0", "3Rh_0", "6Bb_0", "6Yb_0", "9Bh_0", "9Rh_0", "ABb_0", "AYb_0"
            }.Select(x => testTileSet.MakeFastCalcTile(x)).ToArray();
            var unusedCalc = new[] {
                "1Rh_0", "2Bb_0", "2Rb_0", "2Yh_0", "3Yh_0",
                "4Bb_0", "4Bb_0", "4Rb_0", "4Th_0", "4Yh_0", "5Bb_0", "5Yh_0",
                "7Bh_0", "7Th_0", "7Yb_0", "8Bb_0", "8Rh_0", "8Yb_0",
                "9Yb_0", "CYb*0",
                "3Bb*1", "3Rb*1", "9Bb*1", "9Rh_1", 

                //simulated junk
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
