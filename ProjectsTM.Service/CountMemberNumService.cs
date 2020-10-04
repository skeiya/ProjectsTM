using ProjectsTM.Model;
using System.Collections.Generic;
using System.Linq;


namespace ProjectsTM.Service
{
    public static class CountMemberNumService
    {
        public  static string GetCountStr(Members members )
        {
            var temp = new HashSet<string>();
            foreach (var m in members)
            {
                var belengTo = m.Company;
                temp.Add(belengTo);
            }
            var companyList = temp.ToArray();

            var countList = new List<int>();
            int count;
            foreach (var i in companyList)
            {
                count = 0;
                foreach (var m in members)
                {
                    if (i.Equals(m.Company))
                    {
                        count++;
                    }
                }
                countList.Add(count);
            }
            count = 0;
            string dispStr = string.Format("Total:{0} (", members.Count);
            foreach (var i in companyList)
            {
                dispStr += Concat(i, countList[count]);
                count++;
            }
            return dispStr + ")";
        }

        private static string Concat(string belongTo, int count)
        {
            return $"{belongTo}:{count} ";
        }
    }
}
