using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public struct BitVector128
    {
        public BitVector128(uint val)
        {
            low = val;
            high = 0;
        }
        private ulong high;
        private ulong low;
        public void Clear()
        {
            high = 0;
            low = 0;
        }
        public override string ToString()
        {
            return string.Join("",(Convert.ToString((long)high, 2) + Convert.ToString((long)low, 2)).Reverse());
        }
        public bool this[int index]
        {
            get
            {
                if (index >= 64)
                {
                    index -= 64;
                    return (1 & (high >> index))==1;
                }
                return (1 & (low >> index))==1;
            }
            set
            {
                if (index >= 64)
                {
                    index -= 64;
                    high = (high & (~(1UL<<index))) | ((value ? 1UL : 0UL) << index);
                }
                low = (low & (~(1UL<<index))) | ((value ? 1UL : 0UL) << index);
            }
        }
    }
}
