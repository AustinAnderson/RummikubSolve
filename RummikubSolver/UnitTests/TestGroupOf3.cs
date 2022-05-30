using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLogic.Models;
using System.Collections.Generic;
using System.Linq;
using UnitTests;

namespace TestMaxGroupIteration
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
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,0);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(selected, group,0);
        }
        [TestMethod]
        public void AllOption()
        {
            var tiles = "3B,3R,3T".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            var selected = tiles;
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,1);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(selected, group,1);
        }
    }
}