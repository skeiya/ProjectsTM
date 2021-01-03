using ProjectsTM.Model;
using System.Collections.Generic;

namespace ProjectsTM.ViewModel
{
    public interface IEditedEventArgs
    {
        IEnumerable<Member> UpdatedMembers { get; }
    }
}
