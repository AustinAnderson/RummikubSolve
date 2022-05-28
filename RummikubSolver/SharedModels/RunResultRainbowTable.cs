using System.Runtime.Serialization.Formatters.Binary;

namespace SharedModels
{
    public class RunResultRainbowTable
    {
        public const string RUNS_RAINBOW_TABLE_FILE_NAME = "runsRainbowTable.bin";
        private static RunResult[]? values;
        public static void Load(string path="")
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
        public static RunResult Get(int i)
        {
            if(values == default)//this if should be branch predicted away
            {
                throw new ArgumentException($"{nameof(Load)} must be called before the table can be used");
            }
            return values[i];
        }
    }
}
