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
        public async Task<int> Solve()
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
            var confs = new GroupConf[expectedPossibilitiesCount];
            var done = false;
            int currentDigit = 0;
            int currentPossibility = 0;
            int[] lasts = groups.Select(x => x.PossibilityCount - 1).ToArray();
            //which possibility on each group
            int[] maxConfs = new int[groups.Count];
            while (!done)
            {
                confs[currentPossibility] = new GroupConf(maxConfs);
                if (maxConfs[currentDigit]==lasts[currentDigit])
                {
                    while (!done && maxConfs[currentDigit]==lasts[currentDigit])
                    {
                        currentDigit++;
                        if(currentDigit >= groups.Count)
                        {
                            done = true;
                        }
                    }
                    if (!done)
                    {
                        maxConfs[currentDigit]++;
                        for(int i = 0; i < currentDigit; i++)
                        {
                            maxConfs[i] = 0;
                        }
                        currentDigit = 0;
                    }
                }
                else
                {
                    maxConfs[currentDigit]++;
                }
                currentPossibility++;
            }

            int numThreads = 4;
            var tasks=new Task<(GroupConf,int)>[numThreads];
            int chunkSize=expectedPossibilitiesCount / numThreads;
            for(int i = 0; i < numThreads; i++)
            {
                int lBound = i*chunkSize;
                int uBound = (i+1)*chunkSize;
                tasks[i] = Task.Run(() =>
                {
                    GroupConf solutionKey = default;
                    int score = int.MaxValue;
                    var groupIterable = new MaxGroupIterable(groups);
                    for (int j = lBound; j < uBound; j++)
                    {
                        var conf = confs[j];
                        var (unusedSet, unusedCount) = groupIterable.GetUnusedForKey(conf);
                        int currentScore = RunScorer.Score(baseUnusedFastCalcArray, unusedSet, unusedCount);
                        if (currentScore < score)
                        {
                            score = currentScore;
                            solutionKey = conf;
                        }
                    }
                    return (solutionKey, score);
                });
            }
            var res=await Task.WhenAll(tasks);
            var (solkey,finalScore)=res.OrderBy(x => x.Item2).First();
            watch.Stop();
            Console.WriteLine("done in "+watch.Elapsed.TotalSeconds+ " seconds");
            if (finalScore < int.MaxValue)
            {
                Console.WriteLine("Solution found at");
                Console.WriteLine($"[{solkey}]");
            }
            else
            {
                Console.WriteLine("no solution found");
            }
            //with the solution key, pick that configuration of groups,
            //and now actually find the most possible runs with the remaining tiles
            return finalScore;
        }
    }
}
