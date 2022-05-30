using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLogic.Models;
using System.Collections.Generic;
using System.Linq;
using UnitTests;

namespace TestMaxGroupIteration
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
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,0);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(selected, group,0);
        }
        [TestMethod]
        public void FirstGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            var selected = tiles.TileExcept("3B");
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,1);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(selected, group,1);
        }
        [TestMethod]
        public void SecondGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            var selected = tiles.TileExcept("3R");
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,2);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(selected, group,2);
        }
        [TestMethod]
        public void ThirdGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            var selected = tiles.TileExcept("3T");
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,3);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(selected, group,3);
        }
        [TestMethod]
        public void FourthGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            var selected = tiles.TileExcept("3Y");
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,4);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(selected, group,4);
        }
        [TestMethod]
        public void AllOption()
        {
            var tiles = "3B,3R,3T,3Y".Split(",").Select(x => new Tile(x)).ToList();
            var group = new MaxGroup(tiles);
            var selected = tiles;
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,5);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(selected, group,5);
        }
    }
}