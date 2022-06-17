using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class CurrentBoard
    {
        public CurrentBoard() { }
        public CurrentBoard(CurrentBoard toClone)
        {
            foreach(var run in toClone.Runs)
            {
                Runs.Add(new InitialRun(run));
            }
            foreach(var group in toClone.Groups)
            {
                Groups.Add(new InitialGroup(group));
            }
        }
        public List<InitialRun> Runs { get; set; } = new List<InitialRun>();
        public List<InitialGroup> Groups { get; set; } = new List<InitialGroup>();
        public List<Tile> Flattened => Runs.Cast<InitialList>().Concat(Groups).SelectMany(x=>x).ToList();
    }
}
