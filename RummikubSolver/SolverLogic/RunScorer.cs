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
        private BitVector128 removed = new BitVector128();
        public bool skipValidation = false;
        private class SortZipEnumerable : IEnumerable<FastCalcTile>
        {
            private readonly FastCalcTile[] baseUnused;
            private readonly UnusedFastCalcArray unusedForSelected;
            public readonly bool reverse;
            private int it1;
            private int it2;
            //allows starting a new enumeration from the current's position without changing the original's re-entrant state
            public SortZipEnumerable(SortZipEnumerable forkFrom)
            {
                baseUnused = forkFrom.baseUnused;
                unusedForSelected = forkFrom.unusedForSelected;
                reverse = forkFrom.reverse;
                it1 = forkFrom.it1;
                it2 = forkFrom.it2;
            }

            public SortZipEnumerable(FastCalcTile[] baseUnused, ref UnusedFastCalcArray unusedForSelected, bool reverse)
            {
                this.baseUnused = baseUnused;
                this.unusedForSelected = unusedForSelected;
                this.reverse = reverse;

                if (reverse)
                {
                    it1 = baseUnused.Length - 1;//here! overwrites on fork
                    it2 = unusedForSelected.Count - 1;
                }
            }

            public IEnumerator<FastCalcTile> GetEnumerator()
            {
                if (reverse)
                {
                    while (it1 >= 0 && it2 >= 0)
                    {
                        if ((int)baseUnused[it1] < (int)unusedForSelected.Set[it2])
                        {
                            yield return unusedForSelected.Set[it2];
                            it2--;
                        }
                        else if ((int)unusedForSelected.Set[it2] < (int)baseUnused[it1])
                        {
                            yield return baseUnused[it1];
                            it1--;
                        }
                        //tile int encodes unique id, so never equal
                    }
                    while (it1 >= 0)
                    {
                        yield return baseUnused[it1];
                        it1--;
                    }
                    while (it2 >= 0)
                    {
                        yield return unusedForSelected.Set[it2];
                        it2--;
                    }

                }
                else
                {
                    while (it1 < baseUnused.Length && it2 < unusedForSelected.Count)
                    {
                        if ((int)baseUnused[it1] < (int)unusedForSelected.Set[it2])
                        {
                            yield return baseUnused[it1];
                            it1++;
                        }
                        else if ((int)unusedForSelected.Set[it2] < (int)baseUnused[it1])
                        {
                            yield return unusedForSelected.Set[it2];
                            it2++;
                        }
                        //tile int encodes unique id, so never equal
                    }
                    while (it1 < baseUnused.Length)
                    {
                        yield return baseUnused[it1];
                        it1++;
                    }
                    while (it2 < unusedForSelected.Count)
                    {
                        yield return unusedForSelected.Set[it2];
                        it2++;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        //both unusedFromSelectedGroups and baseUnused should be sorted,
        //so for this set of tiles not already in groups, find the maximum runs we can
        //by zipping the two into a single list while checking how many are left, returning a "score" of the number left
        //golf rules
        public int Score (FastCalcTile[] baseUnused,ref UnusedFastCalcArray unusedForSelected)
        {
            removed.Clear();
            if (!skipValidation)
            {
                FastCalcTile last = baseUnused[0];
                for(int i = 1; i < baseUnused.Length; i++)
                {
                    if ((int)last > (int)baseUnused[i])
                        throw new ArgumentException("lists must be presorted",nameof(baseUnused));
                    last = baseUnused[i];
                }
                last = unusedForSelected.Set[0];
                for(int i = 1; i < unusedForSelected.Count; i++)
                {
                    if ((int)last > (int)unusedForSelected.Set[i])
                        throw new ArgumentException("lists must be presorted",nameof(unusedForSelected));
                    last = unusedForSelected.Set[i];
                }

            }
            RunCalcState state = new RunCalcState();
            int score=IterateBackwardsThroughDups(baseUnused, ref unusedForSelected);
            if (score == int.MaxValue) return score;
            int ndx = 0;
            foreach(var current in new SortZipEnumerable(baseUnused, ref unusedForSelected, false))
            {
                if (!removed[ndx] && UpdateBreaking(ref score, current, ref state)) 
                    return int.MaxValue;
                ndx++;
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
        private bool UpdateBreaking(ref int score, FastCalcTile current, ref RunCalcState state)
        {
            int colorNdx = (int)current.TileColor;
            if((int)state.lastForColor[colorNdx] == (int)FastCalcTile.MaxValue)
            {
                state.chainIsRunBreakingIfUnused[colorNdx] = current.IsInvalidIfUnused;
                state.chainScore[colorNdx] = 1;
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
        private int IterateBackwardsThroughDups(FastCalcTile[] baseUnused, ref UnusedFastCalcArray unusedForSelected)
        {
            int score = 0;
            RunCalcState state = new RunCalcState();
            int ndx = baseUnused.Length + unusedForSelected.Count - 1;
            var enumerable = new SortZipEnumerable(baseUnused, ref unusedForSelected, true);
            foreach(var current in enumerable)
            {

                if (current.Originality == 0)
                {
                    break;
                }
                removed[ndx] = true;
                if (UpdateBreakingReverse(ref score, current, ref state, ndx, enumerable))
                {
                    return int.MaxValue;
                }
                ndx--;
            }
            for(int i = 0; i < 4; i++)
            {
                if(state.chainScore[i] < 3)
                {
                    int needToFind = state.lastForColor[i].Number-1;
                    //iterate down from the current original enumerable's state to see if any match to complete the run
                    foreach(var tile in new SortZipEnumerable(enumerable))
                    {
                        if(tile.Number==needToFind && tile.TileColor == (TileColor)i)
                        {
                            removed[ndx] = true;
                            needToFind = tile.Number - 1;
                        }
                        ndx--;
                    }
                    if (needToFind == state.lastForColor[i].Number - 1)
                    {
                        if (state.chainIsRunBreakingIfUnused[i])
                        {
                            return int.MaxValue;
                        }
                        score += state.chainScore[i];
                    }
                }
            }
            return score;
        }
        private bool UpdateBreakingReverse(ref int score, FastCalcTile current, ref RunCalcState state, int currentNdx, SortZipEnumerable original)
        {
            int colorNdx = (int)current.TileColor;
            if((int)state.lastForColor[colorNdx] == (int)FastCalcTile.MaxValue)
            {
                state.chainIsRunBreakingIfUnused[colorNdx] = current.IsInvalidIfUnused;
                state.chainScore[colorNdx] = 1;
            }
            else
            {
                if(current.Number == state.lastForColor[colorNdx].Number - 1)
                {
                    state.chainScore[colorNdx]++;
                    state.chainIsRunBreakingIfUnused[colorNdx] |= current.IsInvalidIfUnused;
                    state.lastForColor[colorNdx] = current;
                }
                else
                {
                    if(state.chainScore[colorNdx] < 3)
                    {
                        int needToFind = state.lastForColor[colorNdx].Number-1;
                        //iterate down from the current original enumerable's state to see if any match to complete the run
                        foreach(var tile in new SortZipEnumerable(original))
                        {
                            if(tile.Number==needToFind && tile.TileColor == current.TileColor)
                            {
                                removed[currentNdx] = true;
                                needToFind = tile.Number - 1;
                            }
                            currentNdx--;
                        }
                        if (needToFind == state.lastForColor[colorNdx].Number - 1)
                        {
                            if (state.chainIsRunBreakingIfUnused[colorNdx])
                            {
                                return true;
                            }
                            score += state.chainScore[colorNdx];
                        }
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
