using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class InitialHand : InitialList
    {
        public InitialHand() { }
        public InitialHand(InitialHand other)
        {
            foreach(var tile in other)
            {
                Add(new Tile(tile));
            }
        }
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
