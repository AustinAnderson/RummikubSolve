using System;
namespace Rummikub.Models
{
    public class Tile
    {
        //126 tiles in a game
        private static byte currentId=0;
        public static byte NextId=>currentId++;
        public Tile(string rep)
        {
            if(rep=="J"){
                IsJoker=true;
                Color=TileColor.Black;
            }
            else{
                if(rep.Length<2||rep.Length>3) {
                    throw new ArgumentException($"length must be 2 or 3 chars, got '{rep}'");
                }
                Color=rep[rep.Length-1] switch {
                    'B' => TileColor.Black,
                    'Y' => TileColor.Yellow,
                    'T' => TileColor.Teal,
                    'R' => TileColor.Red,
                    _=> throw new ArgumentException($"last char of tile must be char in range [BYTR], got '{rep[rep.Length-1]}'")
                };
                Number=int.Parse(rep.Substring(0,rep.Length-1));
            }
        }
        public override string ToString()=>""+ID+":"+(IsJoker?"J":(""+Number+Color.Letter));
        public string PrintString()
        {
            string result=TileColor.TileBG+"("+ID.ToString("X2")+")|"+Color?.ANSICode??TileColor.TileBG;
            if(IsJoker){
                result+=" J";
            }
            else{
                result+=(""+Number).PadLeft(2,' ');
            }
            return result+ TileColor.TileBG+ "|"+ TileColor.ResetCode;
        }
        public byte ID {get;} =NextId;
        public bool IsJoker {get;}
        public TileColor Color {get;}
        public int Number {get;}

        /* should reference equals
        public override bool Equals(object obj)
        {
            return (obj as Tile)?.ID==ID;
        }
        public override int GetHashCode()
        {
            return (((byte)(Color?.Letter??0))<<8|Number)*(IsJoker?-1:1);
        }
        */
    }
}

