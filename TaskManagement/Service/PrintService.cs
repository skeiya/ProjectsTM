using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using TaskManagement.UI;
using TaskManagement.ViewModel;

namespace TaskManagement.Service
{
    class PrintService : IDisposable
    {
        private PrintDocument _printDocument = new PrintDocument();
        private Font _font;
        private ViewData _viewData;

        internal PrintService(ViewData viewData, Font font)
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

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            using (var grid = new WorkItemGrid())
            {
                grid.Size = e.PageBounds.Size;
                grid.Initialize(_viewData);
                grid.AdjustForPrint(e.PageBounds);
                grid.Print(e.Graphics);
            }
        }

        internal void Print()
        {
            using (var printPreviewDialog1 = new PrintPreviewDialog())
            using (var font = new Font(_font.FontFamily, _viewData.FontSize))
            {
                printPreviewDialog1.Document = _printDocument;
                using (var dlg = new PrintDialog())
                {
                    dlg.Document = printPreviewDialog1.Document;
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                }
                if (printPreviewDialog1.ShowDialog() != DialogResult.OK) return;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _printDocument.Dispose();
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~PrintService()
        // {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
