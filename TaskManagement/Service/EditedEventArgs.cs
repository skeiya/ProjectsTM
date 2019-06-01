using System.Collections.Generic;
using TaskManagement.Model;

namespace TaskManagement.Service
{
    public class EditedEventArgs
    {
        public EditedEventArgs(List<Member> members)
        {
            UpdatedMembers = members;
        }
        public List<Member> UpdatedMembers { get; internal set; }
    }
}