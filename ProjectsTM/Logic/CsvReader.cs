using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ProjectsTM.Model;

namespace ProjectsTM.Logic
{
    class CsvReader
    {
        private static Tags ParseTags(string tag)
        {
            var result = new List<string>();
            var words = tag.Split('|');
            foreach (var w in words)
            {
                result.Add(w);
            }
            return new Tags(result);
        }

        private static Project ParseProject(string tag, string workitem, IEnumerable<string> existingProjects)
        {
            foreach (var w in tag.Split('|'))
            {
                foreach (var p in existingProjects)
                {
                    if (w.Contains(p)) return new Project(p);
                }
            }
            foreach (var p in existingProjects)
            {
                if (workitem.Contains(p)) return new Project(p);
            }
            return new Project("NoPrj");
        }

        private static Member ParseMember(string str)
        {
            var groups = Regex.Match(str, "([a-zA-Z]+)(.*)").Groups;
            switch (groups.Count)
            {
                case 2:
                    return new Member(groups[1].Value, "", "");
                case 3:
                    return new Member(groups[2].Value, "", groups[1].Value);
                default:
                    break;
            }
            return new Member("", "", "");
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
            using (var r = StreamFactory.CreateReader(fileName))
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
            using (var r = StreamFactory.CreateReader(fileName))
            {
                r.ReadLine(); //タイトル行を読み捨てる
                while (true)
                {
                    var line = r.ReadLine();
                    if (string.IsNullOrEmpty(line)) return result;
                    result.Add(ReadMemberLine(line));
                }
            }
        }

        private static Member ReadMemberLine(string text)
        {
            var words = text.Split(',');
            return ParseMember(words[0]);
        }

        public static WorkItems ReadWorkItems(string fileName)
        {
            var result = new WorkItems();
            MessageBox.Show("プロジェクト名定義ファイルを選択してください。（フォーマット：pro1|pro2b|pro3）");
            string[] existingProjects;
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return result;
                var file = dlg.FileName;
                if (!File.Exists(file)) return result;
                existingProjects = File.ReadAllText(file).Split('|');
                if (null == existingProjects) return result;
            }
            using (var r = StreamFactory.CreateReader(fileName))
            {
                r.ReadLine(); //タイトル行を読み捨てる
                var lineNo = 1;
                while (true)
                {
                    var line = r.ReadLine();
                    lineNo++;
                    if (string.IsNullOrEmpty(line)) return result;
                    result.Add(ReadWorkItemLine(line, lineNo, existingProjects));
                }
            }
        }

        private static WorkItem ReadWorkItemLine(string text, int lineNo, IEnumerable<string> existingProjects)
        {
            var words = text.Split(',');
            if (words.Length != 10)
            {
                throw new System.Exception(string.Format("{0}行目の区切り数が異常です。", lineNo));
            }

            var project = ParseProject(words[5], words[3], existingProjects);
            var tags = ParseTags(words[5]);
            var taskName = words[3];
            var period = ParsePeriod(words[1], words[2]);
            var member = ParseMember(words[0]);
            return new WorkItem(project, taskName, tags, period, member, TaskState.New, string.Empty);
        }
    }
}
