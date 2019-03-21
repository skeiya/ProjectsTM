using System;
using System.Collections;
using System.Collections.Generic;

namespace TaskManagement
{
    public class Members : IEnumerable
    {
        private List<Member> _members = new List<Member>();

        public int Count => _members.Count;

        public IEnumerator GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        internal void Add(Member member)
        {
            _members.Add(member);
        }
    }
}