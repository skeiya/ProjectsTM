using ProjectsTM.Model;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ProjectsTM.Service
{
    public class AppDataFileIOService : IDisposable
    {
        public event EventHandler FileWatchChanged;
        public event EventHandler<string> FileOpened;
        public event EventHandler FileSaved;
        private DateTime _last;
        private GitRepositoryService _gitRepositoryService;

        public AppDataFileIOService(GitRepositoryService gitRepositoryService)
        {
            _watcher = new FileSystemWatcher();
            _watcher.Changed += _watcher_Changed;
            _gitRepositoryService = gitRepositoryService;
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!IsEnoughTerm()) return;
            FileWatchChanged?.Invoke(sender, e);
        }

        private bool IsEnoughTerm()
        {
            if (_last == null) return true;
            var now = DateTime.Now;
            var span = now - _last;
            if (span.TotalSeconds < 3) return false;
            _last = now;
            return true;
        }

        private FileSystemWatcher _watcher;
        private string _previousFileName;
        public string FilePath => _previousFileName;

        public bool Save(AppData appData, Action showOverwrapCheck)
        {
            if (string.IsNullOrEmpty(_previousFileName))
            {
                return SaveOtherName(appData, showOverwrapCheck);
            }
            if (!CheckOverwrap(appData, showOverwrapCheck)) return false;
            _watcher.EnableRaisingEvents = false;
            try
            {
                AppDataSerializeService.Serialize(_previousFileName, appData);
                FileSaved?.Invoke(this, null);
            }
            finally
            {
                _watcher.EnableRaisingEvents = true;
            }
            return true;
        }

        public bool SaveOtherName(AppData appData, Action showOverwrapCheck)
        {
            if (!CheckOverwrap(appData, showOverwrapCheck)) return false;
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return false;
                _watcher.EnableRaisingEvents = false;
                try
                {
                    AppDataSerializeService.Serialize(dlg.FileName, appData);
                    FileSaved?.Invoke(this, null);
                    _previousFileName = dlg.FileName;
                }
                finally
                {
                    _watcher.Path = Path.GetDirectoryName(_previousFileName);
                    _watcher.Filter = Path.GetFileName(_previousFileName);
                    _watcher.EnableRaisingEvents = true;
                }
                return true;
            }
        }

        private static bool CheckOverwrap(AppData appData, Action showOverwrapCheck)
        {
            if (OverwrapedWorkItemsCollectService.Get(appData.WorkItems).Count == 0) return true;
            if (MessageBox.Show("範囲が重複している項目があります。保存を継続しますか？", "要確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes) return true;
            showOverwrapCheck();
            return false;
        }

        public AppData Open()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "日程表ﾃﾞｰﾀ (*.xml)|*.xml|All files (*.*)|*.*";
                if (dlg.ShowDialog() != DialogResult.OK) return null;
                _previousFileName = dlg.FileName;
                return OpenFile(dlg.FileName);
            }
        }

        public AppData ReOpen()
        {
            if (!File.Exists(_previousFileName)) return null;
            return OpenFile(_previousFileName);
        }

        public AppData OpenFile(string fileName)
        {
            if (VersionUpdateService.UpdateByFileServer(Path.GetDirectoryName(fileName))) return null;
            if (IsFutureVersion(fileName))
            {
                MessageBox.Show("ご使用のツールより新しいバージョンで保存されたファイルです。ツールを更新してから開いてください。");
                return null;
            }
            _previousFileName = fileName;
            _watcher.Path = Path.GetDirectoryName(fileName);
            _watcher.Filter = Path.GetFileName(fileName);
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
            FileOpened?.Invoke(this, fileName);
            CheckRemoteBranchAppData(fileName);
            return AppDataSerializeService.Deserialize(fileName);
        }

        private void CheckRemoteBranchAppData(string filePath)
        {
            _gitRepositoryService.IsRemoteBranchAppDataNew = false;
            _gitRepositoryService.CheckRemoteBranchAppDataFile(filePath);
        }

        private bool IsFutureVersion(string fileName)
        {
            XmlDocument oDom = new XmlDocument();
            oDom.Load(fileName);
            var node = oDom.SelectSingleNode("//AppData/Version");
            if (node == null) return false;
            string str = node.InnerText;
            if (!Int32.TryParse(str, out var version)) return false;
            return AppData.DataVersion < version;
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}
