using System;
using System.Collections.Generic;
using System.Linq;
using Rummikub.Logic;
using Rummikub.Models;
using Rummikub.Util;

namespace Rummikub
{
    class Program
    {
        static PlayerView ParseState(string handString, string boardString)
        {
            return new PlayerView{
                Board= boardString.Split("\r\n").SelectMany(x=>x.Split("\n")).SelectMany(x=>x.Split(" "))
                .Select(x=>x.Trim()).Where(x=>!string.IsNullOrWhiteSpace(x))
                .Select(x=>
                    x.Split(',').Select(x=>x.Trim()).Where(x=>!string.IsNullOrWhiteSpace(x))
                    .Select(x=>new Tile(x)).ToList()
                ).ToList(),
                Hand=handString.Split("\r\n").SelectMany(x=>x.Split("\n")).SelectMany(x=>x.Split(","))
                               .Select(x=>x.Trim()).Where(x=>!string.IsNullOrEmpty(x)).Select(x=>new Tile(x)).ToList()
            };
        }
        static void Main(string[] args)
        {
            Console.WriteLine("hello");
            string hand=@"2Y,7B,9B,1T,4T,3Y,5Y,12Y,1R,3R
            9R,9R,8R,7T,3B,4Y";
            /*
            string board=@"6Y,7Y,J,9Y,10Y 2B,3B,4B,5B,6B
            12B,12T,12R 11B,11Y,11R 10B,10T,10Y
            J,12T,12R 2R,3R,4R 8B,9B,10B 4B,4T,4Y,4R 1B,1T,1Y 11T,11B,11Y
            5R,5Y,5T,5B";
            */
            string board=@"6Y,7Y,8Y,9Y,10Y 2B,3B,4B,5B,6B
            12B,12T,12R  11B,11Y,11R 10B,10T,10Y
            12Y,12T,12R  2R,3R,4R 8B,9B,10B   4B,4T,4Y,4R   1B,1T,1Y   11T,11B,11Y
            5R,5Y,5T,5B";
            
            /*
            Console.WriteLine("enter hand, double enter to finish");
            string hand="";
            while(tryReadLine(out var line))
            {
                hand+=line;
            }
            Console.WriteLine("enter board, double enter to finish");
            string board="";
            while(tryReadLine(out var line))
            {
                board+=line;
            }
            */
            var initialState=ParseState(hand,board);
            var res=Solver.Solve(initialState);
            /*foreach(var print in Solve(initialState)){
                Console.WriteLine(print);
                Console.WriteLine();
                Console.WriteLine();
            }
            */

            Console.WriteLine(initialState.Print());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(res.ToPlayerView().Print());
        }
        static bool tryReadLine(out string res){
            res=Console.ReadLine();
            return !string.IsNullOrWhiteSpace(res);
        }
    }
}
