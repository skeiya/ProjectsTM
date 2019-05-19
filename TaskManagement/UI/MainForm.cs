using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using TaskManagement.Logic;
using TaskManagement.Model;
using TaskManagement.Service;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public partial class MainForm : Form
    {
        private ViewData _viewData = new ViewData(new AppData());
        private SearchWorkitemForm _searchForm;
        private TaskGrid _grid;
        private FileDragService _fileDragService = new FileDragService();
        private AppDataFileIOService _fileIOService = new AppDataFileIOService();
        private OldFileService _oldFileService = new OldFileService();
        private PrintService _printService;
        private WorkItemDragService _workItemDragService = new WorkItemDragService();
        private UndoService _undoService = new UndoService();
        private WorkItemEditService _editService;
        private Cursor _originalCursor;

        public MainForm()
        {
            InitializeComponent();
            menuStrip1.ImageScalingSize = new Size(16, 16);
            _printService = new PrintService(this.Font, _viewData);
            _editService = new WorkItemEditService(_viewData, _undoService);
            _undoService.Changed += _undoService_Changed;
            panelTaskGrid.Resize += Panel1_Resize;
            panelTaskGrid.Scroll += Panel1_Scroll;
            panelTaskGrid.AutoScroll = true;// _viewData.IsEnlarged();
            panelFullView.Paint += PanelTaskGrid_Paint;
            statusStrip1.Items.Add("");
            InitializeTaskDrawArea();
            InitializeFilterCombobox();
            InitializeViewData();
            this.FormClosed += MainForm_FormClosed;
            LoadUserSetting();
            UpdateGrid();
            UpdatePanelLayout(_viewData.Detail);
            toolStripStatusLabelViewRatio.Text = "拡大率:" + _viewData.Detail.ViewRatio.ToString();
        }

        private void UpdateGrid()
        {
            _grid = new TaskGrid(_viewData, this.taskDrawArea.Bounds, panelFullView.Font, false);
            _grid.OnResize(taskDrawArea.Size, _viewData.Detail, false);
            taskDrawArea.Size = _grid.Size;
            _grid.UpdateFont(_viewData.FontSize);
        }

        private void PanelTaskGrid_Paint(object sender, PaintEventArgs e)
        {
            _grid.DrawTaskAreaOnPain(e.Graphics, _viewData, panelTaskGrid.Location, new Point(-taskDrawArea.Bounds.X, -taskDrawArea.Bounds.Y), _workItemDragService.CopyingItem, e.ClipRectangle);
        }

        private void LoadUserSetting()
        {
            try
            {
                var setting = UserSettingUIService.Load(UserSettingPath);
                toolStripComboBoxFilter.Text = setting.FilterName;
                _viewData.FontSize = setting.FontSize;
                _viewData.Detail = setting.Detail;
                var appData = _fileIOService.OpenFile(setting.FilePath);
                if (appData == null) return;
                _viewData.Original = appData;
                _viewData.Selected = null;
                taskDrawArea.Invalidate();
            }
            catch
            {

            }
        }

        private string UserSettingPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TaskManagementTool", "UserSetting.xml");

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var setting = new UserSetting
            {
                FilterName = toolStripComboBoxFilter.Text,
                FontSize = _viewData.FontSize,
                FilePath = _fileIOService.FilePath,
                Detail = _viewData.Detail
            };
            UserSettingUIService.Save(UserSettingPath, setting);
        }

        private void InitializeViewData()
        {
            _viewData.FilterChanged += _viewData_FilterChanged;
            _viewData.SelectedWorkItemChanged += _viewData_SelectedWorkItemChanged;
            _viewData.FontChanged += _viewData_FontChanged;
            _viewData.AppDataChanged += _viewData_AppDataChanged;
        }

        private void _viewData_FontChanged(object sender, EventArgs e)
        {
            _grid.UpdateFont(_viewData.FontSize);
            taskDrawArea.Invalidate();
            panelFullView.Invalidate();
        }

        private void UpdatePanelLayout(Detail detail)
        {
            var width = detail.DateWidth;
            var hight = detail.CompanyHeight + detail.NameHeight;
            panelTaskGrid.Location = new Point(width, hight);
            panelTaskGrid.Size = new Size(panelFullView.Width - width, panelFullView.Height - hight);
        }

        private void _undoService_Changed(object sender, EventArgs e)
        {
            UpdateDisplayOfSum();
            taskDrawArea.Invalidate();
        }

        private void _viewData_AppDataChanged(object sender, EventArgs e)
        {
            UpdateDisplayOfSum();
        }

        private void UpdateDisplayOfSum()
        {
            var sum = 0;
            foreach (var w in _viewData.GetFilteredWorkItems())
            {
                sum += _viewData.Original.Callender.GetPeriodDayCount(w.Period);
            }
            toolStripStatusLabelSum.Text = string.Format("SUM:{0}人日({1:0.0}人月)", sum, sum / 20f);
        }

        private void Panel1_Scroll(object sender, ScrollEventArgs e)
        {
            taskDrawArea.Invalidate();
            panelFullView.Invalidate();
        }

        private void InitializeFilterCombobox()
        {
            toolStripComboBoxFilter.Items.Clear();
            toolStripComboBoxFilter.Items.Add("ALL");
            try
            {
                var filters = Directory.GetFiles("./filters");
                foreach (var f in filters)
                {
                    toolStripComboBoxFilter.Items.Add(f);
                }
            }
            catch
            {
            }
            finally
            {
                toolStripComboBoxFilter.SelectedIndex = 0;
                toolStripComboBoxFilter.SelectedIndexChanged += ToolStripComboBoxFilter_SelectedIndexChanged;
            }
        }

        private void ToolStripComboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _viewData.Selected = null;
            var path = toolStripComboBoxFilter.SelectedItem.ToString();
            if (path.Equals("ALL"))
            {
                _viewData.SetFilter(null);
                return;
            }
            if (!File.Exists(path)) return;
            using (var rs = new StreamReader(path))
            {
                var x = new XmlSerializer(typeof(Filter));
                var filter = (Filter)x.Deserialize(rs);
                _viewData.SetFilter(filter);
            }
        }

        void InitializeTaskDrawArea()
        {
            //        taskDrawArea.Size = new Size(panelTaskGrid.Width - taskDrawArea.Location.X, panelTaskGrid.Height - taskDrawArea.Location.Y);
            taskDrawArea.Paint += TaskDrawArea_Paint;
            taskDrawArea.MouseDown += TaskDrawArea_MouseDown;
            taskDrawArea.MouseUp += TaskDrawArea_MouseUp;
            taskDrawArea.MouseMove += TaskDrawArea_MouseMove;
            taskDrawArea.MouseWheel += TaskDrawArea_MouseWheel;
            taskDrawArea.AllowDrop = true;
            taskDrawArea.DragEnter += TaskDrawArea_DragEnter;
            taskDrawArea.DragDrop += TaskDrawArea_DragDrop;
            taskDrawArea.ContextMenu = new ContextMenu();
            taskDrawArea.ContextMenu.Popup += ContextMenu_Popup;
            this.KeyUp += MainForm_KeyUp;
            this.KeyDown += MainForm_KeyDown;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _workItemDragService.ToCopyMode(_viewData.Original.WorkItems);
            }
            if (e.KeyCode == Keys.Escape)
            {
                _workItemDragService.End(_editService, _viewData, true);
                _viewData.Selected = null;
            }
            taskDrawArea.Invalidate();
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            taskDrawArea.ContextMenu.MenuItems.Clear();
            var editMenu = new MenuItem("編集...");
            editMenu.Click += EditMenu_Click;
            taskDrawArea.ContextMenu.MenuItems.Add(editMenu);
            var devideMenu = new MenuItem("分割...");
            devideMenu.Click += DevideMenu_Click;
            taskDrawArea.ContextMenu.MenuItems.Add(devideMenu);
        }

        private void DevideMenu_Click(object sender, EventArgs e)
        {
            Devide();
        }

        private void Devide()
        {
            try
            {
                var selected = _viewData.Selected;
                var count = _viewData.Original.Callender.GetPeriodDayCount(selected.Period);
                using (var dlg = new DevideWorkItemForm(count))
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    _editService.Devide(selected, dlg.Devided, dlg.Remain);
                }
            }
            catch
            {
                return;
            }
        }

        private void EditMenu_Click(object sender, EventArgs e)
        {
            EditSelectedWorkItem();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (_viewData.Selected == null) return;
                _editService.Delete(_viewData.Selected);
                _viewData.Selected = null;
            }
            if (e.KeyCode == Keys.ControlKey)
            {
                _workItemDragService.ToMoveMode(_viewData.Original.WorkItems);
            }
            taskDrawArea.Invalidate();
        }

        private void TaskDrawArea_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!IsControlDown())
            {
                Panel1_Scroll(null, null);
                return;
            }
            if (e.Delta > 0)
            {
                ToolStripMenuItemLargeRatio_Click(sender, e);
            }
            else
            {
                ToolStripMenuItemSmallRatio_Click(sender, e);
            }
        }

        private void TaskDrawArea_DragDrop(object sender, DragEventArgs e)
        {
            var fileName = _fileDragService.Drop(e);
            if (string.IsNullOrEmpty(fileName)) return;
            var appData = _fileIOService.OpenFile(fileName);
            if (appData == null) return;
            _viewData.Original = appData;
            _viewData.Selected = null;
            taskDrawArea.Invalidate();
        }

        private void TaskDrawArea_DragEnter(object sender, DragEventArgs e)
        {
            _fileDragService.DragEnter(e);
        }

        private void _viewData_SelectedWorkItemChanged(object sender, EventArgs e)
        {
            try
            {
                using (var c = new Control())
                {
                    if (_viewData.Selected == null) return;
                    var bounds = _grid.GetWorkItemVisibleBounds(_viewData.Selected, _viewData.Filter);
                    bounds.X += taskDrawArea.Location.X;
                    bounds.Y += taskDrawArea.Location.Y;
                    c.Bounds = Rectangle.Round(bounds);
                    panelTaskGrid.Controls.Add(c);
                    panelTaskGrid.ScrollControlIntoView(c);
                    panelTaskGrid.Controls.Remove(c);
                }
            }
            finally
            {
                taskDrawArea.Invalidate();
            }
        }

        private void Panel1_Resize(object sender, EventArgs e)
        {
            taskDrawArea.Invalidate();
            panelFullView.Invalidate();
        }

        private void _viewData_FilterChanged(object sender, EventArgs e)
        {
            taskDrawArea.Invalidate();
            UpdateDisplayOfSum();
        }

        private void TaskDrawArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (_grid == null) return;
            UpdateHoveringText(e);
            _workItemDragService.UpdateDraggingItem(_grid, e.Location, _viewData);
            if (_grid.IsWorkItemExpandArea(_viewData, e.Location))
            {
                if (this.Cursor != Cursors.SizeNS)
                {
                    _originalCursor = this.Cursor;
                    this.Cursor = Cursors.SizeNS;
                }
            }
            else
            {
                if (this.Cursor == Cursors.SizeNS)
                {
                    this.Cursor = _originalCursor;
                }
            }
            taskDrawArea.Invalidate();
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
            toolStripStatusLabelSelect.Text = wi == null ? string.Empty : wi.ToString();
        }

        private void TaskDrawArea_MouseUp(object sender, MouseEventArgs e)
        {
            _workItemDragService.End(_editService, _viewData, false);
        }

        private void TaskDrawArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (_grid.IsWorkItemExpandArea(_viewData, e.Location))
            {
                if (e.Button != MouseButtons.Left) return;
                _workItemDragService.StartExpand(_grid.GetExpandDirection(_viewData, e.Location), _viewData.Selected);
            }
            else
            {
                var wi = _grid.PickFromPoint(e.Location, _viewData);
                if (wi == null) return;
                _viewData.Selected = wi;

                _workItemDragService.StartDrag(wi, e.Location, _grid);
            }
        }

        private void TaskDrawArea_Paint(object sender, PaintEventArgs e)
        {
            _grid.DrawAlwaysFrame(e.Graphics, _viewData, panelTaskGrid.Location, new Point(-taskDrawArea.Bounds.X, -taskDrawArea.Bounds.Y), _workItemDragService.CopyingItem, e.ClipRectangle);
        }

        private void ToolStripMenuItemImportOldFile_Click(object sender, EventArgs e)
        {
            _oldFileService.ImportMemberAndWorkItems(_viewData);
            taskDrawArea.Invalidate();
        }

        private void ToolStripMenuItemExportRS_Click(object sender, EventArgs e)
        {
            RSFileExporter.Export(_viewData.Original);
        }

        private void ToolStripMenuItemPrint_Click(object sender, EventArgs e)
        {
            _viewData.Selected = null;
            _printService.Print();
        }

        private void ToolStripMenuItemAddWorkItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new EditWorkItemForm(null, _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var wi = dlg.GetWorkItem(_viewData.Original.Callender);
                _viewData.UpdateCallenderAndMembers(wi);
                _editService.Add(wi);
                _undoService.Push();
            }
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
            _viewData.Selected = null;
            UpdateGrid();
            taskDrawArea.Invalidate();
        }


        private void ToolStripMenuItemFilter_Click(object sender, EventArgs e)
        {
            using (var dlg = new FilterForm(_viewData.Original.Members, _viewData.Filter == null ? new Filter() : _viewData.Filter.Clone(), _viewData.Original.Callender))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                _viewData.SetFilter(dlg.GetFilter());
                taskDrawArea.Invalidate();
            }
        }

        private void ToolStripMenuItemColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorManagementForm(_viewData.Original.ColorConditions))
            {
                dlg.ShowDialog();
            }
            taskDrawArea.Invalidate();
        }

        private void ToolStripMenuItemLargerFont_Click(object sender, EventArgs e)
        {
            _viewData.IncFont();
        }

        private void ToolStripMenuItemSmallFont_Click(object sender, EventArgs e)
        {
            _viewData.DecFont();
        }

        private void ToolStripMenuItemSearch_Click(object sender, EventArgs e)
        {

            if (_searchForm == null || _searchForm.IsDisposed)
            {
                _searchForm = new SearchWorkitemForm(_viewData, _editService);
            }
            if (!_searchForm.Visible) _searchForm.Show(this);
        }

        private void TaskDrawArea_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditSelectedWorkItem();
        }

        private void EditSelectedWorkItem()
        {
            var wi = _viewData.Selected;
            if (wi == null) return;
            using (var dlg = new EditWorkItemForm(wi.Clone(), _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var newWi = dlg.GetWorkItem(_viewData.Original.Callender);
                _viewData.UpdateCallenderAndMembers(newWi);
                _editService.Replace(wi, newWi);
                _viewData.Selected = newWi;
            }
        }

        private void ToolStripMenuItemWorkingDas_Click(object sender, EventArgs e)
        {
            using (var dlg = new ManagementWokingDaysForm(_viewData.Original.Callender, _viewData.Original.WorkItems))
            {
                dlg.ShowDialog();
            }
            taskDrawArea.Invalidate();
        }

        private void ToolStripMenuItemSmallRatio_Click(object sender, EventArgs e)
        {
            _viewData.DecRatio();
            toolStripStatusLabelViewRatio.Text = "拡大率:" + _viewData.Detail.ViewRatio.ToString();
            UpdatePanelLayout(_viewData.Detail);
            _grid.OnResize(taskDrawArea.Size, _viewData.Detail, false);
            taskDrawArea.Size = _grid.Size;
            taskDrawArea.Invalidate();
            panelFullView.Invalidate();
        }

        private void ToolStripMenuItemLargeRatio_Click(object sender, EventArgs e)
        {
            _viewData.IncRatio();
            toolStripStatusLabelViewRatio.Text = "拡大率:" + _viewData.Detail.ViewRatio.ToString();
            UpdatePanelLayout(_viewData.Detail);
            _grid.OnResize(taskDrawArea.Size, _viewData.Detail, false);
            taskDrawArea.Size = _grid.Size;
            taskDrawArea.Invalidate();
            panelFullView.Invalidate();
        }

        private void ToolStripMenuItemManageMember_Click(object sender, EventArgs e)
        {
            using (var dlg = new ManageMemberForm(_viewData.Original))
            {
                dlg.ShowDialog(this);
            }
            taskDrawArea.Invalidate();
        }

        private void ToolStripMenuItemSaveAsOtherName_Click(object sender, EventArgs e)
        {
            _fileIOService.SaveOtherName(_viewData.Original);
        }

        private void ToolStripMenuItemUndo_Click(object sender, EventArgs e)
        {
            _undoService.Undo(_viewData);
        }

        private void ToolStripMenuItemRedo_Click(object sender, EventArgs e)
        {
            _undoService.Redo(_viewData);
        }

        private void ToolStripMenuItemMileStone_Click(object sender, EventArgs e)
        {
            using (var dlg = new ManageMileStoneForm(_viewData.Original.MileStones.Clone(), _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.Original.MileStones = dlg.MileStones;
            }
            taskDrawArea.Invalidate();
        }

        private void ToolStripMenuItemHelp_Click_1(object sender, EventArgs e)
        {
            Process.Start(@".\Help\help.html");
        }

        private void ToolStripMenuItemDevide_Click(object sender, EventArgs e)
        {
            Devide();
        }

        private void ToolStripMenuItemGenerateDummyData_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                DummyDataService.Save(dlg.FileName);
            }
        }

        private void ToolStripMenuItemDetail_Click(object sender, EventArgs e)
        {
            using (var dlg = new ViewDetailSettingForm(_viewData.Detail.Clone()))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.Detail = dlg.Detail;
                UpdatePanelLayout(_viewData.Detail);
                _grid.OnResize(taskDrawArea.Size, _viewData.Detail, false);
                panelFullView.Invalidate();
            }
        }
    }
}
