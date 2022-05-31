using SharedModels;
using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic
{
    public class JokerHandlingSolver
    {
        private readonly Solver solver;

        public JokerHandlingSolver(IRunResultRainbowTable table)
        {
            solver = new Solver(table);
        }
        public SolveResult Solve(List<Tile> boardTiles, List<Tile> handTiles)
        {

            return null;
        }
    }
}
