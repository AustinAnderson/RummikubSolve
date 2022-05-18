// See https://aka.ms/new-console-template for more information
using SolverLogic;
using SolverLogic.Models;

Console.WriteLine("Hello, World!");
var solver = new Solver(

                ("6Y,7Y,8Y,9Y,10Y 2B,3B,4B,5B,6B " +
            "12B,12T,12R  11B,11Y,11R 10B,10T,10Y " +
                "12Y,12T,12R  2R,3R,4R 8B,9B,10B   4B,4T,4Y,4R   1B,1T,1Y   11T,11B,11Y " +
                "5R,5Y,5T,5B").Split(new[] { ',', ' ' }).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Tile(x.Trim())).ToList(),
        "2Y,7B,9B,1T,4T,3Y,5Y,12Y,1R,3R,9R,9R,8R,7T,3B,4Y".Split(",").Select(x => new Tile(x.Trim())).ToList()
);
var res = await solver.Solve();
foreach(var group in res.Groups)
{
    Console.WriteLine(string.Join(" ",group));
}
foreach(var run in res.Runs)
{
    Console.WriteLine(string.Join(" ", run));
}
Console.WriteLine("------------------------------------------------------------------");
Console.WriteLine(string.Join("        ", res.Hand));
