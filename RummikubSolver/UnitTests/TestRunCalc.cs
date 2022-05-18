﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        
        [TestMethod]
        public void FindsScoreCorrectly()
        {
            var baseUnused = new[]
            {
                RunTestUtil.MakeFastCalcTile("1Th"),
                RunTestUtil.MakeFastCalcTile("3Bh"),
                RunTestUtil.MakeFastCalcTile("3Rh"),
                RunTestUtil.MakeFastCalcTile("6Bb"),
                RunTestUtil.MakeFastCalcTile("6Yb"),
                RunTestUtil.MakeFastCalcTile("9Bh"),
                RunTestUtil.MakeFastCalcTile("9Rh"),
                RunTestUtil.MakeFastCalcTile("ABb"),
                RunTestUtil.MakeFastCalcTile("AYb"),
            };
            var currentPossibilitySetUnused = new[]
            {
                RunTestUtil.MakeFastCalcTile("4Bb"),
                RunTestUtil.MakeFastCalcTile("4Rb"),
                RunTestUtil.MakeFastCalcTile("4Th"),
                RunTestUtil.MakeFastCalcTile("4Yh"),
                RunTestUtil.MakeFastCalcTile("5Rb"),
                RunTestUtil.MakeFastCalcTile("5Bb"),
                RunTestUtil.MakeFastCalcTile("5Yh"),
                RunTestUtil.MakeFastCalcTile("BBb"),
                RunTestUtil.MakeFastCalcTile("BYb"),
                RunTestUtil.MakeFastCalcTile("CYb*"),



                RunTestUtil.MakeFastCalcTile("9Bh"),
                RunTestUtil.MakeFastCalcTile("9Bh"),
                RunTestUtil.MakeFastCalcTile("9Bh"),
                RunTestUtil.MakeFastCalcTile("9Bh"),
                RunTestUtil.MakeFastCalcTile("9Bh"),
                RunTestUtil.MakeFastCalcTile("9Bh"),
            };
            var arr = new UnusedFastCalcArray
            {
                Set = currentPossibilitySetUnused,
                Count = 10
            };
            int currentScore = RunScorer.Score(baseUnused, ref arr);
            Assert.AreEqual(3, currentScore, "score should be 3");
        }
    }
}
