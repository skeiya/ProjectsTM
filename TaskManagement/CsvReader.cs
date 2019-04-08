using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TaskManagement
{
    class CsvReader
    {
        private static List<string> _existingProjects = new List<string>() { "C171", "C173", "C174", "C181", "C175A", "C141B" };

        private static Tags ParseTags(string tag)
        {
            var result = new List<string>();
            var words = tag.Split('|');
            foreach (var w in words)
            {
                if (_existingProjects.Contains(w)) continue;
                result.Add(w);
            }
            return new Tags(result);
        }

        private static Project ParseProject(string tag)
        {
            var words = tag.Split('|');
            foreach (var w in words)
            {
                if (_existingProjects.Contains(w)) return new Project(w);
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

        private static Period ParsePeriod(string from, string to)
        {
            var f = CallenderDay.Parse(from);
            var t = CallenderDay.Parse(to);
            return new Period(f, t);
        }

        public static Callender ReadWorkingDays(string fileName)
        {
            var result = new Callender();
            using (var r = new StreamReader(fileName))
            {
                while (true)
                {
                    var line = r.ReadLine();
                    if (string.IsNullOrEmpty(line)) break;
                    result.Add(CallenderDay.Parse(line));
                }
            }
            return result;
        }

        public static Members ReadMembers(string fileName)
        {
            var result = new Members();
            var isFirstLine = true;
            using (var r = new StreamReader(fileName))
            {
                while (true)
                {
                    var line = r.ReadLine();
                    if (string.IsNullOrEmpty(line)) return result;
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    var words = line.Split(',');
                    var member = ParseMember(words[0]);
                    result.Add(member);
                }
            }
        }

        public static WorkItems ReadWorkItems(string fileName)
        {
            var result = new WorkItems();
            var isFirstLine = true;
            using (var r = new StreamReader(fileName))
            {
                var lineNo = 0;
                while (true)
                {
                    var line = r.ReadLine();
                    lineNo++;
                    if (string.IsNullOrEmpty(line)) return result;
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    var words = line.Split(',');
                    if (words.Length != 10) {

                        throw new System.Exception(String.Format("{0}行目の区切り数が異常です。", lineNo));
                    }

                    var project = ParseProject(words[5]);
                    var tags = ParseTags(words[5]);
                    var taskName = words[3];
                    var period = ParsePeriod(words[1], words[2]);
                    var member = ParseMember(words[0]);

                    result.Add(new WorkItem(project, taskName, tags, period, member));
                }
            }
        }
    }
}
