using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TaskManagement.UI;

namespace TaskManagement
{
    public partial class Form1 : Form
    {
        private ViewData _viewData = new ViewData(new AppData());
        private SearchWorkitemForm _searchForm;
        private int _fontSize = 6;
        TaskGrid _grid;
        WorkItem _draggingWorkItem = null;
        CallenderDay _draggedDay = null;
        Period _draggedPeriod = null;
        private float _viewRatio = 1.0f;
        string _previousFileName;

        public Form1()
        {
            InitializeComponent();
            menuStrip1.ImageScalingSize = new Size(16, 16);

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
            _viewData.SelectedWorkItemChanged += _viewData_SelectedWorkItemChanged;
            this.panel1.Resize += Panel1_Resize;
            taskDrawAria.Size = panel1.Size;
            statusStrip1.Items.Add("");
            taskDrawAria.AllowDrop = true;
            taskDrawAria.DragEnter += TaskDrawAria_DragEnter;
            taskDrawAria.DragDrop += TaskDrawAria_DragDrop;
        }

        private void TaskDrawAria_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (fileName.Length == 0) return;
            if (string.IsNullOrEmpty(fileName[0])) return;
            OpenFile(fileName[0]);
            taskDrawAria.Invalidate();
        }

        private void TaskDrawAria_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void _viewData_SelectedWorkItemChanged(object sender, EventArgs e)
        {
            taskDrawAria.Invalidate();
        }

        private void Panel1_Resize(object sender, EventArgs e)
        {
            ApplyViewRatio();
        }

        private void _viewData_FilterChanged(object sender, EventArgs e)
        {
            taskDrawAria.Invalidate();
        }

        private void TaskDrawAria_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateHoveringTest(e);
            UpdateDraggingItem(e);
            taskDrawAria.Invalidate();
        }

        private void UpdateDraggingItem(MouseEventArgs e)
        {
            if (_draggingWorkItem == null) return;
            if (_grid == null) return;
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
                _draggingWorkItem.Period = _draggedPeriod.ApplyOffset(offset, _viewData.Original.Callender);
            }
        }

        private void UpdateHoveringTest(MouseEventArgs e)
        {
            if (IsDradding()) return;
            if (_grid == null) return;
            var wi = _grid.PickFromPoint(e.Location, _viewData);
            statusStrip1.Items[0].Text = wi == null ? string.Empty : wi.ToString(_viewData.Original.Callender);
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

        private bool IsDradding()
        {
            return _draggingWorkItem != null;
        }

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

        private void ImportMemberAndWorkItems()
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.Original.Members = CsvReader.ReadMembers(dlg.FileName);
                _viewData.Original.WorkItems = CsvReader.ReadWorkItems(dlg.FileName);
                foreach (var w in _viewData.Original.WorkItems) // TODO 暫定
                {
                    _viewData.UpdateCallenderAndMembers(w);
                }
            }
        }

        private void ToolStripMenuItemImportOldFile_Click(object sender, EventArgs e)
        {
            ImportMemberAndWorkItems();
            taskDrawAria.Invalidate();
        }

        private void ToolStripMenuItemExportRS_Click(object sender, EventArgs e)
        {
            RSFileExporter.Export(_viewData.Original);
        }

        private void ToolStripMenuItemPrint_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument;
            using (var dlg = new PrintDialog())
            {
                dlg.Document = printPreviewDialog1.Document;
                if (dlg.ShowDialog() != DialogResult.OK) return;
            }
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
            if (string.IsNullOrEmpty(_previousFileName))
            {
                ToolStripMenuItemSaveAsOtherName_Click(sender, e);
                return;
            }
            AppDataSerializer.Serialize(_previousFileName, _viewData.Original);
        }

        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                OpenFile(dlg.FileName);
                _previousFileName = dlg.FileName;
            }
            taskDrawAria.Invalidate();
        }

        private void OpenFile(string fileName)
        {
            string error;
            var result = AppDataSerializer.Deserialize(fileName, out error);
            if (result == null)
            {
                MessageBox.Show(error);
                return;
            }
            _viewData.Original = result;
        }

        private void ToolStripMenuItemFilter_Click(object sender, EventArgs e)
        {
            using (var dlg = new FilterForm(_viewData))
            {
                dlg.ShowDialog(this);
            }
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

        private void ToolStripMenuItemSmallRatio_Click(object sender, EventArgs e)
        {
            if (_viewRatio <= 0.2) return;
            _viewRatio -= 0.1f;
            ApplyViewRatio();
        }

        private void ApplyViewRatio()
        {
            panel1.AutoScroll = _viewRatio > 1;
            taskDrawAria.Size = new Size((int)(panel1.Size.Width * _viewRatio), (int)(panel1.Size.Height * _viewRatio));
            taskDrawAria.Invalidate();
        }

        private void ToolStripMenuItemLargeRatio_Click(object sender, EventArgs e)
        {
            _viewRatio += 0.1f;
            ApplyViewRatio();
        }

        private void ToolStripMenuItemManageMember_Click(object sender, EventArgs e)
        {
            using (var dlg = new ManageMemberForm(_viewData.Original))
            {
                dlg.ShowDialog(this);
            }
            taskDrawAria.Invalidate();
        }

        private void ToolStripMenuItemSaveAsOtherName_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                AppDataSerializer.Serialize(dlg.FileName, _viewData.Original);
                _previousFileName = dlg.FileName;
            }
        }
    }
}
