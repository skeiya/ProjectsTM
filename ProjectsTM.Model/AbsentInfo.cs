using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class AbsentInfo : IEnumerable<AbsentTerm>
    {
        private SortedDictionary<Member, AbsentTerms> _items = new SortedDictionary<Member, AbsentTerms>();

        public AbsentTerms OfMember(Member m) => _items.ContainsKey(m) ? _items[m] : new AbsentTerms();

        // XMLシリアライズ用
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
            if (member == null || absentTerms == null) return;
            if (absentTerms.Count() == 0) return;
            RemoveInfoOfMember(member);
            _items.Add(member, absentTerms);
        }

        public void RemoveInfoOfMember(Member m)
        {
            if (m == null) return;
            if (!_items.ContainsKey(m)) return;
            _items.Remove(m);
        }

        internal XElement ToXml()
        {
            var xml = new XElement(nameof(AbsentInfo));
            foreach(var a in _items)
            {
                var eachMember = new XElement("Info");
                eachMember.SetAttributeValue("Name", a.Key.ToSerializeString());
                eachMember.Add(a.Value.ToXml());
                xml.Add(eachMember);
            }
            return xml;
        }

        public IEnumerator<AbsentTerm> GetEnumerator()
        {
            return _items.SelectMany((s) => s.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
