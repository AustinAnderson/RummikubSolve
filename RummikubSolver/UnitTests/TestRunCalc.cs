using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLogic;
using SolverLogic.Models;
using System.Collections.Specialized;
using System.Linq;
using UnitTests;

namespace TestRunCalc
{
    public static class RunCalcTestUtil
    {
        public static void AssertCorrectScoreFound(int expectedScore, string[] baseUnused, string[] unusedCalc)
        {
            var testTileSet=new TestTileSet();
            var arr = new UnusedFastCalcArray
            {
                Set = unusedCalc.Concat(new[] { 
                    "9Bh_0", "9Bh_0", "9Bh_0", "9Bh_0", "9Bh_0",//simulated arbitrary trash from last add try: trimmed out
                }).Select(x=>testTileSet.MakeFastCalcTile(x)).ToArray(),
                Count = unusedCalc.Length,
            };
            int currentScore = 0; //new RunScorer().Score(baseUnused.Select(x=>testTileSet.MakeFastCalcTile(x)).ToArray(), ref arr);
            Assert.AreEqual(expectedScore, currentScore, $"score should be {expectedScore}");

        }
    }
    [TestClass]
    public class TestHandOrBoardValidity
    {
        
        [TestMethod]
        public void ValidIfHandUnused()
        {
            RunCalcTestUtil.AssertCorrectScoreFound(4,
                new[] { "1Th_0", "3Bh_0", "3Rh_0", "6Bb_0", "6Yb_0", "9Bh_0", "9Rh_0", "ABb_0", "AYb_0" },
                //                      valid because would be left over but came from the hand
                new[] {//                                                                     |
                    "4Bb_0", "4Rb_0", "4Th_0", "4Yh_0", "5Bb_0", "5Rb_0", "5Yh_0", "BBb_0", "BRh_0", "BYb_0", "CYb*_0",
                }
            );
        }
        [TestMethod]
        public void ValidIfBoardUnusedWithHandEquivalent()
        {
            RunCalcTestUtil.AssertCorrectScoreFound(4,
                new[] { "1Th_0", "3Bh_0", "3Rh_0", "6Bb_0", "6Yb_0", "9Bh_0", "9Rh_0", "ABb_0", "AYb_0" },
                //       valid because would be left over but can be subbed for one in the hand
                new[] {//                                                                     |
                    "4Bb_0", "4Rb_0", "4Th_0", "4Yh_0", "5Bb_0", "5Rb_0", "5Yh_0", "BBb_0", "BRb*0", "BYb_0", "CYb*_0",
                }
            );
        }
        [TestMethod]
        public void InvalidIfBoardUnused()
        {
            RunCalcTestUtil.AssertCorrectScoreFound(int.MaxValue,
                new[] { "1Th_0", "3Bh_0", "3Rh_0", "6Bb_0", "6Yb_0", "9Bh_0", "9Rh_0", "ABb_0", "AYb_0" },
                //        invalid because would be left over, and originally came from the board
                new[] {//                                                                      |
                    "4Bb_0", "4Rb_0", "4Th_0", "4Yh_0", "5Bb_0", "5Rb_0", "5Yh_0", "BBb_0", "BRb_0", "BYb_0", "CYb*_0",
                }
            );
        }
        [TestMethod]
        public void TestThatOneCaseThatFailed()
        {
            //B (2 3 4 5 6 7 8 9 A) (3 4) (9)
            //R (1 2 3 4) (8 9) (3) (9)
            //T (1) (4) (7) 
            //Y (2 3 4 5 6 7 8 9 A) (C)
            RunCalcTestUtil.AssertCorrectScoreFound(11,
                new[] { "1Th_0", "3Bh_0", "3Rh_0", "6Bb_0", "6Yb_0", "9Bh_0", "9Rh_0", "ABb_0", "AYb_0" },
                new[] {
                    "1Rh_0", "2Bb_0", "2Rb_0", "2Yh_0", "3Yh_0",
                    "4Bb_0", "4Rb_0", "4Th_0", "4Yh_0", "5Bb_0", "5Yh_0",
                    "7Bh_0", "7Th_0", "7Yb_0", "8Bb_0", "8Rh_0", "8Yb_0",
                    "9Yb_0", "CYb*0",
                    "3Bb*1", "3Rb*1", "4Bb_1", "9Bb*1", "9Rh_1",
                }
            );
        }
    }
    [TestClass]
    public class Test2Dups
    {
        [TestMethod]
        public void DupsHigh()
        {
            //2B 3B
            //4B 5B 6B
            RunCalcTestUtil.AssertCorrectScoreFound(2,
                new[] { "2Bb_0", "3Bb_0", "4Bb_0", "5Bb*0", "6Bb*0"  },
                new[] { "5Bh_1", "6Bh_1" }
            );
        }
        [TestMethod]
        public void DupsMid()
        {
            //2B 3B 4B 
            //3B 4B 5B 
            RunCalcTestUtil.AssertCorrectScoreFound(0,
                new[] { "2Bb_0", "3Bb*0", "4Bb*0", "5Bb_0" },
                new[] { "3Bh_1", "4Bh_1" }
            );
        }
        [TestMethod]
        public void DupsLow()
        {
            //2B 3B 
            //2B 3B 4B 5B 6B
            RunCalcTestUtil.AssertCorrectScoreFound(2,
                new[] { "2Bb*0", "3Bb*0", "4Bb_0", "5Bb_0", "6Bb_0" },
                new[] { "2Bh_1", "3Bh_1" }
            );
        }
    }
    [TestClass]
    public class Test3Dups
    {
        [TestMethod]
        public void DupsHigh()
        {
            RunCalcTestUtil.AssertCorrectScoreFound(0,
                new[] { "2Bb_0", "3Bb_0", "4Bb_0", "5Bb*0", "6Bb*0"  },
                new[] { "4Bh_1", "5Bh_1", "6Bh_1" }
            );
        }
        [TestMethod]
        public void DupsMid()
        {
            //1B 2B 3B 4B 
            //3B 4B 5B 6B
            RunCalcTestUtil.AssertCorrectScoreFound(0,
                new[] { "1Bh_0", "2Bb_0", "3Bb*0", "4Bb*0", "5Bb_0", "6Bb_0"  },
                new[] { "2Bh_1", "3Bh_1", "4Bh_1" }
            );
        }
        [TestMethod]
        public void DupsLow()
        {
            //should get
            //2B 3B
            //5B
            //2B 3B 4B 5B 6B

            //but will get
            //2B 3B 4B 5B
            //2B 3B
            //5B 6B
            RunCalcTestUtil.AssertCorrectScoreFound(3,
                new[] { "2Bb*0", "3Bb*0", "4Bb_0", 
                    "5Bb*0", "6Bb_0" },
                new[] { "2Bh_1", "3Bh_1", 
                    "5Bh_1"}
            );
            //just trying all combinations of 3 partions with mini's no good since
            // 1 23 4   wouldn't be hit
        }
    }
}
