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
            var g = e.Graphics;
            FullDraw(g);
            taskDrawAria.Invalidate();
        }

        private void FullDraw(Graphics g)
        {
            DrawCallenderDays(g);
            DrawTeamMembers(g);
        }

        private void DrawTeamMembers(Graphics g)
        {
            float x = g.RenderingOrigin.X;
            float y = g.RenderingOrigin.Y;
            float offset = GetColWidth(g);
            foreach (var member in _appData.Members)
            {
                x += offset;
                g.DrawString(member.ToString(), this.Font, Brushes.Blue, x, y);
            }
        }

        private void DrawCallenderDays(Graphics g)
        {
            float x = g.RenderingOrigin.X;
            float y = g.RenderingOrigin.Y;
            float offset = GetRowHeight(g);
            foreach (var day in _appData.Callender.Days)
            {
                y += offset;
                g.DrawString(day.ToString(), this.Font, Brushes.Green, x, y);
            }
        }

        float GetRowHeight(Graphics g)
        {
            return g.VisibleClipBounds.Height / (_appData.Callender.Days.Count + 1);
        }

        float GetColWidth(Graphics g)
        {
            return g.VisibleClipBounds.Width / (_appData.Members.Count + 1);
        }

        //SizeF GetCallenderCellSize(Graphics g)
        //{
        //    return g.MeasureString(_appData.Callender.Days[0].ToString(), this.Font);
        //}

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
