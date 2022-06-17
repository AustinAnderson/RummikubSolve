using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class InitialRun:InitialList
    {
        public InitialRun() { }
        public InitialRun(InitialRun other) 
        { 
            foreach(var tile in other)
            {
                Add(new Tile(tile));
            }
        }

        protected override void ValidateModification(Tile[] preposedNewState)
        {
            string changeRep = ToString()+"=>"+StringRep(preposedNewState);
            if(preposedNewState.Length > 13)
            {
                throw new ArgumentException("invalid change "+changeRep+": runs have at most tiles 1-13");
            }
            TileColor color;
            int i = 0;
            while(i<preposedNewState.Length && preposedNewState[i].IsJoker)
            {
                i++;
            }
            int lastNum = preposedNewState[i].Number;
            color=preposedNewState[i].Color;
            i++;
            for(; i < preposedNewState.Length; i++)
            {
                if (preposedNewState[i].IsJoker)
                {
                    lastNum++;
                }
                else if(color!=preposedNewState[i].Color)
                {
                    throw new ArgumentException("invalid change "+changeRep+": runs be a single color");
                }
                else if ((lastNum + 1) != preposedNewState[i].Number)
                {
                    throw new ArgumentException("invalid change "+changeRep+": runs have contiguous numbers");
                }
                else
                {
                    lastNum = preposedNewState[i].Number;
                }
            }
        }
        public override void UpdateJokerValues()
        {
            for(int i = 0; i < JokerIndexes.Count; i++)
            {
                if (JokerIndexes[i] == 0)
                {
                //j j n
                    if(JokerIndexes[i+1] == 1)
                    {
                        this[0].SetJokerValue(this[2].Number - 2, this[2].Color);
                    }
                //j n 
                    else
                    {
                        this[0].SetJokerValue(this[1].Number-1, this[1].Color);
                    }
                }
                else
                {
                    //n j | j# j
                    //tile right before this one is either normal number or joker we already set the number for
                    this[JokerIndexes[i]].SetJokerValue(this[JokerIndexes[i] - 1].Number + 1, this[0].Color);
                }
            }
        }
    }
}
