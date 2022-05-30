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
}
