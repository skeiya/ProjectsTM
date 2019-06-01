using System;
using System.Collections.Generic;
using System.Drawing;
using TaskManagement.Model;
using TaskManagement.Service;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public class TaskGrid
    {
        private CommonGrid _grid;
        private Dictionary<int, Member> _colToMember = new Dictionary<int, Member>();
        private Dictionary<Member, int> _memberToCol = new Dictionary<Member, int>();
        private Dictionary<CallenderDay, int> _dayToRow = new Dictionary<CallenderDay, int>();
        private Dictionary<int, CallenderDay> _rowToDay = new Dictionary<int, CallenderDay>();
        private ColorConditions _colorConditions;
        private CellBoundsCache _cellBoundsCache = new CellBoundsCache();

        public Size Size => _grid.Size;

        public TaskGrid(ViewData viewData, Rectangle pageBounds, Font font, bool isPrint)
        {
            _grid = new CommonGrid(font);
            UpdateRowColMap(viewData);
            _colorConditions = viewData.Original.ColorConditions;
        }

        public void OnResize(Size s, Detail detail, bool isPrint)
        {
            SetRowHeights(s, detail, isPrint);
            SetColWidths(s, detail, isPrint);
            CreateCellBoundsCache();
        }

        private void CreateCellBoundsCache()
        {
            var top = 0f;
            for (var r = 0; r < _grid.RowCount; r++)
            {
                var left = 0f;
                var height = _grid.RowHeight(r);
                for (var c = 0; c < _grid.ColCount; c++)
                {
                    var width = _grid.ColWidth(c);
                    _cellBoundsCache.Set(r, c, new RectangleF(left, top, width, height));
                    left += width;
                }
                top += height;
            }
        }

        private void SetRowHeights(Size s, Detail detail, bool isPrint)
        {
            var company = isPrint ? 20 : 0;
            var name = isPrint ? 20 : 0;
            _grid.SetRowHeight(0, company);
            _grid.SetRowHeight(1, name);
            var height = isPrint ? ((float)s.Height - name) / (_grid.RowCount - Members.RowCount) : detail.RowHeight;
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                _grid.SetRowHeight(r, height);
            }

        }

        private void SetColWidths(Size s, Detail detail, bool isPrint)
        {
            var year = isPrint ? 40 : 0;
            var month = isPrint ? 20 : 0;
            var day = isPrint ? 20 : 0;
            _grid.SetColWidth(0, year);
            _grid.SetColWidth(1, month);
            _grid.SetColWidth(2, day);
            var width = isPrint ? ((float)(s.Width) - year - month - day) / (_grid.ColCount - Callender.ColCount) : detail.ColWidth;
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                _grid.SetColWidth(c, width);
            }

        }

        public void UpdateRowColMap(ViewData viewData)
        {
            ClearRowColMap();
            _grid.RowCount = viewData.GetDaysCount() + Members.RowCount;
            _grid.ColCount = viewData.GetFilteredMembers().Count + Callender.ColCount;
            int c = Callender.ColCount;
            foreach (var m in viewData.GetFilteredMembers())
            {
                _colToMember.Add(c, m);
                _memberToCol.Add(m, c);
                c++;
            }

            int r = Members.RowCount;
            foreach (var d in viewData.GetFilteredDays())
            {
                _dayToRow.Add(d, r);
                _rowToDay.Add(r, d);
                r++;
            }
        }

        private void ClearRowColMap()
        {
            _colToMember.Clear();
            _memberToCol.Clear();
            _dayToRow.Clear();
            _rowToDay.Clear();
        }

        internal void UpdateFont(int size)
        {
            if (_grid.Font != null)
            {
                _grid.Font.Dispose();
            }
            _grid.Font = new Font(_grid.Font.FontFamily, size);
        }

        public WorkItem PickFromPoint(PointF point, ViewData viewData)
        {
            var member = GetMemberFromX(point.X);
            var day = GetDayFromY(point.Y);
            if (member == null || day == null) return null;
            foreach (var wi in viewData.Original.WorkItems)
            {
                if (!wi.AssignedMember.Equals(member)) continue;
                if (!wi.Period.Contains(day)) continue;
                return wi;
            }
            return null;
        }

        public Member GetMemberFromX(float x)
        {
            float left = 0;
            for (int c = 0; c < _grid.ColCount; c++)
            {
                var w = _grid.ColWidth(c);
                if (left <= x && x < left + w)
                {
                    Member m;
                    if (_colToMember.TryGetValue(c, out m)) return m;
                    return null;
                }
                left += w;
            }
            return null;
        }

        public CallenderDay GetDayFromY(float y)
        {
            float top = 0;
            for (int r = 0; r < _grid.RowCount; r++)
            {
                var h = _grid.RowHeight(r);
                if (top <= y && y < top + h)
                {
                    CallenderDay d;
                    if (_rowToDay.TryGetValue(r, out d)) return d;
                    return null;
                }
                top += h;
            }
            return null;
        }

        internal int GetExpandDirection(ViewData viewData, Point location)
        {
            if (viewData.Selected == null) return 0;
            var bounds = GetWorkItemVisibleBounds(viewData.Selected, viewData.Filter);
            if (IsTopBar(bounds, location)) return +1;
            if (IsBottomBar(bounds, location)) return -1;
            return 0;
        }

        internal bool IsTopBar(RectangleF workItemBounds, PointF point)
        {
            var topBar = WorkItemDragService.GetTopBarRect(workItemBounds, _grid.RowHeight(2));
            return topBar.Contains(point);
        }

        internal bool IsBottomBar(RectangleF workItemBounds, PointF point)
        {
            var bottomBar = WorkItemDragService.GetBottomBarRect(workItemBounds, _grid.RowHeight(2));
            return bottomBar.Contains(point);
        }

        internal bool IsWorkItemExpandArea(ViewData viewData, Point location)
        {
            if (viewData.Selected == null) return false;
            var bounds = GetWorkItemVisibleBounds(viewData.Selected, viewData.Filter);
            return IsTopBar(bounds, location) || IsBottomBar(bounds, location);
        }

        public void DrawPrint(Graphics g, ViewData viewData)
        {
            DrawCallenderDays(g);
            DrawTeamMembers(g);
            DrawWorkItems(g, viewData, null, null, false);
            DrawMileStones(g, viewData.Original.MileStones);
        }

        private void DrawMileStonesTaskArea(Graphics g, Detail detail, Point panelLocation, float offsetFromHiddenHight, MileStones mileStones)
        {
            if (mileStones.IsEmpty()) return;
            var dayWidth = GetDayWidth(detail);
            var monthWidth = GetMonthWidth(detail);
            var yearWidth = GetYearWidth(detail);
            var height = _grid.RowHeight(2);

            foreach (var m in mileStones)
            {
                var r = _dayToRow[m.Day];
                var bounds = _cellBoundsCache.Get(r, 0);
                var bottom = bounds.Bottom;// + offsetFromHiddenHight;
                _grid.DrawMileStoneLine(g, bottom, m.Color);
            }
        }

        private void DrawMileStonesFixedArea(Graphics g, Detail detail, Point panelLocation, float offsetFromHiddenHight, MileStones mileStones)
        {
            if (mileStones.IsEmpty()) return;
            var dayWidth = GetDayWidth(detail);
            var monthWidth = GetMonthWidth(detail);
            var yearWidth = GetYearWidth(detail);
            var height = _grid.RowHeight(2);

            foreach (var m in mileStones)
            {
                var r = _dayToRow[m.Day];
                var bounds = _cellBoundsCache.Get(r, 0);
                var bottom = bounds.Bottom + offsetFromHiddenHight;
                using (var b = new SolidBrush(m.Color))
                {
                    g.DrawString(m.Name, _grid.Font, b, panelLocation.X - (dayWidth + monthWidth + yearWidth), bottom - height * 2 / 3);
                }
                _grid.DrawMileStoneLineFiexed(g, bottom, m.Color, detail.DateWidth);
            }
        }

        private void DrawMileStones(Graphics g, MileStones mileStones)
        {
            foreach (var m in mileStones)
            {
                var r = _dayToRow[m.Day];
                var rect = _cellBoundsCache.Get(r, 0);
                var bottom = rect.Bottom;
                _grid.DrawMileStoneLine(g, bottom, m.Color);
                rect.Offset(0, rect.Height / 2);
                _grid.DrawString(g, m.Name, rect, m.Color);
            }
        }

        private void DrawCallenderDaysOutOfTaskArea(Graphics g, Detail detail, Point panelLocation, float offsetFromHiddenHight)
        {
            var dayWidth = GetDayWidth(detail);
            var monthWidth = GetMonthWidth(detail);
            var yearWidth = GetYearWidth(detail);
            DrawYear(g, panelLocation, offsetFromHiddenHight, dayWidth, monthWidth, yearWidth, detail.FixedHeight);
            DrawMonth(g, panelLocation, offsetFromHiddenHight, dayWidth, monthWidth, detail.FixedHeight);
            DrawDay(g, panelLocation, offsetFromHiddenHight, dayWidth, detail.FixedHeight);
        }

        private float GetYearWidth(Detail detail)
        {
            return detail.DateWidth / 2;
        }

        private float GetMonthWidth(Detail detail)
        {
            return detail.DateWidth * 3 / 10;
        }

        private float GetDayWidth(Detail detail)
        {
            return detail.DateWidth / 5;
        }

        private void DrawDay(Graphics g, Point panelLocation, float offsetFromHiddenHight, float dayWidth, float fixedHeight)
        {
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var rect = _cellBoundsCache.Get(r, 2);
                var y = panelLocation.Y + rect.Y - offsetFromHiddenHight;
                if (y < fixedHeight) continue;
                g.DrawString(_rowToDay[r].Day.ToString(), _grid.Font, Brushes.Black, panelLocation.X - dayWidth, y);
            }
        }

        private void DrawMonth(Graphics g, Point panelLocation, float offsetFromHiddenHight, float dayWidth, float monthWidth, float fixedHeight)
        {
            int m = 0;
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var month = _rowToDay[r].Month;
                if (m == month) continue;
                var rect = _cellBoundsCache.Get(r, 1);
                rect.Height = rect.Height * 2;//TODO: 適当に広げている
                var y = panelLocation.Y + rect.Y - offsetFromHiddenHight;
                if (y < fixedHeight) continue;
                g.DrawString(month.ToString() + "/", _grid.Font, Brushes.Black, panelLocation.X - (dayWidth + monthWidth), y);
                m = month;
            }
        }

        private void DrawYear(Graphics g, Point panelLocation, float offsetFromHiddenHight, float dayWidth, float monthWidth, float yearWidth, float fixedHeight)
        {
            int y = 0;
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var year = _rowToDay[r].Year;
                if (y == year) continue;
                var rect = _cellBoundsCache.Get(r, 0);
                rect.Height = rect.Height * 2;//TODO: 適当に広げている
                var yPos = panelLocation.Y + rect.Y - offsetFromHiddenHight;
                if (yPos < fixedHeight) continue;
                g.DrawString(year.ToString() + "/", _grid.Font, Brushes.Black, panelLocation.X - (dayWidth + monthWidth + yearWidth), yPos);
                y = year;
            }
        }

        private void DrawCallenderDays(Graphics g)
        {
            int y = 0;
            int m = 0;
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var year = _rowToDay[r].Year;
                if (y == year) continue;
                y = year;
                var rect = _cellBoundsCache.Get(r, 0);
                rect.Height = rect.Height * 2;//TODO: 適当に広げている
                _grid.DrawString(g, year.ToString() + "/", rect);
            }
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var month = _rowToDay[r].Month;
                if (m == month) continue;
                m = month;
                var rect = _cellBoundsCache.Get(r, 1);
                rect.Height = rect.Height * 2;//TODO: 適当に広げている
                _grid.DrawString(g, month.ToString() + "/", rect);
            }
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var rect = _cellBoundsCache.Get(r, 2);
                _grid.DrawString(g, _rowToDay[r].Day.ToString(), rect);
            }
        }

        private void DrawTeamMembers(Graphics g)
        {
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                var rect = _cellBoundsCache.Get(0, c);
                _grid.DrawString(g, _colToMember[c].Company, rect);
            }
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                var rect = _cellBoundsCache.Get(1, c);
                _grid.DrawString(g, _colToMember[c].DisplayName, rect);
            }
        }

        private void DrawTeamMembersOutOfTaskArea(Graphics g, Detail detail, Point panelLocation, float offsetFromHiddenWidth)
        {
            var companyHight = detail.CompanyHeight;
            var nameHeight = detail.NameHeight;
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                var rect = _cellBoundsCache.Get(0, c);
                var x = rect.X + panelLocation.X - offsetFromHiddenWidth;
                if (x < detail.DateWidth) continue;
                g.DrawString(_colToMember[c].Company, _grid.Font, Brushes.Black, x, panelLocation.Y - (companyHight + nameHeight));
            }
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                var rect = _cellBoundsCache.Get(1, c);
                var x = rect.X + panelLocation.X - offsetFromHiddenWidth;
                if (x < detail.DateWidth) continue;
                g.DrawString(_colToMember[c].DisplayName, _grid.Font, Brushes.Black, x, panelLocation.Y - nameHeight);
            }
        }

        internal void DrawTaskArea(Graphics g, ViewData viewData, Point panelLocation, Point offsetFromHiddenLocation, WorkItem draggingItem, RectangleF clip, bool isDragging)
        {
            panelLocation.Offset(-3, 0);
            DrawWorkItems(g, viewData, draggingItem, clip, isDragging);
            DrawMileStonesTaskArea(g, viewData.Detail, new Point(0, 0), 0, viewData.Original.MileStones);
        }

        internal void DrawFixedArea(Graphics g, ViewData viewData, Point panelLocation, Point offsetFromHiddenLocation, WorkItem draggingItem, RectangleF clip)
        {
            DrawCallenderDaysOutOfTaskArea(g, viewData.Detail, panelLocation, offsetFromHiddenLocation.Y);
            DrawTeamMembersOutOfTaskArea(g, viewData.Detail, panelLocation, offsetFromHiddenLocation.X);
            DrawMileStonesFixedArea(g, viewData.Detail, panelLocation, viewData.Detail.FixedHeight - offsetFromHiddenLocation.Y, viewData.Original.MileStones);
        }

        private void DrawWorkItems(Graphics g, ViewData viewData, WorkItem draggingItem, RectangleF? clip, bool isDragging)
        {
            foreach (var wi in viewData.GetFilteredWorkItems())
            {
                DrawWorkItem(g, viewData, wi, clip);
            }
            if (draggingItem != null) DrawWorkItem(g, viewData, draggingItem, clip);

            if (viewData.Selected != null)
            {
                DrawWorkItem(g, viewData, viewData.Selected, clip);
                var bounds = GetWorkItemVisibleBounds(viewData.Selected, viewData.Filter);
                g.DrawRectangle(Pens.LightGreen, Rectangle.Round(bounds));
                if (!isDragging)
                {
                    DrawTopDragBar(g, bounds);
                    DrawBottomDragBar(g, bounds);
                }
            }
        }

        private void DrawWorkItem(Graphics g, ViewData viewData, WorkItem wi, RectangleF? clip)
        {
            var bounds = GetWorkItemVisibleBounds(wi, viewData.Filter);
            if (clip.HasValue && !clip.Value.IntersectsWith(bounds)) return;
            var colorContidion = _colorConditions.GetMatchColorCondition(wi.ToString());
            if (colorContidion != null) g.FillRectangle(new SolidBrush(colorContidion.BackColor), Rectangle.Round(bounds));
            var front = colorContidion == null ? Color.Black : colorContidion.ForeColor;
            if (bounds.Width > 5 && bounds.Height > 5)
            {
                _grid.DrawString(g, wi.ToDrawString(viewData.Original.Callender), bounds, front);
            }
            g.DrawRectangle(Pens.Black, Rectangle.Round(bounds));
        }

        public RectangleF GetWorkItemVisibleBounds(WorkItem w, Filter filter)
        {
            var period = GetVisiblePeriod(filter, w);
            var col = _memberToCol[w.AssignedMember];
            var rowTop = _dayToRow[period.From];
            var rowBottom = _dayToRow[period.To];
            var top = _cellBoundsCache.Get(rowTop, col);
            var bottom = _cellBoundsCache.Get(rowBottom, col);
            return new RectangleF(top.Location, new SizeF(top.Width, bottom.Y - top.Y + top.Height));
        }

        private void DrawBottomDragBar(Graphics g, RectangleF bounds)
        {
            var rect = WorkItemDragService.GetBottomBarRect(bounds, _grid.RowHeight(2));// TODO (2)はやめる
            var points = WorkItemDragService.GetBottomBarLine(bounds, _grid.RowHeight(2));
            g.FillRectangle(Brushes.DarkBlue, rect);
            g.DrawLine(Pens.White, points.Item1, points.Item2);
        }

        private void DrawTopDragBar(Graphics g, RectangleF bounds)
        {
            var rect = WorkItemDragService.GetTopBarRect(bounds, _grid.RowHeight(2));
            var points = WorkItemDragService.GetTopBarLine(bounds, _grid.RowHeight(2));
            g.FillRectangle(Brushes.DarkBlue, rect);
            g.DrawLine(Pens.White, points.Item1, points.Item2);
        }

        private static Period GetVisiblePeriod(Filter filter, WorkItem wi)
        {
            var org = wi.Period;
            if (filter == null) return org;
            if (filter.Period == null) return org;
            var from = org.From;
            var to = org.To;
            if (from.LesserThan(filter.Period.From)) from = filter.Period.From;
            if (filter.Period.To.LesserThan(to)) to = filter.Period.To;
            return new Period(from, to);
        }
    }
}