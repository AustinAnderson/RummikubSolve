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

        public MaxGroupIterable(List<MaxGroup> groups)
        {
            this.groups = groups;
        }
        public void MarkUnusedForConf(ref UnusedTilesState tilesState,GroupConf conf)
        {
            for(int i = 0; i < groups.Count; i++)
            {
                groups[i].MarkUsedForSelected(ref tilesState, conf[i]);
            }
        }
        public List<Tile[]> GetGroupsForKey(GroupConf conf)
        {
            var list=new List<Tile[]>();
            for(int i = 0; i < groups.Count; i++)
            {
                list.Add(groups[i].GetGroupForPossibilityKey(conf[i]));
            }
            return list;
        }

    }
}
