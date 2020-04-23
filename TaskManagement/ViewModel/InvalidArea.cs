using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Model;

namespace TaskManagement.ViewModel
{
    class InvalidArea
    {
        private List<Member> _clearList = new List<Member>();
        internal void Clear(Member m)
        {
            if (!IsDirty(m)) return;
            _clearList.Add(m);
        }

        internal bool IsDirty(Member m)
        {
            return !_clearList.Contains(m);
        }
    }
}
