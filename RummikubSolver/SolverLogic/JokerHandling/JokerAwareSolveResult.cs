using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class JokerAwareSolveResult
    {
        public List<JokerClearMove> JokerClearingMoves { get; set; }=new List<JokerClearMove>();
        public SolveResult SolveResult { get; set; }
    }
}
