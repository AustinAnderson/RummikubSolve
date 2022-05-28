using RunsRainbowTableGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public struct UsedTilesState
    {
        public UsedTilesState(TileSetForCurrentHand tileSet)
        {
            InvalidIfUnusedFlags = new BitVector32[4];
            UsedInGroupsFlags = new BitVector32[4];
            foreach (var tile in tileSet.Tiles)
            {
                if(tile.IsBoardTile && tile.EquivalentHandTileId == null)
                {
                    //           11   1111111222222
                    //0123456789012   3456789012345
                    //123456789ABCD   123456789ABCD
                    int ndx = tile.Number;
                    if (tile.Originality != 0)
                    {
                        ndx += 13;
                    }
                    InvalidIfUnusedFlags[(int)tile.Color][ndx] = true;
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
        public void ClearUsed()
        {
            for(int i = 0; i < UsedInGroupsFlags.Length; i++)
            {
                UsedInGroupsFlags[i] = new BitVector32();
            }
        }
        //set the flag according to key 123456789ABCD123456789ABCD
        //for used in the groups, then the int with match the index of the runs look up table
        public BitVector32[] UsedInGroupsFlags { get; }
    }
}
