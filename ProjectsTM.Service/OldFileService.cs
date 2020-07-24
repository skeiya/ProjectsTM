using ProjectsTM.Logic;
using ProjectsTM.ViewModel;
using System;
using System.Windows.Forms;

namespace ProjectsTM.Service
{
    public static class OldFileService
    {

        public static void ImportMemberAndWorkItems(ViewData viewData)
        {
            try
            {
                using (var dlg = new OpenFileDialog())
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    viewData.Original.Members = CsvReadService.ReadMembers(dlg.FileName);
                    viewData.Original.WorkItems = CsvReadService.ReadWorkItems(dlg.FileName);
                    viewData.ClearCallenderAndMembers();
                    foreach (var w in viewData.Original.WorkItems) // TODO 暫定
                    {
                        viewData.UpdateCallenderAndMembers(w);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
