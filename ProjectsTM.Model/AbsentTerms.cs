using System.Collections;
using System.Collections.Generic;

namespace ProjectsTM.Model
{
    public class AbsentTerms: IEnumerable<AbsentTerm>
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
    }
}
