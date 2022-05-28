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
        //why doesn't the built in version support this :(
        public BitVector32(uint initial)
        {
            data = initial;
        }
        public BitVector32(bool[] init)
        {
            data = 0;
            for(int i = 0; i < init.Length; i++)
            {
                this[i] = true;
            }
        }
        public override string ToString() 
        {
            return string.Join("", Convert.ToString(Data, 2).Reverse());
        }
        public bool this[int index]
        {
            get => GetBit(data, index);
            set => SetBit(ref data, index, value);
        }
        public static bool GetBit(uint dataRef, int index)
        {
            return (1 & (dataRef>>index)) == 1;
        }
        public static void SetBit(ref uint dataRef, int index, bool value)
        {
            //clear the bit by anding with not of that bit, then set it to true or false
            dataRef = (dataRef & (~(1U << index))) | ((value ? 1U : 0) << index);
        }
    }
}
