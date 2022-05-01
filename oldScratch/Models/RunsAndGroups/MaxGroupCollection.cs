using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rummikub.Models.RunsAndGroups
{
    public class MaxGroupCollection:IEnumerable<PossibleGroupingConfig>
    {
        public MaxGroupCollection(IEnumerable<HandOrBoardTile> allTiles)
        {
            //sort all tiles and set the reference property on board hand pairs for swapping later

            //always at most 2 dups, TODO: make sure to follow this when enumerating joker
            //TODO: might need to sort then by ! is board tile, want to make sure board type comes first
            List<HandOrBoardTile> unused=allTiles.OrderBy(x=>x.Tile.Number).ThenBy(x=>x.Tile.Color.Letter).ThenBy(x=>!x.IsBoardTile).ToList();
            for(int i=0;i<unused.Count-1;i++){
                if(unused[i].SameValue(unused[i+1])&&unused[i].IsBoardTile&&!unused[i+1].IsBoardTile){
                    unused[i].BoardHandDuplicateTile=unused[i+1];
                }
            }
            //pull out all the dups and put them on the end

            //this kinda thing would normally be a linked list type use case,
            //but will only run at most 26*26 times over a list that is at most 126 long
            //so not worth the hassle of converting between list types
            var dups=new List<HandOrBoardTile>();
            for(int i=unused.Count-1;i>0;i--)
            {
                if(unused[i].SameValue(unused[i-1])){
                    dups.Insert(0,unused[i]);
                    unused.RemoveAt(i);
                }
            }
            unused.AddRange(dups);


            var groups=new List<List<HandOrBoardTile>>();
            bool newAddg=true;
            for(int i=0;i<unused.Count-1;i++)
            {
                if(newAddg){
                    groups.Add(new List<HandOrBoardTile>{unused[i]});
                }
                if(unused[i].Tile.Number==unused[i+1].Tile.Number)
                {
                    groups[groups.Count-1].Add(unused[i+1]);
                    newAddg=false;
                }else{
                    newAddg=true;
                }
            }
            foreach(var group in (groups.Where(x=>x.Count>=3)))
            {
                foundGroup.Add(new MaxGroup(group.Select(x=>x.Tile.ID).ToArray()));
                foreach(var tile in group){
                    unused.Remove(tile);
                }
            }
            unusedIds=Encoding.ASCII.GetString(unused.OrderBy(x=>x.Tile.Number).ThenBy(x=>x.Tile.Color.Letter).Select(x=>x.Tile.ID).ToArray());
        }
        public List<MaxGroup> foundGroup=new List<MaxGroup>();
        public string unusedIds;

        private PossibleGroupingConfig GenerateGroupConfig(){
            //should make a copy of base set of unused for the .current method to add to
            var res=new PossibleGroupingConfig(unusedIds);
            for(int i=0;i<foundGroup.Count;i++)
            {
                int? group=foundGroup[i].Current(ref res.unusedTiles);
                if(group.HasValue){
                    res.groups.Add(group.Value);
                }
            }
            groupsCreated++;
            if(groupsCreated%100000==0){
                Console.WriteLine(groupsCreated);
            }
            return res;
        }
        public PossibleGroupingConfig[] GeneratePossibilities(){
            int size=1;
            for(int i=0;i<foundGroup.Count;i++){
                size*=foundGroup[i].Count;
            }
            var results=new PossibleGroupingConfig[size];
            int ndx=0;
            //model it after counting, carrying the one by incrementing the next group
            //and reseting all the ones before it
            int currentDigit=0;
            bool done=false;
            while(!done){
                results[ndx]=GenerateGroupConfig();
                if(foundGroup[currentDigit].IsAtEnd())
                {
                    while(!done&&foundGroup[currentDigit].IsAtEnd()){
                        currentDigit++;
                        if(currentDigit>=foundGroup.Count) 
                        {
                            done=true;
                        }
                    }
                    if(!done){
                        foundGroup[currentDigit].MoveNext();
                        for(int i=0;i<currentDigit;i++){
                            foundGroup[i].ResetIteration();
                        }
                        currentDigit=0;
                    }
                }
                else{
                    foundGroup[currentDigit].MoveNext();
                }
                ndx++;
            }
            return results;
        }

        private static int groupsCreated=0;
        public IEnumerator<PossibleGroupingConfig> GetEnumerator()
        {
            //model it after counting, carrying the one by incrementing the next group
            //and reseting all the ones before it
            int currentDigit=0;
            bool done=false;
            while(!done){
                yield return GenerateGroupConfig();
                if(foundGroup[currentDigit].IsAtEnd())
                {
                    while(!done&&foundGroup[currentDigit].IsAtEnd()){
                        currentDigit++;
                        if(currentDigit>=foundGroup.Count) 
                        {
                            done=true;
                        }
                    }
                    if(!done){
                        foundGroup[currentDigit].MoveNext();
                        for(int i=0;i<currentDigit;i++){
                            foundGroup[i].ResetIteration();
                        }
                        currentDigit=0;
                    }
                }
                else{
                    foundGroup[currentDigit].MoveNext();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

