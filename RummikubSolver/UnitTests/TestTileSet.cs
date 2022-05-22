using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class TestTileSet
    {
        private int idCounter=0;
        public FastCalcTile MakeFastCalcTile(string desc, bool includeId=true)
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
            if (includeId)
            {
                tile.Id = idCounter++;
            }
            tile.IsBoardTile = desc[2] == 'b';
            if(desc.Length > 3 && desc[3]=='*')
            {
                tile.EquivalentHandTileId = 0;//calc just checks if null
            }
            return tile.ToFastCalcTile();
        }
    }
}
