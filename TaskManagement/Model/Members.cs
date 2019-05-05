using System;
using System.Collections;
using System.Collections.Generic;

namespace TaskManagement.Model
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

        internal Members Clone()
        {
            var result = new Members();
            foreach (var m in _members) result.Add(m.Clone());
            return result;
        }

        public static int RowCount => 2;

        internal void Up(Member m)
        {
            var index = FindIndex(m);
            if (index == 0) return;
            var tmp = _members[index - 1];
            _members[index - 1] = m;
            _members[index] = tmp;
        }

        private int FindIndex(Member m)
        {
            return _members.FindIndex((x)=>x.Equals(m));
        }

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

        internal void Down(Member m)
        {
            var index = FindIndex(m);
            if (index == _members.Count - 1) return;
            var tmp = _members[index + 1];
            _members[index + 1] = m;
            _members[index] = tmp;
        }

        public override int GetHashCode()
        {
            return 229854969 + EqualityComparer<List<Member>>.Default.GetHashCode(_members);
        }
    }
}