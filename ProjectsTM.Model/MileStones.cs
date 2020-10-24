using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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

        public void Replace(MileStone before, MileStone after)
        {
            _list[_list.FindIndex(ind => ind.Equals(before))] = after;
        }

        public void Delete(MileStone m)
        {
            _list.Remove(m);
        }

        public XElement ToXml()
        {
            var xml = new XElement(nameof(MileStones));
            _list.ForEach(m => xml.Add(m.ToXml()));
            return xml;
        }

        public static MileStones FromXml(XElement xml)
        {
            var result = new MileStones();
            foreach(var m in xml.Elements(nameof(MileStone)))
            {
                result.Add(MileStone.FromXml(m));
            }
            return result;
        }

        public void Sort()
        {
            _list.Sort();
        }

        public bool IsEmpty()
        {
            return _list.Count == 0;
        }

        public MileStoneFilters GetMileStoneFilters()
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

        public override bool Equals(object obj)
        {
            var target = obj as MileStones;
            if (target == null) return false;
            if (target._list.Count != _list.Count) return false;
            for(var idx = 0; idx <_list.Count; idx++)
            {
                if (!target._list[idx].Equals(_list[idx])) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
