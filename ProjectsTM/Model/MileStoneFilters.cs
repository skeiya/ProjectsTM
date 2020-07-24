using System;
using System.Collections;
using System.Collections.Generic;

namespace ProjectsTM.Model
{
    public class MileStoneFilters : IEnumerable<MileStoneFilter>
    {
        private List<MileStoneFilter> _mileStoneFilters = new List<MileStoneFilter>();

        public int Count => _mileStoneFilters.Count;

        public IEnumerator<MileStoneFilter> GetEnumerator()
        {
            return _mileStoneFilters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _mileStoneFilters.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return 229854969 + EqualityComparer<List<MileStoneFilter>>.Default.GetHashCode(_mileStoneFilters);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MileStoneFilter);
        }

        public bool Equals(MileStoneFilters other)
        {
            if (other == null) return false;
            if (this._mileStoneFilters.Count != other._mileStoneFilters.Count) return false;
            for (var i = 0; i < this._mileStoneFilters.Count; i++)
            {
                if (!this._mileStoneFilters[i].Equals(other._mileStoneFilters[i])) return false;
            }
            return true;
        }

        internal MileStoneFilters Clone()
        {
            var result = new MileStoneFilters();
            foreach (var msf in _mileStoneFilters) result.Add(msf.Clone());
            return result;
        }

        public void Add(MileStoneFilter msf)
        {
            if (this.Contains(msf)) return;
            _mileStoneFilters.Add(msf);
        }

        public bool Contains(MileStoneFilter msf)
        {
            return _mileStoneFilters.Contains(msf);
        }

        internal void Clear()
        {
            _mileStoneFilters.Clear();
        }
    }
}
