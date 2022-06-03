using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class JokerAwareSolveResult
    {
        List<JokerClearMove> JokerClearingMoves { get; set; }
        SolveResult SolveResult { get; set; }
    }
}
