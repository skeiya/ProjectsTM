using ProjectsTM.Service;
using ProjectsTM.ViewModel;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectsTM.UI.Main
{
    public static class ImageOutputer
    {
        public static void Save(MainViewData viewData, WorkItemGrid orgGrid)
        {
            var selected = viewData.Selected;
            viewData.Selected = null;
            try
            {
                using (var fileIOService = new AppDataFileIOService())
                using (var grid = new WorkItemGrid(viewData, null, fileIOService))
                {
                    var size = new Size(orgGrid.GridWidth, orgGrid.GridHeight);
                    grid.Size = size;
                    using (var bmp = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        var g = Graphics.FromImage(bmp);
                        grid.OutputImage(g);
                        using (var dlg = new SaveFileDialog())
                        {
                            dlg.Filter = "Image files (*.png)|*.png|All files (*.*)|*.*";
                            if (dlg.ShowDialog() != DialogResult.OK) return;
                            bmp.Save(dlg.FileName);
                        }
                    }
                }
            }
            finally
            {
                viewData.Selected = selected;
            }
        }
    }
}
