using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rummikub.Logic;

namespace Rummikub.Models.RunsAndGroups
{
    public struct GroupList : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            for(int i=0;i<count;i++){
                yield return list[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()=>this.GetEnumerator();

        private int count;
        public int Count=>count;
        private int[] list;
        public void Add(int value){
            if(list==null) {
                list=new int[24];
                count=0;
            }
            list[count]=value;
            count++;
        }
        public int this[int i]{
            get=>list[i];
        }
    }
    public class PossibleGroupingConfig
    {
        public PossibleGroupingConfig(string originalUnusedList){
            //should make a copy
            unusedTiles=originalUnusedList;
            groups=new GroupList();
        }
        public GroupList groups;
        public string unusedTiles;
        public string groupNumberString()
        {
            System.Text.StringBuilder builder=new System.Text.StringBuilder();
            for(int i=0;i<groups.Count;i++){
                char x=IdTileMap.map[(byte)groups[i]].Tile.Number switch {
                    1=>'1',2=>'2',3=>'3',4=>'4',5=>'5',6=>'6',7=>'7',8=>'8',
                    9=>'9',10=>'a',11=>'b',12=>'c',_=>'x'
                };
                builder.Append(x);
                for(int j=0;j<groups[i].GroupLength();j++){
                    builder.Append(
                        IdTileMap.map[groups[i].GroupTileAt(j)].Tile.Color.Letter
                    );
                }
            }
            return builder.ToString();
        }
        public override string ToString()
        {
            return string.Join("\r\n",groups.Select(x=>x.ToGroup().ToString()))+"["+
                string.Join(",",Encoding.ASCII.GetBytes(unusedTiles.ToString()).Select(x=>IdTileMap.map[x].ToString()))
            +"]";
        }
    }

}

