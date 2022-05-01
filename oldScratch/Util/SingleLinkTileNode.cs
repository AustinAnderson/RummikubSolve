using System;
using System.Collections;
using System.Collections.Generic;
using Rummikub.Models;

namespace Rummikub.Util
{
    public class SingleLinkTileNode : IEnumerable<Tile>
    {
        public SingleLinkTileNode(Tile t){
            Value=t;
        }
        public Tile Value;
        public SingleLinkTileNode Next;


        public IEnumerator<Tile> GetEnumerator()
        {
            return new ListEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public class ListEnumerator : IEnumerator<Tile>
        {
            public ListEnumerator(SingleLinkTileNode node)
            {
                this.node=node;
                start=node;
            }
            private SingleLinkTileNode start;
            private SingleLinkTileNode node;
            public Tile Current => node.Value;

            object IEnumerator.Current => (Tile)Current;

            public void Dispose() { }
            public bool MoveNext()
            {
                if(node.Next==null)
                {
                    return false;
                } 
                node=node.Next;
                return true;
            }

            public void Reset()
            {
                node=start;
            }
        }
    }
}

