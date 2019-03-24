using System.Collections.Generic;

namespace TaskManagement
{
    public class Member
    {
        public Member(string lastName, string firstName, string company)
        {
            FirstName = firstName;
            LastName = lastName;
            Company = company;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Company { get; }
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
    }
}