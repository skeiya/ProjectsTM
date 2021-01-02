using ProjectsTM.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsTM.Service
{
    public class AtomicAction : IEnumerable<EditAction>
    {
        private readonly List<EditAction> _list = new List<EditAction>();

        public List<Member> Members => _list.Select((l) => l.Member).ToList();

        public IEnumerator<EditAction> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        internal void Add(EditAction action)
        {
            _list.Add(action);
        }

        internal void Clear()
        {
            _list.Clear();
        }

        internal AtomicAction Clone()
        {
            var result = new AtomicAction();
            foreach (var i in _list) result.Add(i);
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
