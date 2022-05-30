using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunsRainbowTableGenerator;
using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusedTileState
{
    [TestClass]
    public class TestUnusedTileState
    {
        [TestMethod]
        public void TestInvalidsSetCorrectly()
        {
            var tileSet=new TileSetForCurrentHand(
                ("6Y,7Y,8Y,9Y,10Y 2B,3B,4B,5B,6B " +
                "12B,12T,12R  11B,11Y,11R 10B,10T,10Y " +
                "12Y,12T,12R  2R,3R,4R 8B,9B,10B   4B,4T,4Y,4R   1B,1T,1Y  11T,11B,11Y " +
                "5R,5Y,5T,5B").Split(new[] { ',', ' ' }).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Tile(x.Trim())).ToList(),
                "2Y,7B,9B,1T,4T,3Y,5Y,12Y,1R,3R,9R,9R,8R,7T,3B,4Y".Split(",").Select(x => new Tile(x.Trim())).ToList()
            );
            /*
             ' = duped to board (invalid)
             * = duped to hand (valid)
             b = black, r= red, t=teal, y=yellow
                6y 7y 8y 9y Ay 2b 3b* 4b 5b 6b
                Cb Ct Cr  Bb By Br Ab At, Ay'
                Cy* Ct' Cr'  2r 3r* 4r 8b 9b* Ab'   4b' 4t* 4y* 4r'   1b 1t* 1y  Bt Bb' By'
                5r 5y* 5t 5b'

            //hand is always valid
                2y, 7b, 9b, 1t, 4t, 3y, 5y, 12y, 1r, 3r, 9r, 9r, 8r, 7t, 3b, 4y
            */
            var bitMaps = new UnusedTilesState(tileSet);
            int b = (int)TileColor.BLACK;
            int r = (int)TileColor.RED;
            int t = (int)TileColor.TEAL;
            int y = (int)TileColor.YELLOW;

            var expecteds = new BitVector32[4];
            //set bit if board with no dup in hand

            //                                1234567890123 4567890123456 789012
            //                                123456789ABCD 123456789ABCD 654321
            expecteds[b] = new BitVector32(0b_1101110101110_0001100001100_000000);
            expecteds[r] = new BitVector32(0b_0101100000110_0001000000010_000000);
            expecteds[t] = new BitVector32(0b_0000100001110_0000000000010_000000);
            expecteds[y] = new BitVector32(0b_1000011111100_0000000001100_000000);
            for(int i=0;i< 4; i++)
            {
                Assert.AreEqual(expecteds[i].ToString(), bitMaps.InvalidIfUnusedFlags.GetBitVectorCopy((TileColor)i).ToString(),$"failed for color {(TileColor)i}");
            }
        }
    }
}
