using System.Windows.Forms;

namespace TaskManagement.Service
{
    class AppDataFileIOService
    {
        private string _previousFileName;

        internal void Save(AppData appData)
        {
            appData.WorkItems.Sort();
            if (string.IsNullOrEmpty(_previousFileName))
            {
                SaveOtherName(appData);
                return;
            }
            AppDataSerializer.Serialize(_previousFileName, appData);
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
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                AppDataSerializer.Serialize(dlg.FileName, appData);
                _previousFileName = dlg.FileName;
            }
        }

        public AppData OpenFile(string fileName)
        {
            return AppDataSerializer.Deserialize(fileName);
        }
    }
}
