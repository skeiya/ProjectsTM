using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            printDocument.PrintPage += PrintDocument_PrintPage;
            this.taskDrawAria.Paint += TaskDrawAria_Paint;
        }

        private void TaskDrawAria_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            FullDraw(g);
            taskDrawAria.Invalidate();
        }

        private void FullDraw(Graphics g)
        {
            float x = g.RenderingOrigin.X;
            float y = g.RenderingOrigin.Y;
            foreach (var day in AppData.Callender.Days)
            {
                var size = g.MeasureString(day.ToString(), this.Font);
                g.DrawString(day.ToString(), this.Font, Brushes.Green, x, y);
                y += size.Height;
            }
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            FullDraw(e.Graphics);
        }

        private void buttonPrintPreview_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument;
            if (printPreviewDialog1.ShowDialog() != DialogResult.OK) return;
        }
    }
}
