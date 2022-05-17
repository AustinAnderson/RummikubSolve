using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TestGroupConf
    {
        [TestMethod]
        public void TestRoundTripLow()
        {
            var expected = new[] { 2, 3, 5, 1, 6 };
            var conf = new GroupConf(expected);
            string actualStr = "";
            for(int i = 0; i < expected.Length; i++)
            {
                actualStr += conf[i];
            }
            Assert.AreEqual(string.Join("",expected), actualStr);

        }
        [TestMethod]
        public void TestRoundTripHigh()
        {

            Random rand = new Random();
            var expected=Enumerable.Range(0, 25).Select(x => rand.Next(1, 7)).ToArray();
            var conf = new GroupConf(expected);
            string actualStr = "";
            for(int i=0; i<expected.Length; i++)
            {
                actualStr += conf[i];
            }
            Assert.AreEqual(string.Join("",expected), actualStr);

        }
    }
}
