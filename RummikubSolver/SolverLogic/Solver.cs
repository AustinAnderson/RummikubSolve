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
            int[] solutionKey = new int[groups.Count];
            int score = int.MaxValue;
            foreach(var (unusedSet, unusedCount) in new MaxGroupIterable(groups)) 
            { 
                int currentScore = RunScorer.Score(baseUnusedFastCalcArray, unusedSet, unusedCount);
                if (currentScore < score)
                {
                    score= currentScore;
                    for(int i = 0; i < groups.Count; i++)
                    {
                        solutionKey[i] = groups[i].CurrentPossibilityKey;
                    }
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
