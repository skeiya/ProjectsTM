using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class AbsentTerms : IEnumerable<AbsentTerm>
    {
        private List<AbsentTerm> _absentTerms = new List<AbsentTerm>();

        public void Add(AbsentTerm absentTerm)
        {
            if (_absentTerms.Contains(absentTerm)) return;
            _absentTerms.Add(absentTerm);
        }

        public void Remove(AbsentTerm absentTerm)
        {
            if (!_absentTerms.Contains(absentTerm)) return;
            _absentTerms.Remove(absentTerm);
        }

        public void Replace(AbsentTerm before, AbsentTerm after)
        {
            if (!_absentTerms.Contains(before)) return;
            _absentTerms.Remove(before);
            _absentTerms.Add(after);
        }

        public IEnumerator<AbsentTerm> GetEnumerator()
        {
            return _absentTerms.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _absentTerms.GetEnumerator();
        }

        internal XElement ToXml()
        {
            var xml = new XElement(nameof(AbsentTerms));
            _absentTerms.ForEach(a => xml.Add(a.ToXml()));
            return xml;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AbsentTerms target)) return false;
            if (target._absentTerms.Count != _absentTerms.Count) return false;
            for (var idx = 0; idx < _absentTerms.Count; idx++)
            {
                if (!target._absentTerms[idx].Equals(_absentTerms[idx])) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return 1155826442 + EqualityComparer<List<AbsentTerm>>.Default.GetHashCode(_absentTerms);
        }
    }
}
