using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic
{
    public class RunScorer
    {
        //both unusedFromSelectedGroups and baseUnused should be sorted,
        //so for this set of tiles not already in groups, find the maximum runs we can
        //by zipping the two into a single list while checking how many are left, returning a "score" of the number left
        //golf rules
        public static int Score (FastCalcTile[] baseUnused,ref UnusedFastCalcArray unusedForSelected)
        {
            int score = 0;
            int it1 = 0;
            int it2 = 0;
            RunCalcState state = new RunCalcState();
            FastCalcTile current;
            while(it1< baseUnused.Length && it2 < unusedForSelected.Count)
            {
                if ((int)baseUnused[it1] < (int)unusedForSelected.Set[it2])
                {
                    current = baseUnused[it1];
                    if (UpdateBreaking(ref score, current, ref state)) return int.MaxValue;
                    it1++;
                }
                else if((int)unusedForSelected.Set[it2]< (int)baseUnused[it1])
                {
                    current = unusedForSelected.Set[it2];
                    if (UpdateBreaking(ref score, current, ref state)) return int.MaxValue;
                    it2++;
                }
                //tile int encodes unique id, so never equal
            }
            while (it1 < baseUnused.Length)
            {
                current = baseUnused[it1];
                if (UpdateBreaking(ref score, current, ref state)) return int.MaxValue;
                it1++;
            }
            while (it2 < unusedForSelected.Count)
            {
                current = unusedForSelected.Set[it2];
                if (UpdateBreaking(ref score, current, ref state)) return int.MaxValue;
                it2++;
            }
            for(int i = 0; i < 4; i++)
            {
                if(state.chainScore[i] < 3)
                {
                    if (state.chainIsRunBreakingIfUnused[i])
                    {
                        return int.MaxValue;
                    }
                    score+= state.chainScore[i];
                }
            }
            return score;
        }
        /*
        y
                          t                                b     y  r
        v                    v                                v     v  v
        1T 2T 3B 3R 4B 4R 4T 4Y 5B 5R 5Y 6B 6Y 9B 9R JB JY QB QY KY KR
          2        2  2        3  3  2  4   3        2     3  2  3
         */
        private static bool UpdateBreaking(ref int score, FastCalcTile current, ref RunCalcState state)
        {
            int colorNdx = (int)current.TileColor;
            if((int)state.lastForColor[colorNdx] == (int)FastCalcTile.MaxValue)
            {
                state.chainIsRunBreakingIfUnused[colorNdx] = current.IsInvalidIfUnused;
            }
            else
            {
                if(current.Number == state.lastForColor[colorNdx].Number + 1)
                {
                    state.chainScore[colorNdx]++;
                    state.chainIsRunBreakingIfUnused[colorNdx] |= current.IsInvalidIfUnused;
                    state.lastForColor[colorNdx] = current;
                }
                else
                {
                    if(state.chainScore[colorNdx] < 3)
                    {
                        if (state.chainIsRunBreakingIfUnused[colorNdx])
                        {
                            return true;
                        }
                        score+=state.chainScore[colorNdx];
                    }
                    state.chainScore[colorNdx] = 1;
                    state.chainIsRunBreakingIfUnused[colorNdx]=current.IsInvalidIfUnused;
                }
            }
            state.lastForColor[colorNdx] = current;
            return false;
        }
    }
}
