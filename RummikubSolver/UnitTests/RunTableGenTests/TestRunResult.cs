using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunsRainbowTableGenerator;
using SharedModels;
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
            //                 12345678901234567890123456
            //                 1234567890123456789012345678912
            Assert.AreEqual("7 10100100110000000000001100", result.ToString());
        }
        [TestMethod]
        public void TestBoolConstructor()
        {
            var I = true;
            var O = false;
            var i = '1';
            var o = '0';
            //                                    1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6
            RunResult result=new RunResult(new[] {I,O,I,O,O,I,O,O,I,I,O,O,O,O,O,O,O,O,O,O,O,I,I,O,O,O});
            var expected = string.Join("", new[] {i,o,i,o,o,i,o,o,i,i,o,o,o,o,o,o,o,o,o,o,o,i,i,o,o,o});
            result.ScoreIfValid = 7;
            Assert.AreEqual("7 "+expected, result.ToString());
        }
        [TestMethod]
        public void TestIntConstructor()
        {
            //bit string is reversed for treating as array left to right,
            //                                 12345678901234567890123456
            //so                                  MLKJIHGFEDCBA9876543210
            RunResult result = new RunResult(0b00011000000000001100100101);
                                           //7 10100100110000000000001100
            result.ScoreIfValid = 7;
            //                 1234567890123456789012345
            //                 0123456789ABCDEFGHIJKLM
            Assert.AreEqual("7 10100100110000000000011000", result.ToString());
        }
        [TestMethod]
        public void TestHiScore()
        {
            //test with score of 10101 (21)
            RunResult result = new RunResult();
            for(int i = 0; i < 21; i++)
            {
                result[i] = true;
            }
            result.ScoreIfValid = 21;
            //                  12345678901234567890123456
            Assert.AreEqual("21 11111111111111111111100000", result.ToString());
        }
    }
}
