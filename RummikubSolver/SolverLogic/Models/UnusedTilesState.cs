using RunsRainbowTableGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public struct UnusedTilesState
    {
        public UnusedTilesState()
        {
            InvalidIfUnusedFlags = new BitVector32[4];
            UnusedInGroupsFlags = new BitVector32[4];
        }
        public UnusedTilesState(TileSetForCurrentHand tileSet)
        {
            InvalidIfUnusedFlags = new BitVector32[4];
            UnusedInGroupsFlags = new BitVector32[4];
            foreach (var tile in tileSet.Tiles)
            {
                if(tile.IsBoardTile && tile.EquivalentHandTileId == null)
                {
                    InvalidIfUnusedFlags[(int)tile.Color][tile.CanonicalIndex] = true;
                }
            }
        }
        //set the flag according to key 123456789ABCD123456789ABCD
        //if that particular tile is invalid if unused, then can be anded with
        //the RunResult from the lookup to get 0 if valid or anything else invalid
        /// <summary>
        /// true in the index matching 123456789ABCD123456789ABCD for a given color index is invalid
        /// <para>
        /// intended to be fully read only, don't set bits
        /// </para>
        /// </summary>
        public BitVector32[] InvalidIfUnusedFlags { get; }
        public void ClearToBaseUnused(ref UnusedTilesState baseUnused)
        {
            for(int i = 0; i < UnusedInGroupsFlags.Length; i++)
            {
                //bv32 is struct so this makes a copy so further changes
                //dont change baseUnused
                UnusedInGroupsFlags[i] = baseUnused.UnusedInGroupsFlags[i];
            }
        }
        //set the flag according to key 123456789ABCD123456789ABCD
        //for used in the groups, then the int with match the index of the runs look up table
        public BitVector32[] UnusedInGroupsFlags { get; }

        private static readonly Dictionary<(bool, bool), char> ToStringLookup = new Dictionary<(bool, bool), char>
        {
            //inv,   unused
            {(false, false),'0'},
            {(false,  true),'1'},
            {( true, false),'O'},
            {( true,  true),'I'},
        };
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("_123456789ABCD123456789ABCD");
            for(int i= 0; i < UnusedInGroupsFlags.Length; i++)
            {
                sb.Append(((TileColor)i).Char());
                for(int j = 0; j < 32; j++)
                {
                    sb.Append(ToStringLookup[(InvalidIfUnusedFlags[i][j],UnusedInGroupsFlags[i][j])]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
