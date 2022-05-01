using System;
using System.Collections.Generic;
using System.Linq;
using Rummikub.Logic;
using Rummikub.Models.RunsAndGroups;

namespace Rummikub.Models
{
    public class PossibleResult
    {
        public PossibleResult(List<int> groups,List<HandOrBoardTile> unused){
            Groups=groups.Select(i=>i.ToGroup()).ToList();
            Runs=new List<Run>();
            leftOver=unused;
        }
        public List<Group> Groups;
        public List<Run> Runs;
        public List<HandOrBoardTile> leftOver;
        //side effect of making sure dup pairs are in order
        public bool IsValid(){
            for(int i=0;i<leftOver.Count;i++){
                if(leftOver[i].IsBoardTile&&leftOver[i].BoardHandDuplicateTile==null){
                    return false;
                }
            }
            return true;
        }
        public void SwapBoardHandPairs(){
            for(int i=0;i<leftOver.Count;i++){
                if(leftOver[i].BoardHandDuplicateTile!=null){
                    leftOver[i].SwapBoardHandDup();
                }
            }
        }
        public PlayerView ToPlayerView(){
            return new PlayerView{
                Board=Groups.Select(g=>g.Select(id=>IdTileMap.map[id].Tile).ToList()).Concat(
                    this.Runs.Select(r=>r.Select(t=>t.Tile).ToList())
                ).ToList(),
                Hand=this.leftOver.Select(x=>x.Tile).ToList()
            };
        }
    }
}

