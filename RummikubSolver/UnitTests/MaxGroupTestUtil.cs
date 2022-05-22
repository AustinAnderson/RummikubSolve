using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public static void AssertCurrentUnusedMatches(IEnumerable<Tile> expected, MaxGroup group, int key)
        {
            var unused = new FastCalcTile[10];
            int addLocation = 0;
            group.AddUnusedForSelected(unused, ref addLocation,key);
            var unusedStr=string.Join(",", unused.Take(addLocation).Select(x => x.ToString()));
            var expectedUnused = string.Join(",", expected.Select(x => x.ToFastCalcTile().ToString()));
            Assert.AreEqual(expectedUnused, unusedStr,"unused lists don't match");
        }
        public static void AssertGroupedMatches([DisallowNull] IEnumerable<Tile> expected, MaxGroup group, int key)
        {
            if (expected == null) Assert.Inconclusive();//false positive CS8604
            var selected=string.Join(",",group.GetGroupForPossibilityKey(key).Select(x=>x.ToString()));
#pragma warning disable CS8604 
            var expectedStr=string.Join(",",expected.Select(t => t.ToFastCalcTile().ToString()).ToArray());
#pragma warning restore CS8604 
            Assert.AreEqual(expectedStr,selected,"selected groups don't match");
        }
    }
}
