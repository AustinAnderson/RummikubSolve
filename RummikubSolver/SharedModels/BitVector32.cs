using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunsRainbowTableGenerator
{
    public struct BitVector32
    {
        public uint Data { get; private set; }
        public BitVector32()
        {
            Data=0;
        }
        //why doesn't the built in version support this :(
        public BitVector32(uint initial)
        {
            Data = initial;
        }
        public override string ToString() 
        {
            return string.Join("", Convert.ToString(Data, 2).Reverse());
        }
        public bool this[int index]
        {
            get => (1 & (Data>>index)) == 1;
            set
            {
                //clear the bit by anding with not of that bit, then set it to true or false
                Data = (Data & (~(1U << index))) | ((value ? 1U : 0) << index);
            }
        }
    }
}
