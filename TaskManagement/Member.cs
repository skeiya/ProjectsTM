namespace TaskManagement
{
    public class Member
    {
        public Member(string firstName, string lastName, string company)
        {
            FirstName = firstName;
            LastName = lastName;
            Company = company;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Company { get; }

        public override string ToString()
        {
            return LastName.Substring(0) + FirstName.Substring(0) + "(" + Company + ")";
        }
    }
}