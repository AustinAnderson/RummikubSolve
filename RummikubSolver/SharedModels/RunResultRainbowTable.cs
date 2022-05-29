using System.Runtime.Serialization.Formatters.Binary;

namespace SharedModels
{
    public interface IRunResultRainbowTable
    {
        RunResult GetFor(uint possibility);
    }

    public class RunResultRainbowTable:IRunResultRainbowTable
    {
        public const string RUNS_RAINBOW_TABLE_FILE_NAME = "runsRainbowTable.bin";
        private RunResult[] values;
        public RunResultRainbowTable(string path="")
        {
            var fullPath = RUNS_RAINBOW_TABLE_FILE_NAME;
            if(path!=null) fullPath = Path.Combine(path, fullPath);
            var datFile=new FileInfo(fullPath);
            if (!datFile.Exists)
            {
                throw new FileNotFoundException(datFile.FullName);
            }
            values=new RunResult[datFile.Length/4];
            using var readStream=new FileStream(datFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader=new BinaryReader(readStream);
            int current = 0;
            while(reader.BaseStream.Position != reader.BaseStream.Length)
            {
                values[current]=new RunResult(reader.ReadUInt32());
                current++;
            }
        }
        public RunResult GetFor(uint i) => values[i];
    }
}
