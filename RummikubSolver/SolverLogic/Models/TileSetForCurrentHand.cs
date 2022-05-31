using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class TileSetForCurrentHand
    {
        //TileSet should be the only thing that can set these properties,
        //and it should use a copy of the tiles passed in
        //borderline inheritance abuse, but suits the needs neatly
        public class TileSetTile : Tile
        {
            public TileSetTile(Tile orig) : base(orig) { }
            public new int? EquivalentHandTileId { 
                get => base.EquivalentHandTileId; 
                set => base.EquivalentHandTileId = value;
            }
            public new bool IsBoardTile
            {
                get => base.IsBoardTile;
                set => base.IsBoardTile = value;
            }
            public new int Id
            {
                get => base.Id;
                set => base.Id = value;
            }
            public new int Originality
            {
                get => base.Originality;
                set => base.Originality = value;
            }
        }
        public TileSetForCurrentHand(List<Tile> boardTiles,List<Tile> handTiles)
        {
            var boardList=boardTiles.Select(x => new TileSetTile(x)).ToList();
            var handList=handTiles.Select(x => new TileSetTile(x)).ToList();
            foreach(var tile in boardList)
            {
                tile.IsBoardTile = true;
            }
            foreach(var tile in handList)
            {
                tile.IsBoardTile= false;
            }
            var tiles = boardList.Concat(handList).ToList();
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
                    originalities[(tiles[i].Number, tiles[i].Color)] = 1;
                }
                tiles[i].Id = i;
                if (tiles[i].SameValue(tiles[i + 1]) && tiles[i].IsBoardTile && !tiles[i + 1].IsBoardTile)
                {
                    //sorted by number,color,!isBoard; that means if two next to each other
                    //first is board and second is hand
                    //set to i+1 since that's the id that will be assigned to tiles[i+1] next iteration
                    tiles[i].EquivalentHandTileId = i + 1;
                }
            }
            //handle the fact the loop stops one early
            tiles[tiles.Count-1].Id = tiles[tiles.Count-2].Id+1;
            if(originalities.TryGetValue((tiles[tiles.Count-1].Number,tiles[tiles.Count-1].Color),out int orig))
            {
                tiles[tiles.Count-1].Originality = orig;
            }
            Tiles = tiles.OrderBy(t=>t.Originality).ThenBy(t=>t.Number).ThenBy(t=>t.Color).ToList();
        }
        public IReadOnlyList<Tile> Tiles { get; }
    }
}
