using ProjectsTM.Service;
using ProjectsTM.ViewModel;
using System.Windows.Forms;

namespace ProjectsTM.UI.Main
{
    public partial class MainFormStatusStrip : StatusStrip
    {
#pragma warning disable CA2213 // 破棄可能なフィールドは破棄しなければなりません
        private readonly ToolStripItem toolStripStatusLabelSum = new ToolStripStatusLabel();
        private readonly ToolStripItem toolStripStatusLabelViewRatio = new ToolStripStatusLabel();
        private readonly ToolStripItem toolStripStatusHasUnpushedCommit = new ToolStripStatusLabel();
        private readonly ToolStripItem toolStripStatusHasUncommittedChange = new ToolStripStatusLabel();
#pragma warning restore CA2213 // 破棄可能なフィールドは破棄しなければなりません
        private readonly MainViewData _viewData;
        private readonly RemoteChangePollingService _remoteChangePollingService;
        private readonly CalculateSumService _calculateSumService = new CalculateSumService();

        public MainFormStatusStrip(MainViewData viewData, RemoteChangePollingService remoteChangePollingService)
        {
            InitializeComponent();
            this.Dock = DockStyle.Bottom;
            this.Items.AddRange(new ToolStripItem[]
            {
            this.toolStripStatusLabelSum,
            this.toolStripStatusLabelViewRatio,
            this.toolStripStatusHasUnpushedCommit,
            this.toolStripStatusHasUncommittedChange,
            });
            this._viewData = viewData;
            this._viewData.RatioChanged += (s, e) => { UpdateRatio(); };

            this._remoteChangePollingService = remoteChangePollingService;
            this._remoteChangePollingService.CheckedUnpushedChange += (s, e) => { toolStripStatusHasUnpushedCommit.Text = (_remoteChangePollingService?.HasUnpushedCommit ?? false) ? " ***未プッシュのコミットがあります***" : string.Empty; };
            this._remoteChangePollingService.FoundRemoteChange += (s, e) => { toolStripStatusHasUncommittedChange.Text = (_remoteChangePollingService?.HasUncommitedChange ?? false) ? " ***コミットされていない変更があります***" : string.Empty; };

            this._viewData.UndoBuffer.Changed += (s, e) => { UpdateDisplayOfSum(e); };
            this._viewData.AppDataChanged += (s, e) =>
            { 
                UpdateDisplayOfSum(new EditedEventArgs(_viewData.Original.Members));
                UpdateRatio();
            };
        }

        private void UpdateRatio()
        {
            toolStripStatusLabelViewRatio.Text = "拡大率:" + _viewData.Detail.ViewRatio.ToString();
        }

        private void UpdateDisplayOfSum(IEditedEventArgs e)
        {
            var sum = _calculateSumService.Calculate(_viewData.Core, e.UpdatedMembers);
            toolStripStatusLabelSum.Text = string.Format("SUM:{0}人日({1:0.0}人月)", sum, sum / 20f);
        }
    }
}
