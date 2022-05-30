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
        public int Score(ref UnusedTilesState unusedTiles)
        {
            int score = 0;
            for (int i = 0; i < 4; i++)
            {
                var runRes = rainbowTable.GetFor(unusedTiles.UnusedInGroupsFlags.GetBitVectorCopy((TileColor)i).Data >> 6);
                //if the unused after getting all the runs with the current set of unused
                //has any bits in common with the invalidIfUnused bit array,
                //then this configuration is invalid as a whole
                if ((runRes.Unused & unusedTiles.InvalidIfUnusedFlags.GetBitVectorCopy((TileColor)i).Data) != 0)
                {
                    return int.MaxValue;
                }
                score += runRes.ScoreIfValid;
            }
            return score;
        }
    }
}
