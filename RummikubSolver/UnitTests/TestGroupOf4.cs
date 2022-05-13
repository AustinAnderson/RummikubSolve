using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLogic.Models;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.TestMaxGroupIteration
{
    [TestClass]
    public class TestGroupOf4
    {
        [TestMethod]
        public void NoneOption()
        {
            var tiles = "3B,3R,3T,3Y".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            var selected=new List<Tile>();
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
        [TestMethod]
        public void FirstGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            group.MoveNext();
            var selected = tiles.TileExcept("3B");
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
        [TestMethod]
        public void SecondGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            group.MoveNext();
            group.MoveNext();
            var selected = tiles.TileExcept("3R");
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
        [TestMethod]
        public void ThirdGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            group.MoveNext();
            group.MoveNext();
            group.MoveNext();
            var selected = tiles.TileExcept("3T");
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
        [TestMethod]
        public void FourthGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            group.MoveNext();
            group.MoveNext();
            group.MoveNext();
            group.MoveNext();
            var selected = tiles.TileExcept("3Y");
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
        [TestMethod]
        public void AllOption()
        {
            var tiles = "3B,3R,3T,3Y".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            group.MoveNext();
            group.MoveNext();
            group.MoveNext();
            group.MoveNext();
            group.MoveNext();
            Assert.IsTrue(group.IsAtLast, "all should be last");
            var selected = tiles;
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
    }
}