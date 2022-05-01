using System;
namespace Rummikub.Models
{
    public class HandOrBoardTile:IComparable<HandOrBoardTile>
    {
        public HandOrBoardTile(bool isBoardTile,Tile tile){
            IsBoardTile=isBoardTile;
            Tile=tile;
        }
        public bool IsBoardTile;
        public Tile Tile;
        //jokers to be handled outside of solving by doing multiple runs
        public bool SameValue(HandOrBoardTile otherTile)
            =>Tile.Color==otherTile.Tile.Color&&
              Tile.Number==otherTile.Tile.Number;
        //from a board tile, this property should be a ref
        //to the hand tile if any with the same Color and Number
        public HandOrBoardTile BoardHandDuplicateTile=null;
        public void SwapBoardHandDup(){
            if(BoardHandDuplicateTile==null) return;
            var tmp=BoardHandDuplicateTile.Tile;
            BoardHandDuplicateTile.IsBoardTile=!BoardHandDuplicateTile.IsBoardTile;
            BoardHandDuplicateTile.Tile=Tile;
            Tile=tmp;
            IsBoardTile=!this.IsBoardTile;
        }
        public override string ToString()
        {
            return (IsBoardTile?" ":"H")+Tile.ToString();
        }

        public int CompareTo(HandOrBoardTile other)
        {
            int res=Tile.Number.CompareTo(other.Tile.Number);
            if(res==0) res=Tile.Color.Letter.CompareTo(other.Tile.Color.Letter);
            return res;
        }
    }
}

