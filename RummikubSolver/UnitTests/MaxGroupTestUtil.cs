using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunsRainbowTableGenerator;
using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    internal static class MaxGroupTestUtil
    {

        public static IEnumerable<Tile> TileExceptIndexes(this IEnumerable<Tile> tiles,IEnumerable<int> excepts)
        {
            var result=new List<Tile>();
            var index = 0;
            foreach (var tile in tiles)
            {
                if (!excepts.Contains(index))
                {
                    result.Add(tile);
                }
                index++;
            }
            return result;
        }
        public static IEnumerable<Tile> TileExcept(this IEnumerable<Tile> tiles,IEnumerable<Tile> excepts)
        {
            var result=new List<Tile>();
            var exceptsStr = new HashSet<string>(excepts.Select(x => x.DebugDisplay));
            foreach (var tile in tiles)
            {
                if (!exceptsStr.Contains(tile.DebugDisplay))
                {
                    result.Add(tile);
                }
            }
            return result;
        }
        public static IEnumerable<Tile> TileExcept(this IEnumerable<Tile> tiles,string tileRep)
        {
            return tiles.TileExcept(new[] { new Tile(tileRep) });
        }
        public static void AssertCurrentUsedMatches(IEnumerable<Tile> expectedUsed, MaxGroup group, int key)
        {
            var actualState = new UnusedTilesState();
            var expectedState = new UnusedTilesState();
            group.MarkUsedForSelected(ref actualState, key);
            foreach (var tile in expectedUsed)
            {
                expectedState.UnusedInGroupsFlags[(int)tile.Color][tile.Number + ((tile.Originality > 0 ? 1 : 0) * 13)] = true;
            }
            //not used == unused
            Assert.AreEqual(expectedState.ToString(), actualState.ToString(), "expected unused numbers to match for key 123456789ABCD123456789ABCD");
        }
        public static void AssertGroupedMatches([DisallowNull] IEnumerable<Tile> expected, MaxGroup group, int key)
        {
            if (expected == null) Assert.Inconclusive();//false positive CS8604
            var selected=string.Join(",",group.GetGroupForPossibilityKey(key).Select(x=>x.ToString()));
#pragma warning disable CS8604 
            var expectedStr=string.Join(",",expected.Select(t => t.ToString()).ToArray());
#pragma warning restore CS8604 
            Assert.AreEqual(expectedStr,selected,"selected groups don't match");
        }
    }
}
