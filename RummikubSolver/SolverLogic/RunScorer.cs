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
            var listsPerColor = new[]
            {
                new List<int>{ },
                new List<int>{ },
                new List<int>{ },
                new List<int>{ }
            };
            foreach(var tile in new SortZipEnumerable(baseUnused,ref unusedForSelected, false))
            {
                listsPerColor[((int)tile.TileColor)].Add(tile.Number);
            }
            int score = 0;
            foreach(var list in listsPerColor)
            {
                score += ScoreList(list);
            }
            return 0;
        }
        public int ScoreList(List<int> numbers)
        {
            List<List<int>> possibleRuns = new List<List<int>>();
            foreach(var number in numbers)
            {
                int top = possibleRuns.LastOrDefault()?.LastOrDefault()??default;
                if(top!=0&&top+1 == number)
                {
                    possibleRuns.Last().Add(number);
                }
                else
                {
                    possibleRuns.Add(new List<int> { number });
                }
            }
            return 0;
            //1 2 3  6 7   A    1   4 5 6 7   C D 
            //var lowestFirst=possibleRuns.OrderBy(x=>x.Count).ToList();

        }
    }
}
