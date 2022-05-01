using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rummikub.Logic;

namespace Rummikub.Models.RunsAndGroups
{
    public class Group:List<byte>
    {
        public override string ToString()
        {
            return "#"+IdTileMap.map[this[0]].Tile.Number+
            $"<{string.Join("",this.Select(x=>IdTileMap.map[x].Tile.Color.Letter))}>";
        }
    }
}

