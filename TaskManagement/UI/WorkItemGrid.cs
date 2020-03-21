using FreeGridControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Model;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public class WorkItemGrid : FreeGridControl.GridControl
    {
        private ViewData _viewData;

        public WorkItemGrid() { }

        internal void Initialize(ViewData viewData)
        {
            LockUpdate = true;
            this._viewData = viewData;
            var fixedRows = 2;
            var fixedCols = 3;
            this.Rows = _viewData.GetFilteredDays().Count + fixedRows;
            this.Cols = _viewData.GetFilteredMembers().Count + fixedCols;
            this.FixedRows = fixedRows;
            this.FixedCols = fixedCols;

            this.OnDrawCell += WorkItemGrid_OnDrawCell;
            this.OnDrawNormalArea += WorkItemGrid_OnDrawNormalArea;
            LockUpdate = false;
        }

        private void WorkItemGrid_OnDrawCell(object sender, FreeGridControl.DrawCellEventArgs e)
        {
            var memberIndex = e.ColIndex - this.FixedCols;
            if (0 <= memberIndex)
            {
                var member = _viewData.GetFilteredMembers().ElementAt(memberIndex);
                if (e.RowIndex == 0 && this.FixedCols <= e.ColIndex)
                {
                    e.Graphics.DrawString(member.Company, this.Font, Brushes.Black, e.Rect);
                }
                if (e.RowIndex == 1 && this.FixedCols <= e.ColIndex)
                {
                    e.Graphics.DrawString(member.DisplayName, this.Font, Brushes.Black, e.Rect);
                }
            }

            var dayIndex = e.RowIndex - this.FixedRows;
            if (0 <= dayIndex)
            {
                var day = _viewData.GetFilteredDays().ElementAt(dayIndex);
                if (this.FixedRows <= e.RowIndex && e.ColIndex == 0)
                {
                    e.Graphics.DrawString(day.Year.ToString(), this.Font, Brushes.Black, e.Rect);
                }
                if (this.FixedRows <= e.RowIndex && e.ColIndex == 1)
                {
                    e.Graphics.DrawString(day.Month.ToString(), this.Font, Brushes.Black, e.Rect);
                }
                if (this.FixedRows <= e.RowIndex && e.ColIndex == 2)
                {
                    e.Graphics.DrawString(day.Day.ToString(), this.Font, Brushes.Black, e.Rect);
                }
            }
        }

        private void WorkItemGrid_OnDrawNormalArea(object sender, DrawNormalAreaEventArgs e)
        {
            if (!e.VisibleRowColRect.HasValue) return;
            var visibleRowColRect = e.VisibleRowColRect.Value;
            foreach (var c in Enumerable.Range(visibleRowColRect.X, visibleRowColRect.Width))
            {
                var m = _viewData.GetFilteredMembers().ElementAt(c);
                foreach (var wi in GetVisibleWorkItems(m, visibleRowColRect.Y, visibleRowColRect.Height))
                {
                    var colorCondition = _viewData.Original.ColorConditions.GetMatchColorCondition(wi.ToString());
                    var rect = e.GetRect(c, GetRowRange(wi, visibleRowColRect));
                    if(colorCondition != null) e.Graphics.FillRectangle(new SolidBrush(colorCondition.BackColor), Rectangle.Round(rect));
                    var front = colorCondition == null ? Color.Black : colorCondition.ForeColor;
                    e.Graphics.DrawString(wi.ToDrawString(_viewData.Original.Callender), this.Font, BrushCache.GetBrush(front), rect);
                    e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(rect));
                }
            }
        }

        private Tuple<int, int> GetRowRange(WorkItem wi, Rectangle visibleRowColRect)
        {
            var filteredDays = _viewData.GetFilteredDays();
            var visibleTopDay = filteredDays.ElementAt(visibleRowColRect.Top);
            var visibleButtomDay = filteredDays.ElementAt(visibleRowColRect.Top + visibleRowColRect.Height - 1);
            var visiblePeriod = new Period(visibleTopDay, visibleButtomDay);
            if (!visiblePeriod.HasInterSection(wi.Period)) return null;
            if (wi.Period.Contains(visibleTopDay) && wi.Period.Contains(visibleButtomDay)) return new Tuple<int, int>(visibleRowColRect.Top, visibleRowColRect.Height);
            if (wi.Period.Contains(visibleTopDay) && !wi.Period.Contains(visibleButtomDay)) return new Tuple<int, int>(visibleRowColRect.Top, GetRow(wi.Period.To, visibleRowColRect) - visibleRowColRect.Top + 1);
            if (!wi.Period.Contains(visibleTopDay) && !wi.Period.Contains(visibleButtomDay)) return new Tuple<int, int>(GetRow(wi.Period.From, visibleRowColRect), GetRow(wi.Period.To, visibleRowColRect) - GetRow(wi.Period.From, visibleRowColRect) + 1);
            return new Tuple<int, int>(GetRow(wi.Period.From, visibleRowColRect), visibleRowColRect.Bottom - GetRow(wi.Period.From, visibleRowColRect));
        }

        private int GetRow(CallenderDay day, Rectangle visibleRowColRect)
        {
            foreach (var r in Enumerable.Range(visibleRowColRect.Y, visibleRowColRect.Height))
            {
                if (_viewData.GetFilteredDays().ElementAt(r).Equals(day)) return r;
            }
            Debug.Assert(false);
            return 0;
        }

        private IEnumerable<Member> GetVisibleMembers(int left, int count)
        {
            for (var index = left; index < left + count; index++)
            {
                yield return _viewData.GetFilteredMembers().ElementAt(index);
            }
        }

        private IEnumerable<WorkItem> GetVisibleWorkItems(Member m, int top, int count)
        {
            var topDay = _viewData.GetFilteredDays().ElementAt(top);
            var buttomDay = _viewData.GetFilteredDays().ElementAt(top + count - 1);
            foreach (var wi in _viewData.GetFilteredWorkItemsOfMember(m))
            {
                if (!wi.Period.HasInterSection(new Period(topDay, buttomDay))) continue;
                yield return wi;
            }
        }
    }
}
