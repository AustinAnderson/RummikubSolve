using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public struct FastCalcTile
    {
        private const int IS_BOARD_TILE_MASK = 2 << 21;
        private const int HAS_EQUIVALENT_HAND_TILE_MASK = 1 << 21;
        private const int BOARD_WITH_EQUIVALENT_MASK = IS_BOARD_TILE_MASK| HAS_EQUIVALENT_HAND_TILE_MASK;
        private const int COLOR_MASK = 3 << 23;
        public static readonly FastCalcTile MaxValue = (FastCalcTile)int.MaxValue;
        /// <summary>
        /// bundles metadata on a tile into a single struct,
        /// the constraints aren't checked for speed
        /// </summary>
        /// <param name="originality"></param>
        /// <param name="number">the rummikub number, 1-15</param>
        /// <param name="color">the rummikub color</param>
        /// <param name="isBoardTile">did it originally come from the board (true) or the player's hand (false)</param>
        /// <param name="hasEquivalentHandTile">if board tile, could it have come from the player hand instead</param>
        /// <param name="id">sequential id tracking tile accross duplicates</param>
        public FastCalcTile(byte number, TileColor color, bool isBoardTile, bool hasEquivalentHandTile, ushort id)
        {
            //        color
            //          |  isBoard
            //     num  |  | hasHandEquivalent
            //      |   |  | | padding       id
            //  ____|___|__|_|__/|            |
            // /    |   |  | |   |            |
            // v    v   v  v v   v            v
            // 000 0000 00 0 0 00000  0000000000000000
            // 000 0000 00 1 0 00000  0000000000000000 true
            // 000 0000 00 1 1 00000  0000000000000000 false
            // 000 0000 00 0 0 false
            //             0 1 false
            data =                  (number << 25) | 
                            ((ushort)color) << 23  | 
                      (isBoardTile ? 1 : 0) << 22  | 
            (hasEquivalentHandTile ? 1 : 0) << 21  | 
                                               id;


        }
        private int data;
        public bool IsBoardTile => (data & IS_BOARD_TILE_MASK) == IS_BOARD_TILE_MASK;
        public bool HasEquivalentHandTile => (data & HAS_EQUIVALENT_HAND_TILE_MASK) == HAS_EQUIVALENT_HAND_TILE_MASK;
        public bool IsInvalidIfUnused => (data & BOARD_WITH_EQUIVALENT_MASK) == IS_BOARD_TILE_MASK;
        public TileColor TileColor => (TileColor)((COLOR_MASK & data) >> 23);
        public int Number => (int)(((uint)data) >> 25);
        public int Id => (ushort)data;
        public override string ToString() => $"{{{""+Number,2}{TileColor.Char()}" +
            $"({(IsBoardTile?"B":"H")}{(HasEquivalentHandTile?"*":" ")})_" +
            $"{Id:X2}}}";
        public string BitString => Convert.ToString(data, 2);
        public static explicit operator int(FastCalcTile t) => t.data;
        public static explicit operator FastCalcTile(int i) 
        {
            var t=new FastCalcTile();
            t.data=i;
            return t;
        }

    }
}
