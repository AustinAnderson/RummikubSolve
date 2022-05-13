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
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
        [TestMethod]
        public void FirstGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            for(int i = 0; i < 1; i++) group.MoveNext();
            var selected = tiles.TileExceptIndexes(new[] { 0, 4, 5 });
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
        [TestMethod]
        public void SecondGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            for(int i = 0; i < 2; i++) group.MoveNext();
            var selected = tiles.TileExceptIndexes(new[] { 1, 4, 5 });
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
        [TestMethod]
        public void ThirdGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            for(int i = 0; i < 3; i++) group.MoveNext();
            var selected = tiles.TileExceptIndexes(new[] { 2, 4, 5 });
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
        [TestMethod]
        public void FourthGroupOf3Option()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            for(int i = 0; i < 4; i++) group.MoveNext();
            var selected = tiles.TileExceptIndexes(new[] { 3, 4, 5 });
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
        [TestMethod]
        public void GroupOf4Option()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            for(int i = 0; i < 5; i++) group.MoveNext();
            var selected = tiles.TileExceptIndexes(new[] { 4, 5 });
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
        [TestMethod]
        public void AllOption()
        {
            var tiles = "3B,3R,3T,3Y,3B,3Y".Split(",").Select(x => new Tile(x)).ToList();
            int id = 0;
            tiles.ForEach(x=>x.Id = id++);
            var group = new MaxGroup(tiles);
            for(int i = 0; i < 6; i++) group.MoveNext();
            Assert.IsTrue(group.IsAtLast, "all should be last");
            var selected = tiles;
            MaxGroupTestUtil.AssertGroupedMatches(selected, group);
            MaxGroupTestUtil.AssertCurrentUnusedMatches(tiles.TileExcept(selected), group);
        }
    }
}