using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RummikubSolver
{
    public class StringNotationParser
    {
        private const string t = "(?:\\d\\d?[BRTY]|J)";
        private const string tsBeforeLast = "(?:\\s*" + t + "\\s*,\\s*)*";
        private const string ts = tsBeforeLast + t + "\\s*";
        private const string group="\\s*(\\{" + ts + "\\}),?\\s*,?";
        private const string run = "\\s*(\\[" + ts + "\\]),?\\s*,?";
        private const string groupOrRuns = "(?:" + group + "|" + run + ")";
        private static readonly Regex groupOrRunsTokenizer = new Regex(groupOrRuns,RegexOptions.Compiled);
        private static readonly Regex boardRepValidate = new Regex("^" + groupOrRuns + "*$", RegexOptions.Compiled);
        private static readonly Regex handValidate = new Regex("^"+ts+"$",RegexOptions.Compiled);
        public static InitialHand ParseHand(string rep)
        {
            if (!handValidate.IsMatch(rep))
            {
                throw new ArgumentException($"hand string must match '{handValidate}'");
            }
            var tiles = new InitialHand();
            foreach(var match in rep.Split(',').Select(s=> s.Trim()).Where(x=>!string.IsNullOrEmpty(x)))
            {
                tiles.Add(new Tile(match.Trim()));
            }
            return tiles;
        }
        public static CurrentBoard ParseBoardShort(string rep)
        {
            rep = rep.ToLower().ReplaceLineEndings().Replace(Environment.NewLine, " ");
            const string digits = "0123456789";
            CurrentBoard board = new CurrentBoard();
            var matches = rep.Split(" ");
            var matchesNoEmpty=matches.Where(x => !string.IsNullOrWhiteSpace(x));
            foreach(var match in matchesNoEmpty)
            {
                var trimmed=match.Trim();
                if (trimmed.Contains("-"))
                {
                    char color = trimmed.Last();
                    var range=trimmed.Substring(0, trimmed.Length - 1).Split("-").Select(num => int.Parse(num)).ToArray();
                    var r=new InitialRun();
                    for(int i = range[0]; i <= range[1]; i++)
                    {
                        r.Add(new Tile("" + i + color.ToString().ToUpper()));
                    }
                    board.Runs.Add(r);
                }
                else
                {
                    string colors = "";
                    string number = "";
                    if(digits.Contains(trimmed[1]))
                    {
                        number = ""+trimmed[0]+trimmed[1];
                        colors = trimmed.Substring(2);
                    }
                    else
                    {
                        number = ""+trimmed[0];
                        colors = trimmed.Substring(1);
                    }
                    var g=new InitialGroup();
                    foreach(char color in colors.ToUpper())
                    {
                        g.Add(new Tile(number + color));
                    }
                    board.Groups.Add(g);
                }
            }
            return board;
        }
        /// <summary>
        //{ nB,nR,nT, nY} [ nB, n+1B, n+2B...]
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static CurrentBoard ParseBoardSet(string rep)
        {
            if (!boardRepValidate.IsMatch(rep))
            {
                throw new ArgumentException($"board string must match '{boardRepValidate}'");
            }
            CurrentBoard initialList = new CurrentBoard();
            var split=groupOrRunsTokenizer.Split(rep);
            foreach(var match in groupOrRunsTokenizer.Split(rep).Where(x=>!string.IsNullOrWhiteSpace(x)))
            {
                var trimmed=match.Trim();
                if (trimmed.StartsWith("{"))
                {
                    var g=new InitialGroup();
                    foreach(var tile in trimmed.Split(',',' ','\t','\r','\n','{','}').Where(s=>s.Length>0).Select(x=>new Tile(x.Trim())))
                    {
                        g.Add(tile);
                    }
                    initialList.Groups.Add(g);
                }
                else
                {
                    var r=new InitialRun();
                    foreach(var tile in trimmed.Split(',',' ','\t','\r','\n','[',']').Where(s=>s.Length>0).Select(x=>new Tile(x.Trim())))
                    {
                        r.Add(tile);
                    }
                    initialList.Runs.Add(r);
                }
            }
            return initialList;
        }
    }
}
