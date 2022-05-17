using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class MaxGroupIterable 
    {
        private readonly List<MaxGroup> groups;
        private readonly FastCalcTile[] buffer;

        public MaxGroupIterable(List<MaxGroup> groups)
        {
            this.groups = groups;
            buffer = new FastCalcTile[groups.Count * 6];
        }
        public (FastCalcTile[],int) GetUnusedForKey(GroupConf conf)
        {
            int possibilitySetSize = 0;
            for(int i = 0; i < groups.Count; i++)
            {
                groups[i].AddUnusedForSelected(buffer, ref possibilitySetSize, conf[i]);
            }
            return (buffer, possibilitySetSize);
        }

    }
}
