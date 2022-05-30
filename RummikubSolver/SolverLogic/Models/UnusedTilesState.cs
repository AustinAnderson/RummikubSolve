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
            InvalidIfUnusedFlags = new BitVectorPerColor();
            UnusedInGroupsFlags = new BitVectorPerColor();
        }
        public UnusedTilesState(TileSetForCurrentHand tileSet)
        {
            InvalidIfUnusedFlags = new BitVectorPerColor();
            UnusedInGroupsFlags = new BitVectorPerColor();
            foreach (var tile in tileSet.Tiles)
            {
                if(tile.IsBoardTile && tile.EquivalentHandTileId == null)
                {
                    //I'm pretty sure this would return a copy of the bv32 and then set a bit on the copy
                    //InvalidIfUnusedFlags[(int)tile.Color][tile.CanonicalIndex] = true;
                    //so 
                    InvalidIfUnusedFlags.SetColorBit(tile.Color, tile.CanonicalIndex, true);
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
        public BitVectorPerColor InvalidIfUnusedFlags;
        //set the flag according to key 123456789ABCD123456789ABCD
        //for used in the groups, then the int with match the index of the runs look up table
        /// <summary>
        /// get private set
        /// </summary>
        public BitVectorPerColor UnusedInGroupsFlags;
        public void ClearToBaseUnused(ref UnusedTilesState baseUnused)
        {
            //rely on value type (FourArray) copy to reinitialize without changing baseUnused
            //on subseqent changing
            UnusedInGroupsFlags = baseUnused.UnusedInGroupsFlags;
        }

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
            for(int i= 0; i < 4; i++)
            {
                sb.Append(((TileColor)i).Char());
                for(int j = 0; j < 32; j++)
                {
                    sb.Append(ToStringLookup[(
                        InvalidIfUnusedFlags.GetBitVectorCopy((TileColor)i)[j],
                        UnusedInGroupsFlags.GetBitVectorCopy((TileColor)i)[j]
                    )]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
