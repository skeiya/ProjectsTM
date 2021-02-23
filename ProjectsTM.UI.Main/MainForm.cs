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
        private readonly CalculateSumService _calculateSumService = new CalculateSumService();
        private readonly FilterComboBoxService _filterComboBoxService;
        private readonly TaskListManager _taskListManager;
        private readonly PatternHistory _patternHistory = new PatternHistory();
        private readonly EditorFindService _lastUpdateDateAndUserNameService = new EditorFindService();
        private Member _me = null;
        private bool _hideSuggestionForUserNameSetting = false;
        private readonly RemoteChangePollingService _remoteChangePollingService;
        private readonly FileWatchManager _fileWatchManager;

        public MainForm()
        {
            InitializeComponent();
            _filterComboBoxService = new FilterComboBoxService(_viewData.Core, toolStripComboBoxFilter);
            _taskListManager = new TaskListManager(_viewData.Core, _patternHistory, this);
            _fileWatchManager = new FileWatchManager(this, Reload);
            _viewData.FilterChanged += (s, e) => UpdateView();
            _viewData.AppDataChanged += (s, e) => UpdateView();
            _viewData.UndoBuffer.Changed += _undoService_Changed;
            _fileIOService.FileWatchChanged += (s, e) => _fileWatchManager.ConfirmReload();
            _fileIOService.FileOpened += FileIOService_FileOpened;
            _fileIOService.FileSaved += FileIOService_FileSaved;
            _remoteChangePollingService = new RemoteChangePollingService(_fileIOService);
            _remoteChangePollingService.FoundRemoteChange += _remoteChangePollingService_FoundRemoteChange;
            _remoteChangePollingService.CheckedUnpushedChange += _remoteChangePollingService_CheckedUnpushedChange;
            workItemGrid1.DragDrop += TaskDrawArea_DragDrop;
            workItemGrid1.RatioChanged += (s, e) => UpdateView();
            this.FormClosed += MainForm_FormClosed;
            this.FormClosing += MainForm_FormClosing;
            this.Shown += (s, e) => { workItemGrid1.MoveToTodayAndMember(_me); SuggestSetting(); };
            this.Load += MainForm_Load;
        }

        private void FileIOService_FileSaved(object sender, string filePath)
        {
            _lastUpdateDateAndUserNameService.Load(filePath);
        }

        private void SuggestSetting()
        {
            if (_hideSuggestionForUserNameSetting) return;
            if (_me != null) return;
            using (var dlg = new ManageMySettingForm(_viewData.Original.Members, _me, _hideSuggestionForUserNameSetting))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                _me = dlg.Selected;
                _hideSuggestionForUserNameSetting = dlg.HideSetting;
            }
        }

        private void _remoteChangePollingService_CheckedUnpushedChange(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _viewData.Selected = new WorkItems();
            _taskListManager.UpdateView();
            workItemGrid1.Initialize(_viewData, _lastUpdateDateAndUserNameService);
            _filterComboBoxService.UpdateAppDataPart();
            UpdateDisplayOfSum(new EditedEventArgs(_viewData.Original.Members));
            toolStripStatusLabelViewRatio.Text = "拡大率:" + _viewData.Detail.ViewRatio.ToString();
            toolStripStatusHasUnpushedCommit.Text = (_remoteChangePollingService?.HasUnpushedCommit ?? false) ? " ***未プッシュのコミットがあります***" : string.Empty;
            toolStripStatusHasUncommittedChange.Text = (_remoteChangePollingService?.HasUncommitedChange ?? false) ? " ***コミットされていない変更があります***" : string.Empty;
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
            MainFormStateManager.Load(this);
            try
            {
                var setting = UserSettingUIService.Load();
                _viewData.FontSize = setting.FontSize;
                _viewData.Detail = setting.Detail;
                _patternHistory.CopyFrom(setting.PatternHistory);
                SetAppData(string.IsNullOrEmpty(setting.FilePath) ? AppData.Dummy : _fileIOService.OpenFile(setting.FilePath));
                _filterComboBoxService.Text = setting.FilterName;
                _me = Member.Parse(setting.UserName);
                _hideSuggestionForUserNameSetting = setting.HideSuggestionForUserNameSetting;
            }
            catch
            {
            }
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
                UserName = _me == null ? string.Empty : _me.ToSerializeString(),
                HideSuggestionForUserNameSetting = _hideSuggestionForUserNameSetting,
            };
            UserSettingUIService.Save(setting);
            MainFormStateManager.Save(this);
        }

        private void FileIOService_FileOpened(object sender, string filePath)
        {
            _filterComboBoxService.UpdateFilePart(filePath);
            _patternHistory.Load(FilePathService.GetPatternHistoryPath(filePath));
            _lastUpdateDateAndUserNameService.Load(filePath);
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
            UpdateDisplayOfSum(e);
        }

        private void UpdateDisplayOfSum(IEditedEventArgs e)
        {
            var sum = _calculateSumService.Calculate(_viewData.Core, e.UpdatedMembers);
            toolStripStatusLabelSum.Text = string.Format("SUM:{0}人日({1:0.0}人月)", sum, sum / 20f);
        }

        private void TaskDrawArea_DragDrop(object sender, DragEventArgs e)
        {
            var fileName = FileDragService.Drop(e);
            SetAppData(_fileIOService.OpenFile(fileName));
        }

        private void ToolStripMenuItemExportRS_Click(object sender, EventArgs e)
        {
            RsExportManager.Export(_viewData.Original);
        }

        private void ToolStripMenuItemOutputImage_Click(object sender, EventArgs e)
        {
            ImageOutputer.Save(_viewData, workItemGrid1);
        }

        private void ToolStripMenuItemAddWorkItem_Click(object sender, EventArgs e)
        {
            workItemGrid1.AddNewWorkItem(null);
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
                SetAppData(_fileIOService.Open(dlg.FileName));
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
            workItemGrid1.DecRatio();
        }

        private void ToolStripMenuItemLargeRatio_Click(object sender, EventArgs e)
        {
            workItemGrid1.IncRatio();
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
            workItemGrid1.Undo();
        }

        private void ToolStripMenuItemRedo_Click(object sender, EventArgs e)
        {
            workItemGrid1.Redo();
        }

        private void ToolStripMenuItemMileStone_Click(object sender, EventArgs e)
        {
            using (var dlg = new ManageMileStoneForm(_viewData.Original.MileStones.Clone(), _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.Original.MileStones = dlg.MileStones;
            }
            workItemGrid1.Refresh();
        }

        private void ToolStripMenuItemDivide_Click(object sender, EventArgs e)
        {
            workItemGrid1.Divide();
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
            SetAppData(_fileIOService.ReOpen());
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
            _taskListManager.Show();
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolStripMenuItemMySetting_Click(object sender, EventArgs e)
        {
            using (var dlg = new ManageMySettingForm(_viewData.Original.Members, _me, _hideSuggestionForUserNameSetting))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                _me = dlg.Selected;
                _hideSuggestionForUserNameSetting = dlg.HideSetting;
            }
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                // Dispose stuff here
                _fileIOService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
