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
        private TileSetForCurrentHand tileSet;
        public Solver(TileSetForCurrentHand tileSet)
        {
            this.tileSet = tileSet;
        }
        public async Task<SolveResult> Solve()
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

            int numThreads = 1;
            var tasks=new Task<(GroupConf,int)>[numThreads];
            int chunkSize=expectedPossibilitiesCount / numThreads;
            int finalScore = 0;
            GroupConf solkey = default;
            for(int i = 0; i < numThreads; i++)
            {
                int lBound = i*chunkSize;
                int uBound = (i+1)*chunkSize;
                //tasks[i] = Task.Run(() =>
                //{
                    RunScorer scorer=new RunScorer();
                    GroupConf solutionKey = default;
                    int score = int.MaxValue;
                    var groupIterable = new MaxGroupIterable(groups);
                //for (int j = lBound; j < uBound; j++)
                //{
                var conf = new GroupConf(new[] {2,0,0,1,0,5,0,0,0,1,1,4,1});//confs[j];
                        var unused = groupIterable.GetUnusedForKey(conf);
                        int currentScore = scorer.Score(baseUnusedFastCalcArray, ref unused);
                        if (currentScore < score)
                        {
                            score = currentScore;
                            solutionKey = conf;
                        }
                    //}
                finalScore = score;
                solkey = solutionKey;
                    //return (solutionKey, score);
                //});
            }
            //var res=await Task.WhenAll(tasks);
            //var (solkey,finalScore)=res.OrderBy(x => x.Item2).First();
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
            var allGroups= new MaxGroupIterable(groups);
            var finalGroups=allGroups.GetGroupsForKey(solkey);
            //and now actually find the most possible runs with the remaining tiles
            var finalRuns = RunFinder.FindRuns(
                baseUnusedFastCalcArray.Concat(allGroups.GetUnusedForKey(solkey).Trim()).ToList()
            );
            return new SolveResult(tileSet,finalGroups,finalRuns);
        }
    }
}
