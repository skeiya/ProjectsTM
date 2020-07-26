using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System.Collections.Generic;

namespace ProjectsTM.Service
{
    public class EditedEventArgs : IEditedEventArgs
    {
        public EditedEventArgs(List<Member> members)
        {
            UpdatedMembers = members;
        }
        public List<Member> UpdatedMembers { get; internal set; }
    }
}