using System;
namespace Rummikub.Models
{
    public class TileColor
    {
        public char Letter { get; }
        public string ANSICode { get; }
        public const string ResetCode = "\u001b[0m";
        public const string TileBG="\u001b[1;30;47m";
        private TileColor(string ansiiCode,char letter){
            ANSICode=ansiiCode;
            Letter=letter;
        }
        public static TileColor Red=new TileColor("\u001b[0;31;47m",'R');
        public static TileColor Yellow=new TileColor("\u001b[1;33;47m",'Y');
        public static TileColor Teal=new TileColor("\u001b[1;34;47m",'T');
        public static TileColor Black=new TileColor("\u001b[0;30;47m",'B');
    }
}

