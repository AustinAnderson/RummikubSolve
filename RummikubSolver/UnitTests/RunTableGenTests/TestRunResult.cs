using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunsRainbowTableGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilTests
{
    [TestClass]
    public class TestRunResult
    {
        [TestMethod]
        public void TestBitSetting()
        {
            RunResult result=new RunResult();
            result[0] = true;
            result[2] = true;
            result[5] = true;
            result[8] = true;
            result[9] = true;
            result[22] = true;
            result[23] = true;
            result.ScoreIfValid = 7;
            //                 01234567890123456789012345
            //                 01234567890123456789012345678912
            Assert.AreEqual("7 10100100110000000000001100000000", result.ToString());
        }
        [TestMethod]
        public void TestBoolConstructor()
        {
            var I = true;
            var O = false;
            var i = '1';
            var o = '0';
            //                                    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5
            RunResult result=new RunResult(new[] {I,O,I,O,O,I,O,O,I,I,O,O,O,O,O,O,O,O,O,O,O,I,I,O,O,O});
            var expected = string.Join("", new[] {i,o,i,o,o,i,o,o,i,i,o,o,o,o,o,o,o,o,o,o,o,i,i,o,o,o,o,o,o,o,o,o });
            result.ScoreIfValid = 7;
            Assert.AreEqual("7 "+expected, result.ToString());
        }
        [TestMethod]
        public void TestIntConstructor()
        {
            //bit string is reversed for treating as array left to right,
            //so                               MLKJIHGFEDCBA9876543210
            RunResult result = new RunResult(0b11000000000001100100101);
            result.ScoreIfValid = 7;
            //                 0123456789ABCDEFGHIJKLM
            Assert.AreEqual("7 10100100110000000000011000000000", result.ToString());
        }
    }
}
