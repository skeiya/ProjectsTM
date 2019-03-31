using System;
using System.Collections;
using System.Collections.Generic;

namespace TaskManagement
{
    public class Members : IEnumerable<Member>
    {
        private List<Member> _members = new List<Member>();

        public int Count => _members.Count;

        public IEnumerator<Member> GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        public void Add(Member member)
        {
            if (_members.Contains(member)) return;
            _members.Add(member);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        public static int RowCount => 2;


        public bool Contain(Member m)
        {
            foreach(var f in _members)
            {
                if (f.Equals(m)) return true;
            }
            return false;
        }

        public bool IsEmpty()
        {
            return _members == null || _members.Count == 0;
        }

        public override bool Equals(object obj)
        {
            var target = obj as Members;
            if (target == null) return false;
            if (_members.Count != target._members.Count) return false;
            for (int index = 0; index < _members.Count; index++)
            {
                if (!_members[index].Equals(target._members[index])) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return 229854969 + EqualityComparer<List<Member>>.Default.GetHashCode(_members);
        }
    }
}