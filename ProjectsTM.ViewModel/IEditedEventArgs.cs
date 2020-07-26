using ProjectsTM.Model;
using System.Collections.Generic;

namespace ProjectsTM.ViewModel
{
    public interface IEditedEventArgs
    {
        List<Member> UpdatedMembers { get; }
    }
}
