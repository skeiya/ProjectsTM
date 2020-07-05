using System.Collections.Generic;
using ProjectsTM.Model;

namespace ProjectsTM.Service
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