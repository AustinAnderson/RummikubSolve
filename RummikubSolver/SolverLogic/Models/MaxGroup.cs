using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class MaxGroup
    {
        private readonly List<FastCalcTile> allGroup;
        private int selected;
        public int PossibilityCount { get; private set; }
        public int CurrentPossibilityKey => selected;
        public bool IsAtLast => selected == PossibilityCount - 1;

        public MaxGroup(List<Tile> tilesFound)
        {
            allGroup = tilesFound.Select(x=>x.ToFastCalcTile()).ToList();
            //if 3, all or nothing,
            //if 4, all, nothing, and the 4 3-groups leaving out 1
            //if 6, same as 4 but also the possibility of 2 3-groups at the same time
            PossibilityCount = 7;
            if (allGroup.Count == 3)
            {
                PossibilityCount = 2;
            }
            else if (allGroup.Count == 4)
            {
                PossibilityCount = 6;
            }
            string errorCtx = $"invalid group [{string.Join(",", tilesFound.Select(x => x.DebugDisplay))}]: ";
            if(!new[] { 3, 4, 6 }.Contains(allGroup.Count))
            {
                throw new ArgumentException(errorCtx+"size must be either 3, 4, or 6");
            }
            int max = tilesFound.Count;
            if (max > 4) max= 4;
            int num = tilesFound[0].Number;
            var lastColor=tilesFound[0].Color;
            for(int i = 1; i < max; i++)
            {
                if(tilesFound[i].Number != num)
                {
                    throw new ArgumentException(errorCtx+"numbers in groups must match");
                }
                if(tilesFound[i].Color <= lastColor)
                {
                    throw new ArgumentException(errorCtx+"the non duplicate colors must be sequential");
                }
                lastColor = tilesFound[i].Color;
            }
        }
        public List<FastCalcTile[]?> GetGroupForPossibilityKey(int key)
        {
            FastCalcTile[]? group = null;
            //last is all used
            if (key == PossibilityCount - 1)
            {
                group = allGroup.ToArray();
            }
            else if (key != 0)
            {

                //copy all the tiles from the all group that aren't allGroup[possibilityCount-1]
                if (PossibilityCount - 1 < allGroup.Count)
                {
                    group=new FastCalcTile[allGroup.Count - 1];
                }
                else
                {
                    group=new FastCalcTile[allGroup.Count];
                }
                int index = 0;
                for (int i = 0; i < allGroup.Count; i++)
                {
                    if (i != PossibilityCount - 1)
                    {
                        group[index]=allGroup[i];
                        index++;
                    }
                }
            }
            else
            {
                //else key == 0, all unused, return empty group
                group = new FastCalcTile[] { };
            }
            return new List<FastCalcTile[]?> { group };
        }
        public void AddCurrentUnused(FastCalcTile[] addTo, ref int addLocation)
        {
            if(selected == 0)
            {
                for(int i = 0; i < allGroup.Count; i++)
                {
                    addTo[addLocation]=allGroup[i];
                    addLocation++;
                }
            }
            else
            {
                //if(possibilityCount==2) done, using all tiles
                //1-4= return that tile as unused
                //else possibility #6: selected==5 and so using all the tiles, no add
                if (selected < 5)
                {
                    addTo[addLocation] = allGroup[selected - 1];
                    addLocation++;
                }
                //selected==5 same as 4 tile case (using the first 4), but add the two dups to unused
                //if we have the 2 dups add them unless all used
                if (PossibilityCount == 7 && selected != 6)
                {
                    addTo[addLocation] = allGroup[4];
                    addLocation++;
                    addTo[addLocation] = allGroup[5];
                    addLocation++;
                }
            }
        }

        public void MoveNext()
        {
            selected++;
        }
        public void ResetIteration()
        {
            selected = 0;
        }
        public static IComparer<MaxGroup> Comparer { get; } = Comparer<MaxGroup>.Create((x, y) => x.allGroup[0].Number - y.allGroup[0].Number);
        public override string ToString() => $"({selected}/{PossibilityCount - 1})[{string.Join(",", allGroup.Select(x => x.ToString()))}])";
    }
}
