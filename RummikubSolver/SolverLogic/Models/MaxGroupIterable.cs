using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class MaxGroupIterable : IEnumerable<(FastCalcTile[],int)>
    {
        private readonly List<MaxGroup> groups;

        public MaxGroupIterable(List<MaxGroup> groups)
        {
            this.groups = groups;
        }
        public IEnumerator<(FastCalcTile[],int)> GetEnumerator()
        {
            var done = false;
            int currentDigit = 0;
            //which possibility on each group
            var solutionKey = new int[groups.Count];
            int currentPossibility = 0;
            var currentPossibilitySetFastCalc=new FastCalcTile[groups.Count*6];
            while (!done)
            {
                int possibilitySetSize = 0;
                for(int i = 0; i < groups.Count; i++)
                {
                    groups[i].AddCurrentUnused(currentPossibilitySetFastCalc, ref possibilitySetSize);
                }
                yield return (currentPossibilitySetFastCalc,possibilitySetSize);
                if (groups[currentDigit].IsAtLast)
                {
                    while (!done && groups[currentDigit].IsAtLast)
                    {
                        currentDigit++;
                        if(currentDigit >= groups.Count)
                        {
                            done = true;
                        }
                    }
                    if (!done)
                    {
                        groups[currentDigit].MoveNext();
                        for(int i = 0; i < currentDigit; i++)
                        {
                            groups[i].ResetIteration();
                        }
                        currentDigit = 0;
                    }
                }
                else
                {
                    groups[currentDigit].MoveNext();
                }
                currentPossibility++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
