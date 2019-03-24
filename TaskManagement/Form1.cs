using System;
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
            var wi = _grid.PickFromPoint(e.Location);
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
            _grid.Draw();
            taskDrawAria.Invalidate();
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var grid = new TaskGrid(_appData, e.Graphics, e.PageBounds, this.Font);
            grid.Draw();
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
    }
}
