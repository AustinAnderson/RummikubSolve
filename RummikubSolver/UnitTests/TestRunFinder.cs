using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TestRunFinder
    {
        [TestMethod]
        public void TestRunFound()
        {
            var tiles= new[]
            {
                RunTestUtil.MakeFastCalcTile("1Th"),
                RunTestUtil.MakeFastCalcTile("3Bh"),
                RunTestUtil.MakeFastCalcTile("3Rh"),
                RunTestUtil.MakeFastCalcTile("6Bb"),
                RunTestUtil.MakeFastCalcTile("6Yb"),
                RunTestUtil.MakeFastCalcTile("9Bh"),
                RunTestUtil.MakeFastCalcTile("9Rh"),
                RunTestUtil.MakeFastCalcTile("ABb"),
                RunTestUtil.MakeFastCalcTile("AYb"),

                RunTestUtil.MakeFastCalcTile("4Bb"),
                RunTestUtil.MakeFastCalcTile("4Rb"),
                RunTestUtil.MakeFastCalcTile("4Th"),
                RunTestUtil.MakeFastCalcTile("4Yh"),
                RunTestUtil.MakeFastCalcTile("5Rb"),
                RunTestUtil.MakeFastCalcTile("5Bb"),
                RunTestUtil.MakeFastCalcTile("5Yh"),
                RunTestUtil.MakeFastCalcTile("BBb"),
                RunTestUtil.MakeFastCalcTile("BYb"),
                RunTestUtil.MakeFastCalcTile("CYb*")
            };
            var results= RunFinder.FindRuns(tiles);
            var found=results.Runs.Select(x=>string.Join(",",x)).ToArray();
            var expectedRuns = new[]
            {
                new[]{ "3Rh","4Rb","5Rb"},
                new[]{ "4Yh","5Yh","6Yb"},
                new[]{ "AYb","BYb","CYb*"},
                new[]{ "3Bh","4Bb","5Bb","6Bb"},
                new[]{ "9Bh","ABb","BBb"}

            }.Select(a=>string.Join(",",a.Select(x=>RunTestUtil.MakeFastCalcTile(x)))).ToArray();
            foreach(var expectedRun in expectedRuns)
            {
                Assert.IsTrue(found.Contains(expectedRun),$"found is missing '{expectedRun}'");
            }
            var expectedHand= new[]
            {
                RunTestUtil.MakeFastCalcTile("1Th"),
                RunTestUtil.MakeFastCalcTile("4Th"),
                RunTestUtil.MakeFastCalcTile("9Rh")
            };
            Assert.AreEqual(string.Join(",", expectedHand), string.Join(",", results.Remainder));
        }
    }
}
