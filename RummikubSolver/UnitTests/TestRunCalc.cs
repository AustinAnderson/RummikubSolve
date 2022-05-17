using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLogic;
using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TestRunCalc
    {
        private FastCalcTile MakeFastCalcTile(string desc)
        {
            var num = desc[0] switch
            {
                'A' => "10",
                'B' => "11",
                'C' => "12",
                'D' => "13",
                'E' => "14",
                'F' => "15",
                _ => ""+desc[0]
            };

            var tile = new Tile(num + desc[1]);
            tile.IsBoardTile = desc[2] == 'b';
            if(desc.Length > 3 && desc[3]=='*')
            {
                tile.EquivalentHandTile = tile;
            }
            return tile.ToFastCalcTile();
        }
        [TestMethod]
        public void FindsScoreCorrectly()
        {
            var baseUnused = new[]
            {
                MakeFastCalcTile("1Th"),
                MakeFastCalcTile("3Bh"),
                MakeFastCalcTile("3Rh"),
                MakeFastCalcTile("6Bb"),
                MakeFastCalcTile("6Yb"),
                MakeFastCalcTile("9Bh"),
                MakeFastCalcTile("9Rh"),
                MakeFastCalcTile("ABb"),
                MakeFastCalcTile("AYb"),
            };
            var currentPossibilitySetUnused = new[]
            {
                MakeFastCalcTile("4Bb"),
                MakeFastCalcTile("4Rb"),
                MakeFastCalcTile("4Th"),
                MakeFastCalcTile("4Yh"),
                MakeFastCalcTile("5Rb"),
                MakeFastCalcTile("5Bb"),
                MakeFastCalcTile("5Yh"),
                MakeFastCalcTile("BBb"),
                MakeFastCalcTile("BYb"),
                MakeFastCalcTile("CYb*"),



                MakeFastCalcTile("9Bh"),
                MakeFastCalcTile("9Bh"),
                MakeFastCalcTile("9Bh"),
                MakeFastCalcTile("9Bh"),
                MakeFastCalcTile("9Bh"),
                MakeFastCalcTile("9Bh"),
            };
            int currentScore = RunScorer.Score(baseUnused, currentPossibilitySetUnused, 10);
            Assert.AreEqual(3, currentScore, "score should be 3");
        }
    }
}
