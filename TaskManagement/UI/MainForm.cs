using System;
using System.Drawing;
using System.Windows.Forms;
using TaskManagement.Service;
using TaskManagement.UI;

namespace TaskManagement
{
    public partial class Form1 : Form
    {
        private ViewData _viewData = new ViewData(new AppData());
        private SearchWorkitemForm _searchForm;
        private TaskGrid _grid;
        private WorkItemDragService _workItemDragService = new WorkItemDragService();
        private FileDragService _fileDragService = new FileDragService();
        private AppDataFileIOService _fileIOService = new AppDataFileIOService();
        private OldFileService _oldFileService = new OldFileService();

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
            this.taskDrawAria.MouseWheel += TaskDrawAria_MouseWheel;
            _viewData.FilterChanged += _viewData_FilterChanged;
            _viewData.SelectedWorkItemChanged += _viewData_SelectedWorkItemChanged;
            this.panel1.Resize += Panel1_Resize;
            taskDrawAria.Size = panel1.Size;
            statusStrip1.Items.Add("");
            taskDrawAria.AllowDrop = true;
            taskDrawAria.DragEnter += TaskDrawAria_DragEnter;
            taskDrawAria.DragDrop += TaskDrawAria_DragDrop;
        }

        private void TaskDrawAria_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!IsControlDown()) return;
            if (e.Delta > 0)
            {
                ToolStripMenuItemLargeRatio_Click(sender, e);
            }
            else
            {
                ToolStripMenuItemSmallRatio_Click(sender, e);
            }
        }

        private void TaskDrawAria_DragDrop(object sender, DragEventArgs e)
        {
            var fileName = _fileDragService.Drop(e);
            if (string.IsNullOrEmpty(fileName)) return;
            var appData = _fileIOService.OpenFile(fileName);
            if (appData == null) return;
            _viewData.Original = appData;
            taskDrawAria.Invalidate();
        }

        private void TaskDrawAria_DragEnter(object sender, DragEventArgs e)
        {
            _fileDragService.DragEnter(e);
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
            UpdateHoveringText(e);
            _workItemDragService.UpdateDraggingItem(_grid, e.Location, _viewData.Original.Callender);
            taskDrawAria.Invalidate();
        }

        private bool IsControlDown()
        {
            return (Control.ModifierKeys & Keys.Control) == Keys.Control;
        }

        private void UpdateHoveringText(MouseEventArgs e)
        {
            if (_workItemDragService.IsDragging()) return;
            if (_grid == null) return;
            var wi = _grid.PickFromPoint(e.Location, _viewData);
            statusStrip1.Items[0].Text = wi == null ? string.Empty : wi.ToString(_viewData.Original.Callender);
        }

        private void TaskDrawAria_MouseUp(object sender, MouseEventArgs e)
        {
            _workItemDragService.End();
        }

        private void TaskDrawAria_MouseDown(object sender, MouseEventArgs e)
        {
            var wi = _grid.PickFromPoint(e.Location, _viewData);
            if (wi == null) return;
            _viewData.Selected = wi;

            _workItemDragService.Start(wi, e.Location, _grid);

            taskDrawAria.Invalidate();
        }

        private void TaskDrawAria_Paint(object sender, PaintEventArgs e)
        {
            _grid = new TaskGrid(_viewData, e.Graphics, this.taskDrawAria.Bounds, this.Font);
            _grid.Draw(_viewData);
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var grid = new TaskGrid(_viewData, e.Graphics, e.PageBounds, this.Font);
            grid.Draw(_viewData);
        }

        private void ToolStripMenuItemImportOldFile_Click(object sender, EventArgs e)
        {
            _oldFileService.ImportMemberAndWorkItems(_viewData);
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
            _fileIOService.Save(_viewData.Original);
        }

        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            var appData = _fileIOService.Open();
            if (appData == null) return;
            _viewData.Original = appData;
            taskDrawAria.Invalidate();
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
            _viewData.IncFont();
            taskDrawAria.Invalidate();
        }

        private void フォント小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewData.DecFont();
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
            _viewData.DecRatio();
            ApplyViewRatio();
        }

        private void ApplyViewRatio()
        {
            panel1.AutoScroll = _viewData.IsEnlarged();
            taskDrawAria.Size = new Size((int)(panel1.Size.Width * _viewData.Ratio), (int)(panel1.Size.Height * _viewData.Ratio));
            taskDrawAria.Invalidate();
        }

        private void ToolStripMenuItemLargeRatio_Click(object sender, EventArgs e)
        {
            _viewData.IncRatio();
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
            _fileIOService.SaveOtherName(_viewData.Original);
        }
    }
}
