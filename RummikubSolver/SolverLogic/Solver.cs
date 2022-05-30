﻿using RunsRainbowTableGenerator.Logic;
using SharedModels;
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
        private readonly IRunResultRainbowTable rainbowTable;

        public Solver(TileSetForCurrentHand tileSet,IRunResultRainbowTable rainbowTable)
        {
            this.tileSet = tileSet;
            this.rainbowTable = rainbowTable;
        }
        public async Task<SolveResult> Solve()
        {
            Stopwatch watch= new Stopwatch();
            watch.Start();
            var groups = MaxGroupFinder.FindMaxGroups(tileSet, out List<Tile> groupBaseUnused);
            var baseUnusedTiles= new UnusedTilesState();
            foreach(var tile in groupBaseUnused)
            {
                baseUnusedTiles.UnusedInGroupsFlags.SetColorBit(tile.Color, tile.CanonicalIndex, true);
            }
            var unusedTilesState = new UnusedTilesState(tileSet);
            groups.Sort(MaxGroup.Comparer);
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

            int score = int.MaxValue;
            GroupConf solutionKey = default;
            var groupIterable = new MaxGroupIterable(groups);
            var scorer = new RunScorer(rainbowTable);
            UnusedTilesState solutionGroupUnused;
            for(int i = 0; i < confs.Length; i++)
            {
                var conf = confs[i];
                unusedTilesState.ClearToBaseUnused(ref baseUnusedTiles);
                groupIterable.MarkUnusedForConf(ref unusedTilesState,conf);
                int currentScore = scorer.Score(ref unusedTilesState);
                if (currentScore < score)
                {
                    solutionGroupUnused = unusedTilesState;
                    score = currentScore;
                    solutionKey = conf;
                }
            }
            watch.Stop();
            Console.WriteLine("done in "+watch.Elapsed.TotalSeconds+ " seconds");
            if (score < int.MaxValue)
            {
                Console.WriteLine("Solution found at");
                Console.WriteLine($"[{solutionKey}]");
            }
            else
            {
                Console.WriteLine("no solution found");
            }
            //with the solution key, pick that configuration of groups,
            var allGroups = new MaxGroupIterable(groups);
            var finalGroups = allGroups.GetGroupsForKey(solutionKey);
            //and now actually find the most possible runs with the remaining tiles

            var finder = new RunFinder(new RunSolver());
            var finalRuns = finder.FindRuns(
                tileSet.Tiles.Except(finalGroups.SelectMany(t=>t))
            );
            return new SolveResult(tileSet,finalGroups,finalRuns);
        }
    }
}
