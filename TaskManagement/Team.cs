using System.Collections.Generic;

namespace TaskManagement
{
    public class Team
    {
        private List<Member> _members = new List<Member>();

        public Team()
        {
            _members.Add(new Member("下村", "圭矢", "K"));
            _members.Add(new Member("hoge", "foo", "AB"));
            _members.Add(new Member("avd", "rg", "AB"));
        }

        public List<Member> Members => _members;
    }
}