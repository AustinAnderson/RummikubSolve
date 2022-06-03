using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.JokerHandling
{
    public class JokerSolveOption
    {
        public List<InitialList> PreSolved { get; set; }
        public CurrentBoard BoardWithoutPreSolved { get; set; }
        public InitialHand HandWithoutPreSolved { get; set; }
    }
}
