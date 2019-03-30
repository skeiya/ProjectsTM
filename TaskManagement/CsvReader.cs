using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaskManagement
{
    class CsvReader
    {
        public static AppData ReadOriginalData(string fileName, Callender callender/*TODO*/)
        {
            var original = new AppData(false);
            var isFirstLine = true;
            using (var r = new StreamReader(fileName))
            {
                while (true)
                {
                    var line = r.ReadLine();
                    if (string.IsNullOrEmpty(line)) return original;
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    var words = line.Split(',');

                    var project = ParseProject(words[5]);
                    var taskName = words[3];
                    var period = ParsePeriod(words[1], words[2], callender);
                    var member = ParseMember(words[0]);

                    original.WorkItems.Add(new WorkItem(project, taskName, period, member));
                    original.Callender = callender;
                    original.Members.Add(member);
                    original.Projects.Add(project);
                }
            }
        }

        private  static Project ParseProject(string tag)
        {
            var words = tag.Split('|');
            foreach (var w in words)
            {
                if (w.Equals("C171") || w.Equals("C173") || w.Equals("C174")) return new Project(w);
            }
            return new Project("tmp");
        }

        private static Member ParseMember(string str)
        {
            string lastName = "";
            string firstName = "";
            string company = "";

            var match = Regex.Match(str, "([a-zA-Z]+)(.*)");
            var groups = match.Groups;
            switch (groups.Count)
            {
                case 0:
                case 1:
                    break;
                case 2:
                    lastName = groups[1].Value;
                    break;
                case 3:
                    company = groups[1].Value;
                    lastName = groups[2].Value;
                    break;
                default:
                    break;
            }
            return new Member(lastName, firstName, company);
        }

        private static Period ParsePeriod(string from, string to, Callender callender)
        {
            var f = GetDay(from, callender);
            var t = GetDay(to, callender);
            return new Period(f, t, callender);
        }

        private static CallenderDay GetDay(string dayString, Callender callender)
        {
            var words = dayString.Split('/');
            var year = int.Parse(words[0]);
            var month = int.Parse(words[1]);
            var day = int.Parse(words[2]);
            return callender.Get(year, month, day);
        }
    }
}
