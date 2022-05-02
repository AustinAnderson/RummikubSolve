using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic
{
    public class Solver
    {
        private TileSet tileSet;
        public Solver(List<Tile> boardTiles, List<Tile> handTiles)
        {
            foreach(var tile in boardTiles)
            {
                tile.IsBoardTile = true;
            }
            foreach(var handTile in handTiles)
            {
                handTile.IsBoardTile = false;
            }
            tileSet = new TileSet(boardTiles.Concat(handTiles).ToList());
        }
        public int Solve()
        {
            var groups = MaxGroupFinder.FindMaxGroups(tileSet, out List<Tile> groupBaseUnused);
            groups.Sort(MaxGroup.Comparer);
            var baseUnusedFastCalcArray=groupBaseUnused.Select(t=>t.ToFastCalcTile()).OrderBy(x=>(int)x).ToArray();
            int expectedPossibilitiesCount = 1;
            foreach(var group in groups)
            {
                expectedPossibilitiesCount *= group.PossibilityCount;
                Console.WriteLine(group);
            }
            Console.WriteLine();
            Console.WriteLine($"Checking {expectedPossibilitiesCount} possibilities...");

            var currentPossibilitySetFastCalc=new FastCalcTile[groups.Count*6];
            int currentDigit = 0;
            var done = false;
            //invalid should also get max int as score to make currentScore<score false
            int score = int.MaxValue;
            //which possibility on each group
            var solutionKey = new int[groups.Count];
            var currentConfigStr = "";
            int currentPossibility = 0;
            while (!done)
            {
                int possibilitySetSize = 0;
                currentConfigStr = "";
                for(int i = 0; i < groups.Count; i++)
                {
                    groups[i].AddCurrentUnused(currentPossibilitySetFastCalc, ref possibilitySetSize);
                    currentConfigStr += "" + groups[i].CurrentPossibilityKey;
                }
                if (currentConfigStr == "2005050001640")
                {
                    Console.WriteLine("yay");
                }
                int currentScore = RunScorer.Score(baseUnusedFastCalcArray, currentPossibilitySetFastCalc, possibilitySetSize);
                if (currentScore < score)
                {
                    for(int i = 0; i < groups.Count; i++)
                    {
                        solutionKey[i] = groups[i].CurrentPossibilityKey;
                    }
                }
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
                if (currentPossibility % 100000 == 0)
                {
                    Console.WriteLine($"{currentPossibility} possibilities visited");
                }
            }
            Console.WriteLine("done");
            if (score < int.MaxValue)
            {
                Console.WriteLine("Solution found at");
                Console.WriteLine($"[{string.Join(",", solutionKey)}]");
            }
            else
            {
                Console.WriteLine("no solution found");
            }
            return score;
        }
    }
}
