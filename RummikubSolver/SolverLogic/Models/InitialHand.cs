using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class InitialHand : InitialList
    {
        public override void UpdateJokerValues()
        {
            //no op, jokers can be anything
        }

        protected override void ValidateModification(Tile[] preposedNewState)
        {
            //no op, anything valid
        }
    }
}
