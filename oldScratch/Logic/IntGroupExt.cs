using System;
using Rummikub.Models.RunsAndGroups;

namespace Rummikub.Logic
{
    //model a group as an int containing 3 or 4 bytes which are the ids
    public static class IntGroup
    {

        //ids run 0-126, so sign bit is free to use, indicates group of 4
        //|
        //v
        //00000000 00000000 00000000 00000000
        //  id3?  |  id2   |   id1  |  id0
        public static int BundleGroup(byte[] ids)
        {
            int group=(ids[2]<<16)|(ids[1]<<8)|ids[0];
            if(ids.Length==4){
                group|=ids[3]<<24;
                group|=1<<31;
            }
            return group;
        }
        public static int GroupLength(this int group)=>group>0?3:4;
        public static byte GroupTileAt(this int group,int index)
        {
            if(index>=group.GroupLength()) 
            {
                throw new IndexOutOfRangeException(
                    $"for group {group} with length {group.GroupLength()}," +
                    $" attempt to access index {index}"
                );
            }
            return (byte)((index==3?(group&~(1<<31)):group)>>(index*8));
        }
        public static Group ToGroup(this int group)
        {
            if(IdTileMap.map.Count==0) throw new InvalidOperationException("please fill IdTileMap before calling int.toGroup");
            Group converted=new Group();
            for(int i=0;i<group.GroupLength();i++){
                converted.Add(group.GroupTileAt(i));
            }
            return converted;
        }
    }
}

