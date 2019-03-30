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

        internal void Add(Member member)
        {
            if (_members.Contains(member)) return;
            _members.Add(member);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        public static int RowCount => 2;


        internal bool Contain(Member m)
        {
            foreach(var f in _members)
            {
                if (f.Equals(m)) return true;
            }
            return false;
        }

        internal bool IsEmpty()
        {
            return _members == null || _members.Count == 0;
        }
    }
}