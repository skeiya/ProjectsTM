﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class Form1 : Form
    {
        private AppData _appData;

        public Form1()
        {
            InitializeComponent();
            _appData = new AppData(true);

            foreach (System.Drawing.Printing.PaperSize s in printDocument.DefaultPageSettings.PrinterSettings.PaperSizes)
            {
                if (s.Kind == System.Drawing.Printing.PaperKind.A3)
                {
                    printDocument.DefaultPageSettings.PaperSize = s;
                }
            }
            printDocument.DefaultPageSettings.Landscape = true;
            printDocument.PrintPage += PrintDocument_PrintPage;
            this.taskDrawAria.Paint += TaskDrawAria_Paint;
            this.taskDrawAria.MouseDown += TaskDrawAria_MouseDown;
            this.taskDrawAria.MouseUp += TaskDrawAria_MouseUp;
            this.taskDrawAria.MouseMove += TaskDrawAria_MouseMove;
        }

        WorkItem _draggingWorkItem = null;
        CallenderDay _draggedDay = null;
        Period _draggedPeriod = null;

        private void TaskDrawAria_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingWorkItem == null) return;

            var member = _grid.GetMemberFromX(e.Location.X);
            if (member == null) return;
            var curDay = _grid.GetDayFromY(e.Location.Y);
            if (curDay == null) return;

            _draggingWorkItem.AssignedMember = member;
            var offset = _appData.Callender.GetOffset(_draggedDay, curDay);
            _draggingWorkItem.Period = _draggedPeriod.ApplyOffset(offset);
        }

        private void TaskDrawAria_MouseUp(object sender, MouseEventArgs e)
        {
            _draggingWorkItem = null;
        }

        private void TaskDrawAria_MouseDown(object sender, MouseEventArgs e)
        {
            var wi = _grid.PickFromPoint(e.Location, _appData);
            if (wi == null) return;
            _draggingWorkItem = wi;
            _draggedPeriod = wi.Period.Clone();
            _draggedDay = _grid.GetDayFromY(e.Location.Y);
            label1.Text = wi.ToString();
        }

        TaskGrid _grid;
        private void TaskDrawAria_Paint(object sender, PaintEventArgs e)
        {
            _grid = new TaskGrid(_appData, e.Graphics, this.taskDrawAria.Bounds, this.Font);
            _grid.Draw(_appData);
            taskDrawAria.Invalidate();
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var grid = new TaskGrid(_appData, e.Graphics, e.PageBounds, this.Font);
            grid.Draw(_appData);
        }

        private void buttonPrintPreview_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument;
            if (printPreviewDialog1.ShowDialog() != DialogResult.OK) return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _appData.WorkItems.SetFilter(textBox1.Text);
        }

        private void buttonColorSetting_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorManagementForm(_appData.ColorConditions, this))
            {
                dlg.ShowDialog();
            }
        }

        private void ButtonImportCSV_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _appData = LoadFile(dlg.FileName);
            }
        }

        private AppData LoadFile(string fileName)
        {
            var appData = new AppData(false);
            var isFirstLine = true;
            using (var r = new StreamReader(fileName))
            {
                while (true)
                {
                    var line = r.ReadLine();
                    if (string.IsNullOrEmpty(line)) return appData;
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    var words = line.Split(',');

                    var project = GetProject(words[5]);
                    var taskName = words[3];
                    var period = GetPeriod(words[1], words[2]);
                    var member = GetMember(words[0]);

                    appData.WorkItems.Add(new WorkItem(project, taskName, period, member));
                    appData.Callender = _appData.Callender;
                    appData.Members.Add(member);
                    appData.Projects.Add(project);
                }
            }
        }

        private Member GetMember(string str)
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
            var member = new Member(lastName, firstName, company);
            foreach (var m in _appData.Members)
            {
                if (m.Equals(member)) return m;
            }
            _appData.Members.Add(member);
            return member;
        }

        private Period GetPeriod(string from, string to)
        {
            var f = GetDay(from);
            var t = GetDay(to);
            return new Period(f, t, _appData.Callender);
        }

        private CallenderDay GetDay(string dayString)
        {
            var words = dayString.Split('/');
            var year = int.Parse(words[0]);
            var month = int.Parse(words[1]);
            var day = int.Parse(words[2]);
            return _appData.Callender.Get(year, month, day);
        }

        private Project GetProject(string tag)
        {
            var words = tag.Split('|');
            foreach (var w in words)
            {
                if (w.Equals("C171") || w.Equals("C173") || w.Equals("C174")) return new Project(w);
            }
            return new Project("tmp");
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            using (var dlg = new FilterForm(_appData.Members, _appData.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _appData.Callender.SetFilter(dlg.Period);
                _appData.Members.SetFilter(dlg.FilterMembers);
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            var members = GetMembers();
            var projects = GetProjects();
            var months = GetMonths();
            var rowCount = members.Count * projects.Count;
            var colCount = months.Count + 3;
            var csv = new string[rowCount, colCount];

            var r = 0;
            foreach (var m in members)
            {
                foreach (var p in GetProjects())
                {
                    csv[r, 0] = m.Company;
                    csv[r, 1] = m.LastName + " " + m.FirstName;
                    csv[r, 2] = p.ToString();
                    r++;
                }
            }
            var c = 0;
            foreach (var month in GetMonths())
            {
                r = 0;
                foreach (var member in members)
                {
                    foreach (var project in GetProjects())
                    {
                        csv[r, 3 + c] = GetRatio(month.Item1, month.Item2, member, project);
                        r++;
                    }
                }
                c++;
            }

            var result = string.Empty;
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    result += csv[row, col] + ",";
                }
                result += Environment.NewLine;
            }

            using(var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                File.WriteAllText(dlg.FileName, result);
            }
        }

        private string GetRatio(int year, int month, Member member, Project project)
        {
            return string.Format("{0:0.0}",(float)GetTargetDays(year, month, member, project) / (float)GetTotalDays(year, month));
        }

        private int GetTotalDays(int year, int month)
        {
            return _appData.Callender.GetDaysOfMonth(year, month);
        }

        private int GetTargetDays(int year, int month, Member member, Project project)
        {
            return _appData.WorkItems.GetWorkItemDays(year, month, member, project);
        }

        private List<Tuple<int, int>> GetMonths()
        {
            var result = new List<Tuple<int, int>>();

            var month = 0;
            foreach (var d in _appData.Callender.Days)
            {
                if (month != d.Month)
                {
                    result.Add(new Tuple<int, int>(d.Year, d.Month));
                    month = d.Month;
                }
            }
            return result;
        }

        private List<Project> GetProjects()
        {
            var result = new List<Project>();
            foreach (var p in _appData.Projects)
            {
                result.Add(p.Value);
            }
            return result;
        }

        private List<Member> GetMembers()
        {
            var result = new List<Member>();
            foreach (var m in _appData.Members)
            {
                result.Add(m);
            }
            result.Sort(); // 会社優先でソート
            return result;
        }
    }
}
