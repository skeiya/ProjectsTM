using ProjectsTM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsTM.ViewModel
{
    public interface IEditedEventArgs
    {
        List<Member> UpdatedMembers { get; }
    }
}
