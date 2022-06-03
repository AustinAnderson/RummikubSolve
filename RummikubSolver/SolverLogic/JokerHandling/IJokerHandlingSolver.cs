using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.JokerHandling
{
    internal interface IJokerHandlingSolver
    {
        JokerAwareSolveResult Solve(CurrentBoard board, InitialHand hand);
    }
}
