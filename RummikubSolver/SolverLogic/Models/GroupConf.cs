using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public struct GroupConf
    {
        const int SwitchToHighIndex = 64 / 3;
        const ulong ValueMask = 0b00000111;
        //up to 6 options for a MaxGroup, encode as 3 bits
        //1-13 x2 = 26 possible groups = 78 total bits needed,
        //most times will only need low long
        public GroupConf(int[] values)
        {
            low = 0;
            high = 0;
            for(int i = 0; i < values.Length; i++)
            {
                if (i < SwitchToHighIndex)
                {
                    //assumes int will be <= 6
                    low |= (((ulong)values[i]) << (i * 3));
                }
                else
                {
                    high |= (ushort)(((ushort)values[i]) << ((i-SwitchToHighIndex) * 3));
                }
            }
        }
        public int this[int index]
        {
            get
            {
                if (index < SwitchToHighIndex)
                {
                    //TODO: explore if faster to use 3 ints to avoid moving from extended register to default
                    return (int)((low >> (index * 3)) & ValueMask);
                }
                else
                {
                    return (high >> ((index-SwitchToHighIndex) * 3)) & ((ushort)ValueMask);
                }
            }
        }
        private ulong low;
        private ushort high;

        public string BitString
        {
            get
            {
                var s=Convert.ToString(high, 2);
                return s+" "+Convert.ToString((long)low,2);
            }
        }
        public override string ToString()
        {
            int max = (16 + 64) / 3;
            string digits = "";
            for(int i = 0; i < max; i++)
            {
                digits += this[i];
            }
            return digits;
        }

    }
}
