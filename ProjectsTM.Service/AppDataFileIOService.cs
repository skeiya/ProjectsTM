using ProjectsTM.Model;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace ProjectsTM.Service
{
    public class AppDataFileIOService : IDisposable
    {
        public event EventHandler FileWatchChanged;
        public event EventHandler<string> FileOpened;
        public event EventHandler FileSaved;
        private DateTime _last = DateTime.MinValue;
        private bool _isDirty = false;

        public AppDataFileIOService()
        {
            _watcher = new FileSystemWatcher();
            _watcher.Changed += _watcher_Changed;
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!IsEnoughTerm()) return;
            FileWatchChanged?.Invoke(sender, e);
        }

        private bool IsEnoughTerm()
        {
            var now = DateTime.Now;
            var span = now - _last;
            if (span.TotalSeconds < 3) return false;
            _last = now;
            return true;
        }

        private readonly FileSystemWatcher _watcher;
        private string _previousFileName = string.Empty;
        private bool disposedValue;

        public string FilePath => _previousFileName;

        public bool IsDirty => _isDirty;

        public bool Save(AppData appData, Action showOverlapCheck)
        {
            if (string.IsNullOrEmpty(_previousFileName))
            {
                return SaveOtherName(appData, showOverlapCheck);
            }
            if (!CheckOverlap(appData, showOverlapCheck)) return false;
            _watcher.EnableRaisingEvents = false;
            try
            {
                appData.WorkItems.SortByPeriodStartDate();
                AppDataSerializeService.Serialize(_previousFileName, appData);
                FileSaved?.Invoke(this, null);
                _isDirty = false;
            }
            finally
            {
                _watcher.EnableRaisingEvents = true;
            }
            return true;
        }

        public bool SaveOtherName(AppData appData, Action showOverlapCheck)
        {
            if (!CheckOverlap(appData, showOverlapCheck)) return false;
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return false;
                _watcher.EnableRaisingEvents = false;
                try
                {
                    appData.WorkItems.SortByPeriodStartDate();
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

        private static bool CheckOverlap(AppData appData, Action showOverlapCheck)
        {
            if (!OverlapedWorkItemsCollectService.Get(appData.WorkItems).Any()) return true;
            if (MessageBox.Show("範囲が重複している項目があります。保存を継続しますか？", "要確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes) return true;
            showOverlapCheck();
            return false;
        }

        public AppData Open(string path)
        {
            _previousFileName = path;
            return OpenFile(path);
        }

        public AppData ReOpen()
        {
            if (!File.Exists(_previousFileName)) return null;
            return OpenFile(_previousFileName);
        }

        public AppData OpenFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            if (VersionUpdateService.UpdateByFileServer(Path.GetDirectoryName(fileName))) return null;
            if (IsFutureVersion(fileName))
            {
                MessageBox.Show("ご使用のツールより新しいバージョンで保存されたファイルです。ツールを更新してから開いてください。");
                Environment.Exit(0);
                return null;
            }
            _previousFileName = fileName;
            _watcher.Path = Path.GetDirectoryName(fileName);
            _watcher.Filter = Path.GetFileName(fileName);
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
            FileOpened?.Invoke(this, fileName);
            _isDirty = false;
            return AppDataSerializeService.Deserialize(fileName);
        }

        private static bool IsFutureVersion(string fileName)
        {
            XmlDocument oDom = new XmlDocument();
            oDom.Load(fileName);
            var node = oDom.SelectSingleNode("//AppData/Version");
            if (node == null) return false;
            string str = node.InnerText;
            if (!Int32.TryParse(str, out var version)) return false;
            return AppData.DataVersion < version;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _watcher.Dispose();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~AppDataFileIOService()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void SetDirty()
        {
            _isDirty = true;
        }
    }
}
