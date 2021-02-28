using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.ViewModel;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.Main
{
    public partial class MainForm : Form
    {
        private readonly MainViewData _viewData = new MainViewData(new AppData());
        private readonly AppDataFileIOService _fileIOService = new AppDataFileIOService();
        private readonly FilterComboBoxService _filterComboBoxService;
        private readonly TaskListManager _taskListManager;
        private readonly PatternHistory _patternHistory = new PatternHistory();
        private readonly EditorFindService _editorFindService;
        private readonly RemoteChangePollingService _remoteChangePollingService;
        private readonly FileWatchManager _fileWatchManager;
#pragma warning disable CA2213 // 破棄可能なフィールドは破棄しなければなりません
        private readonly WorkItemGrid _workItemGrid;
#pragma warning restore CA2213 // 破棄可能なフィールドは破棄しなければなりません

        public MainForm()
        {
            InitializeComponent();
            _remoteChangePollingService = new RemoteChangePollingService(_fileIOService);
            _editorFindService = new EditorFindService(_fileIOService);
            _workItemGrid = new WorkItemGrid(_viewData, _editorFindService, _fileIOService);
            this.Controls.Add(_workItemGrid);
            this.Controls.Add(new MainFormStatusStrip(_viewData, _remoteChangePollingService));
            _filterComboBoxService = new FilterComboBoxService(_viewData.Core, toolStripComboBoxFilter);
            _taskListManager = new TaskListManager(_viewData.Core, _patternHistory, this);
            _fileWatchManager = new FileWatchManager(this, Reload);
            _viewData.FilterChanged += (s, e) => UpdateView();
            _viewData.AppDataChanged += (s, e) => UpdateView();
            _viewData.UndoBuffer.Changed += _undoService_Changed;
            _fileIOService.FileWatchChanged += (s, e) => _fileWatchManager.ConfirmReload();
            _fileIOService.FileOpened += FileIOService_FileOpened;
            _remoteChangePollingService.FoundRemoteChange += _remoteChangePollingService_FoundRemoteChange;
            this.FormClosed += MainForm_FormClosed;
            this.FormClosing += MainForm_FormClosing;
            this.Shown += (s, e) => { _workItemGrid.MoveToMeToday(); SuggestSetting(); SuggestSolveErorr(); };
            this.Load += MainForm_Load;
        }

        private void SuggestSetting()
        {
            if (_viewData.Detail.HideSuggestionForUserNameSetting) return;
            if (_viewData.Detail.Me != Member.Invalid) return;
            using (var dlg = new ManageMySettingForm(_viewData.Original.Members, _viewData.Detail.Me, _viewData.Detail.HideSuggestionForUserNameSetting))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                _viewData.Detail.Me = dlg.Selected;
                _viewData.Detail.HideSuggestionForUserNameSetting = dlg.HideSetting;
            }
        }

        private void SuggestSolveErorr()
        {
            if (TaskErrorCheckService.IsUserErrorExist(_viewData))
            {
                var me = _viewData.Detail.Me;
                if (!(MessageBox.Show($"{me}さんエラーになっている項目があります。確認しますか？", "要確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)) return;
                _taskListManager.ShowUsersError(me);
            }
        }

        private void UpdateView()
        {
            _viewData.Selected = new WorkItems();
            _taskListManager.UpdateView();
            _workItemGrid.UpdateGridFrame();
            _filterComboBoxService.UpdateAppDataPart();
        }

        private void _remoteChangePollingService_FoundRemoteChange(object sender, bool isRemoteBranchAppDataNew)
        {
            this.Text = "ProjectsTM";
            if (isRemoteBranchAppDataNew)
            {
                this.Text += "     ***リモートブランチのデータに更新があります***";
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var setting = UserSettingUIService.Load();
            _viewData.FontSize = setting.FontSize;
            _viewData.Detail = setting.Detail;
            _patternHistory.CopyFrom(setting.PatternHistory);
            if (_fileIOService.TryOpenFile(setting.FilePath, out var appData))
            {
                SetAppData(appData);
            }
            else
            {
                SetAppData(AppData.Dummy);
            }
            _filterComboBoxService.Text = setting.FilterName;
            MainFormStateManager.Load(this);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var setting = new UserSetting
            {
                FilterName = _filterComboBoxService.Text,
                FontSize = _viewData.FontSize,
                FilePath = _fileIOService.FilePath,
                Detail = _viewData.Detail,
                PatternHistory = _patternHistory,
            };
            UserSettingUIService.Save(setting);
            MainFormStateManager.Save(this);
        }

        private void FileIOService_FileOpened(object sender, string filePath)
        {
            _filterComboBoxService.UpdateFilePart(filePath);
            _patternHistory.Load(FilePathService.GetPatternHistoryPath(filePath));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_fileIOService.IsDirty) return;
            if (MessageBox.Show("保存されていない変更があります。上書き保存しますか？", "保存", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            if (!_fileIOService.Save(_viewData.Original, _taskListManager.ShowOverlapCheck)) e.Cancel = true;
        }

        private void _undoService_Changed(object sender, IEditedEventArgs e)
        {
            _fileIOService.SetDirty();
        }

        private void ToolStripMenuItemExportRS_Click(object sender, EventArgs e)
        {
            RsExportManager.Export(_viewData.Original);
        }

        private void ToolStripMenuItemOutputImage_Click(object sender, EventArgs e)
        {
            ImageOutputer.Save(_viewData, _workItemGrid);
        }

        private void ToolStripMenuItemAddWorkItem_Click(object sender, EventArgs e)
        {
            _workItemGrid.AddNewWorkItem(null);
        }

        private void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            _fileIOService.Save(_viewData.Original, _taskListManager.ShowOverlapCheck);
        }

        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "日程表ﾃﾞｰﾀ (*.xml)|*.xml|All files (*.*)|*.*";
                if (dlg.ShowDialog() != DialogResult.OK) return;
                if (!_fileIOService.TryOpen(dlg.FileName, out var appData)) return;
                SetAppData(appData);
            }
        }

        private void ToolStripMenuItemFilter_Click(object sender, EventArgs e)
        {
            using (var dlg = new FilterForm(_viewData.Core, _patternHistory))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                _viewData.SetFilter(dlg.GetFilter());
            }
        }

        private void ToolStripMenuItemColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorManagementForm(_viewData.Original.ColorConditions.Clone()))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                _viewData.SetColorConditions(dlg.GetColorConditions());
            }
        }

        private void ToolStripMenuItemSmallRatio_Click(object sender, EventArgs e)
        {
            _viewData.DecRatio();
        }

        private void ToolStripMenuItemLargeRatio_Click(object sender, EventArgs e)
        {
            _viewData.IncRatio();
        }

        private void ToolStripMenuItemManageMember_Click(object sender, EventArgs e)
        {
            using (var dlg = new ManageMemberForm(_viewData.Original))
            {
                dlg.ShowDialog(this);
                UpdateView();
            }
        }

        private void ToolStripMenuItemSaveAsOtherName_Click(object sender, EventArgs e)
        {
            _fileIOService.SaveOtherName(_viewData.Original, _taskListManager.ShowOverlapCheck);
        }

        private void ToolStripMenuItemUndo_Click(object sender, EventArgs e)
        {
            _workItemGrid.Undo();
        }

        private void ToolStripMenuItemRedo_Click(object sender, EventArgs e)
        {
            _workItemGrid.Redo();
        }

        private void ToolStripMenuItemMileStone_Click(object sender, EventArgs e)
        {
            using (var dlg = new ManageMileStoneForm(_viewData.Original.MileStones.Clone(), _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.Original.MileStones = dlg.MileStones;
            }
            _workItemGrid.Refresh();
        }

        private void ToolStripMenuItemDivide_Click(object sender, EventArgs e)
        {
            _workItemGrid.Divide();
        }

        private void ToolStripMenuItemGenerateDummyData_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                DummyDataService.Save(dlg.FileName);
            }
        }

        private void ToolStripMenuItemReload_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void Reload()
        {
            if (!_fileIOService.TryReOpen(out var appData)) return;
            SetAppData(appData);
        }

        private void SetAppData(AppData appData)
        {
            _viewData.SetAppData(appData);
        }

        private void ToolStripMenuItemHowToUse_Click(object sender, EventArgs e)
        {
            LaunchHelpService.Show();
        }

        private void ToolStripMenuItemVersion_Click(object sender, EventArgs e)
        {
            using (var dlg = new VersionForm())
            {
                dlg.ShowDialog(this);
            }
        }

        private void ToolStripMenuItemTaskList_Click(object sender, EventArgs e)
        {
            _taskListManager.Show(_viewData.Detail.Me);
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolStripMenuItemMySetting_Click(object sender, EventArgs e)
        {
            using (var dlg = new ManageMySettingForm(_viewData.Original.Members, _viewData.Detail.Me, _viewData.Detail.HideSuggestionForUserNameSetting))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                _viewData.Detail.Me = dlg.Selected;
                _viewData.Detail.HideSuggestionForUserNameSetting = dlg.HideSetting;
            }
            _taskListManager.UpdateMySetting(_viewData.Detail.Me);
        }

        private void ToolStripMenuItemTrendChart_Click(object sender, EventArgs e)
        {
            ShowTrendChartForm();
        }

        private void ShowTrendChartForm()
        {
            using (var dlg = new TrendChart(_viewData.Original, _fileIOService.FilePath))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
