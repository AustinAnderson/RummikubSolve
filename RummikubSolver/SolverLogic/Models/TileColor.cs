using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public static class ConsoleColor
    {
        public static string ColorCode(this TileColor tileColor) => tileColor switch
        {
            TileColor.BLACK => "\u001b[0;30;100m",
            TileColor.RED => "\u001b[0;31m",
            TileColor.TEAL => "\u001b[1;34m",
            TileColor.YELLOW => "\u001b[1;33m",
            _ => throw new NotImplementedException($"no console code for {nameof(TileColor)} '{tileColor}'")
        };
        public static char Char(this TileColor tileColor) => tileColor.ToString()[0];

        public const string TileBackground = "\u001b[1;30;100m";
        public const string ResetCode = "\u001b[0m";
    }
    public enum TileColor
    {
        BLACK = 0,
        RED = 1,
        TEAL = 2,
        YELLOW = 3
    }
}
