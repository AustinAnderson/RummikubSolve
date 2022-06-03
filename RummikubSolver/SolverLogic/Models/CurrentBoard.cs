using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class CurrentBoard
    {
        public List<InitialRun> Runs { get; set; } = new List<InitialRun>();
        public List<InitialGroup> Groups { get; set; } = new List<InitialGroup>();
    }
}
