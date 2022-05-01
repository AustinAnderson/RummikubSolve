using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rummikub.Models;
using Rummikub.Models.RunsAndGroups;

namespace Rummikub.Logic
{
    public class Solver
    {
        public static PossibleResult Solve(PlayerView initialState)
        {
            IdTileMap.map=
                initialState.Board.SelectMany(x=>x.Select(y=>new HandOrBoardTile(true,y))).Concat(
                    initialState.Hand.Select(x=>new HandOrBoardTile(false,x))
                ).ToDictionary(x=>x.Tile.ID,x=>x);

            var possibileGroupsConfigEnumerator=new MaxGroupCollection(IdTileMap.map.Values);
            //var res=possibileGroupsConfigEnumerator.GeneratePossibilities();
            //current hardcoded list should have 5971968 entries
            //var res=possibileGroupsConfigEnumerator.ToList();
            // - find as many runs as possible,
            // - swap all instances of unused board tile with used hand tile dup,
            // - confirm all remaining unused are hand tiles
            int bestScore=int.MaxValue;//golf
            PossibleGroupingConfig grouping=null;
            foreach(var possibleGrouping in possibileGroupsConfigEnumerator)
            {
                var score=CalculateScore(possibleGrouping);
                if(score.isValid&&(grouping==null||score.Count<bestScore))
                {
                    grouping=possibleGrouping;
                    bestScore=score.Count;
                }
            }
            var best=SolveWithSet(grouping);
            best.SwapBoardHandPairs();
            //will either find the best play or the current state again
            return best;
        }
        public struct Score{
            public int Count;
            public bool isValid;

        }
        public static Score CalculateScore(PossibleGroupingConfig grouping)
        {
            //slowed by sort, could pre sort static part,
            //then have new unused parts sorted separate, zip together here in linear time,
            //but does it really matter if only up to 40 or so unused
            var bytes=Encoding.ASCII.GetBytes(grouping.unusedTiles);
            var arr=new List<HandOrBoardTile>(bytes.Length);
            for(int i=0;i<bytes.Length;i++){
                arr.Add(IdTileMap.map[bytes[i]]);
            }
            arr.Sort();
            var indexArrs=new int[4][];
            indexArrs[0]=new int[arr.Count];
            indexArrs[1]=new int[arr.Count];
            indexArrs[2]=new int[arr.Count];
            indexArrs[3]=new int[arr.Count];
            var currentCount=new[]{1,1,1,1};
            var lastNumber=new[]{-1,-1,-1,-1};
            for(int i=0;i<arr.Count;i++){
                int colorndx= arr[i].Tile.Color.Letter switch { 
                    'B'=>0,'R'=>1,'T'=>2,'Y'=>3 ,_=> throw new ArgumentException("weird color")
                };
                if(arr[i].Tile.Number==lastNumber[colorndx]+1)
                {
                    currentCount[colorndx]++;
                }
                else{
                    currentCount[colorndx]=1;
                }
                lastNumber[colorndx]=arr[i].Tile.Number;
                indexArrs[colorndx][i]=currentCount[colorndx];
            }
            Score score=new Score{Count=0,isValid=true};
            lastNumber[0]=0;
            lastNumber[1]=0;
            lastNumber[2]=0;
            lastNumber[3]=0;
            var lastLastNumber=new[]{0,0,0,0};
            for(int i=arr.Count-1;i>=0;i--)
            {
                var tile=arr[i];
                int colorndx= tile.Tile.Color.Letter switch { 
                    'B'=>0,'R'=>1,'T'=>2,'Y'=>3 ,_=> throw new ArgumentException("weird color")
                };
                if(indexArrs[colorndx][i]==1){
                    if(!(lastNumber[colorndx]==2&&lastLastNumber[colorndx]==3))
                    {
                        //if we get to a 1 and the last two non zeroed are not 2 and 3,
                        //then this tile is not part of a group, or part of a 2 group
                        if(tile.IsBoardTile&&tile.BoardHandDuplicateTile==null){
                            score.isValid=false;
                            break;
                        }
                        else{
                            score.Count++;
                        }
                    }
                }
                else if(indexArrs[colorndx][i]==2&& lastNumber[colorndx]!=3){
                    //if we get to a 2 and the last non zeroed was not 3,
                    //then this tile is part group of 2, which isn't valid so its unused
                    if(tile.IsBoardTile&&tile.BoardHandDuplicateTile==null){
                        score.isValid=false;
                        break;
                    }
                    else{
                        score.Count++;
                    }
                }

                if(indexArrs[colorndx][i]!=0){
                    lastLastNumber[colorndx]=lastNumber[colorndx];
                    lastNumber[colorndx]=indexArrs[colorndx][i];
                }
            }
            return score;
        }
        public static PossibleResult SolveWithSet(PossibleGroupingConfig grouping)
        {
            var Reds=new List<HandOrBoardTile>();
            var RedsTail=new List<HandOrBoardTile>();
            var Yellows=new List<HandOrBoardTile>();
            var YellowsTail=new List<HandOrBoardTile>();
            var Teals=new List<HandOrBoardTile>();
            var TealsTail=new List<HandOrBoardTile>();
            var Blacks=new List<HandOrBoardTile>();
            var BlacksTail=new List<HandOrBoardTile>();
            foreach(var tileid in Encoding.ASCII.GetBytes(grouping.unusedTiles.ToString()).OrderBy(x=>IdTileMap.map[x].Tile.Number)){
                var tile=IdTileMap.map[tileid];
                if(tile.Tile.Color==TileColor.Red){
                    if(Reds.LastOrDefault()?.Tile?.Number==tile.Tile.Number){
                        RedsTail.Add(tile);
                    }else{
                        Reds.Add(tile);
                    }
                }
                else if(tile.Tile.Color==TileColor.Yellow){
                    if(Yellows.LastOrDefault()?.Tile?.Number==tile.Tile.Number){
                        YellowsTail.Add(tile);
                    }else{
                        Yellows.Add(tile);
                    }
                }
                else if(tile.Tile.Color==TileColor.Teal){
                    if(Teals.LastOrDefault()?.Tile?.Number==tile.Tile.Number){
                        TealsTail.Add(tile);
                    }else{
                        Teals.Add(tile);
                    }
                }
                else if(tile.Tile.Color==TileColor.Black){
                    if(Blacks.LastOrDefault()?.Tile?.Number==tile.Tile.Number){
                        BlacksTail.Add(tile);
                    }else{
                        Blacks.Add(tile);
                    }
                }
            }
            var result=new PossibleResult(grouping.groups.ToList(),
                Encoding.ASCII.GetBytes(grouping.unusedTiles.ToString()).Select(x=>IdTileMap.map[x]).ToList());
            CalculateRunsForList(result,Reds);
            CalculateRunsForList(result,RedsTail);
            CalculateRunsForList(result,Yellows);
            CalculateRunsForList(result,YellowsTail);
            CalculateRunsForList(result,Teals);
            CalculateRunsForList(result,TealsTail);
            CalculateRunsForList(result,Blacks);
            CalculateRunsForList(result,BlacksTail);
            return result;
        }
        private static void CalculateRunsForList(PossibleResult toUpdate, List<HandOrBoardTile> sortedSingleColorList)
        {
            var potentials=new List<Run>();
            var newAdd=true;
            for(int i=0;i<sortedSingleColorList.Count-1;i++){
                if(newAdd){
                    potentials.Add(new Run{sortedSingleColorList[i]});
                }
                if(sortedSingleColorList[i].Tile.Number==sortedSingleColorList[i+1].Tile.Number-1){
                    potentials[potentials.Count-1].Add(sortedSingleColorList[i+1]);
                    newAdd=false;
                }else{
                    newAdd=true;
                }
            }
            foreach(var run in potentials.Where(x=>x.Count>=3)){
                toUpdate.Runs.Add(run);
                foreach(var tile in run){
                    toUpdate.leftOver.Remove(tile);
                }
            }
        }
    }
}

