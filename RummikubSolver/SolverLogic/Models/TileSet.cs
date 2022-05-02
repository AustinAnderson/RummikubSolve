using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    internal class TileSet
    {
        public TileSet(List<Tile> tiles)
        {
            tiles.Sort(Tile.Comparer);
            for(int i = 0; i < tiles.Count-1; i++)
            {
                tiles[i].Id = i;
                if (tiles[i].SameValue(tiles[i + 1]) && tiles[i].IsBoardTile && !tiles[i + 1].IsBoardTile)
                {
                    //sorted by number,color,!isBoard; that means if two next to each other
                    //first is board and second is hand
                    tiles[i].EquivalentHandTile = tiles[i + 1];
                }
            }
            Tiles = tiles;
        }
        public IReadOnlyList<Tile> Tiles { get; }
    }
}
