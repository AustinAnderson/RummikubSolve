using RunsRainbowTableGenerator;
using SharedModels;
using SolverLogic.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic
{
    public class RunScorer
    {
        private readonly IRunResultRainbowTable rainbowTable;
        public RunScorer(IRunResultRainbowTable rainbowTable)
        {
            this.rainbowTable = rainbowTable;
        }
        public ushort Score(ref UnusedTilesState unusedTiles, int jokerCount)
        {
            var scoresPerColor = new JokerScores[4];
            for (int i = 0; i < 4; i++)
            {
                var runRes = rainbowTable.GetFor(unusedTiles.UnusedInGroupsFlags.GetBitVectorCopy((TileColor)i).Data >> 6);
                scoresPerColor[i]= ScoreWithJoker(runRes, unusedTiles.InvalidIfUnusedFlags.GetBitVectorCopy((TileColor)i), jokerCount);
                if (scoresPerColor[i].AllInvalid) return ushort.MaxValue;
            }
            int[] scores = new[]
            {
                scoresPerColor[0][0]+ scoresPerColor[1][0]+scoresPerColor[2][0]+ scoresPerColor[3][0],


                scoresPerColor[0][1]+ scoresPerColor[1][0]+scoresPerColor[2][0]+ scoresPerColor[3][0],
                scoresPerColor[0][0]+ scoresPerColor[1][1]+scoresPerColor[2][0]+ scoresPerColor[3][0],
                scoresPerColor[0][0]+ scoresPerColor[1][0]+scoresPerColor[2][1]+ scoresPerColor[3][0],
                scoresPerColor[0][0]+ scoresPerColor[1][0]+scoresPerColor[2][0]+ scoresPerColor[3][1],

                scoresPerColor[0][2]+ scoresPerColor[1][0]+scoresPerColor[2][0]+ scoresPerColor[3][0],
                scoresPerColor[0][0]+ scoresPerColor[1][2]+scoresPerColor[2][0]+ scoresPerColor[3][0],
                scoresPerColor[0][0]+ scoresPerColor[1][0]+scoresPerColor[2][2]+ scoresPerColor[3][0],
                scoresPerColor[0][0]+ scoresPerColor[1][0]+scoresPerColor[2][0]+ scoresPerColor[3][2],



                scoresPerColor[0][1]+ scoresPerColor[1][2]+scoresPerColor[2][0]+ scoresPerColor[3][0],
                scoresPerColor[0][1]+ scoresPerColor[1][0]+scoresPerColor[2][2]+ scoresPerColor[3][0],
                scoresPerColor[0][1]+ scoresPerColor[1][0]+scoresPerColor[2][0]+ scoresPerColor[3][2],

                scoresPerColor[0][2]+ scoresPerColor[1][1]+scoresPerColor[2][0]+ scoresPerColor[3][0],
                scoresPerColor[0][0]+ scoresPerColor[1][1]+scoresPerColor[2][2]+ scoresPerColor[3][0],
                scoresPerColor[0][0]+ scoresPerColor[1][1]+scoresPerColor[2][0]+ scoresPerColor[3][2],

                scoresPerColor[0][2]+ scoresPerColor[1][0]+scoresPerColor[2][1]+ scoresPerColor[3][0],
                scoresPerColor[0][0]+ scoresPerColor[1][2]+scoresPerColor[2][1]+ scoresPerColor[3][0],
                scoresPerColor[0][0]+ scoresPerColor[1][0]+scoresPerColor[2][1]+ scoresPerColor[3][2],

                scoresPerColor[0][2]+ scoresPerColor[1][0]+scoresPerColor[2][0]+ scoresPerColor[3][1],
                scoresPerColor[0][0]+ scoresPerColor[1][2]+scoresPerColor[2][0]+ scoresPerColor[3][1],
                scoresPerColor[0][0]+ scoresPerColor[1][0]+scoresPerColor[2][2]+ scoresPerColor[3][1],

            };
            int min = int.MaxValue;
            foreach(var sum in scores)
            {
                if(sum < min)
                {
                    min = sum;
                }
            }
            ushort score = ushort.MaxValue;
            if (min < ushort.MaxValue)
            {
                score = (ushort)min;
            }


            /*//making it faster, not the problem I want to solve right now
            ushort scoreIfNoJokers = 0;
            int minOneJokerIndex = 0;
            int minTwoJokerIndex = 0;
            for(int i=0;i<4; i++)
            {
                scoreIfNoJokers += scoresPerColor[i].ScoreWithNone;
                if (scoresPerColor[minOneJokerIndex].ScoreWithOne < scoresPerColor[i].ScoreWithOne)
                {
                    minOneJokerIndex = i;
                }
                if (scoresPerColor[minTwoJokerIndex].ScoreWithTwo < scoresPerColor[i].ScoreWithTwo)
                {
                    minTwoJokerIndex = i;
                }
            }
            score = scoreIfNoJokers;
            if(jokerCount > 0)
            {
                if (scoresPerColor[minOneJokerIndex].ScoreWithOne < scoresPerColor[minOneJokerIndex].ScoreWithNone)
                {
                    //back out the addition of the minjoker color's score with none, then add that color's joker score
                    score=(ushort)(scoreIfNoJokers - scoresPerColor[minOneJokerIndex].ScoreWithNone +
                          scoresPerColor[minOneJokerIndex].ScoreWithOne);
                }
                if (jokerCount > 1)
                {
                    if(scoresPerColor[minTwoJokerIndex].ScoreWithTwo < scoresPerColor[])
                }
            }
            */

            return score;
        }


        /*
       invalidMasked@i+1 invalidMasked@i+14 newUnused@i+1 newUnused@i+14
           0                   0              0              0                invalid
           0                   0              0              1                newUnused@i+14
           0                   0              1              0                newUnused@i+1
           0                   0              1              1                newUnused@i+1
           0                   1              0              0                invalid
           0                   1              0              1                newUnused@i+14
           0                   1              1              0                invalid
           0                   1              1              1                newUnused@i+14
           1                   0              0              0                invalid
           1                   0              0              1                invalid
           1                   0              1              0                newUnused@i+1
           1                   0              1              1                newUnused@i+1
           1                   1              0              0                invalid
           1                   1              0              1                invalid
           1                   1              1              0                invalid
           1                   1              1              1                invalid
        */

        bool checkAndSetBit(uint invalidMaskedCpy, ref uint data,int index)
        {
            bool wasSet = false;
            const bool t = true;
            const bool f = false;
            var switchVal = (
                invalidMaskedCpy.Get(index),
                invalidMaskedCpy.Get(index + 13),
                data.Get(index),
                data.Get(index + 13)
            );
            var indexShift = switchVal switch
            {
                (f, f, f, t) => 13,
                (f, f, t, f) => 0,
                (f, f, t, t) => 0,
                (f, t, f, t) => 13,
                (f, t, t, t) => 13,
                (t, f, t, f) => 0,
                (t, f, t, t) => 0,
                _ => -1
            };
            if (indexShift >= 0)
            {
                wasSet = true;
                data.Set(index + indexShift, false);
            }
            return wasSet;
        }
        private JokerScores ScoreWithJoker(RunResult res, BitVector32 invalidIfUnusedFlags, int jokerCount)
        {
            JokerScores result = new JokerScores
            {
                ScoreWithNone = res.ScoreIfValid,
                ScoreWithOne = ushort.MaxValue,
                ScoreWithTwo = ushort.MaxValue,
            };
            //if the unused after getting all the runs with the current set of unused
            //has any bits in common with the invalidIfUnused bit array,
            //then this configuration is invalid as a whole
            uint invalidMasked=invalidIfUnusedFlags.Data & res.Data;
            if(invalidMasked != 0)
            {
                result.ScoreWithNone = ushort.MaxValue;
            }
            //but might be salvagable with jokers, so either 0 with no jokers (best result)
            //or no jokers available, so immediately done with this solve
            if(result.ScoreWithNone == 0 || jokerCount == 0) 
                return result;

            if (invalidMasked != 0)
            {
                if (jokerCount == 1)
                {
                    //foreach false bit in unused,
                    //if would be part of 3 true bits if it was true (including dup set),
                    //set all those to false
                    //if invalid & newVal now clear and newVal bitCount < current min,
                    //update ScoreWithOne
                    
                    List<int> setBitIndexes = new List<int>();
                    for(int i = 0; i < 26; i++)
                    {
                        var newUnused = res.Unused;
                        if (!newUnused.Get(i))
                        {
                            //test joker as 1X_0
                            if (i == 0)
                            {
                                if(checkAndSetBit(invalidMasked,ref newUnused,i+1) && checkAndSetBit(invalidMasked,ref newUnused, i + 2))
                                {

                                }
                            }
                            else if(i < 13)
                            {

                            }
                        }
                    }
                }
                else if(jokerCount == 2)
                {

                }
            }
            else
            {
                if (jokerCount == 1)
                {

                }
                else if (jokerCount == 2)
                {
                }
            }
            return result;
        }
        private struct JokerScores
        {
            public bool AllInvalid
                => ScoreWithNone==ScoreWithOne && ScoreWithOne==ScoreWithTwo && ScoreWithTwo==ushort.MaxValue;
            public ushort this[int i] => i switch
            {
                0 => ScoreWithNone,
                1 => ScoreWithOne,
                2 => ScoreWithTwo,
                _ => ushort.MinValue
            };
            public ushort ScoreWithNone;
            public ushort ScoreWithOne;
            public ushort ScoreWithTwo;
        }
    }
}
