﻿using RunsRainbowTableGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
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
    [Serializable]
    public struct RunResult
    {

        private const uint SCORE_IF_VALID_MASK = 0b111111;
        public RunResult(uint data)
        {
            this.data = data;
        }
        public RunResult(bool[] toEncode)
        {
            data = 0;
            if (toEncode?.Length != 26)
            {
                throw new ArgumentException("length must be 26",nameof(toEncode)+".Length");
            }
            for(int i=0; i < toEncode.Length; i++)
            {
                this[i]=toEncode[i];
            }
        }
        private uint data;
        public uint Data => data;
        public uint Unused => data & ~SCORE_IF_VALID_MASK;
        public ushort ScoreIfValid
        {
            get => (ushort)(data & SCORE_IF_VALID_MASK);
            set
            {
                data = Unused; //clear out score
                data |= value;
            }
        } 
        public override string ToString() 
        {
            return ""+ScoreIfValid+" "+Convert.ToString(Unused, 2).PadLeft(32,'0').Substring(0,26);
        }
        public bool this[int index]
        {
            get => BitVector32.GetBit(data, index);
            set => BitVector32.SetBit(ref data, index, value);
        }
        public static explicit operator uint(RunResult res) => res.data;
        public static explicit operator RunResult(uint data) => new RunResult(data);
    }
}
