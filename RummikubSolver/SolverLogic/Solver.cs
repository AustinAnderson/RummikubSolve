using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Stopwatch watch= new Stopwatch();
            watch.Start();
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

            //iterate over the possible selections of groupings for each number,
            //and for each set of groups, find the the number of tiles left over after 
            //putting all the remaining tiles in the maximum number of runs
            //the run scorer doesn't actually need to find the max number of runs,
            //as long as it gets the remaining count, and we have the solution key for the groups
            //by the time the group possibility iteration is done.
            var currentPossibilitySetFastCalc=new FastCalcTile[groups.Count*6];
            int currentDigit = 0;
            var done = false;
            //invalid should also get max int as score to make currentScore<score false
            int score = int.MaxValue;
            //which possibility on each group
            var solutionKey = new int[groups.Count];
            int currentPossibility = 0;
            while (!done)
            {
                int possibilitySetSize = 0;
                for(int i = 0; i < groups.Count; i++)
                {
                    groups[i].AddCurrentUnused(currentPossibilitySetFastCalc, ref possibilitySetSize);
                }
                int currentScore = RunScorer.Score(baseUnusedFastCalcArray, currentPossibilitySetFastCalc, possibilitySetSize);
                if (currentScore < score)
                {
                    score= currentScore;
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
            watch.Stop();
            Console.WriteLine("done in "+watch.Elapsed.TotalSeconds+ " seconds");
            if (score < int.MaxValue)
            {
                Console.WriteLine("Solution found at");
                Console.WriteLine($"[{string.Join(",", solutionKey)}]");
            }
            else
            {
                Console.WriteLine("no solution found");
            }
            //with the solution key, pick that configuration of groups,
            //and now actually find the most possible runs with the remaining tiles
            return score;
        }
    }
}
