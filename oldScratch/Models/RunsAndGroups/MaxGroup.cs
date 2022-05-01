using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rummikub.Logic;

namespace Rummikub.Models.RunsAndGroups
{
    //easy to find grouping where nB nT nY nR,
    //then generate 6 possibilities as iterable:
    //nB nT nY nR
    //   nT nY nR with nB added to unused
    //nB    nY nR with nT added to unused
    //nB nT    nR with nY added to unused
    //nB nT nY    with nR added to unused
    //            with nB nT nY nR added to unused
    //other possibility is 3 tiles where it's either all or nothing
    public struct GroupIteration{
        public int? groupInt;
        public string addToUnused;
    }
    public class MaxGroup
    {
        public MaxGroup(byte[] tilesFound)
        {
            tiles=tilesFound;
            ResetIteration();
            possibilities=new List<GroupIteration>();
            possibilities.Add(new GroupIteration{
                groupInt=IntGroup.BundleGroup(tilesFound),
                addToUnused=""
            });
            if(tilesFound.Length==4){
                possibilities.Add(new GroupIteration{
                    groupInt=IntGroup.BundleGroup(new byte[]{tilesFound[0],tilesFound[1],tilesFound[2]}),
                    addToUnused=Encoding.ASCII.GetString(new byte[]{tilesFound[3]})
                });
                possibilities.Add(new GroupIteration{
                    groupInt=IntGroup.BundleGroup(new byte[]{tilesFound[0],tilesFound[1],tilesFound[3]}),
                    addToUnused=Encoding.ASCII.GetString(new byte[]{tilesFound[2]})
                });
                possibilities.Add(new GroupIteration{
                    groupInt=IntGroup.BundleGroup(new byte[]{tilesFound[0],tilesFound[2],tilesFound[3]}),
                    addToUnused=Encoding.ASCII.GetString(new byte[]{tilesFound[1]})
                });
                possibilities.Add(new GroupIteration{
                    groupInt=IntGroup.BundleGroup(new byte[]{tilesFound[1],tilesFound[2],tilesFound[3]}),
                    addToUnused=Encoding.ASCII.GetString(new byte[]{tilesFound[0]})
                });
            }
            possibilities.Add(new GroupIteration{groupInt=null,addToUnused=Encoding.ASCII.GetString(tilesFound)});
        }
        private List<GroupIteration> possibilities;
        public byte[] tiles;
        private int currentGroupConfig;

        public void ResetIteration(){
            currentGroupConfig=0;
        }
        public void MoveNext(){
            currentGroupConfig++;
        }
        public bool IsAtEnd()=> currentGroupConfig==possibilities.Count-1;
        public int Count=> possibilities.Count;
        public int? Current(ref string unusedToAddTo)
        {
            unusedToAddTo+=possibilities[currentGroupConfig].addToUnused;
            return possibilities[currentGroupConfig].groupInt;
        }
        /*
        public override string ToString()
        {
            string part="#"+tiles[0].Tile.Number+
            $"<{string.Join("",tiles.Select(x=>x.Tile.Color.Letter))}>({currentGroupConfig})";
            if(tiles.Length==4) part+= $"({5-currentGroupConfig}/6)";
            else part += $"({4-currentGroupConfig}/2)";
            return part;
        }
        */
    }
}

