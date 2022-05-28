using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public struct UsedTilesState
    {
        public UsedTilesState()
        {
            InvalidIfUnusedFlags = new BitVector32[4];
            UsedInGroupsFlags = new BitVector32[4];
        }
        //set the flag according to key 123456789ABCD123456789ABCD
        //if that particular tile is invalid if unused, then can be anded with
        //the RunResult from the lookup to get 0 if valid or anything else invalid
        public BitVector32[] InvalidIfUnusedFlags { get; } = new BitVector32[4];
        //assuming my bitvec32 works the same as the built in...
        //set the flag according to key 123456789ABCD123456789ABCD
        //for used in the groups, then the int with match the index of the runs look up table
        public BitVector32[] UsedInGroupsFlags { get; } = new BitVector32[4];
    }
}
