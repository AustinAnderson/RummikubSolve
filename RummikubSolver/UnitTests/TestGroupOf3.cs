using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLogic.Models;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.TestMaxGroupIteration
{
    [TestClass]
    public class TestGroupOf3
    {
        [TestMethod]
        public void NoneOption()
        {
            var tiles = "3B,3R,3T".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            var selected=new List<Tile>();
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.Except(selected), group);
        }
        [TestMethod]
        public void AllOption()
        {
            var tiles = "3B,3R,3T".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            group.MoveNext();
            Assert.IsTrue(group.IsAtLast, "all should be last");
            var selected = tiles;
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.Except(selected), group);
        }
    }
}