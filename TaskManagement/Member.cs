using System;
using System.Collections.Generic;

namespace TaskManagement
{
    public class Member : IComparable<Member>
    {
        public Member(string lastName, string firstName, string company)
        {
            FirstName = firstName;
            LastName = lastName;
            Company = company;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Company { get; private set; }
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(FirstName)) return "XX";
                if (!string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(FirstName))
                {
                    return LastName.Substring(0, 1) + FirstName.Substring(0, 1);
                }
                if (!string.IsNullOrEmpty(LastName)) return LastName.Substring(0, 1);
                return FirstName.Substring(0, 1);
            }
        }

        internal string ToSerializeString()
        {
            return LastName + "/" + FirstName + "/" + Company;
        }

        internal static Member Parse(string text)
        {
            var words = text.Split('/');
            if (words.Length < 3) return null;
            return new Member(words[0], words[1], words[2]);
        }

        public override string ToString()
        {
            return DisplayName + "(" + Company + ")";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var target = obj as Member;
            if (target == null) return false;
            if (!FirstName.Equals(target.FirstName)) return false;
            if (!LastName.Equals(target.LastName)) return false;
            return Company.Equals(target.Company);
        }

        public override int GetHashCode()
        {
            var hashCode = 826127486;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Company);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
            return hashCode;
        }

        public int CompareTo(Member other)
        {
            var cmp = this.Company.CompareTo(other.Company);
            if (cmp != 0) return cmp;
            return this.DisplayName.CompareTo(other.DisplayName);
        }
    }
}