using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rummikub.Models
{
    public class PlayerView
    {
        public void MoveFromHandToBoard(List<List<Tile>> newRunsAndGroups)
        {
            foreach(var tile in newRunsAndGroups.SelectMany(x=>x))
            {
                Hand.Remove(tile);
            }
            Board.AddRange(newRunsAndGroups);
        }
        public List<List<Tile>> Board {get;set;}
        public List<Tile> Hand {get;set;}
        public string Print(){
            StringBuilder builder=new StringBuilder();
            foreach(var list in Board){
                foreach(var tile in list){
                    builder.Append(tile.PrintString()+" ");
                }
                builder.AppendLine();
            }
            builder.Append("_________________________________________________");
            builder.AppendLine("_________________________________________________");
            for(int i=0;i<Hand.Count;i++){
                builder.Append(Hand[i].PrintString()+"  ");
                if((i+1)%10==0) builder.AppendLine();
            }
            builder.AppendLine();
            return builder.ToString();
        }
    }
}

