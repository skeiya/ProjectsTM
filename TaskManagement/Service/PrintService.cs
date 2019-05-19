using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using TaskManagement.UI;
using TaskManagement.ViewModel;

namespace TaskManagement.Service
{
    class PrintService
    {
        private PrintDocument _printDocument = new PrintDocument();
        private PrintPreviewDialog _printPreviewDialog1 = new PrintPreviewDialog();
        private Font _font;
        private ViewData _viewData;

        internal PrintService(Font font, ViewData viewData)
        {
            _font = font;
            _viewData = viewData;
            foreach (PaperSize s in _printDocument.DefaultPageSettings.PrinterSettings.PaperSizes)
            {
                if (s.Kind == PaperKind.A3)
                {
                    _printDocument.DefaultPageSettings.PaperSize = s;
                }
            }
            _printDocument.DefaultPageSettings.Landscape = true;
            _printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var grid = new TaskGrid(_viewData, e.PageBounds, _font, true);
            grid.OnResize(e.PageBounds.Size, null, true);
            grid.DrawPrint(e.Graphics, _viewData);
        }

        internal void Print()
        {
            _printPreviewDialog1.Document = _printDocument;
            using (var dlg = new PrintDialog())
            {
                dlg.Document = _printPreviewDialog1.Document;
                if (dlg.ShowDialog() != DialogResult.OK) return;
            }
            if (_printPreviewDialog1.ShowDialog() != DialogResult.OK) return;
        }
    }
}
