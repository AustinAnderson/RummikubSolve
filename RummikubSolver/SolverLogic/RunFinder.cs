using RunsRainbowTableGenerator.Logic;
using SharedModels;
using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic
{
    public class RunFinder
    {
        private RunSolver solver;
        public RunFinder(RunSolver solver)
        {
            this.solver = solver;
        }
        public RunsAndRemainder FindRuns(IEnumerable<Tile> unused)
        {
            var unusedList=unused.ToList();
            unusedList.Sort(Tile.Comparer);
            var result = new RunsAndRemainder
            {
                Runs = new List<Tile[]>(),
                Remainder = new List<Tile>()
            };


            var mapBack = new Tile[4][];
            mapBack[(int)TileColor.BLACK] = new Tile[26];
            mapBack[(int)TileColor.RED] = new Tile[26];
            mapBack[(int)TileColor.TEAL] = new Tile[26];
            mapBack[(int)TileColor.YELLOW] = new Tile[26];
            foreach(var tile in unusedList)
            {
                mapBack[(int)tile.Color][tile.CanonicalIndex] = tile;
            }


            var origs = new List<int>[4]
            {
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>()
            };
            var dups = new List<int>[4]
            {
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>()
            };
            foreach(var tile in unusedList)
            {
                if (tile.Originality == 0)
                {
                    origs[(int)tile.Color].Add(tile.Number);
                }
                else
                {
                    dups[(int)tile.Color].Add(tile.Number);
                }
            }

            for(int i = 0; i < 4; i++)
            {
                foreach(var potential in solver.GetPotentialRuns(origs[i], dups[i]))
                {
                    if(potential.Count < 3)
                    {
                        foreach (var num in potential) 
                        {
                            result.Remainder.Add(mapBack[i][num.Index]);
                        }
                    }
                    else
                    {
                        result.Runs.Add(potential.Select(num => mapBack[i][num.Index]).ToArray());
                    }
                }

            }
            result.Remainder.Sort(Tile.Comparer);
            return result;
        }
    }
}
