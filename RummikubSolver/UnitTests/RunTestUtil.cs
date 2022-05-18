using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class RunTestUtil
    {
        public static FastCalcTile MakeFastCalcTile(string desc)
        {
            var num = desc[0] switch
            {
                'A' => "10",
                'B' => "11",
                'C' => "12",
                'D' => "13",
                'E' => "14",
                'F' => "15",
                _ => ""+desc[0]
            };

            var tile = new Tile(num + desc[1]);
            tile.IsBoardTile = desc[2] == 'b';
            if(desc.Length > 3 && desc[3]=='*')
            {
                tile.EquivalentHandTile = tile;
            }
            return tile.ToFastCalcTile();
        }
    }
}
