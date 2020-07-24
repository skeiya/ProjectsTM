using System.Collections;
using System.Collections.Generic;

namespace ProjectsTM.Model
{
    public class MileStones : IEnumerable<MileStone>
    {
        private List<MileStone> _list = new List<MileStone>();

        public MileStones Clone()
        {
            var result = new MileStones();
            foreach (var m in _list)
            {
                result.Add(m.Clone());
            }
            return result;
        }

        public void Add(MileStone m)
        {
            _list.Add(m);
        }

        public IEnumerator<MileStone> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void Replace(MileStone before, MileStone after)
        {
            _list[_list.FindIndex(ind => ind.Equals(before))] = after;
        }

        public void Delete(MileStone m)
        {
            _list.Remove(m);
        }

        public void Sort()
        {
            _list.Sort();
        }

        internal bool IsEmpty()
        {
            return _list.Count == 0;
        }
    }
}
