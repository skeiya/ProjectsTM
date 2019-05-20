using System.Windows.Forms;
using TaskManagement.Logic;
using TaskManagement.Model;

namespace TaskManagement.Service
{
    class AppDataFileIOService
    {
        private string _previousFileName;
        public string FilePath => _previousFileName;

        internal void Save(AppData appData)
        {
            appData.WorkItems.Sort();
            if (string.IsNullOrEmpty(_previousFileName))
            {
                SaveOtherName(appData);
                return;
            }
            if (!CheckOverwrap(appData)) return;
            AppDataSerializer.Serialize(_previousFileName, appData);
        }

        private bool CheckOverwrap(AppData appData)
        {
            if (OverwrapedWorkItemsGetter.Get(appData.WorkItems).Count == 0) return true;
            if (MessageBox.Show("範囲が重複している項目があります。保存を継続しますか？", "要確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return false;
            return true;
        }

        internal AppData Open()
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return null;
                _previousFileName = dlg.FileName;
                return OpenFile(dlg.FileName);
            }
        }

        internal void SaveOtherName(AppData appData)
        {
            if (!CheckOverwrap(appData)) return;
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                AppDataSerializer.Serialize(dlg.FileName, appData);
                _previousFileName = dlg.FileName;
            }
        }

        public AppData OpenFile(string fileName)
        {
            _previousFileName = fileName;
            return AppDataSerializer.Deserialize(fileName);
        }
    }
}
