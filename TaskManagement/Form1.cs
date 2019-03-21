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
            printDocument.PrintPage += PrintDocument_PrintPage;
            this.taskDrawAria.Paint += TaskDrawAria_Paint;
        }

        private void TaskDrawAria_Paint(object sender, PaintEventArgs e)
        {
            var grid = new TaskGrid(_appData, e.Graphics, this.Font);
            grid.Draw();
            taskDrawAria.Invalidate();
        }

        float GetRowHeight(Graphics g)
        {
            return g.VisibleClipBounds.Height / (_appData.Callender.Days.Count + 1);
        }

        float GetColWidth(Graphics g)
        {
            return g.VisibleClipBounds.Width / (_appData.Members.Count + 1);
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var grid = new TaskGrid(_appData, e.Graphics, this.Font);
            grid.Draw();
        }

        private void buttonPrintPreview_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument;
            if (printPreviewDialog1.ShowDialog() != DialogResult.OK) return;
        }
    }
}
