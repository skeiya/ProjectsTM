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
        public string Name => LastName.Substring(0, 1) + FirstName.Substring(0, 1);

        public override string ToString()
        {
            return Name + "(" + Company + ")";
        }
    }
}