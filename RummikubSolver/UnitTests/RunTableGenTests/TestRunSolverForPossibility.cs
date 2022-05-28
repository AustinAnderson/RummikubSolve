using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunsRainbowTableGenerator;
using RunsRainbowTableGenerator.Logic;
using SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace TestRunSolverForPossibility
{
    public static class TestAssertUtil
    {
        public static void AssertMapsToAllUsed(BitVector32 dataWithAllUsedExpected)//List<int> origs,List<int> dups)
        {
            var solver = new RunSolver();
            var res = solver.SolveForPossibility(dataWithAllUsedExpected.Data);
            var expected = new RunResult();//all used
            Assert.AreEqual(expected.ToString(), res.ToString());
        }
        public static void AssertMapsCorrectly(BitVector32 data, RunResult expected)
        {
            var solver = new RunSolver();
            var res = solver.SolveForPossibility(data.Data);
            Assert.AreEqual(expected.ToString(), res.ToString());
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
            TestAssertUtil.AssertMapsCorrectly(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { t, t, t, t, t, t, f, f, f, f, f, f, f, f, f, f, f, f, f, f, t, t, f, f, f, f }),
            //                        1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new RunResult(new[] { f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, t, t, f, f, f, f })
                {
                    ScoreIfValid=2
                }
            );
        }
        [TestMethod]
        public void Higher()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { t, t, t, t, t, t, f, f, f, f, f, f, f, f, f, f, f, f, f, t, t, f, f, f, f, f })
            );
        }
        [TestMethod]
        public void HighOneOverlap()
        {
            //setting true for the other 6 is equivalent, but it doesn't matter for the purposes of checking as a solution
            //when used in the solver algorithm
            TestAssertUtil.AssertMapsCorrectly(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { t, t, t, t, t, t, f, f, f, f, f, f, f, f, f, f, f, f, t, t, f, f, f, f, f, f}),
            //                        1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new RunResult(new[] { f, f, f, f, f, t, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f })
                {
                    ScoreIfValid=1
                }
            );
        }
        [TestMethod]
        public void High()
        {
            //setting true for the other 5 6 is equivalent, but it doesn't matter for the purposes of checking as a solution
            //when used in the solver algorithm
            TestAssertUtil.AssertMapsCorrectly(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { t, t, t, t, t, t, f, f, f, f, f, f, f, f, f, f, f, t, t, f, f, f, f, f, f, f}),
            //                        1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new RunResult(new[] { f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, t, t, f, f, f, f, f, f, f })
                {
                    ScoreIfValid = 2
                }
            );
        }
        [TestMethod]
        public void Low()
        {
            //setting true for the other 1 2 is equivalent, but it doesn't matter for the purposes of checking as a solution
            //when used in the solver algorithm
            TestAssertUtil.AssertMapsCorrectly(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { t, t, t, t, t, t, f, f, f, f, f, f, f, t, t, f, f, f, f, f, f, f, f, f, f, f}),
            //                        1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new RunResult(new[] { f, f, f, f, f, f, f, f, f, f, f, f, f, t, t, f, f, f, f, f, f, f, f, f, f, f })
                {
                    ScoreIfValid = 2
                }
                
            );
        }
        [TestMethod]
        public void Lower()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, f, t, t, t, t, f, f, f, f, f, f, f, t, t, f, f, f, f, f, f, f, f, f, f, f})
            );
        }
        [TestMethod]
        public void LowOneOverlap()
        {
            TestAssertUtil.AssertMapsCorrectly(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, t, t, t, t, t, f, f, f, f, f, f, f, t, t, f, f, f, f, f, f, f, f, f, f, f}),
                 //                   1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new RunResult(new[] { f, t, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f })
                {
                    ScoreIfValid = 1
                }
            );
        }
        [TestMethod]
        public void Lowest()
        {
            TestAssertUtil.AssertMapsCorrectly(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, f, f, t, t, t, t, t, f, f, f, f, f, t, t, f, f, f, f, f, f, f, f, f, f, f}),
                //                    1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new RunResult(new[] { f, f, f, f, f, f, f, f, f, f, f, f, f, t, t, f, f, f, f, f, f, f, f, f, f, f })
                {
                    ScoreIfValid = 2
                }
            );
        }
        [TestMethod]
        public void Mid()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { t, t, t, t, t, t, f, f, f, f, f, f, f, f, f, t, t, f, f, f, f, f, f, f, f, f})
            );

        }
        [TestMethod]
        public void MinMid()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, t, t, t, t, f, f, f, f, f, f, f, f, f, f, t, t, f, f, f, f, f, f, f, f, f})
            );
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
            TestAssertUtil.AssertMapsCorrectly(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, t, t, t, t, f, f, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f, f, f}),
                //                    1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new RunResult(new[] { f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f, f, f })
                {
                    ScoreIfValid = 1
                }
            );
        }
        [TestMethod]
        public void Higher()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, t, t, t, t, f, f, f, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f, f})
            );
        }
        [TestMethod]
        public void Highest()
        {
            TestAssertUtil.AssertMapsCorrectly(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, t, t, t, t, f, f, f, f, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f}),
                //                    1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new RunResult(new[] { f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f })
                {
                    ScoreIfValid = 1
                }
            );
        }
        [TestMethod]
        public void MinHigh()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, f, f, t, t, f, f, f, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f, f})
            );
        }

        [TestMethod]
        public void Low()
        {
            TestAssertUtil.AssertMapsCorrectly(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, f, f, f, f, f, t, t, t, t, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f}),
                //                    1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new RunResult(new[] { f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f })
                {
                    ScoreIfValid = 1
                }
            );
        }
        [TestMethod]
        public void Lower()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, f, f, f, f, f, t, t, t, t, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f, f})
            );
        }
        [TestMethod]
        public void Lowest()
        {
            TestAssertUtil.AssertMapsCorrectly(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, f, f, f, f, f, t, t, t, t, f, f, f, f, f, f, f, t, f, f, f, f, f, f, f, f}),
                //                    1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new RunResult(new[] { f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f, f, f })
                {
                    ScoreIfValid = 1
                }
            );
        }
        [TestMethod]
        public void MinLow()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, f, f, f, f, f, t, t, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f, f})
            );
        }
        [TestMethod]
        public void Mid()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, f, f, t, t, t, t, t, t, t, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f})
            );
        }
        [TestMethod]
        public void MinMid()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, f, f, f, t, t, t, t, t, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f})
            );
        }
        [TestMethod]
        public void Sandwich()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, f, f, f, t, t, f, t, t, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f})
            );
        }
        [TestMethod]
        public void MinSandwich()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, f, f, f, f, t, f, t, f, f, f, f, f, f, f, f, f, f, f, t, f, f, f, f, f, f})
            );
        }
    }
    [TestClass]
    public class TestMutualOneDup
    {
        private const bool t=true;
        private const bool f=false;
        [TestMethod]
        public void TestMutualSandwich()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, t, f, f, f, t, f, t, t, t, f, f, f, t, f, t, f, f, f, t, f, f, f, f, f, f})
            );
        }
        [TestMethod]
        public void TestOneSamichOneHigh()
        {
            TestAssertUtil.AssertMapsToAllUsed(
            //                          1  2  3  4  5  6  7  8  9  A  B  C  D  1  2  3  4  5  6  7  8  9  A  B  C  D 
                new BitVector32(new[] { f, t, f, t, t, t, f, t, t, f, f, f, f, t, f, t, f, f, f, f, f, f, t, f, f, f})
            );
        }
    }
}