using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class AbsentInfo : IEnumerable<AbsentTerm>
    {
        private readonly SortedDictionary<Member, AbsentTerms> _items = new SortedDictionary<Member, AbsentTerms>();

        public AbsentTerms OfMember(Member m) => _items.ContainsKey(m) ? _items[m] : new AbsentTerms();

        public AbsentTerms GetAbsentTerms(Member m)
        {
            if (!_items.TryGetValue(m, out AbsentTerms value)) return new AbsentTerms();
            return value;
        }

        public void Add(AbsentTerm a)
        {
            if (!_items.ContainsKey(a.Member))
            {
                _items.Add(a.Member, new AbsentTerms());
            }
            _items[a.Member].Add(a);
        }

        public void Replace(Member member, AbsentTerms absentTerms)
        {
            if (!absentTerms.Any()) return;
            RemoveInfoOfMember(member);
            _items.Add(member, absentTerms);
        }

        public void RemoveInfoOfMember(Member m)
        {
            if (!_items.ContainsKey(m)) return;
            _items.Remove(m);
        }

        public XElement ToXml()
        {
            var xml = new XElement(nameof(AbsentInfo));
            foreach (var a in _items)
            {
                var eachMember = new XElement("Info");
                eachMember.SetAttributeValue("Name", a.Key.ToSerializeString());
                eachMember.Add(a.Value.ToXml());
                xml.Add(eachMember);
            }
            return xml;
        }

        public static AbsentInfo FromXml(XElement xml)
        {
            var result = new AbsentInfo();
            foreach (var a in xml.Elements("Info"))
            {
                var member = Member.Parse(a.Attribute("Name").Value);
                foreach (var t in a.Element(nameof(AbsentTerms)).Elements(nameof(AbsentTerm)))
                {
                    var period = Period.FromXml(t);
                    result.Add(new AbsentTerm(member, period));
                }
            }
            return result;
        }

        public IEnumerator<AbsentTerm> GetEnumerator()
        {
            return _items.SelectMany((s) => s.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AbsentInfo target)) return false;
            if (target._items.Count != this._items.Count) return false;
            for (var idx = 0; idx < _items.Count; idx++)
            {
                var key = _items.Keys.ElementAt(idx);
                if (!_items[key].Equals(target._items[key])) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return -566117206 + EqualityComparer<SortedDictionary<Member, AbsentTerms>>.Default.GetHashCode(_items);
        }
    }
}
