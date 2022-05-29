using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunsRainbowTableGenerator
{
    public struct BitVector32
    {
        private uint data;
        public uint Data => data;
        public BitVector32()
        {
            data=0;
        }
        public BitVector32(uint initial)
        {
            data = initial;
        }
        public BitVector32(bool[] init)
        {
            data = 0;
            for(int i = 0; i < init.Length; i++)
            {
                this[i] = init[i];
            }
        }
        public override string ToString() 
        {
            return Convert.ToString(Data, 2).PadLeft(32,'0');
        }
        public bool this[int index]
        {
            get => GetBit(data, index);
            set => SetBit(ref data, index, value);
        }
        public static bool GetBit(uint dataRef, int index)
        {
            //            12345678901234567890123456789012
            uint mask = 0b10000000000000000000000000000000;
            //  ==1 ==0
            //    vv
            //00001010100001001001001001010
            //----------------------------
            //00001000000000000000000000000
            //00000100000000000000000000000
            return (dataRef & (mask >> index)) != 0;
        }
        public static void SetBit(ref uint dataRef, int index, bool value)
        {
            //            12345678901234567890123456789012
            uint mask = 0b10000000000000000000000000000000;
            uint setBit= 0;
            if (value)
            {
                setBit = mask;
            }
            //clear the bit by anding with not of that bit, then set it to true or false
            dataRef = (dataRef & (~(mask >> index)) | (setBit >> index));
        }
    }
}
