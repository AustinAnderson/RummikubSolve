using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunsRainbowTableGenerator
{
    /*
      unused
     /  set count     unused flags (set if number in that position unused)
    v   v                  v
    0 00000 00000000000000000000000000
            DCBA987654321DCBA987654321
    */
    /// <summary>
    /// encodes a bool array of if the tile number in that position is unused according to key 123456789ABCD123456789ABCD with the second set being the dups
    /// <para>
    /// the top 6 bits encode the unused count. Because we're tracking unused as true, all bits default to used
    /// </para>
    /// </summary>
    public struct RunResult
    {
        private const uint UNUSED_FLAGS_MASK = (~1U) >> 6;
        public RunResult(byte[] bytes)
        {
            if(bytes.Length != 4)
            {
                throw new ArgumentException("byte array must have exactly 4 bytes",nameof(bytes)+".Length");
            }
            //Not sure on this, since the dat file should be written by the write all bytes on the Bytes getter
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            Data=BitConverter.ToUInt32(bytes, 0);
        }
        public RunResult(uint data)
        {
            Data = data;
        }
        public RunResult(bool[] toEncode)
        {
            Data = 0;
            if (toEncode?.Length != 26)
            {
                throw new ArgumentException("length must be 26",nameof(toEncode)+".Length");
            }
            for(int i=0; i < toEncode.Length; i++)
            {
                this[i]=toEncode[i];
            }
        }
        public uint Data { get; private set; }
        public uint Unused => Data & UNUSED_FLAGS_MASK;
        public byte[] Bytes => BitConverter.GetBytes(Data);
        public int ScoreIfValid
        {
            get => (int)((Data & ~UNUSED_FLAGS_MASK)>>26);
            set
            {
                Data |= (((uint)value) << 26);
            }
        } 
        public override string ToString() 
        {
            return ""+ScoreIfValid+" "+string.Join("", Convert.ToString(Unused, 2).PadLeft(32,'0').Reverse());
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
        public static explicit operator uint(RunResult res) => res.Data;
        public static explicit operator RunResult(uint data) => new RunResult(data);
    }
}
