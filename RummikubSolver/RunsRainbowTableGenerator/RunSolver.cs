using SharedModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunsRainbowTableGenerator.Logic
{
    public struct DupTrackingInt
    {
        public int Int;
        public bool IsDup;
        public override string ToString()
        {
            string res = "";
            if (IsDup)
            {
                //combining underline
                res += "\u0332";
            }
            res += Int;
            return res;
        }
        public int Index
        {
            get
            {
                var ndx = Int - 1;//                                 v first dup
                if (IsDup) ndx += 13;//1,2,3,4,5,6,7,8,9,10,11,12,13,1
                return ndx;
            }
        }
    }
    [DebuggerDisplay("{DebugDisplay}")]
    public class PotentialRun : List<DupTrackingInt>
    {
        public string DebugDisplay=> string.Join("", this);
        //can cheat a little on these because they're garunteed to be unique, contiguous,
        //and strictly ascending
        /**<summary>finds the index where the sequence occurs and returns true, or false and -1</summary>**/
        public bool TryContainsIndex(int[] seq,out int startNdx)
        {
            startNdx = -1;
            // 4 5 
            // 2 3 4 5 
            if(Count<seq.Length) return false;
            bool containsStart = false;
            int startIndex = 0;
            for(; startIndex< Count; startIndex++)
            {
                if (seq[0] == this[startIndex].Int)
                {
                    containsStart = true;
                    break;
                }
            }
            bool contains = containsStart && (startIndex + seq.Length <= Count);
            if (contains)
            {
                startNdx = startIndex;
            }
            return contains;
        }
        public bool StartsWith(int[] seq)
        {
            if(Count<seq.Length) return false;
            bool startsWith = true;
            for(int i = 0; i < seq.Length; i++)
            {
                if (seq[i] != this[i].Int)
                {
                    startsWith = false;
                }
            }
            return startsWith;
        }
        public bool EndsWith(int[] seq)
        {
            if(Count<seq.Length) return false;
            bool containsStart = false;
            int startIndex = 0;
            for(; startIndex< Count; startIndex++)
            {
                if (seq[0] == this[startIndex].Int)
                {
                    containsStart = true;
                    break;
                }
            }
            return containsStart && (startIndex + seq.Length == Count);
        }
    }
    public class RunSolver
    {
        public static readonly IReadOnlyList<int> TilesOfSingleColor = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
        public RunResult SolveForPossibility(uint possibility)
        {
            List<int> solveWith=new List<int>();
            List<int> solveWithDups=new List<int>();
            var bv = new BitVector32(possibility);
            for(int j = 0; j < TilesOfSingleColor.Count; j++)
            {
                if(bv[j])
                {
                    if (j < 13)
                    {
                        solveWith.Add(TilesOfSingleColor[j]);
                    }
                    else
                    {
                        solveWithDups.Add(TilesOfSingleColor[j]);
                    }
                }
            }
            return Solve(solveWith,solveWithDups);
        }

        public RunResult Solve(List<int> solveWith, List<int> solveWithDups)
        {
            var originals = GetPotentialRuns(solveWith.Select(x => new DupTrackingInt
            {
                Int = x,
                IsDup = false }));
            var dups= GetPotentialRuns(solveWithDups.Select(x => new DupTrackingInt
            {
                Int = x,
                IsDup = true
            }));
            //all 1 and 2 groups from one that can be satisfied with the other
            ShiftAndSpliceCopies(dups, originals);
            //then any remaining in the other list that can be satisfied with the updated original
            ShiftAndSpliceCopies(originals, dups);
            //because we're tracking unused as true and it all defaults to false,
            //this will only show unused from the passed in numbers, assuming the numbers not passed in are already used in rummikub groups
            RunResult result= new RunResult();
            int unusedCount = 0;
            foreach(var unused in dups.Concat(originals).Where(x=>x.Count<3))
            {
                foreach(var tile in unused)
                {
                    result[tile.Index] = true;
                    unusedCount++;
                }
            }
            result.ScoreIfValid = unusedCount;
            return result;
        }
        private void ShiftAndSpliceCopies(List<PotentialRun> dups, List<PotentialRun> origs)
        {
            for(int dupNdx=dups.Count-1; dupNdx>=0; dupNdx--) 
            {
                var pDupRun=dups[dupNdx];
                if (pDupRun.Count == 1)
                {
                    //dup tile on it's own,
                    //need to find potential ending with (x-2, x-1)
                    //or potential starting with (x+1, x+2)
                    //or potential containing (x-2, x-1, x, x+1, x+2)
                    //else find 
                    //potential run ending with x-1 and potential run starting with x+1
                    //to splice in
                    for (int j= origs.Count - 1; j >= 0;j--)
                    {
                        var pOrigRun=origs[j];
                        if (j>0 &&
                            origs[j].StartsWith(new[] { pDupRun[0].Int+1 }) &&
                            origs[j-1].EndsWith(new[] { pDupRun[0].Int-1 })
                        )
                        {
                            origs[j - 1].Add(pDupRun[0]);
                            origs[j - 1].AddRange(origs[j]);
                            origs.RemoveAt(j);
                            dups.RemoveAt(dupNdx);
                            break;
                        }
                        else if(pOrigRun.EndsWith(new[] { pDupRun[0].Int - 2, pDupRun[0].Int - 1 }))
                        {
                            //shift it over to the to run
                            pOrigRun.Add(pDupRun[0]);
                            dups.RemoveAt(dupNdx);
                            break;
                        }
                        else if (pOrigRun.StartsWith(new[] { pDupRun[0].Int+1,pDupRun[0].Int+2}))
                        {
                            pOrigRun.Insert(0, pDupRun[0]);
                            dups.RemoveAt(dupNdx);
                            break;
                        }
                        else if (pOrigRun.TryContainsIndex(new[] {
                            pDupRun[0].Int - 2, pDupRun[0].Int - 1, pDupRun[0].Int,
                            pDupRun[0].Int + 1, pDupRun[0].Int + 2
                        },out int startNdx))
                        {
                            var lowSplit = new PotentialRun();
                            for(int k = 0; k < startNdx; k++)
                            {
                                lowSplit.Add(pOrigRun[k]);
                            }
                            lowSplit.Add(pOrigRun[startNdx]);
                            lowSplit.Add(pOrigRun[startNdx+1]);
                            lowSplit.Add(pOrigRun[startNdx+2]);
                            var highSplit = new PotentialRun
                            {
                                pDupRun[0],
                            };
                            for(int k = startNdx + 3; k < pOrigRun.Count; k++)
                            {
                                highSplit.Add(pOrigRun[k]);
                            }
                            origs.RemoveAt(j);
                            origs.Add(lowSplit);
                            origs.Add(highSplit);
                            dups.RemoveAt(dupNdx);
                            break;
                        }
                    }
                }
                else if(dups[dupNdx].Count == 2)
                {

                    //two contiguous, [x, y=x+1]
                    //need to find potential ending with (x-1)
                    //potential starting with (x+2)
                    //potential containing (x-1, x, x+1, x+2)
                    //also can get 1 less unused if
                    //ends with (x)
                    //starts with (x+1)
                    //but do those last incase they shadow the other better options
                    for (int origNdx= origs.Count - 1; origNdx >= 0;origNdx--)
                    {
                        var pOrigRun=origs[origNdx];
                        if(pOrigRun.EndsWith(new[] { pDupRun[0].Int - 1 }))
                        {
                            //shift it over to the to run
                            pOrigRun.AddRange(pDupRun);
                            dups.RemoveAt(dupNdx);
                            break;
                        }
                        else if (pOrigRun.StartsWith(new[] { pDupRun[0].Int+2}))
                        {
                            pOrigRun.Insert(0, pDupRun[1]);
                            pOrigRun.Insert(0, pDupRun[0]);
                            dups.RemoveAt(dupNdx);
                            break;
                        }
                        else if (pOrigRun.TryContainsIndex(new[] {
                            pDupRun[0].Int - 1, 
                            //x cpy            y cpy
                            pDupRun[0].Int, pDupRun[0].Int + 1, 
                            pDupRun[0].Int + 2
                        },out int startNdx))
                        {                            
                            var lowSplit = new PotentialRun();
                            for(int k = 0; k < startNdx; k++)
                            {
                                lowSplit.Add(pOrigRun[k]);
                            }
                            lowSplit.Add(pOrigRun[startNdx]);//x-1
                            lowSplit.Add(pOrigRun[startNdx+1]);//x
                            lowSplit.Add(pOrigRun[startNdx+2]);//x+1
                            var highSplit = new PotentialRun
                            {
                                pDupRun[0],//x cpy
                                pDupRun[1],//y cpy
                            };
                            for(int k = startNdx + 3; k < pOrigRun.Count; k++)
                            {
                                highSplit.Add(pOrigRun[k]);
                            }
                            origs.RemoveAt(origNdx);
                            origs.Add(lowSplit);
                            origs.Add(highSplit);
                            dups.RemoveAt(dupNdx);
                            break;

                        }
                        else if (pOrigRun.StartsWith(new[] { pDupRun[1].Int }))
                        {
                            //234 |1'2'
                            origs.Add(new PotentialRun { pOrigRun.First() });
                            //234     2 |   1'2'
                            pOrigRun[0] = pDupRun[1];
                            //2'34    2 |   1'2'
                            pOrigRun.Insert(0, pDupRun[0]);
                            //1'2'34  2 |   1'2'
                            dups.RemoveAt(dupNdx);
                            break;
                        }
                        else if (pOrigRun.EndsWith(new[] { pDupRun[0].Int }))
                        {
                            //1234 | 4'5'
                            origs.Add(new PotentialRun { pOrigRun.Last() });
                            //1234  4  |    4'5'
                            pOrigRun[pOrigRun.Count - 1] = pDupRun[0];
                            //1234' 4  | 4'5'
                            pOrigRun.Add(pDupRun[1]);
                            //1234'5' 4 |  4'5'
                            dups.RemoveAt(dupNdx);
                            //1234'5' 4  
                            break;
                        }
                    }
                }
                //else 3, already valid, keep for pool and do it merging to to from
            }
        }
        private List<PotentialRun> GetPotentialRuns(IEnumerable<DupTrackingInt> values)
        {
            var potentials = new List<PotentialRun>();
            foreach(var num in values)
            {
                DupTrackingInt top = potentials.LastOrDefault()?.LastOrDefault() ?? default;
                if (top.Int != default(DupTrackingInt).Int && top.Int + 1 == num.Int)
                {
                    potentials.Last().Add(num);
                }
                else
                {
                    potentials.Add(new PotentialRun { num });
                }
            }
            return potentials;
        }
    }
}
