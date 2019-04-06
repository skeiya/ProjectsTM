using System;
using System.Drawing;
using System.Windows.Forms;
using TaskManagement.UI;

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
            _viewData.FilterChanged += _viewData_FilterChanged;
            this.panel1.Resize += Panel1_Resize;
            taskDrawAria.Size = panel1.Size;
            statusStrip1.Items.Add("");
        }

        private void Panel1_Resize(object sender, EventArgs e)
        {
            taskDrawAria.Size = new Size((int)(panel1.Size.Width * _viewRatio), (int)(panel1.Size.Height * _viewRatio));
            taskDrawAria.Invalidate();
        }

        private void _viewData_FilterChanged(object sender, EventArgs e)
        {
            taskDrawAria.Invalidate();
        }

        WorkItem _draggingWorkItem = null;
        CallenderDay _draggedDay = null;
        Period _draggedPeriod = null;

        private void TaskDrawAria_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingWorkItem == null)
            {
                if (_grid == null) return;
                var wi = _grid.PickFromPoint(e.Location, _viewData);
                if (wi == null)
                {
                    statusStrip1.Items[0].Text = string.Empty;
                }
                else
                {
                    statusStrip1.Items[0].Text = wi.ToString();
                }
                return;
            }

            var member = _grid.GetMemberFromX(e.Location.X);
            if (member == null) return;
            var curDay = _grid.GetDayFromY(e.Location.Y);
            if (curDay == null) return;

            if (!_draggingWorkItem.AssignedMember.Equals(member))
            {
                _draggingWorkItem.AssignedMember = member;
            }
            var offset = _viewData.Original.Callender.GetOffset(_draggedDay, curDay);
            if (offset != 0)
            {
                _draggingWorkItem.Period = _draggedPeriod.ApplyOffset(offset);
            }
            taskDrawAria.Invalidate();
        }

        private void TaskDrawAria_MouseUp(object sender, MouseEventArgs e)
        {
            _draggingWorkItem = null;
        }

        private void TaskDrawAria_MouseDown(object sender, MouseEventArgs e)
        {
            var wi = _grid.PickFromPoint(e.Location, _viewData);
            if (wi == null) return;
            _viewData.Selected = wi;

            _draggingWorkItem = wi;
            _draggedPeriod = wi.Period.Clone();
            _draggedDay = _grid.GetDayFromY(e.Location.Y);

            taskDrawAria.Invalidate();
        }

        TaskGrid _grid;
        private SearchWorkitemForm _searchForm;
        private float _viewRatio = 1.0f;

        private void TaskDrawAria_Paint(object sender, PaintEventArgs e)
        {
            _grid = new TaskGrid(_viewData, e.Graphics, this.taskDrawAria.Bounds, new Font(this.Font.FontFamily, _fontSize));
            _grid.Draw(_viewData);
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
                foreach (var w in _viewData.Original.WorkItems) // TODO 暫定
                {
                    _viewData.UpdateCallenderAndMembers(w);
                }
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
            var workItemImportable = !_viewData.Original.Callender.IsEmpty() && !_viewData.Original.Members.IsEmpty();
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
            taskDrawAria.Invalidate();
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
            using (var dlg = new EditWorkItemForm(null, _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var wi = dlg.GetWorkItem(_viewData.Original.Callender);
                _viewData.Original.WorkItems.Add(wi);
                _viewData.UpdateCallenderAndMembers(wi);
            }
            taskDrawAria.Invalidate();
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
                _viewData.Original = result;
            }
            taskDrawAria.Invalidate();
        }

        private void ToolStripMenuItemFilter_Click(object sender, EventArgs e)
        {
            if (_filterForm == null || _filterForm.IsDisposed)
            {
                _filterForm = new FilterForm(_viewData);
            }
            if (!_filterForm.Visible) _filterForm.Show(this);
        }

        private void OnApplyFilter()
        {
            taskDrawAria.Invalidate();
        }

        private void ToolStripMenuItemColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorManagementForm(_viewData.Original.ColorConditions))
            {
                dlg.ShowDialog();
            }
            taskDrawAria.Invalidate();
        }

        private void ToolStripMenuItemLargerFont_Click(object sender, EventArgs e)
        {
            _fontSize++;
            taskDrawAria.Invalidate();
        }

        private void フォント小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_fontSize <= 1) return;
            _fontSize--;
            taskDrawAria.Invalidate();
        }

        private void ToolStripMenuItemSearch_Click(object sender, EventArgs e)
        {

            if (_searchForm == null || _searchForm.IsDisposed)
            {
                _searchForm = new SearchWorkitemForm(_viewData);
            }
            if (!_searchForm.Visible) _searchForm.Show(this);
        }

        private void TaskDrawAria_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var wi = _grid.PickFromPoint(e.Location, _viewData);
            if (wi == null) return;
            using (var dlg = new EditWorkItemForm(wi, _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.UpdateCallenderAndMembers(wi);
            }
            taskDrawAria.Invalidate();
        }

        private void ToolStripMenuItemWorkingDas_Click(object sender, EventArgs e)
        {
            using (var dlg = new ManagementWokingDaysForm(_viewData.Original.Callender, _viewData.Original.WorkItems))
            {
                dlg.ShowDialog();
            }
            taskDrawAria.Invalidate();
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            _viewRatio = 2;
            taskDrawAria.Size = new Size((int)(panel1.Size.Width * _viewRatio), (int)(panel1.Size.Height * _viewRatio));
            taskDrawAria.Invalidate();
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            _viewRatio = 1;
            taskDrawAria.Size = new Size((int)(panel1.Size.Width * _viewRatio), (int)(panel1.Size.Height * _viewRatio));
            taskDrawAria.Invalidate();
        }
    }
}
