﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    internal struct SizeAndExcludeList
    {
        public int size;
        public int[] excludes;
    }
    public class MaxGroup
    {
        public const int Possibilities_3G = 2;
        public const int Possibilities_4G = 6;
        public const int Possibilities_6G = 7;
        private static readonly SizeAndExcludeList[][] SizeAndExcludeMapByPossibilitySelected;
        static MaxGroup()
        {
            SizeAndExcludeMapByPossibilitySelected = new SizeAndExcludeList[8][];
            SizeAndExcludeMapByPossibilitySelected[Possibilities_3G] = new SizeAndExcludeList[]
            {
                new SizeAndExcludeList{ size = 0, excludes = new int[] {0,1,2}},
                new SizeAndExcludeList{ size = 3, excludes = new int[] {}},
            };
            SizeAndExcludeMapByPossibilitySelected[Possibilities_4G] = new SizeAndExcludeList[]
            {
                new SizeAndExcludeList{ size = 0, excludes = new int[] {0,1,2,3}},
                new SizeAndExcludeList{ size = 3, excludes = new int[] {0}},
                new SizeAndExcludeList{ size = 3, excludes = new int[] {1}},
                new SizeAndExcludeList{ size = 3, excludes = new int[] {2}},
                new SizeAndExcludeList{ size = 3, excludes = new int[] {3}},
                new SizeAndExcludeList{ size = 4, excludes = new int[] {}}
            };
            SizeAndExcludeMapByPossibilitySelected[Possibilities_6G] = new SizeAndExcludeList[]
            {
                new SizeAndExcludeList{ size = 0, excludes = new int[] {0,1,2,3,4,5}},
                new SizeAndExcludeList{ size = 3, excludes = new int[] {0,4,5}},
                new SizeAndExcludeList{ size = 3, excludes = new int[] {1,4,5}},
                new SizeAndExcludeList{ size = 3, excludes = new int[] {2,4,5}},
                new SizeAndExcludeList{ size = 3, excludes = new int[] {3,4,5}},
                new SizeAndExcludeList{ size = 4, excludes = new int[] {4,5}},
                new SizeAndExcludeList{ size = 6, excludes = new int[] {}}
            };
        }
        private readonly FastCalcTile[] allGroup;
        private int selected;
        public int PossibilityCount { get; private set; }
        public int CurrentPossibilityKey => selected;
        public bool IsAtLast => selected == PossibilityCount - 1;

        public MaxGroup(List<Tile> tilesFound)
        {
            allGroup = tilesFound.Select(x=>x.ToFastCalcTile()).ToArray();
            //if 3, all or nothing,
            //if 4, all, nothing, and the 4 3-groups leaving out 1
            //if 6, same as 4 but also the possibility of 2 3-groups at the same time
            PossibilityCount = Possibilities_6G;
            if (allGroup.Length== 3)
            {
                PossibilityCount = Possibilities_3G;
            }
            else if (allGroup.Length== 4)
            {
                PossibilityCount = Possibilities_4G;
            }
            string errorCtx = $"invalid group [{string.Join(",", tilesFound.Select(x => x.DebugDisplay))}]: ";
            if(!new[] { 3, 4, 6 }.Contains(allGroup.Length))
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
        public FastCalcTile[] GetGroupForPossibilityKey(int key)
        {
            var res=SizeAndExcludeMapByPossibilitySelected[PossibilityCount][key];
            if (res.size == 0) return new FastCalcTile[] { };
            if (IsAtLast) return allGroup;
            var group = new FastCalcTile[res.size];
            int index = 0;
            for(int i = 0; i < allGroup.Length; i++)
            {
                if(!res.excludes.Contains(i))
                {
                    group[index] = allGroup[i];
                    index++;
                }
            }
            return group;
            /*
            //last is all used
            if (key == PossibilityCount - 1)
            {
                group = allGroup.ToArray();
            }
            else if (key != 0)
            {

                //copy all the tiles from the all group that aren't allGroup[possibilityCount-1]
                if (selected < PossibilityCount - 1)
                {
                    if (PossibilityCount == 7) { }
                    group=new FastCalcTile[allGroup.Count - 1];
                }
                else if (PossibilityCount == 7 && )
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
                    if (i != selected - 1 && i< PossibilityCount)
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
            return group;
            */
        }
        public void AddCurrentUnused(FastCalcTile[] addTo, ref int addLocation)
        {
            if(selected == 0)
            {
                for(int i = 0; i < allGroup.Length; i++)
                {
                    addTo[addLocation]=allGroup[i];
                    addLocation++;
                }
            }
            else if (PossibilityCount != 2)
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
        public override string ToString()
        {
            var unused = new FastCalcTile[10];
            int refAddLoc = 0;
            AddCurrentUnused(unused, ref refAddLoc);
            var reps=new List<string>();
            foreach(var tile in allGroup)
            {
                var selectionIndicator = ">";
                if (unused.Contains(tile))
                {
                    selectionIndicator = " ";
                }
                reps.Add(selectionIndicator+tile);
            }
            return $"({selected}/{PossibilityCount - 1})[{string.Join(",", reps)}])";
        }
    }
}