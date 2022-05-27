﻿// See https://aka.ms/new-console-template for more information

using RunsRainbowTableGenerator;

Console.WriteLine("Hello, World!");
var tilesOfSingleColor = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
RunSolver solver = new RunSolver();
var results = new RunResult[(1 << tilesOfSingleColor.Length)];
for(uint i = 0; i < (1 << tilesOfSingleColor.Length); i++)
{
    List<int> solveWith=new List<int>();
    List<int> solveWithDups=new List<int>();
    for(int j = 0; j < tilesOfSingleColor.Length; j++)
    {
        if(!new BitVector32(i)[j])
        {
            if (j < 13)
            {
                solveWith.Add(tilesOfSingleColor[j]);
            }
            else
            {
                solveWithDups.Add(tilesOfSingleColor[j]);
            }
        }
    }
    results[i] = solver.Solve(solveWith,solveWithDups);
    if (i % 100000 == 0)
    {
        //2^26=67108864, so pad left 8 digits
        Console.Write($"\rsolved {"" + i,8}/{1 << tilesOfSingleColor.Length}");
    }
}
Console.WriteLine();
Console.WriteLine($"solved all combinations, writing solutions...");
var path = new FileInfo(".\\runsRainbowTable.dat").FullName;
await using (var resultFile = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: false))
{
    foreach (var item in results)
    {
        foreach (byte b in item.Bytes)
        {
            resultFile.WriteByte(b);
        }
    }
}
Console.WriteLine("wrote solution to " + path);