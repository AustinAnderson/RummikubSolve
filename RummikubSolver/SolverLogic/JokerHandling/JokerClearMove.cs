using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class JokerClearMove
    {
        public InitialList InitialRunOrGroupWithJoker { get; set; }
        public List<InitialList> AfterClearing { get; set; }
    }
}
