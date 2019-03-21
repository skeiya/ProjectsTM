using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class Form1 : Form
    {
        private AppData _appData;

        public Form1()
        {
            InitializeComponent();
            _appData = new AppData();

            foreach (System.Drawing.Printing.PaperSize s in printDocument.DefaultPageSettings.PrinterSettings.PaperSizes)
            {
                if (s.PaperName.Equals("A3"))
                {
                    printDocument.DefaultPageSettings.PaperSize = s;
                }
            }
            printDocument.PrintPage += PrintDocument_PrintPage;
            this.taskDrawAria.Paint += TaskDrawAria_Paint;
        }

        private void TaskDrawAria_Paint(object sender, PaintEventArgs e)
        {
            var grid = new TaskGrid(_appData, e.Graphics, this.taskDrawAria.Bounds, this.Font);
            grid.Draw();
            taskDrawAria.Invalidate();
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var grid = new TaskGrid(_appData, e.Graphics, e.PageBounds, this.Font);
            grid.Draw();
        }

        private void buttonPrintPreview_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument;
            if (printPreviewDialog1.ShowDialog() != DialogResult.OK) return;
        }
    }
}
