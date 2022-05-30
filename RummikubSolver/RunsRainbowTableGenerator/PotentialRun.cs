using System.Diagnostics;

namespace RunsRainbowTableGenerator.Logic
{
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
}
