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
        public static void AssertCurrentUsedMatches(IEnumerable<Tile> expectedUnused, MaxGroup group, int key)
        {
            var unused = new FastCalcTile[10];
            var tileState = new UsedTilesState();
            group.MarkUsedForSelected(ref tileState, key);
            //not used == unused
            var actualUnusedPerColor = tileState.UsedInGroupsFlags.Select(x => new BitVector32(~x.Data)).ToArray();
            var expectedUnusedPerColor = new BitVector32[4];
            for(int i = 0; i < expectedUnusedPerColor.Length; i++)
            {
                expectedUnusedPerColor[i] = new BitVector32(uint.MaxValue);//all set
            }
            foreach(var tile in expectedUnused)
            {
                int bitIndex = tile.Number;
                if (tile.Originality != 0) bitIndex += 13;
                expectedUnusedPerColor[(int)tile.Color][bitIndex] = false;
            }
            string expected = "";
            for(int i = 0; i < 4; i++)
            {
                expected+=((TileColor)i).Char()+expectedUnusedPerColor[i].ToString();
            }
            string actual = "";
            for(int i = 0; i < 4; i++)
            {
                actual+=((TileColor)i).Char()+actualUnusedPerColor[i].ToString();
            }
            Assert.AreEqual(expected, actual, "expected unused numbers to match for key 123456789ABCD123456789ABCD");
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
