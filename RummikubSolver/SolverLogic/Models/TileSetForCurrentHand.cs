using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class TileSetForCurrentHand
    {
        public TileSetForCurrentHand(List<Tile> boardTiles,List<Tile> handTiles)
        {
            foreach(var tile in boardTiles)
            {
                tile.IsBoardTile = true;
            }
            foreach(var tile in handTiles)
            {
                tile.IsBoardTile= false;
            }
            var tiles = boardTiles.Concat(handTiles).ToList();
            var originalities = new Dictionary<(int num, TileColor color), int>();
            tiles.Sort(Tile.Comparer);
            for(int i = 0; i < tiles.Count-1; i++)
            {
                if(originalities.TryGetValue((tiles[i].Number,tiles[i].Color),out int originality))
                {
                    tiles[i].Originality = originality;
                    originalities[(tiles[i].Number, tiles[i].Color)]++;
                }
                else
                {
                    originalities[(tiles[i].Number, tiles[i].Color)] = 0;
                }
                tiles[i].Id = i;
                if (tiles[i].SameValue(tiles[i + 1]) && tiles[i].IsBoardTile && !tiles[i + 1].IsBoardTile)
                {
                    //sorted by number,color,!isBoard; that means if two next to each other
                    //first is board and second is hand
                    tiles[i].EquivalentHandTileId = tiles[i + 1].Id;
                }
            }
            tiles[tiles.Count-1].Id = tiles[tiles.Count-2].Id+1;
            Tiles = tiles;
        }
        public IReadOnlyList<Tile> Tiles { get; }
    }
}
