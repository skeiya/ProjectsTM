using ProjectsTM.Model;
using ProjectsTM.Service;
using System.Windows.Forms;

namespace ProjectsTM.UI.Main
{
    class RsExportManager
    {
        internal static void Export(AppData appData)
        {
            using (var dlg = new RsExportSelectForm())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                if (dlg.allPeriod)
                {
                    RSFileExportService.Export(appData);
                }
                else
                {
                    RSFileExportService.ExportSelectGetsudo(appData, dlg.selectGetsudo);
                }
            }
        }
    }
}
