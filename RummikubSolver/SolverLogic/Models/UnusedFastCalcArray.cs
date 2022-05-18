using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public struct UnusedFastCalcArray
    {
        public FastCalcTile[] Set;
        public int Count;
        public IEnumerable<FastCalcTile> Trim() => Set.Take(Count);
    }
}
