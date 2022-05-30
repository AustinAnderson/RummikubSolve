using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunsRainbowTableGenerator.Logic;
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
            var runFinder = new RunFinder(new RunSolver());
            var testTileSet=new TestTileSet();
            var tiles = new[]
            {
                "1Th", "3Bh", "3Rh", "6Bb", "6Yb", "9Bh", "9Rh", "ABb", "AYb",
                "4Bb", "4Rb", "4Th", "4Yh", "5Rb", "5Bb", "5Yh", "BBb", "BYb", "CYb*"
            }.Select(x => testTileSet.MakeTile(x, includeId: false)).ToArray();
            var results= runFinder.FindRuns(tiles);
            var found=results.Runs.Select(x=>string.Join(",",x.Select(y=>y.ToString()))).ToArray();
            var expectedRuns = new[]
            {
                new[]{ "3Rh","4Rb","5Rb"},
                new[]{ "4Yh","5Yh","6Yb"},
                new[]{ "AYb","BYb","CYb*"},
                new[]{ "3Bh","4Bb","5Bb","6Bb"},
                new[]{ "9Bh","ABb","BBb"}

            }.Select(a=>string.Join(",",a.Select(x=>testTileSet.MakeTile(x,includeId: false)))).ToArray();
            foreach(var expectedRun in expectedRuns)
            {
                Assert.IsTrue(found.Contains(expectedRun),$"found is missing '{expectedRun}'");
            }
            var expectedHand= new[]
            {
                testTileSet.MakeTile("1Th",includeId: false),
                testTileSet.MakeTile("4Th",includeId: false),
                testTileSet.MakeTile("9Rh",includeId: false)
            };
            Assert.AreEqual(
                string.Join(",", expectedHand.Select(x=>x.ToString())), 
                string.Join(",", results.Remainder.Select(x=>x.ToString()))
            );
        }
    }
}
