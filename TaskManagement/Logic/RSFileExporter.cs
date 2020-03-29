using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TaskManagement.Model;

namespace TaskManagement.Logic
{
    public class RSFileExporter
    {
        public static void Export(AppData appData)
        {
            string result = MakeText(appData);

            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                File.WriteAllText(dlg.FileName, result);
            }
        }

        public static string MakeText(AppData appData)
        {
            var members = GetMembers(appData.Members);
            var months = GetMonths(appData.Callender);
            var projects = GetProjects(appData.WorkItems);
            var rowCount = members.Count * projects.Count;
            var colCount = months.Count + 3;
            var csv = new string[rowCount, colCount];

            var r = MakeMembersAndProjectsTitle(members, projects, csv);
            MakeMembersAndProjectsValue(appData, members, months, projects, csv, r);
            return MakeText(rowCount, colCount, csv);
        }

        private static string MakeText(int rowCount, int colCount, string[,] csv)
        {
            var result = string.Empty;
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    result += csv[row, col] + ",";
                }
                result += Environment.NewLine;
            }

            return result;
        }

        private static void MakeMembersAndProjectsValue(AppData appData, List<Member> members, List<Tuple<int, int>> months, List<Project> projects, string[,] csv, int r)
        {
            var c = 3;
            foreach (var month in months)
            {
                r = 0;
                foreach (var member in members)
                {
                    foreach (var project in projects)
                    {
                        csv[r, c] = GetRatio(month.Item1, month.Item2, member, project, appData.Callender, appData.WorkItems);
                        r++;
                    }
                }
                c++;
            }
        }

        private static int MakeMembersAndProjectsTitle(List<Member> members, List<Project> projects, string[,] csv)
        {
            var r = 0;
            foreach (var m in members)
            {
                foreach (var p in projects)
                {
                    csv[r, 0] = m.Company;
                    csv[r, 1] = m.LastName + " " + m.FirstName;
                    csv[r, 2] = p.ToString();
                    r++;
                }
            }

            return r;
        }

        private static List<Project> GetProjects(WorkItems workItems)
        {
            var result = new List<Project>();
            foreach (var wi in workItems)
            {
                if (!result.Contains(wi.Project)) result.Add(wi.Project);
            }
            return result;
        }

        private static List<Member> GetMembers(Members members)
        {
            var result = new List<Member>();
            foreach (var m in members)
            {
                result.Add(m);
            }
            result.Sort(); // 会社優先でソート
            return result;
        }

        private static List<Tuple<int, int>> GetMonths(Callender callender)
        {
            var result = new List<Tuple<int, int>>();

            var month = 0;
            foreach (var d in callender.Days)
            {
                if (month != d.Month)
                {
                    result.Add(new Tuple<int, int>(d.Year, d.Month));
                    month = d.Month;
                }
            }
            return result;
        }

        private static string GetRatio(int year, int month, Member member, Project project, Callender callender, WorkItems workItems)
        {
            return string.Format("{0:f1}", GetTargetDays(year, month, member, project, workItems, callender) / (float)GetTotalDays(year, month, callender));
        }

        private static int GetTotalDays(int year, int month, Callender callender)
        {
            return callender.GetDaysOfMonth(year, month);
        }

        private static int GetTargetDays(int year, int month, Member member, Project project, WorkItems workItems, Callender callender)
        {
            return workItems.GetWorkItemDaysOfMonth(year, month, member, project, callender);
        }
    }
}
