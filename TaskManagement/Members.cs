using System;
using System.Collections;
using System.Collections.Generic;

namespace TaskManagement
{
    public class Members : IEnumerable<Member>
    {
        private List<Member> _members = new List<Member>();
        private List<Member> _filterMembers;

        public int Count => _members.Count;

        public IEnumerator<Member> GetEnumerator()
        {
            if (_filterMembers != null) return _filterMembers.GetEnumerator();
            return _members.GetEnumerator();
        }

        internal void Add(Member member)
        {
            _members.Add(member);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_filterMembers != null) return _filterMembers.GetEnumerator();
            return _members.GetEnumerator();
        }

        public static int RowCount => 2;

        internal void SetFilter(List<Member> filterMembers)
        {
            _filterMembers = filterMembers;
        }
    }
}