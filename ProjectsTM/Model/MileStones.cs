using System.Collections;
using System.Collections.Generic;

namespace ProjectsTM.Model
{
    public class MileStones : IEnumerable<MileStone>
    {
        private List<MileStone> _list = new List<MileStone>();

        internal MileStones Clone()
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
            if (m == null) return;
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

        internal void Replace(MileStone before, MileStone after)
        {
            _list[_list.FindIndex(ind => ind.Equals(before))] = after;
        }

        internal void Delete(MileStone m)
        {
            _list.Remove(m);
        }

        internal void Sort()
        {
            _list.Sort();
        }

        internal bool IsEmpty()
        {
            return _list.Count == 0;
        }

        internal MileStoneFilters GetMileStoneFilters()
        {
            var result = new MileStoneFilters();
            foreach (var mileStone in this._list)
            {
                if (result.Contains(mileStone.MileStoneFilter)) continue;
                result.Add(mileStone.MileStoneFilter);
            }
            return result;
        }

        public MileStoneFilters GeMatchedMileStoneFilters(string searchPattern)
        {
            var result = new MileStoneFilters();
            if (string.IsNullOrEmpty(searchPattern)) return result;
            try
            {
                foreach (var ms in this._list)
                {
                    if (!ms.IsMatchFilter(searchPattern)) continue;
                    if (result.Contains(ms.MileStoneFilter)) continue;
                    result.Add(ms.MileStoneFilter);
                }
                return result;
            }
            catch { return new MileStoneFilters(); }
        }
    }
}
