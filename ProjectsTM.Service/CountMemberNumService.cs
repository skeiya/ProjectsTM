using ProjectsTM.Model;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProjectsTM.Service
{
    public static class CountMemberNumService
    {
        public  static string GetCountStr(Members members, string selectedCompany)
        {
            var count = members.Count(m => m.Company.Equals(selectedCompany));

            var result = new StringBuilder("Total:" + members.Count.ToString() + " (");

            result.Append(selectedCompany + ":" + count.ToString());

            return result.ToString() + ")"; 
        }
    }
}
