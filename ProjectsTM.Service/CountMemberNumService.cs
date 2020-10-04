using ProjectsTM.Model;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProjectsTM.Service
{
    public static class CountMemberNumService
    {
        public  static string GetCountStr(Members members)
        {
            var companyList = GetCompanyList(members);

            var countList = GetCountList(members, companyList).ToList();

            var count = 0;
            string dispStr = string.Format("Total:{0} (", members.Count);
            foreach (var company in companyList)
            {
                dispStr += Concat(company, countList[count]);
                count++;
            }
            return dispStr + ")";
        }

        private static string Concat(string belongTo, int count)
        {
            return $"{belongTo}:{count} ";
        }

        private static IEnumerable<string> GetCompanyList(Members members)
        {
            var companyList = new HashSet<string>();
            foreach (var m in members)
            {
                var belengTo = m.Company;
                companyList.Add(belengTo);
            }
            return companyList;
        }

        private static IEnumerable<int> GetCountList(Members members, IEnumerable<string> companyList)
        {
            var countList = new List<int>();
            int count;
            foreach (var company in companyList)
            {
                count = 0;
                foreach (var member in members)
                {
                    if (company.Equals(member.Company))
                    {
                        count++;
                    }
                }
                countList.Add(count);
            }
            return countList;
        }

    }
}
