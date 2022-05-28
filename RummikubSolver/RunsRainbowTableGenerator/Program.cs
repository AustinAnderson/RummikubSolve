// See https://aka.ms/new-console-template for more information

using RunsRainbowTableGenerator.Logic;
using SharedModels;

Console.WriteLine("Hello, World!");
RunSolver solver = new RunSolver();
var results = new RunResult[(1 << RunSolver.TilesOfSingleColor.Count)];
for(uint i = 0; i < results.Length; i++)
{
    results[i] = solver.SolveForPossibility(i);
    if (i % 100000 == 0)
    {
        //2^26=67108864, so pad left 8 digits
        Console.Write($"\rsolved {"" + i,8}/{1 << RunSolver.TilesOfSingleColor.Count}");
    }
}
Console.WriteLine();
Console.WriteLine($"solved all combinations, writing solutions...");
var path = new FileInfo(
    Path.Combine(
        Environment.ExpandEnvironmentVariables("%TEMP%"),
        RunResultRainbowTable.RUNS_RAINBOW_TABLE_FILE_NAME
    )
).FullName;
await using (var resultFile = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: false))
{
    await using (var writer = new BinaryWriter(resultFile))
    {
        foreach (var item in results)
        {
            writer.Write(item.Data);
        }
    }
}
Console.WriteLine("wrote solution to " + path);
