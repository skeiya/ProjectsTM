using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System.Collections.Generic;

namespace ProjectsTM.Service
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