using System;
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
                if (s.Kind == System.Drawing.Printing.PaperKind.A3)
                {
                    printDocument.DefaultPageSettings.PaperSize = s;
                }
            }
            printDocument.DefaultPageSettings.Landscape = true;
            printDocument.PrintPage += PrintDocument_PrintPage;
            this.taskDrawAria.Paint += TaskDrawAria_Paint;
            this.taskDrawAria.MouseDown += TaskDrawAria_MouseDown;
            this.taskDrawAria.MouseUp += TaskDrawAria_MouseUp;
            this.taskDrawAria.MouseMove += TaskDrawAria_MouseMove;
        }

        WorkItem _draggingWorkItem = null;
        CallenderDay _draggedDay = null;
        Period _draggedPeriod = null;

        private void TaskDrawAria_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingWorkItem == null) return;
            _draggingWorkItem.AssignedMember = _grid.GetMemberFromX(e.Location.X);
            var curDay = _grid.GetDayFromY(e.Location.Y);
            var offset = _appData.Callender.GetOffset(_draggedDay, curDay);
            _draggingWorkItem.Period = _draggedPeriod.ApplyOffset(offset);
        }

        private void TaskDrawAria_MouseUp(object sender, MouseEventArgs e)
        {
            _draggingWorkItem = null;
        }

        private void TaskDrawAria_MouseDown(object sender, MouseEventArgs e)
        {
            var wi = _grid.PickFromPoint(e.Location);
            if (wi == null) return;
            label1.Text = wi.ToString();
            _draggingWorkItem = wi;
            _draggedPeriod = wi.Period.Clone();
            _draggedDay = _grid.GetDayFromY(e.Location.Y);
        }

        TaskGrid _grid;
        private void TaskDrawAria_Paint(object sender, PaintEventArgs e)
        {
            _grid = new TaskGrid(_appData, e.Graphics, this.taskDrawAria.Bounds, this.Font);
            _grid.Draw();
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
