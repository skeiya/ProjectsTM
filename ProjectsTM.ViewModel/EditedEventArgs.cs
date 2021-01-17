using ProjectsTM.Model;
using System.Collections.Generic;

namespace ProjectsTM.ViewModel
{
    public class EditedEventArgs : IEditedEventArgs
    {
        public EditedEventArgs(IEnumerable<Member> members)
        {
            UpdatedMembers = members;
        }
        public IEnumerable<Member> UpdatedMembers { get; internal set; }
    }
}