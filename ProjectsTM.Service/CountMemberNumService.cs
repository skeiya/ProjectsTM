using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectsTM.Model;


namespace ProjectsTM.Service
{
    public static class CountMemberNumService
    {
        public  static string GetCountStr(AppData appData )
        {
            var temp = new HashSet<string>();
            foreach (var m in appData.Members)
            {
                var belengTo = m.Company;
                temp.Add(belengTo);
            }
            var CompanyList = temp.ToArray();

            var countList = new List<int>();
            int count;
            foreach (var i in CompanyList)
            {
                count = 0;
                foreach (var m in appData.Members)
                {
                    if (i.Equals(m.Company))
                    {
                        count++;
                    }
                }
                countList.Add(count);
            }
            count = 0;
            string dispStr = string.Format("Total:{0} (", appData.Members.Count);
            foreach (var i in CompanyList)
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
