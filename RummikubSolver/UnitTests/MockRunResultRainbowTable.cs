using SharedModels;
using System.Collections.Generic;

namespace TestRunCalc
{
    public class MockRunResultRainbowTable : IRunResultRainbowTable
    {
        public MockRunResultRainbowTable(Dictionary<uint,RunResult> map)
        {
            this.map = map;
        }
        private Dictionary<uint, RunResult> map;
        public RunResult GetFor(uint possibility)=>map[possibility];
    }
}
