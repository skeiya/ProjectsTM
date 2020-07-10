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
        }

        internal void Initialize(ViewData viewData)
        {
            LockUpdate = true;
            var font = this.Font;
            var g = this.CreateGraphics();
            var colWidth = (int)Math.Ceiling(g.MeasureString("2000A12A31", font).Width);
            var memberHeight = (int)Math.Ceiling(g.MeasureString("NAME", font).Height);
            var height = (int)(memberHeight);
            foreach (var c in ColIndex.Range(0, ColCount))
            {
                ColWidths[c.Value] = colWidth;
            }
            foreach (var r in RowIndex.Range(0, RowCount))
            {
                RowHeights[r.Value] = height;
            }
            LockUpdate = false;
        }

        private void TaskListGrid_OnDrawNormalArea(object sender, FreeGridControl.DrawNormalAreaEventArgs e)
        {
            var range = this.VisibleRowColRange;
            foreach (var c in range.Cols)
            {
                foreach (var r in range.Rows)
                {
                    var res = GetRect(c, r, 1, false, false, true);
                    if (!res.HasValue) continue;
                    e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(res.Value));
                }
            }
        }
    }
}
