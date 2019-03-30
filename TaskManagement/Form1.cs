using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class Form1 : Form
    {
        private ViewData _viewData = new ViewData(null);
        private FilterForm _filterForm;

        public Form1()
        {
            InitializeComponent();

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
            var offset = _viewData.Original.Callender.GetOffset(_draggedDay, curDay);
            _draggingWorkItem.Period = _draggedPeriod.ApplyOffset(offset);
        }

        private void TaskDrawAria_MouseUp(object sender, MouseEventArgs e)
        {
            _draggingWorkItem = null;
        }

        private void TaskDrawAria_MouseDown(object sender, MouseEventArgs e)
        {
            var wi = _grid.PickFromPoint(e.Location, _viewData);
            if (wi == null) return;
            _draggingWorkItem = wi;
            _draggedPeriod = wi.Period.Clone();
            _draggedDay = _grid.GetDayFromY(e.Location.Y);
            label1.Text = wi.ToString();
        }

        TaskGrid _grid;
        private void TaskDrawAria_Paint(object sender, PaintEventArgs e)
        {
            _grid = new TaskGrid(_viewData, e.Graphics, this.taskDrawAria.Bounds, this.Font);
            _grid.Draw(_viewData);
            taskDrawAria.Invalidate();
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var grid = new TaskGrid(_viewData, e.Graphics, e.PageBounds, this.Font);
            grid.Draw(_viewData);
        }

        private void buttonPrintPreview_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument;
            if (printPreviewDialog1.ShowDialog() != DialogResult.OK) return;
        }

        private void buttonColorSetting_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorManagementForm(_viewData.ColorConditions, this))
            {
                dlg.ShowDialog();
            }
        }

        private void ButtonImportCSV_Click(object sender, EventArgs e)
        {
            var workItemImportable = !_viewData.Original.Callender.IsEmpty() && _viewData.Original.Members.IsEmpty();
            using(var dlg = new CsvImportSelectForm(workItemImportable))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                switch (dlg.ImportType)
                {
                    case CsvImportType.WorkingDays:
                        ImportWorkingDays();
                        break;
                    case CsvImportType.Members:
                        ImportMembers();
                        break;
                    case CsvImportType.WorkItems:
                        ImportWorkItems();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ImportMembers()
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.Original.Members = CsvReader.ReadMembers(dlg.FileName);
            }
        }

        private void ImportWorkItems()
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData = LoadFile(dlg.FileName);
            }
        }

        private void ImportWorkingDays()
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.Original.Callender = CsvReader.ReadWorkingDays(dlg.FileName);
            }
        }

        private ViewData LoadFile(string fileName)
        {
            return new ViewData(CsvReader.ReadOriginalData(fileName, _viewData.Original.Callender));
        }


        private void buttonFilter_Click(object sender, EventArgs e)
        {
            if (_filterForm == null || _filterForm.IsDisposed)
            {
                _filterForm = new FilterForm(_viewData);
            }
            if (!_filterForm.Visible) _filterForm.Show(this);
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

            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                File.WriteAllText(dlg.FileName, result);
            }
        }

        private string GetRatio(int year, int month, Member member, Project project)
        {
            return string.Format("{0:0.0}", (float)GetTargetDays(year, month, member, project) / (float)GetTotalDays(year, month));
        }

        private int GetTotalDays(int year, int month)
        {
            return _viewData.Original.Callender.GetDaysOfMonth(year, month);
        }

        private int GetTargetDays(int year, int month, Member member, Project project)
        {
            return _viewData.Original.WorkItems.GetWorkItemDaysOfMonth(year, month, member, project);
        }

        private List<Tuple<int, int>> GetMonths()
        {
            var result = new List<Tuple<int, int>>();

            var month = 0;
            foreach (var d in _viewData.Original.Callender.Days)
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
            foreach (var p in _viewData.Original.Projects)
            {
                result.Add(p.Value);
            }
            return result;
        }

        private List<Member> GetMembers()
        {
            var result = new List<Member>();
            foreach (var m in _viewData.Original.Members)
            {
                result.Add(m);
            }
            result.Sort(); // 会社優先でソート
            return result;
        }
    }
}
