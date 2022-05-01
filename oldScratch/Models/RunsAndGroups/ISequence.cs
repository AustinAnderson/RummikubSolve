using System;
using System.Collections.Generic;

namespace Rummikub.Models.RunsAndGroups
{
    public interface ISequence:IEnumerable<Tile>
    {
        Tile RemoveAt(int i);
        bool CanAdd(Tile t);
        void Add(Tile t);
        Tile this[int i] {get;}
        bool IsGroup {get;}
    }
}

