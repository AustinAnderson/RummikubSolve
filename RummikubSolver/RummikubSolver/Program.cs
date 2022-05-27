// See https://aka.ms/new-console-template for more information
using SolverLogic;
using SolverLogic.Models;

/*
Console.WriteLine("Hello, World!");
var solver = new Solver(
    new TileSetForCurrentHand(
                ("6Y,7Y,8Y,9Y,10Y 2B,3B,4B,5B,6B " +
            "12B,12T,12R  11B,11Y,11R 10B,10T,10Y " +
                "12Y,12T,12R  2R,3R,4R 8B,9B,10B   4B,4T,4Y,4R   1B,1T,1Y "+//  11T,11B,11Y " +
                "5R,5Y,5T,5B").Split(new[] { ',', ' ' }).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Tile(x.Trim())).ToList(),
        "2Y,7B,9B,1T,4T,3Y,5Y,12Y,1R,3R,9R,9R,8R,7T,3B,4Y".Split(",").Select(x => new Tile(x.Trim())).ToList()
    )
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
*/

static string StrRep(List<int> l)
{
    string s = "";
    foreach(var item in l)
    {
        s += item switch
        {
            13 => "D",
            12 => "C",
            11 => "B",
            10 => "A",
            _ => "" + item
        };
    }
    return s;
}
//2^26-1 possibilities
var arr = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
for(uint i = 0; i < ((1<<arr.Length)); i++)
{
    List<int> solveWith=new List<int>();
    for(int j = 0; j<arr.Length; j++)
    {
        if(!new BitVector128(i)[j])
        {
            solveWith.Add(arr[j]);
        }
    }

}
static int Solve(List<int> list)
{
    for(int i = 0; i < list.Count; i++)
    {

    }
    return 0;
}
//which one is missing is which bit is set when counting in bin
//1 2 3 4 5 | 0 0 0 0 0
//1 2 3 4   | 0 0 0 0 1
//int that encodes which we have map to int which encodes which are unused
//then we can have an int which encodes which are invalid if unused,
//if & the two != 0 then invalid, else score is bit count of unused
//which can be stored in the top 6 bits of the lookup value


//... but first have to make an algo to accurately find the runs
