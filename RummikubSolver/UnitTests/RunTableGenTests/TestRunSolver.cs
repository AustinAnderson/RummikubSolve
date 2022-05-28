using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunsRainbowTableGenerator;
using System.Collections.Generic;
using System.Linq;

namespace TestRunSolver
{
    public static class BothDirectionsTestUtil
    {
        public static void AssertGood(List<int> origs,List<int> dups)
        {
            var solver = new RunSolver();
            var res = solver.Solve(origs, dups);
            var expected = new RunResult();//all used
            Assert.AreEqual(expected, res, "orig to dups");
            res = solver.Solve(dups, origs);
            expected = new RunResult();//all used
            Assert.AreEqual(expected, res, "dups to orig");
        }
        public static void AssertGood(List<int> origs,List<int> dups, bool[] expectedOrigUnuseds, bool[] expectedDupsUnused)
        {
            var solver = new RunSolver();
            var res = solver.Solve(origs, dups);
            int unusedCount = expectedOrigUnuseds.Count(x => x) + expectedDupsUnused.Count(x => x);
            var expected = new RunResult(expectedOrigUnuseds.Concat(expectedDupsUnused).ToArray());
            expected.ScoreIfValid = unusedCount;
            Assert.AreEqual(expected, res, "orig to dups");
            res = solver.Solve(dups, origs);
            expected = new RunResult(expectedDupsUnused.Concat(expectedOrigUnuseds).ToArray());
            expected.ScoreIfValid = unusedCount;
            Assert.AreEqual(expected, res, "dups to orig");
        }
    }
    [TestClass]
    public class TestTwoDups
    {
        private const bool t=true;
        private const bool f=false;
        [TestMethod]
        public void Highest()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 1, 2, 3, 4, 5, 6 }, new List<int> { 8, 9 },
            //           1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, f, f, f, f, f, f, f },
            //           1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, f, t, t, f, f, f, f }
            );
        }
        [TestMethod]
        public void Higher()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 1, 2, 3, 4, 5, 6 }, new List<int> { 7, 8 });
        }
        [TestMethod]
        public void HighOneOverlap()
        {
            //setting true for the other 6 is equivalent, but it doesn't matter for the purposes of checking as a solution
            //when used in the solver algorithm
            BothDirectionsTestUtil.AssertGood(new List<int> { 1, 2, 3, 4, 5, 6 }, new List<int> { 6, 7 },
                //           1  2  3  4  5  6  7  8  9  A  B  C  D 
                     new[] { f, f, f, f, f, t, f, f, f, f, f, f, f },
                //           1  2  3  4  5  6  7  8  9  A  B  C  D 
                     new[] { f, f, f, f, f, f, f, f, f, f, f, f, f }
            );
        }
        [TestMethod]
        public void High()
        {
            //setting true for the other 5 6 is equivalent, but it doesn't matter for the purposes of checking as a solution
            //when used in the solver algorithm
            BothDirectionsTestUtil.AssertGood(new List<int> { 1, 2, 3, 4, 5, 6 }, new List<int> { 5, 6 },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, f, f, f, f, f, f, f },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, t, t, f, f, f, f, f, f, f }
            );
        }
        [TestMethod]
        public void Low()
        {
            //setting true for the other 1 2 is equivalent, but it doesn't matter for the purposes of checking as a solution
            //when used in the solver algorithm
            BothDirectionsTestUtil.AssertGood(new List<int> { 1, 2, 3, 4, 5, 6 }, new List<int> { 1, 2 },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, f, f, f, f, f, f, f },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { t, t, f, f, f, f, f, f, f, f, f, f, f }
            );
        }
        [TestMethod]
        public void Lower()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 3, 4, 5, 6 }, new List<int> { 1, 2 });
        }
        [TestMethod]
        public void LowOneOverlap()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 2, 3, 4, 5, 6 }, new List<int> { 1, 2 },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, t, f, f, f, f, f, f, f, f, f, f, f },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, f, f, f, f, f, f, f }
            );
        }
        [TestMethod]
        public void Lowest()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 4, 5, 6, 7, 8 }, new List<int> { 1, 2 },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, f, f, f, f, f, f, f },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { t, t, f, f, f, f, f, f, f, f, f, f, f }
            );
        }
        [TestMethod]
        public void Mid()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 1, 2, 3, 4, 5, 6 }, new List<int> { 3, 4 });

        }
        [TestMethod]
        public void MinMid()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 2, 3, 4, 5 }, new List<int> { 3, 4 });
        }
    }
    [TestClass]
    public class TestOneDup
    {
        private const bool t=true;
        private const bool f=false;
        [TestMethod]
        public void High()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 2, 3, 4, 5 }, new List<int> { 5 },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, f, f, f, f, f, f, f },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, t, f, f, f, f, f, f, f, f }
            );
        }
        [TestMethod]
        public void Higher()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 2, 3, 4, 5 }, new List<int> { 6 });
        }
        [TestMethod]
        public void Highest()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 2, 3, 4, 5 }, new List<int> { 7 },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, f, f, f, f, f, f, f },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, t, f, f, f, f, f, f }
            );
        }
        [TestMethod]
        public void MinHigh()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 4, 5 }, new List<int> { 6 });
        }

        [TestMethod]
        public void Low()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 7, 8, 9, 10 }, new List<int> { 7 },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, f, f, f, f, f, f, f },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, t, f, f, f, f, f, f }
            );
        }
        [TestMethod]
        public void Lower()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 7, 8, 9, 10 }, new List<int> { 6 });
        }
        [TestMethod]
        public void Lowest()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 7, 8, 9, 10 }, new List<int> { 5 },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, f, f, f, f, f, f, f, f, f },
                 //      1  2  3  4  5  6  7  8  9  A  B  C  D 
                 new[] { f, f, f, f, t, f, f, f, f, f, f, f, f }
            );
        }
        [TestMethod]
        public void MinLow()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 7, 8 }, new List<int> { 6 });
        }
        [TestMethod]
        public void Mid()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 4, 5, 6, 7, 8, 9, 10 }, new List<int> { 7 });
        }
        [TestMethod]
        public void MinMid()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 5, 6, 7, 8, 9 }, new List<int> { 7 });
        }
        [TestMethod]
        public void Sandwich()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 5, 6, 8, 9 }, new List<int> { 7 });
        }
        [TestMethod]
        public void MinSandwich()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 6, 8 }, new List<int> { 7 });
        }
    }
    [TestClass]
    public class TestMutualOneDup
    {
        [TestMethod]
        public void TestMutualSandwich()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 2, 6, 8, 9, 10 }, new List<int> { 1, 3, 7 });
        }
        [TestMethod]
        public void TestOneSamichOneHigh()
        {
            BothDirectionsTestUtil.AssertGood(new List<int> { 2, 4, 5, 6, 8, 9 }, new List<int> { 1, 3, 10 });
        }
    }
}