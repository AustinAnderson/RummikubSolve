using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFastCalcTile
{
    public static class IntBits
    {
        public static string Show(int val)
        {
            return Convert.ToString(val, 2).PadLeft(32, '0');
        }
    }
    [TestClass]
    public class TestPropertyRetreive
    {
        [TestMethod]
        public void Test1()
        {
            var tile = new Tile("2Y")
            {
                IsBoardTile = true,
                EquivalentHandTileId = 3,
                Originality = 3,
                Id = 2
            };
            Assert.AreEqual(
                FastCalcTile.ToString(
                    tile.Number,tile.Originality,tile.Color,tile.IsBoardTile,true,tile.Id
                ),
                tile.ToFastCalcTile().ToString()
            );
        }
        [TestMethod]
        public void Test2()
        {
            var tile = new Tile("12B")
            {
                IsBoardTile = true,
                EquivalentHandTileId = 3,
                Id = 2
            };
            Assert.AreEqual(
                FastCalcTile.ToString(
                    tile.Number,tile.Originality,tile.Color,tile.IsBoardTile,true,tile.Id
                ),
                tile.ToFastCalcTile().ToString()
            );
        }
        [TestMethod]
        public void Test3()
        {
            var tile = new Tile("12B")
            {
                IsBoardTile = false,
                Id = 2
            };
            Assert.AreEqual(
                FastCalcTile.ToString(
                    tile.Number,tile.Originality,tile.Color,tile.IsBoardTile,false,tile.Id
                ),
                tile.ToFastCalcTile().ToString()
            );
        }
        [TestMethod]
        public void Test4()
        {
            var tile = new Tile("2Y")
            {
                IsBoardTile = true,
                Originality = 3,
                Id = 2
            };
            Assert.AreEqual(
                FastCalcTile.ToString(
                    tile.Number,tile.Originality,tile.Color,tile.IsBoardTile,false,tile.Id
                ),
                tile.ToFastCalcTile().ToString()
            );
        }
    }
    [TestClass]
    public class TestPack
    {
        [TestMethod]
        public void WithOriginality()
        {
            var tile = new Tile("2Y");
            tile.IsBoardTile = true;
            tile.EquivalentHandTileId = 2;
            tile.Id = 4;
            tile.Originality = 2;
            //                  originality (dup count)
            //                  |    color
            //                  |      |  isBoard
            //                  | num  |  | hasHandEquivalent
            //                  | |    |  | | padding       id
            //                __|_|____|__|_|__/|            |
            //               /  | |    |  | |   |            |
            //               v  v v    v  v v   v            v
            int expected = 0b0_10_0010_11_1_1_00000_0000000000000100;
            var fct=tile.ToFastCalcTile();
            Assert.AreEqual(expected, (int)fct, $"expected {IntBits.Show(expected)} got {fct.BitString}");

        }
    }
}
