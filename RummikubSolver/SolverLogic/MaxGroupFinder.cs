using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic
{
    internal class MaxGroupFinder
    {
        public static List<MaxGroup> FindMaxGroups(TileSet tileSet, out List<Tile> groupBaseUnused)
        {
            //tileset is always a sorted list
            //pull out all the dups and put them on the end
            //then group adjacent same numbered tiles, allowing the dups to fall into their own group
            //this presents an edge case when there are 2 dups, where it should be split into 2 3-groups,
            //but will be split into 1 4-group and 2 unused, so handle that manually
            //BRTYBR -> BRTY  BR, need to move the Y over

            //this kinda thing would normally be a linked list type use case,
            //but will only run at most 26*26 times over a list that is at most 106 long
            //so not worth the hassle of converting between list types
            groupBaseUnused = new List<Tile>();
            var dups=new List<Tile>();
            groupBaseUnused.AddRange(tileSet.Tiles);
            for(int i=groupBaseUnused.Count-1; i>0; i--)
            {
                if (groupBaseUnused[i].SameValue(groupBaseUnused[i - 1]))
                {
                    dups.Add(groupBaseUnused[i]);
                    groupBaseUnused.RemoveAt(i);
                }
            }
            dups.Reverse();
            for(int i = 0; i < dups.Count; i++)
            {
                groupBaseUnused.Add(dups[i]);
            }
            //groupBaseUnused now has the sorted lists laid end to end
            //group consecutive same numbered tiles each into it's own list
            //1,1,2,2,4,5,6,7,7,7 becomes
            //{1,1},{2,2},{4},{5},{6},{7,7,7}
            var potentialGroups = new List<List<Tile>>();
            bool newAdd = true;
            for(int i = 0; i < groupBaseUnused.Count-1; i++)
            {
                if (newAdd)
                {
                    potentialGroups.Add(new List<Tile> { groupBaseUnused[i] });
                }
                if(groupBaseUnused[i].Number==groupBaseUnused[i + 1].Number)
                {
                    potentialGroups[potentialGroups.Count-1].Add(groupBaseUnused[i+1]);
                    newAdd = false;
                }
                else
                {
                    newAdd = true;
                }
            }
            //handle the edge case of a group of 4 with 2 dups
            potentialGroups = potentialGroups.OrderBy(x => x[0].Number).ThenByDescending(x => x.Count).ToList();
            for (int i = 0; i < potentialGroups.Count - 1; i++)
            {
                if (potentialGroups[i].Count == 4 && potentialGroups[i + 1].Count == 2 && potentialGroups[i][0].Number == potentialGroups[i + 1][0].Number)
                {
                    potentialGroups[i].Add(potentialGroups[i + 1][0]);
                    potentialGroups[i].Add(potentialGroups[i + 1][1]);
                }
            }

            //with the groups of 4 that have 2 dups converted to groups of 6,
            //add the groups out of the above list that have a length >=3 to the final set,
            //the "groups of 6" should be handled by MaxGroup constructor
            var groups = new List<MaxGroup>();
            for (int i = 0; i < potentialGroups.Count; i++)
            {
                if (potentialGroups[i].Count >= 3)
                {
                    groups.Add(new MaxGroup(potentialGroups[i]));
                    //if one of the potential groups got chosen, all of it's tiles have to be removed from the unused list
                    if (groupBaseUnused.RemoveAll(x => potentialGroups[i].Contains(x)) != potentialGroups[i].Count)
                        throw new ArgumentException($"unabled to find one or more tile to remove for selected group {i}");
                }
            }
            return groups; 
        }
    }
}
