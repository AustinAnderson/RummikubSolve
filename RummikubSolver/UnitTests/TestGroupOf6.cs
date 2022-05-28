using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLogic.Models;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.TestMaxGroupIteration
{
    [TestClass]
    public class TestGroupOf6
    {

        [TestMethod]
        public void NoneOption()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            var selected=new List<Tile>();
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,0);
            MaxGroupTestUtil.AssertCurrentUsedMatches(selected, group,0);
        }
        [TestMethod]
        public void FirstGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            var selected = tiles.TileExceptIndexes(new[] { 0, 4, 5 });
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,1);
            MaxGroupTestUtil.AssertCurrentUsedMatches(selected, group,1);
        }
        [TestMethod]
        public void SecondGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            var selected = tiles.TileExceptIndexes(new[] { 1, 4, 5 });
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,2);
            MaxGroupTestUtil.AssertCurrentUsedMatches(selected, group,2);
        }
        [TestMethod]
        public void ThirdGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            var selected = tiles.TileExceptIndexes(new[] { 2, 4, 5 });
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,3);
            MaxGroupTestUtil.AssertCurrentUsedMatches(selected, group,3);
        }
        [TestMethod]
        public void FourthGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            var selected = tiles.TileExceptIndexes(new[] { 3, 4, 5 });
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,4);
            MaxGroupTestUtil.AssertCurrentUsedMatches(selected, group,4);
        }
        [TestMethod]
        public void GroupOf4Option()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            var selected = tiles.TileExceptIndexes(new[] { 4, 5 });
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,5);
            MaxGroupTestUtil.AssertCurrentUsedMatches(selected, group,5);
        }
        [TestMethod]
        public void AllOption()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            var selected = tiles;
            MaxGroupTestUtil.AssertGroupedMatches(selected, group,6);
            MaxGroupTestUtil.AssertCurrentUsedMatches(selected, group,6);
        }
    }
}