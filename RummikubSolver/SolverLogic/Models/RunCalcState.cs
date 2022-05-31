using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    //use struct to have it all on stack, not sure if switch branch prediction stuff will make this slower than just using array though
    internal struct FourArray<T>: IEnumerable<T>
    {
        private T one;
        private T two;
        private T three;
        private T four;
        public T this[int i]
        {
            get {
                switch (i)
                {
                    case 0: return one;
                    case 1: return two;
                    case 2: return three;
                    default: return four;
                }
            }
            set
            {
                switch (i)
                {
                    case 0: one = value; break;
                    case 1: two = value; break;
                    case 2: three = value; break;
                    default: four = value; break;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < 4; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    internal struct RunCalcState
    {
        public FourArray<bool> chainIsRunBreakingIfUnused;
        public FourArray<int> chainScore;
        public FourArray<FastCalcTile> lastForColor;
        public RunCalcState()
        {
            chainIsRunBreakingIfUnused = new FourArray<bool>();
            chainScore = new FourArray<int>();
            lastForColor = new FourArray<FastCalcTile>();
            for(int i = 0; i < 4; i++)
            {
                chainIsRunBreakingIfUnused[i] = false;
                lastForColor[i] = FastCalcTile.MaxValue;
                chainScore[i] = 0;
            }
        }
        public override string ToString() =>
            $"[{string.Join(",", lastForColor)}] " +
            $"[{string.Join(",", chainIsRunBreakingIfUnused.Select(x => x ? "t" : "f"))}]" +
            $"[{string.Join(",", chainScore)}]";
    }
}
