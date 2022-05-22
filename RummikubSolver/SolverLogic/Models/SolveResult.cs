using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class SolveResult
    {
        //hold a reference to a tile so they can be swapped without
        //repositioning in the list
        private class RefTile
        {
            public RefTile(Tile tile)
            {
                Tile = tile;
            }
            public Tile Tile { get; private set; }
            public RefTile? HandEq { get; set; }
            public void SwapIfEquivalent()
            {
                if(HandEq != null)
                {
                    var temp = Tile;
                    Tile = HandEq.Tile;
                    HandEq.Tile = temp;
                }
            }
        }
        internal SolveResult(TileSetForCurrentHand allTiles,List<FastCalcTile[]> finalGroups, RunsAndRemainder finalRuns)
        {
            var tilesById = allTiles.Tiles.ToDictionary(t => t.Id, t => new RefTile(t));
            foreach(var refTile in tilesById.Values)
            {
                if(refTile.Tile.EquivalentHandTileId != null)
                {
                    refTile.HandEq = tilesById[refTile.Tile.EquivalentHandTileId.Value];
                }
            }
            var groupsRef = finalGroups.Select(fctArr => fctArr.Select(t=>tilesById[t.Id]).ToList()).ToList();
            var handRef = finalRuns.Remainder.Select(t => tilesById[t.Id]).ToList();
            var runsRef = finalRuns.Runs.Select(fctArr => fctArr.Select(t=>tilesById[t.Id]).ToList()).ToList();
            foreach(var tile in handRef)
            {
                //might have a tile that originally came from the board in the hand,
                //with an equivalent dup on the board that originally was in the hand,
                //if so, swap them for id tracking
                tile.SwapIfEquivalent();
            }
            Groups = groupsRef.Select(x => x.Select(y => y.Tile).ToList()).ToList();
            Runs = runsRef.Select(x => x.Select(y => y.Tile).ToList()).ToList();
            Hand = handRef.Select(x => x.Tile).ToList();
        }
        public List<List<Tile>> Groups { get; }
        public List<List<Tile>> Runs { get; }
        public List<Tile> Hand { get; }

    }
}
