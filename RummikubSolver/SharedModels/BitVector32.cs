using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunsRainbowTableGenerator
{
    public static class UInt32Ext
    {
        public static void Set(this ref uint dataRef, int index, bool value)
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
        public static bool Get(this uint dataRef, int index)
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
        public static int SetCount(this uint dataCpy)
        {
            int count = 0;
            while (dataCpy != 0)
            {
                count++;
                dataCpy &= dataCpy - 1;
            }
            return count;
        }
    }
    public struct BitVector32:IEquatable<BitVector32>
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
        public int SetCount => data.SetCount();
        public override string ToString() 
        {
            return Convert.ToString(Data, 2).PadLeft(32,'0');
        }
        public bool this[int index]
        {
            get => GetBit(data, index);
            set => SetBit(ref data, index, value);
        }
        /// <summary>
        /// this.Data |= other.Data;
        /// </summary>
        /// <param name="other"></param>
        public void OrEquals(ref BitVector32 other)
        {
            data |= other.data;
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
            => obj is BitVector32 other && Equals(other);
        public override int GetHashCode() => (int)data;
        public bool Equals(BitVector32 other) => data == other.data;

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
