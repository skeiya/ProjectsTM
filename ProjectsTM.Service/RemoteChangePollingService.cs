using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectsTM.Service
{
    public class RemoteChangePollingService
    {
        public event EventHandler<bool> FoundRemoteChange;
        public event EventHandler CheckedUnpushedCommit;
        private readonly Timer _timer = new Timer();
        private readonly AppDataFileIOService _fileIOService;
        public bool HasUnpushedCommit { get; private set; } = false;

        public RemoteChangePollingService(AppDataFileIOService fileIOService)
        {
            _fileIOService = fileIOService;
            _fileIOService.FileOpened += _fileIOService_FileOpened;
            if (GitRepositoryService.IsActive())
            {
                _timer.Interval = 60 * 1000;
                _timer.Tick += _timer_Tick;
                _timer.Start();
            }
        }

        private async void _fileIOService_FileOpened(object sender, string e)
        {
            await TriggerRemoteChangeCheck(_fileIOService.FilePath).ConfigureAwait(true);
        }

        private async void _timer_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_fileIOService.FilePath)) return;
            await TriggerRemoteChangeCheck(_fileIOService.FilePath).ConfigureAwait(true);
            await TriggerUnpushedCommitCheck(_fileIOService.FilePath).ConfigureAwait(true);
        }

        private async Task TriggerRemoteChangeCheck(string filePath)
        {
            if (_fileIOService.IsDirty) return;
            var hasUnmergedRemoteCommit = await GitRepositoryService.HasUnmergedRemoteCommit(filePath).ConfigureAwait(true);
            if (hasUnmergedRemoteCommit)
            {
                if (GitRepositoryService.TryAutoPull(filePath)) hasUnmergedRemoteCommit = false;
            }
            FoundRemoteChange?.Invoke(this, hasUnmergedRemoteCommit);
        }

        private async Task TriggerUnpushedCommitCheck(string filePath)
        {
            HasUnpushedCommit = await GitRepositoryService.HasUnpushedCommit(filePath).ConfigureAwait(true);
            CheckedUnpushedCommit?.Invoke(this, null);
        }
    }
}
