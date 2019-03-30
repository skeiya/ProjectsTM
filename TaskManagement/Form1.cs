using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class Form1 : Form
    {
        private ViewData _viewData = new ViewData(new AppData());
        private FilterForm _filterForm;
        private int _fontSize = 4;

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
            statusStrip1.Text = wi.ToString();
        }

        TaskGrid _grid;
        private void TaskDrawAria_Paint(object sender, PaintEventArgs e)
        {
            _grid = new TaskGrid(_viewData, e.Graphics, this.taskDrawAria.Bounds, new Font(this.Font.FontFamily, _fontSize));
            _grid.Draw(_viewData);
            taskDrawAria.Invalidate();
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var grid = new TaskGrid(_viewData, e.Graphics, e.PageBounds, new Font(this.Font.FontFamily, _fontSize));
            grid.Draw(_viewData);
        }


        private void ImportMembers()
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.Original.Members = CsvReader.ReadMembers(dlg.FileName);
            }
        }

        private void ImportWorkItems(Callender callender)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.Original.WorkItems = CsvReader.ReadWorkItems(dlg.FileName, callender);
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

        private void ToolStripMenuItemImportOldFile_Click(object sender, EventArgs e)
        {
            var workItemImportable = !_viewData.Original.Callender.IsEmpty() && _viewData.Original.Members.IsEmpty();
            using (var dlg = new CsvImportSelectForm(workItemImportable))
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
                        ImportWorkItems(_viewData.Original.Callender);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ToolStripMenuItemExportRS_Click(object sender, EventArgs e)
        {
            RSFileExporter.Export(_viewData.Original);
        }

        private void ToolStripMenuItemPrint_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument;
            if (printPreviewDialog1.ShowDialog() != DialogResult.OK) return;
        }

        private void ToolStripMenuItemAddWorkItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new EditWorkItem(_viewData.Original))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var wi = dlg.WorkItem;
                _viewData.Original.WorkItems.Add(wi);
                var days = _viewData.Original.Callender.Days;
                if (!days.Contains(wi.Period.From)) days.Add(wi.Period.From);
                if (!days.Contains(wi.Period.To)) days.Add(wi.Period.To);
                days.Sort();
                if (!_viewData.Original.Members.Contain(wi.AssignedMember)) _viewData.Original.Members.Add(wi.AssignedMember);
            }
        }

        private void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                AppDataSerializer.Serialize(dlg.FileName, _viewData.Original);
            }
        }

        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                string error;
                var result = AppDataSerializer.Deserialize(dlg.FileName, out error);
                if (result == null)
                {
                    MessageBox.Show(error);
                    return;
                }
                _viewData = new ViewData(result);
            }
        }

        private void ToolStripMenuItemFilter_Click(object sender, EventArgs e)
        {
            if (_filterForm == null || _filterForm.IsDisposed)
            {
                _filterForm = new FilterForm(_viewData);
            }
            if (!_filterForm.Visible) _filterForm.Show(this);
        }

        private void ToolStripMenuItemColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorManagementForm(_viewData.ColorConditions, this))
            {
                dlg.ShowDialog();
            }
        }

        private void ToolStripMenuItemLargerFont_Click(object sender, EventArgs e)
        {
            _fontSize++;
        }

        private void フォント小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_fontSize <= 1) return;
            _fontSize--;
        }
    }
}
