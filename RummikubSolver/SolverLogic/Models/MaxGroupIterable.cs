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
        public UnusedFastCalcArray GetUnusedForKey(GroupConf conf)
        {
            int possibilitySetSize = 0;
            for(int i = 0; i < groups.Count; i++)
            {
                groups[i].AddUnusedForSelected(buffer, ref possibilitySetSize, conf[i]);
            }
            return new UnusedFastCalcArray
            {
                Set = buffer,
                Count = possibilitySetSize
            };
        }
        public List<FastCalcTile[]> GetGroupsForKey(GroupConf conf)
        {
            var list=new List<FastCalcTile[]>();
            for(int i = 0; i < groups.Count; i++)
            {
                list.Add(groups[i].GetGroupForPossibilityKey(conf[i]));
            }
            return list;
        }

    }
}
