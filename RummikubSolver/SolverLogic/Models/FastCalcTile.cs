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
        private const int NUMBER_MASK= 15 << 25;
        public static readonly FastCalcTile MaxValue = (FastCalcTile)int.MaxValue;
        /// <summary>
        /// bundles metadata on a tile into a single struct,
        /// the constraints aren't checked for speed
        /// </summary>
        /// <param name="originality">unique label for each instance of color/number combo</param>
        /// <param name="number">the rummikub number, 1-15</param>
        /// <param name="color">the rummikub color</param>
        /// <param name="isBoardTile">did it originally come from the board (true) or the player's hand (false)</param>
        /// <param name="hasEquivalentHandTile">if board tile, could it have come from the player hand instead</param>
        /// <param name="id">sequential id tracking tile accross duplicates</param>
        public FastCalcTile(byte originality, byte number, TileColor color, bool isBoardTile, bool hasEquivalentHandTile, ushort id)
        {
            if (originality > 3 || originality < 0) throw new ArgumentException($"invalid {nameof(originality)} '{originality}': must be 0-3 inclusive", nameof(originality));
            if (number > 13 || number < 1) throw new ArgumentException($"invalid {nameof(number)} '{number}', must be 1-13 inclusive",nameof(number));
            //    originality (dup count)
            //    |    color
            //    |      |  isBoard
            //    | num  |  | hasHandEquivalent
            //    | |    |  | | padding       id
            //  __|_|____|__|_|__/|            |
            // /  | |    |  | |   |            |
            // v  v v    v  v v   v            v
            // 0 00 0000 00 0 0 00000  0000000000000000
            // 0 00 0000 00 1 0 00000  0000000000000000 true
            // 0 00 0000 00 1 1 00000  0000000000000000 false
            // 0 00 0000 00 0 0 false
            //              0 1 false
            data =              originality << 29  |
                                     number << 25  | 
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
        public int Number => (NUMBER_MASK & data) >> 25;
        public int Originality => (int)(((uint)data) >> 29);
        public int Id => (ushort)data;
        public static string ToString(int number, int originality, TileColor color, bool isBoardTile, bool hasEquivalentHandTile, int id)
        {
            return $"{{{""+number,2}{color.Char()}" +
            $"({(isBoardTile?"B":"H")}{(hasEquivalentHandTile?"*":" ")}{originality})_" +
            $"{id:X2}}}";
        }
        public override string ToString() => ToString(Number, Originality, TileColor, IsBoardTile, HasEquivalentHandTile, Id);
        public string BitString => Convert.ToString(data, 2).PadLeft(32,'0');
        public static explicit operator int(FastCalcTile t) => t.data;
        public static explicit operator FastCalcTile(int i) => new FastCalcTile { data = i };
    }
}
