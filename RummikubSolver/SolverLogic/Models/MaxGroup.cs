using System;
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
        private readonly List<Tile> allGroup;
        public int PossibilityCount { get; private set; }
        private BitVectorPerColor[] orsPerPossibility;
        public MaxGroup(List<Tile> tilesFound)
        {
            allGroup = tilesFound;
            //if 3, all or nothing,
            //if 4, all, nothing, and the 4 3-groups leaving out 1
            //if 6, same as 4 but also the possibility of 2 3-groups at the same time
            PossibilityCount = Possibilities_6G;
            if (allGroup.Count== 3)
            {
                PossibilityCount = Possibilities_3G;
            }
            else if (allGroup.Count == 4)
            {
                PossibilityCount = Possibilities_4G;
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
            orsPerPossibility = new BitVectorPerColor[PossibilityCount];
            for(int key = 0; key < PossibilityCount; key++)
            {
                var res=SizeAndExcludeMapByPossibilitySelected[PossibilityCount][key];
                for(int i = 0; i < allGroup.Count; i++)
                {
                    if(res.excludes.Contains(i))
                    {
                        orsPerPossibility[key].SetColorBit(allGroup[i].Color, allGroup[i].CanonicalIndex, true);
                    }
                }
            }
        }
        public Tile[] GetGroupForPossibilityKey(int key)
        {
            var res=SizeAndExcludeMapByPossibilitySelected[PossibilityCount][key];
            if (res.size == 0) return new Tile[] { };
            var group = new Tile[res.size];
            int index = 0;
            for(int i = 0; i < allGroup.Count; i++)
            {
                if(!res.excludes.Contains(i))
                {
                    group[index] = allGroup[i];
                    index++;
                }
            }
            return group;
            
        }
        public void MarkUnusedForSelected(ref UnusedTilesState unusedTiles, int key)
        {
            unusedTiles.UnusedInGroupsFlags.OrEquals(ref orsPerPossibility[key]);
        }
        public static IComparer<MaxGroup> Comparer { get; } = Comparer<MaxGroup>.Create((x, y) => x.allGroup[0].Number - y.allGroup[0].Number);
        public override string ToString()
        {
            var reps=new List<string>();
            foreach(var tile in allGroup)
            {
                reps.Add(tile.ToString());
            }
            return $"{PossibilityCount}[{string.Join(",", reps)}]";
        }
    }
}
