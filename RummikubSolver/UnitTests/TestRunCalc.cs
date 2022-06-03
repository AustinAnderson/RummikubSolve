using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunsRainbowTableGenerator;
using SharedModels;
using SolverLogic;
using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitTests;

namespace TestRunCalc
{
    [TestClass]
    public class TestHandOrBoardValidity
    {
            
        const int b = (int)TileColor.BLACK;
        const int r = (int)TileColor.RED;
        const int t = (int)TileColor.TEAL;
        const int y = (int)TileColor.YELLOW;
        [TestMethod]
        public void ValidIfHandUnused()
        {
            /*
                new[] { "1Th_0", "3Bh_0", "3Rh_0", "6Bb_0", "6Yb_0", "9Bh_0", "9Rh_0", "ABb_0", "AYb_0" },
                //                      valid because would be left over but came from the hand
                new[] {//                                                                     |
                    "4Bb_0", "4Rb_0", "4Th_0", "4Yh_0", "5Bb_0", "5Rb_0", "5Yh_0", "BBb_0", "BRh_0", "BYb_0", "CYb*_0",
                }
            );
            */
            //B (3h 4b 5b 6b) (9h Ab Bb)
            //R (3h 4b 5b) 9h Bh
            //T 1h 4h
            //Y (4h 5h 6b) (Ab Bb Cb*)
            //9
            var unusedCalcState = new UnusedTilesState();
            //                                      bit cleared on r row because hand tile
            //                                      v
            //                            1234567890123 4567890123456 789012
            //                            123456789ABCD 123456789ABCD
            var invB = new BitVector32(0b_0001110001100_0000000000000_000000);
            var invR = new BitVector32(0b_0001100000000_0000000000000_000000);
            var invT = new BitVector32(0b_0000000000000_0000000000000_000000);
            var invY = new BitVector32(0b_0000010001100_0000000000000_000000);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.BLACK, invB);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.RED, invR);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.TEAL, invT);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.YELLOW, invY);
            //                               1234567890123 4567890123456 789012
            //                               123456789ABCD 123456789ABCD
            var unusedB = new BitVector32(0b_0011110011100_0011000010000_000000);
            var unusedR = new BitVector32(0b_0011100010100_0010000010000_000000);
            var unusedT = new BitVector32(0b_1001001000000_0000000000000_000000);
            var unusedY = new BitVector32(0b_0001110001110_0000000000000_000000);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.BLACK, unusedB);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.RED, unusedR);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.TEAL, unusedT);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.YELLOW, unusedY);
            var keyB = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.BLACK).Data >> 6;
            var keyR = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.RED).Data >> 6;
            var keyT = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.TEAL).Data >> 6;
            var keyY = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.YELLOW).Data >> 6;
            var scorer = new RunScorer(new MockRunResultRainbowTable(new Dictionary<uint,RunResult>
            {
                //                      12345678901234567890123456789012
                //                      123456789ABCD123456789ABCD
                { keyB, new RunResult(0b00000000000000000000000000000000)},
                { keyR, new RunResult(0b00000000101000000000000000000000) { ScoreIfValid=2 } },
                { keyT, new RunResult(0b10010000000000000000000000000000) { ScoreIfValid=2 } },
                { keyY, new RunResult(0b00000000000000000000000000000000)}
            }));
            Assert.AreEqual(4,scorer.Score(ref unusedCalcState,0));

        }
        //[TestMethod]
        //public void ValidIfBoardUnusedWithHandEquivalent()
        [TestMethod]
        public void InvalidIfBoardUnused()
        {
            /*
                new[] { "1Th_0", "3Bh_0", "3Rh_0", "6Bb_0", "6Yb_0", "9Bh_0", "9Rh_0", "ABb_0", "AYb_0" },
                //        invalid because would be left over, and originally came from the board
                new[] {//                                                                      |
                    "4Bb_0", "4Rb_0", "4Th_0", "4Yh_0", "5Bb_0", "5Rb_0", "5Yh_0", "BBb_0", "BRb_0", "BYb_0", "CYb*_0",
                }
            */
            //B (3h 4b 5b 6b) (9h Ab Bb)
            //R (3h 4b 5b) 9h Bb
            //T 1h 4h
            //Y (4h 5h 6b) (Ab Bb Cb*)
            //9
            var unusedCalcState = new UnusedTilesState();
            //                                      bit set on r row because board tile
            //                                      v
            //                            1234567890123 4567890123456 789012
            //                            123456789ABCD 123456789ABCD
            var invB = new BitVector32(0b_0001110001100_0000000000000_000000);
            var invR = new BitVector32(0b_0001100000100_0000000000000_000000);
            var invT = new BitVector32(0b_0000000000000_0000000000000_000000);
            var invY = new BitVector32(0b_0000010001100_0000000000000_000000);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.BLACK, invB);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.RED, invR);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.TEAL, invT);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.YELLOW, invY);
            //                               1234567890123 4567890123456 789012
            //                               123456789ABCD 123456789ABCD
            var unusedB = new BitVector32(0b_0011110011100_0011000010000_000000);
            var unusedR = new BitVector32(0b_0011100010100_0010000010000_000000);
            var unusedT = new BitVector32(0b_1001001000000_0000000000000_000000);
            var unusedY = new BitVector32(0b_0001110001110_0000000000000_000000);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.BLACK, unusedB);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.RED, unusedR);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.TEAL, unusedT);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.YELLOW, unusedY);
            var keyB = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.BLACK).Data >> 6;
            var keyR = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.RED).Data >> 6;
            var keyT = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.TEAL).Data >> 6;
            var keyY = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.YELLOW).Data >> 6;
            var scorer = new RunScorer(new MockRunResultRainbowTable(new Dictionary<uint,RunResult>
            {
                //                      12345678901234567890123456789012
                //                      123456789ABCD123456789ABCD
                { keyB, new RunResult(0b00000000000000000000000000000000)},
                { keyR, new RunResult(0b00000000101000000000000000000000) { ScoreIfValid=2 } },
                { keyT, new RunResult(0b10010000000000000000000000000000) { ScoreIfValid=2 } },
                { keyY, new RunResult(0b00000000000000000000000000000000)}
            }));
            Assert.AreEqual(ushort.MaxValue,scorer.Score(ref unusedCalcState,0));
        }
        [TestMethod]
        public void TestThatOneCaseThatFailed()
        {
            //B (3 4 5 6 7 8 9 A) (2 3' 4') (9')
            //R (1 2 3 4) (8 9) (3') (9')
            //T (1) (4) (7) 
            //Y (2 3 4 5 6 7 8 9 A) (C)
            //9
            var unusedCalcState = new UnusedTilesState();
            //                           12345678901234567890123456 789012
            //                           123456789ABCD123456789ABCD
            var invB = new BitVector32(0b01011101010000001000000000_000000);
            var invR = new BitVector32(0b01010000000000000000000000_000000);
            var invT = new BitVector32(0b00000000000000000000000000_000000);
            var invY = new BitVector32(0b00000101110000000000000000_000000);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.BLACK, invB);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.RED, invR);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.TEAL, invT);
            unusedCalcState.InvalidIfUnusedFlags.SetBitVectorForColor(TileColor.YELLOW, invY);
            //                              12345678901234567890123456 789012
            //                              123456789ABCD123456789ABCD
            var unusedB = new BitVector32(0b01111111110000011000010000_000000);
            var unusedR = new BitVector32(0b11110001100000010000010000_000000);
            var unusedT = new BitVector32(0b10010010000000000000000000_000000);
            var unusedY = new BitVector32(0b01111111110100000000000000_000000);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.BLACK, unusedB);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.RED, unusedR);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.TEAL, unusedT);
            unusedCalcState.UnusedInGroupsFlags.SetBitVectorForColor(TileColor.YELLOW, unusedY);
            var keyB = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.BLACK).Data >> 6;
            var keyR = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.RED).Data >> 6;
            var keyT = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.TEAL).Data >> 6;
            var keyY = unusedCalcState.UnusedInGroupsFlags.GetBitVectorCopy(TileColor.YELLOW).Data >> 6;
            var scorer = new RunScorer(new MockRunResultRainbowTable(new Dictionary<uint,RunResult>
            {
                //                      12345678901234567890123456789012
                //                      123456789ABCD123456789ABCD
                { keyB, new RunResult(0b00000000000000000000010000000000) { ScoreIfValid=1 } },
                { keyR, new RunResult(0b00000001100000010000010000000000) { ScoreIfValid=4 } },
                { keyT, new RunResult(0b10010010000000000000000000000000) { ScoreIfValid=3 } },
                { keyY, new RunResult(0b00000000000100000000000000000000) { ScoreIfValid=1 } }
            }));
            Assert.AreEqual(9,scorer.Score(ref unusedCalcState,0));
        }
    }
}
