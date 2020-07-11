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
using ProjectsTM.Model;

namespace ProjectsTM.UI
{
    public partial class TaskListGrid : FreeGridControl.GridControl
    {
        private List<WorkItem> _listItems;
        private ViewData _viewData;

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
            _viewData = viewData;
            UpdateListItem();
            ColCount = 9;
            FixedRowCount = 1;
            RowCount = _listItems.Count() + FixedRowCount;

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

        private void UpdateListItem()
        {
            _listItems = _viewData.GetFilteredWorkItems().OrderBy(w => w.Period.To).ToList();
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
            var g = e.Graphics;
            var range = this.VisibleRowColRange;
            DrawTitleRow(g);
            foreach (var r in RowIndex.Range(VisibleNormalTopRow.Value, VisibleNormalRowCount))
            {
                DrawItemRow(g, r);
            }
        }

        private void DrawItemRow(Graphics g, RowIndex r)
        {
            var item = _listItems[r.Value - FixedRowCount];
            foreach (var c in ColIndex.Range(VisibleNormalLeftCol.Value, VisibleNormalColCount))
            {
                var res = GetRect(c, r, 1, false, false, true);
                if (!res.HasValue) continue;
                g.DrawRectangle(Pens.Black, Rectangle.Round(res.Value));
                var text = GetText(item, c);
                g.DrawString(text, this.Font, Brushes.Black, res.Value.Location);
            }
        }

        private string GetText(WorkItem item, ColIndex c)
        {
            var colIndex = c.Value;
            if (colIndex == 0)
            {
                return item.Name;
            }
            else if (colIndex == 1)
            {
                return item.Project.ToString();
            }
            else if (colIndex == 2)
            {
                return item.AssignedMember.ToString();
            }
            else if (colIndex == 3)
            {
                return item.Tags.ToString();
            }
            else if (colIndex == 4)
            {
                return item.State.ToString();
            }
            else if (colIndex == 5)
            {
                return item.Period.From.ToString();
            }
            else if (colIndex == 6)
            {
                return item.Period.To.ToString();
            }
            else if (colIndex == 7)
            {
                return _viewData.Original.Callender.GetPeriodDayCount(item.Period).ToString();
            }
            else if (colIndex == 8)
            {
                return item.Description;
            }
            return string.Empty;
        }

        private void DrawTitleRow(Graphics g)
        {
            foreach (var c in ColIndex.Range(VisibleNormalLeftCol.Value, VisibleNormalColCount))
            {
                var res = GetRect(c, new RowIndex(0), 1, true, false, true);
                if (!res.HasValue) return;
                g.FillRectangle(Brushes.Gray, res.Value);
                g.DrawRectangle(Pens.Black, Rectangle.Round(res.Value));
                g.DrawString(GetTitle(c), this.Font, Brushes.Black, res.Value.Location);
            }
        }

        private static string GetTitle(ColIndex c)
        {
            string[] titles = new string[] { "名前", "プロジェクト", "担当", "タグ", "状態", "開始", "終了", "人日", "備考" };
            return titles[c.Value];
        }
    }
}
