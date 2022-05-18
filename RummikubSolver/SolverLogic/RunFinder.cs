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
        public static RunsAndRemainder FindRuns(IEnumerable<FastCalcTile> unused)
        {
            var unusedNonDup=new List<FastCalcTile>();
            var dups=new List<FastCalcTile>();
            FastCalcTile last = default;
            foreach(var tile in  unused.OrderBy(x => (int)x))
            {
                if (tile.Number == last.Number && tile.TileColor==last.TileColor)
                {
                    dups.Add(tile);
                }
                else
                {
                    unusedNonDup.Add(tile);
                }
                last= tile;
            }
            var runsByColor = new Dictionary<TileColor, List<List<FastCalcTile>>>();
            foreach (var color in Enum.GetValues<TileColor>())
            {
                runsByColor[color] = new List<List<FastCalcTile>>();
            }
            foreach (var tile in unusedNonDup.Concat(dups))
            {
                var top = runsByColor[tile.TileColor].LastOrDefault()?.LastOrDefault()??default;
                if((int)top!=(int)default(FastCalcTile)&&top.Number+1 == tile.Number)
                {
                    runsByColor[tile.TileColor].Last().Add(tile);
                }
                else
                {
                    runsByColor[tile.TileColor].Add(new List<FastCalcTile> { tile });
                }
            }
            var results = new RunsAndRemainder
            {
                Remainder = new List<FastCalcTile>(),
                Runs = new List<FastCalcTile[]>()
            };
            foreach(var cluster in runsByColor.SelectMany(kvp => kvp.Value))
            {
                if(cluster.Count >= 3)
                {
                    results.Runs.Add(cluster.ToArray());
                }
                else
                {
                    results.Remainder.AddRange(cluster);
                }
            }
            results.Runs=results.Runs.OrderBy(x=>(int)x[0]).ToList();
            results.Remainder=results.Remainder.OrderBy(x=>(int)x).ToList();
            return results;
        }
    }
}
