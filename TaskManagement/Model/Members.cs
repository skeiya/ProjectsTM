﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace TaskManagement.Model
{
    public class Members : IEnumerable<Member>, IEquatable<Members>
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
            return _members.FindIndex((x) => x.Equals(m));
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

        internal void SortByCompany()
        {
            _members.Sort((a, b) => a.Company.CompareTo(b.Company));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Members);
        }

        public bool Equals(Members other)
        {
            if (other == null) return false;
            if (this._members.Count != other._members.Count) return false;
            for(var i = 0; i < this._members.Count; i++)
            {
                if (!this._members[i].Equals(other._members[i])) return false;
            }
            return true;
        }
    }
}