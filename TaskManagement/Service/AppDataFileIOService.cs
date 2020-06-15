using System;
using System.IO;
using System.Windows.Forms;
using TaskManagement.Logic;
using TaskManagement.Model;

namespace TaskManagement.Service
{
    class AppDataFileIOService : IDisposable
    {
        public event EventHandler FileChanged;
        public event EventHandler FileSaved;
        private DateTime _last;

        public AppDataFileIOService()
        {
            _watcher = new FileSystemWatcher();
            _watcher.Changed += _watcher_Changed;
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!IsEnoughTerm()) return;
            FileChanged?.Invoke(sender, e);
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

        internal bool Save(AppData appData)
        {
            if (string.IsNullOrEmpty(_previousFileName))
            {
                return SaveOtherName(appData);
            }
            if (!CheckOverwrap(appData)) return false;
            _watcher.EnableRaisingEvents = false;
            try
            {
                AppDataSerializer.Serialize(_previousFileName, appData);
                FileSaved?.Invoke(this, null);
            }
            finally
            {
                _watcher.EnableRaisingEvents = true;
            }
            return true;
        }

        internal bool SaveOtherName(AppData appData)
        {
            if (!CheckOverwrap(appData)) return false;
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return false;
                _watcher.EnableRaisingEvents = false;
                try
                {
                    AppDataSerializer.Serialize(dlg.FileName, appData);
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

        private static bool CheckOverwrap(AppData appData)
        {
            if (OverwrapedWorkItemsGetter.Get(appData.WorkItems).Count == 0) return true;
            if (MessageBox.Show("範囲が重複している項目があります。保存を継続しますか？", "要確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return false;
            return true;
        }

        internal AppData Open()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "日程表ﾃﾞｰﾀ (*.xml)|*.xml|All files (*.*)|*.*";
                if (dlg.ShowDialog() != DialogResult.OK) return null;
                _previousFileName = dlg.FileName;
                return OpenFile(dlg.FileName);
            }
        }

        internal AppData ReOpen()
        {
            return OpenFile(_previousFileName);
        }

        public AppData OpenFile(string fileName)
        {
            _previousFileName = fileName;
            _watcher.Path = Path.GetDirectoryName(fileName);
            _watcher.Filter = Path.GetFileName(fileName);
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
            return AppDataSerializer.Deserialize(fileName);
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}
