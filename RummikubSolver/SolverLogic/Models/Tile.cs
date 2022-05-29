using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    [DebuggerDisplay("{DebugDisplay}")]
    public class Tile
    {
        private readonly static Regex parser = new Regex("^([0-9]?[0-9])([BRTY])$",RegexOptions.Compiled);
        /// <summary>
        /// parse tile of the form (J|[0-9]?[0-9][BRTY])
        /// </summary>
        /// <param name="strRep"></param>
        /// <exception cref="NotImplementedException"></exception>
        public Tile(string strRep)
        {
            IsBoardTile = false;
            if(strRep == "J")
            {
                IsJoker = true;
                Number = 0;
                Color = TileColor.BLACK;
            }
            else
            {
                IsJoker= false;
                var match=parser.Match(strRep);
                if (!match.Success) throw new ArgumentException($"invalid string rep '{strRep}', must match (J|[0-9][0-9][BRTY])");
                Number = int.Parse(match.Groups[1].Value);
                Color = match.Groups[2].Value switch
                {
                    "B" => TileColor.BLACK,
                    "R" => TileColor.RED,
                    "T" => TileColor.TEAL,
                    "Y" => TileColor.YELLOW,
                    _ => throw new NotImplementedException($"no {nameof(TileColor)} for char '{match.Groups[2].Value}'")
                };
            }
        }
        public int? EquivalentHandTileId { get; set; }
        public bool IsBoardTile { get; set; }
        public int Number { get; private set; }
        public int Id { get; set; }
        public int Originality { get; set; }
        /// <summary>
        /// the index of the bit array for key 123456789ABCD123456789ABCD
        /// </summary>
        public int CanonicalIndex => (Number + Originality * 13) - 1;
        public TileColor Color { get; private set; }
        public bool IsJoker { get; private set; }
        public static bool operator < (Tile l, Tile r) => Comparer.Compare(l, r) < 0;
        public static bool operator > (Tile l, Tile r) => Comparer.Compare(l, r) > 0;
        public bool SameValue(Tile other)
        {
            return Number == other.Number && Color == other.Color;
        }

        public static IComparer<Tile> Comparer => new TileComparer();
        public string DebugDisplay => $"({Id:X2})|{""+Number,2}{Color.ToString()[0]} {(IsBoardTile?"b":"h")}{(EquivalentHandTileId==null?" ":"*")}_{Originality}";
        //public override string ToString() => $"{ConsoleColor.TileBackground}({Id:X2})|{ConsoleColor.ResetCode}{Color.ColorCode()}{Number:00}{ConsoleColor.ResetCode}";
        public override string ToString() => $"{ConsoleColor.TileBackground}|{ConsoleColor.ResetCode}{Color.ColorCode()}{Number,-2}{ConsoleColor.TileBackground}|{ConsoleColor.ResetCode}";
        private class TileComparer : IComparer<Tile>
        {
            public int Compare(Tile? x, Tile? y)
            {
                if (x == null && y == null) return 0;
                if (x == null && y != null) return -1;
                if (x != null && y == null) return 1;
#pragma warning disable CS8602 // Dereference of a possibly null reference: handled above
                int comp = x.Number - y.Number;
                if (comp == 0)
                {
                    comp = x.Color - y.Color;
                }
                if(comp == 0)
                {
                    //swapped 0 and 1 is on purpose; not IsBoardTile
                    comp = (x.IsBoardTile ? 0 : 1) - (y.IsBoardTile ? 0 : 1);
                }
                return comp;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        }
    }
}
