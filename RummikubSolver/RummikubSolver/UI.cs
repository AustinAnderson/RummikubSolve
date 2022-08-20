using SharedModels;
using SolverLogic;
using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RummikubSolver
{
    internal class UI
    {
        private readonly Solver solver;
        public UI(IRunResultRainbowTable table)
        {
            solver=new Solver(table);
        }

        public void Run()
        {
            var stateFile = new FileInfo("state.txt");
            while (true)
            {
                Console.WriteLine("press enter to read and solve state from "+stateFile.FullName);
                Console.ReadLine();
                Console.Clear();
                if (!stateFile.Exists)
                {
                    Console.WriteLine(stateFile.FullName+"doesn't exist, please create it");
                    continue;
                }
                Console.WriteLine("solving...");
                var text = File.ReadAllText(stateFile.FullName).ToUpper();
                var splits=text.Split("---");
                if (splits.Length != 2)
                {
                    Console.WriteLine("contents must be board then --- then hand");
                    continue;
                }
                try
                {
                    solver.Solve(
                        new TileSetForCurrentHand(
                            StringNotationParser.ParseBoardShort(splits[0]).Flattened,
                            StringNotationParser.ParseHand(splits[1]).ToList()
                        )
                    ).PrintResult();
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
