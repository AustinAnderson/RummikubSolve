using RunsRainbowTableGenerator;
using System.Collections;

namespace SolverLogic.Models
{
    //use struct to have it all on stack, not sure if switch branch prediction stuff will make this slower than just using array though
    public struct BitVectorPerColor: IEnumerable<BitVector32>
    {
        private BitVector32 black;
        private BitVector32 red;
        private BitVector32 teal;
        private BitVector32 yellow;
        public void SetBitVector(TileColor color, BitVector32 value)
        {
            switch (color)
            {
                case TileColor.BLACK:
                    black = value;
                break;
                case TileColor.RED:
                    red = value;
                break;
                case TileColor.TEAL:
                    teal = value;
                break;
                default:
                    yellow = value;
                break;
            }
        }
        public BitVector32 GetBitVectorCopy(TileColor i)
        {
            return i switch
            {
                TileColor.BLACK => black,
                TileColor.RED => red,
                TileColor.TEAL => teal,
                _ => yellow
            };
        }
        public void SetColorBit(TileColor color, int bit, bool value)
        {
            switch (color)
            {
                case TileColor.BLACK:
                    black[bit] = value;
                break;
                case TileColor.RED:
                    red[bit] = value;
                break;
                case TileColor.TEAL:
                    teal[bit] = value;
                break;
                default:
                    yellow[bit] = value;
                break;
            }
        }
        public void SetBitVectorForColor(TileColor color, BitVector32 value)
        {
            switch (color)
            {
                case TileColor.BLACK:
                    black = value;
                break;
                case TileColor.RED:
                    red = value;
                break;
                case TileColor.TEAL:
                    teal = value;
                break;
                default:
                    yellow = value;
                break;
            }

        }
        /// <summary>
        /// this |= other for each color
        /// </summary>
        /// <param name="other"></param>
        public void OrEquals(ref BitVectorPerColor other)
        {
            black.OrEquals(ref other.black);
            red.OrEquals(ref other.red);
            teal.OrEquals(ref other.teal);
            yellow.OrEquals(ref other.yellow);
        }


        public IEnumerator<BitVector32> GetEnumerator()
        {
            for (int i = 0; i < 4; i++)
            {
                yield return GetBitVectorCopy((TileColor)i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
