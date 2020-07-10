using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectsTM.ViewModel;
using FreeGridControl;

namespace ProjectsTM.UI
{
    public partial class TaskListGrid : FreeGridControl.GridControl
    {
        public TaskListGrid()
        {
            InitializeComponent();
            this.OnDrawNormalArea += TaskListGrid_OnDrawNormalArea;
            this.Resize += TaskListGrid_Resize;
        }

        private void TaskListGrid_Resize(object sender, EventArgs e)
        {
            UpdateLastColWidth();
        }

        private void UpdateLastColWidth()
        {
            if (ColWidths.Count == 0) return;
            if (ColWidths.Count < ColCount) return;
            ColWidths[ColCount - 1] = GetLastColWidth();
        }

        internal void Initialize(ViewData viewData)
        {
            LockUpdate = true;
            var font = this.Font;
            var g = this.CreateGraphics();
            var colWidth = (int)Math.Ceiling(g.MeasureString("2000A12A31", font).Width);
            var memberHeight = (int)Math.Ceiling(g.MeasureString("NAME", font).Height);
            var height = (int)(memberHeight);
            foreach (var c in ColIndex.Range(0, ColCount - 1))
            {
                ColWidths[c.Value] = colWidth;
            }
            UpdateLastColWidth();
            foreach (var r in RowIndex.Range(0, RowCount))
            {
                RowHeights[r.Value] = height;
            }
            LockUpdate = false;
        }

        private float GetLastColWidth()
        {
            if (ColCount == 0) return 0;
            if (ColCount == 1) return Width;
            var rest = 0f;
            for (var idx = 0; idx < ColCount - 1; idx++)
            {
                rest += ColWidths[idx];
            }
            return Math.Max(Width - rest - 17/*スクロールバーの幅*/ - 2, 200);
        }

        private void TaskListGrid_OnDrawNormalArea(object sender, FreeGridControl.DrawNormalAreaEventArgs e)
        {
            var range = this.VisibleRowColRange;
            foreach (var c in range.Cols)
            {
                DrawTitleRow(e, c);
                foreach (var r in range.Rows)
                {
                    var res = GetRect(c, r, 1, false, false, true);
                    if (!res.HasValue) continue;
                    e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(res.Value));
                }
            }
        }

        private void DrawTitleRow(DrawNormalAreaEventArgs e, ColIndex c)
        {
            var res = GetRect(c, new RowIndex(0), 1, true, false, true);
            if (!res.HasValue) return;
            e.Graphics.FillRectangle(Brushes.Gray, res.Value);
            e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(res.Value));
            e.Graphics.DrawString(GetTitle(c), this.Font, Brushes.Black, res.Value.Location);
        }

        private static string GetTitle(ColIndex c)
        {
            string[] titles = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
            return titles[c.Value];
        }
    }
}
