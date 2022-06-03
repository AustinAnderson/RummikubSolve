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
                    //looking for region of inv mask
                    //where bits of unused within that regions bounds
                    //have at most one 0 (gap that can be filled with joker)
                    //inv mask 1000010000
                    //  unused 1010010101

                    //inv mask 1000110000
                    //  unused 1011011001

                    //inv mask is the & of unused and invalid,
                    //so can or inv mask and unused to find region of 1's with at most 1 zero in it
                    //actually will be exactly one zero, otherwise it would already be a run
                    uint newUnused = res.Unused;
                    var setBits = invalidMasked | res.Unused;
                    bool startedSearching = false;
                    for (int i = 0; i < 31; i++)
                    {
                        if(!startedSearching && invalidMasked.Get(i))
                        {
                            startedSearching = true;
                        }
                        else if(startedSearching)
                        {
                            if(!setBits.Get(i) && !setBits.Get(i + 1))
                            {
                                //not valid
                                result.ScoreWithOne = ushort.MaxValue;
                                break;
                            }
                        }
                        newUnused.Set(i, false);
                    }
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
