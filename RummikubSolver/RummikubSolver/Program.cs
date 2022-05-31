// See https://aka.ms/new-console-template for more information
using RunsRainbowTableGenerator;
using SharedModels;
using SolverLogic;
using SolverLogic.Models;
using System.Diagnostics;

Console.WriteLine("Loading table...");
var watch = new Stopwatch();
watch.Start();
var table=new RunResultRainbowTable(Environment.ExpandEnvironmentVariables("%TEMP%"));
watch.Stop();
Console.WriteLine("Table load done in "+watch.Elapsed.TotalSeconds+ " seconds");
var solver = new JokerHandlingSolver(table);
var res = solver.Solve(
        /*
                ("").Split(new[] { ',', ' ' }).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Tile(x.Trim())).ToList(),
        "9T,10B,3B,9R,7Y,9R,5T,10B,4T,12R,13B,1R,2B,13R,5Y,11Y,7T,11Y,12Y,5Y,2T".Split(",").Select(x => new Tile(x.Trim())).ToList()
        /*/
                ("6Y,7Y,8Y,9Y,10Y 2B,3B,4B,5B,6B " +
            "12B,12T,12R  11B,11Y,11R 10B,10T,10Y " +
                "12Y,12T,12R  2R,3R,4R 8B,9B,10B   4B,4T,4Y,4R   1B,1T,1Y   11T,11B,11Y " +
                "5R,5Y,5T,5B").Split(new[] { ',', ' ' }).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Tile(x.Trim())).ToList(),
        "2Y,7B,9B,1T,4T,3Y,5Y,12Y,1R,3R,9R,9R,8R,7T,3B,4Y".Split(",").Select(x => new Tile(x.Trim())).ToList()
        //*/
);
foreach(var group in res.Groups)
{
    Console.WriteLine(string.Join(" ",group.Select(t=>t.DisplayString)));
}
foreach(var run in res.Runs)
{
    Console.WriteLine(string.Join(" ", run.Select(t=>t.DisplayString)));
}
Console.WriteLine("------------------------------------------------------------------");
Console.WriteLine(string.Join("        ", res.Hand.Select(t=>t.DisplayString)));
//which one is missing is which bit is set when counting in bin
//1 2 3 4 5 | 0 0 0 0 0
//1 2 3 4   | 0 0 0 0 1
//int that encodes which we have map to int which encodes which are unused
//then we can have an int which encodes which are invalid if unused,
//if & the two != 0 then invalid, else score is bit count of unused
//which can be stored in the top 6 bits of the lookup value

