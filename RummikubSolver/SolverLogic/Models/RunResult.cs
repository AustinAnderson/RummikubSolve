using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public struct RunResult
    {
        public static RunResult[] LookUpTable { get; private set; }
        public static void LoadLookUpTable(string path="")
        {
            var fullPath = "runsRainbowTable.dat";
            if(path!="") fullPath = Path.Combine(path, fullPath);
            var datFile=new FileInfo(fullPath);
            if (!datFile.Exists)
            {
                throw new FileNotFoundException(datFile.FullName);
            }
            LookUpTable = new RunResult[datFile.Length/4];
            using var readStream=new FileStream(datFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            int current = 0;
            var buffer=new Span<byte>(new byte[4], 0, 4);
            int read = 0;
            do
            {
                read = readStream.Read(buffer);
                LookUpTable[current] = new RunResult(buffer.ToArray());
            }
            while (read == 4);
        }

        private const uint UNUSED_FLAGS_MASK = (~1U) >> 6;
        public RunResult(byte[] bytes)
        {
            if (bytes.Length != 4)
            {
                throw new ArgumentException("byte array must have exactly 4 bytes", nameof(bytes) + ".Length");
            }
            //Not sure on this, since the dat file should be written by the write all bytes on the Bytes getter
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            Data = BitConverter.ToUInt32(bytes, 0);
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
                throw new ArgumentException("length must be 26", nameof(toEncode) + ".Length");
            }
            for (int i = 0; i < toEncode.Length; i++)
            {
                this[i] = toEncode[i];
            }
        }
        public uint Data { get; private set; }
        public uint Unused => Data & UNUSED_FLAGS_MASK;
        public byte[] Bytes => BitConverter.GetBytes(Data);
        public int ScoreIfValid
        {
            get => (int)((Data & ~UNUSED_FLAGS_MASK) >> 26);
            set
            {
                Data |= (((uint)value) << 26);
            }
        }
        public override string ToString()
        {
            return "" + ScoreIfValid + " " + string.Join("", Convert.ToString(Unused, 2).PadLeft(32, '0').Reverse());
        }
        public bool this[int index]
        {
            get => (1 & (Data >> index)) == 1;
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
