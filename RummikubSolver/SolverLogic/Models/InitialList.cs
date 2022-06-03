using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public abstract class InitialList:IList<Tile>
    {
        public IReadOnlyList<int> JokerIndexes => jokerIndexes;
        private List<int> jokerIndexes = new List<int>();
        protected string StringRep(IEnumerable<Tile> tiles)
        {
            return "[" + string.Join(",", tiles.Select(x => x.IsJoker?"J":$"{x.Number}{x.Color.Char()}")) + "]";
        }
        public override string ToString() => StringRep(this);
        private List<Tile> tiles = new List<Tile>();
        public int Count => tiles.Count;
        public bool IsReadOnly => false;
        //wasteful but super easy validation of copy current, change copy,
        //check copy, set tiles = copy, groups are capped at 4 and runs are capped at 13 so its fine
        protected abstract void ValidateModification(Tile[] preposedNewState);
        public abstract void UpdateJokerValues();
        public Tile this[int index] { 
            get => tiles[index];
            set
            {
                var preposedNew=tiles.ToArray();
                preposedNew[index] = value;
                ValidateModification(preposedNew);
                if(tiles[index].IsJoker && !preposedNew[index].IsJoker)
                {
                    jokerIndexes.Remove(index);
                }
                else if(!tiles[index].IsJoker && preposedNew[index].IsJoker)
                {
                    jokerIndexes.Add(index);
                }
                tiles = preposedNew.ToList();
            }
        }
        public void Add(Tile item)
        {
            var preposedNew = tiles.ToArray().ToList();// copy
            preposedNew.Add(item);
            ValidateModification(preposedNew.ToArray());
            if (item.IsJoker)
            {
                jokerIndexes.Add(this.Count - 1);
            }
            tiles=preposedNew.ToList();
        }
        public void Clear() => tiles.Clear();
        public bool Contains(Tile item) => tiles.Contains(item);
        public void CopyTo(Tile[] array, int arrayIndex) => tiles.CopyTo(array, arrayIndex);
        public int IndexOf(Tile item) => tiles.IndexOf(item);
        public void Insert(int index, Tile item)
        {
            var preposedNew = tiles.ToArray().ToList();// copy
            preposedNew.Insert(index, item);
            ValidateModification(preposedNew.ToArray());
            for(int i = 0; i < jokerIndexes.Count; i++)
            {
                if(jokerIndexes[i] >= index)
                {
                    jokerIndexes[i]++;
                }
            }
            if (item.IsJoker)
            {
                jokerIndexes.Add(index);
            }
            tiles=preposedNew.ToList();
        }
        public bool Remove(Tile item) => tiles.Remove(item);
        public void RemoveAt(int index) => tiles.RemoveAt(index);
        public IEnumerator<Tile> GetEnumerator() => tiles.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
