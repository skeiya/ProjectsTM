using FreeGridControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagement.Model;
using TaskManagement.Service;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public class WorkItemGrid : FreeGridControl.GridControl
    {
        private ViewData _viewData;

        private WorkItemDragService _workItemDragService = new WorkItemDragService();
        private UndoService _undoService = new UndoService();
        private WorkItemEditService _editService;

        public WorkItemGrid() { }

        internal void Initialize(ViewData viewData)
        {
            LockUpdate = true;
            if (_viewData != null) DetatchEvents();
            this._viewData = viewData;
            AttachEvents();
            var fixedRows = 2;
            var fixedCols = 3;
            this.Rows = _viewData.GetFilteredDays().Count + fixedRows;
            this.Cols = _viewData.GetFilteredMembers().Count + fixedCols;
            this.FixedRows = fixedRows;
            this.FixedCols = fixedCols;

            _editService = new WorkItemEditService(_viewData, _undoService);

            LockUpdate = false;
        }

        private void AttachEvents()
        {
            this._viewData.SelectedWorkItemChanged += _viewData_SelectedWorkItemChanged;
            this.OnDrawCell += WorkItemGrid_OnDrawCell;
            this.OnDrawNormalArea += WorkItemGrid_OnDrawNormalArea;
            this.MouseDown += WorkItemGrid_MouseDown;
            this.MouseDoubleClick += WorkItemGrid_MouseDoubleClick;
        }

        private void DetatchEvents()
        {
            this._viewData.SelectedWorkItemChanged -= _viewData_SelectedWorkItemChanged;
            this.OnDrawCell -= WorkItemGrid_OnDrawCell;
            this.OnDrawNormalArea -= WorkItemGrid_OnDrawNormalArea;
            this.MouseDown -= WorkItemGrid_MouseDown;
            this.MouseDoubleClick -= WorkItemGrid_MouseDoubleClick;
        }

        private void WorkItemGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_viewData.Selected != null)
            {
                EditSelectedWorkItem();
                return;
            }
            var day = Y2Day(e.Location.Y);
            var member = X2Member(e.Location.X);
            var proto = new WorkItem(new Project(""), "", new Tags(new List<string>()), new Period(day, day), member);
            AddNewWorkItem(proto);
        }

        public void AddNewWorkItem(WorkItem proto)
        {
            using (var dlg = new EditWorkItemForm(proto, _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var wi = dlg.GetWorkItem(_viewData.Original.Callender);
                _viewData.UpdateCallenderAndMembers(wi);
                _editService.Add(wi);
                _undoService.Push();
            }
        }

        public void EditSelectedWorkItem()
        {
            var wi = _viewData.Selected;
            if (wi == null) return;
            using (var dlg = new EditWorkItemForm(wi.Clone(), _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var newWi = dlg.GetWorkItem(_viewData.Original.Callender);
                _viewData.UpdateCallenderAndMembers(newWi);
                _editService.Replace(wi, newWi);
                _viewData.Selected = newWi;
            }
        }

        private void _viewData_SelectedWorkItemChanged(object sender, EventArgs e)
        {
            if (_viewData.Selected != null) MoveVisibleArea(WorkItem2Rect(_viewData.Selected));
            this.Invalidate();
        }

        private void MoveVisibleArea(RectangleF bounds)
        {
            using (var c = new Control())
            {
                //@@@bounds.X += taskDrawArea.Location.X;
                //bounds.Y += taskDrawArea.Location.Y;
                //if (panelTaskGrid.ClientRectangle.IntersectsWith(Rectangle.Round(bounds))) return;
                //c.Bounds = Rectangle.Round(bounds);
                //panelTaskGrid.Controls.Add(c);
                //panelTaskGrid.ScrollControlIntoView(c);
                //panelTaskGrid.Controls.Remove(c);
            }
        }

        private void WorkItemGrid_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.ActiveControl = null;

            //@@@if (_grid.IsWorkItemExpandArea(_viewData, e.Location))
            //{
            //    if (e.Button != MouseButtons.Left) return;
            //    _workItemDragService.StartExpand(_grid.GetExpandDirection(_viewData, e.Location), _viewData.Selected);
            //    return;
            //}

            var wi = PickWorkItemFromPoint(e.Location);
            _viewData.Selected = wi;// _viewData.IsFilteredWorkItem(wi) ? null : wi;
            _workItemDragService.StartMove(_viewData.Selected, e.Location, Y2Day(e.Location.Y));
        }

        private CallenderDay Y2Day(int y)
        {
            var r = Y2Row(y);
            return Row2Day(r);
        }

        private CallenderDay Row2Day(int r)
        {
            var days = _viewData.GetFilteredDays();
            if (r - FixedRows < 0 || days.Count <= r - FixedRows) return null;
            return days.ElementAt(r - FixedRows);
        }

        private Member X2Member(int x)
        {
            var c = X2Col(x);
            return Col2Member(c);
        }

        private Member Col2Member(int c)
        {
            var members = _viewData.GetFilteredMembers();
            if (c - FixedCols < 0 || members.Count <= c - FixedCols) return null;
            return _viewData.GetFilteredMembers().ElementAt(c - FixedCols);
        }

        private WorkItem PickWorkItemFromPoint(Point location)
        {
            var m = X2Member(location.X);
            var d = Y2Day(location.Y);
            if (m == null || d == null) return null;
            return _viewData.PickFilterdWorkItem(m, d);
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
                    var rect = GetRect(c, GetRowRange(wi, visibleRowColRect));
                    if (colorCondition != null) e.Graphics.FillRectangle(new SolidBrush(colorCondition.BackColor), Rectangle.Round(rect));
                    var front = colorCondition == null ? Color.Black : colorCondition.ForeColor;
                    e.Graphics.DrawString(wi.ToDrawString(_viewData.Original.Callender), this.Font, BrushCache.GetBrush(front), rect);
                    e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(rect));
                }
            }

            DrawSelectedWorkItemBound(e);

            DrawMileStones(e.Graphics, GetMileStonesWithToday(_viewData), visibleRowColRect.Y, visibleRowColRect.Height);
        }

        private void DrawSelectedWorkItemBound(DrawNormalAreaEventArgs e)
        {
            if (_viewData.Selected != null)
            {
                var rect = WorkItem2Rect(_viewData.Selected);
                e.Graphics.DrawRectangle(Pens.LightGreen, Rectangle.Round(rect));
            }
        }

        private RectangleF WorkItem2Rect(WorkItem wi)
        {
            var col = Member2Col(wi.AssignedMember);
            var rowRange = Period2RowRange(wi.Period);
            return GetRect(col, rowRange);
        }

        private Tuple<int, int> Period2RowRange(Period period)
        {
            var fromRow = Day2Row(period.From);
            var toRow = Day2Row(period.To);
            return new Tuple<int, int>(fromRow, toRow - fromRow + 1);
        }

        private int Day2Row(CallenderDay day)
        {
            for (int r = 0; r < Rows; r++)
            {
                if (_viewData.GetFilteredDays().ElementAt(r).Equals(day)) return r;// + FixedRows;
            }
            Debug.Assert(false);
            return -1;
        }

        private int Member2Col(Member m)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (_viewData.GetFilteredMembers().ElementAt(c).Equals(m)) return c;// + FixedCols;
            }
            Debug.Assert(false);
            return -1;
        }

        private static MileStones GetMileStonesWithToday(ViewData viewData)
        {
            var result = viewData.Original.MileStones.Clone();
            var date = DateTime.Now;
            var today = new CallenderDay(date.Year, date.Month, date.Day);
            if (viewData.Original.Callender.Days.Contains(today))
            {
                result.Add(new MileStone("Today", today, Color.Red));
            }
            return result;
        }

        private void DrawMileStones(Graphics g, MileStones mileStones, int topRow, int rowCount)
        {
            foreach (var m in mileStones)
            {
                int y;
                if (!Day2Y(m.Day, out y, topRow, rowCount)) continue;
                using (var brush = new SolidBrush(m.Color))
                {
                    g.FillRectangle(brush, 0, y, Width, 5);
                }
            }
        }

        private bool Day2Y(CallenderDay day, out int y, int topRow, int rowCount)
        {
            foreach (var r in Enumerable.Range(topRow, rowCount))
            {
                if (_viewData.GetFilteredDays().ElementAt(r).Equals(day))
                {
                    return RowToY(r, out y);
                }
            }
            y = 0;
            return false;
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
