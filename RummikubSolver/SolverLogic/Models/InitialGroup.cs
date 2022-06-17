using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class InitialGroup:InitialList
    {
        public InitialGroup() { }
        public InitialGroup(InitialGroup other) 
        { 
            foreach(var tile in other)
            {
                Add(new Tile(tile));
            }
        }
        protected override void ValidateModification(Tile[] preposedNewState)
        {
            string changeRep = ToString()+"=>"+StringRep(preposedNewState);
            if(preposedNewState.Length > 4)
            {
                throw new ArgumentException("invalid change "+changeRep+": groups have at most 4 tiles");
            }
            var colorsSeen=new List<TileColor>();
            var numbersSeen = new List<int>();
            for(int i = 0; i < preposedNewState.Length; i++)
            {
                if (!preposedNewState[i].IsJoker)
                {
                    if (colorsSeen.Contains(preposedNewState[i].Color))
                    {
                        throw new ArgumentException("invalid change " + changeRep + ": groups must be made of unique colors");
                    }
                    else
                    {
                        colorsSeen.Add(preposedNewState[i].Color);
                    }
                    if (!numbersSeen.Any())
                    {
                        numbersSeen.Add(preposedNewState[i].Number);
                    }
                    else if (!numbersSeen.Contains(preposedNewState[i].Number))
                    {
                        throw new ArgumentException("invalid change " + changeRep + ": group tiles must share the same number");
                    }
                }
            }
        }
        public override void UpdateJokerValues()
        {
            if (JokerIndexes.Count == 0)
            {
                return;
            }
            var colors = new List<TileColor> { TileColor.BLACK, TileColor.RED, TileColor.TEAL, TileColor.YELLOW };
            //BRT j
            //BR jj
            int num = 0;
            foreach(var tile in this)
            {
                if (!tile.IsJoker)
                {
                    num = tile.Number;
                    colors.Remove(tile.Color);
                }
            }
            //removed non joker colors, so number of remaining colors should match joker count,
            //so jokerIndexes.count==colors.count
            for(int i=0;i<colors.Count; i++)
            {
                this[this.JokerIndexes[i]].SetJokerValue(num,colors[i]);
            }
        }
    }
}
