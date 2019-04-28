using System;
using System.Collections.Generic;
using System.Drawing;
using TaskManagement.Service;

namespace TaskManagement
{
    public class TaskGrid
    {
        private CommonGrid _grid;
        private Dictionary<int, Member> _colToMember = new Dictionary<int, Member>();
        private Dictionary<Member, int> _memberToCol = new Dictionary<Member, int>();
        private Dictionary<CallenderDay, int> _dayToRow = new Dictionary<CallenderDay, int>();
        private Dictionary<int, CallenderDay> _rowToDay = new Dictionary<int, CallenderDay>();
        private ColorConditions _colorConditions;

        public TaskGrid(ViewData viewData, Graphics g, Rectangle pageBounds, Font font)
        {
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            _grid = new CommonGrid(g, viewData.CreateFont(font));

            UpdateRowColMap(viewData);

            _grid.RowCount = viewData.GetDaysCount() + Members.RowCount;
            SetRowHeight(pageBounds);

            _grid.ColCount = viewData.GetVisibleMembers().Count + Callender.ColCount;
            SetColWidth(g, pageBounds);

            _colorConditions = viewData.Original.ColorConditions;
        }

        private void SetColWidth(Graphics g, Rectangle pageBounds)
        {
            var year = _grid.MeasureString("0000/").Width;
            var month = _grid.MeasureString("00/").Width;
            var day = _grid.MeasureString("00").Width;
            _grid.SetColWidth(0, year);
            _grid.SetColWidth(1, month);
            _grid.SetColWidth(2, day);
            var member = ((float)(pageBounds.Width) - year - month - day) / (_grid.ColCount - Callender.ColCount);
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                _grid.SetColWidth(c, member);
            }

        }

        private void SetRowHeight(Rectangle pageBounds)
        {
            var company = _grid.MeasureString("K").Height;
            var name = _grid.MeasureString("下村HF").Height * 1.5f;
            _grid.SetRowHeight(0, company);
            _grid.SetRowHeight(1, name);
            var height = ((float)pageBounds.Height - name) / (_grid.RowCount - Members.RowCount);
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                _grid.SetRowHeight(r, height);
            }

        }

        private void UpdateRowColMap(ViewData viewData)
        {
            int c = Callender.ColCount;
            foreach (var m in viewData.GetVisibleMembers())
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

        public void Draw(ViewData viewData)
        {
            DrawCallenderDays();
            DrawTeamMembers();
            DrawWorkItems(viewData, null);
        }

        private void DrawCallenderDaysOutOfTaskArea(Graphics g, Point panelLocation, float offsetFromHiddenHight)
        {
            int y = 0;
            int m = 0;
            var dayWidth = _grid.GetCellBounds(0, 2).Width;
            var monthWidth = _grid.GetCellBounds(0, 1).Width;
            var yearWidth = _grid.GetCellBounds(0, 0).Width;
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var year = _rowToDay[r].Year;
                if (y == year) continue;
                y = year;
                var rect = _grid.GetCellBounds(r, 0);
                rect.Height = rect.Height * 2;//TODO: 適当に広げている
                g.DrawString(year.ToString() + "/", _grid.Font, Brushes.Black, panelLocation.X - (dayWidth + monthWidth + yearWidth), panelLocation.Y + rect.Y - offsetFromHiddenHight);
            }
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var month = _rowToDay[r].Month;
                if (m == month) continue;
                m = month;
                var rect = _grid.GetCellBounds(r, 1);
                rect.Height = rect.Height * 2;//TODO: 適当に広げている
                g.DrawString(month.ToString() + "/", _grid.Font, Brushes.Black, panelLocation.X - (dayWidth + monthWidth), panelLocation.Y + rect.Y - offsetFromHiddenHight);
            }
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var rect = _grid.GetCellBounds(r, 2);
                g.DrawString(_rowToDay[r].Day.ToString(), _grid.Font, Brushes.Black, panelLocation.X - dayWidth, panelLocation.Y + rect.Y - offsetFromHiddenHight);
            }
        }

        private void DrawCallenderDays()
        {
            int y = 0;
            int m = 0;
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var year = _rowToDay[r].Year;
                if (y == year) continue;
                y = year;
                var rect = _grid.GetCellBounds(r, 0);
                rect.Height = rect.Height * 2;//TODO: 適当に広げている
                _grid.DrawString(year.ToString() + "/", rect);
            }
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var month = _rowToDay[r].Month;
                if (m == month) continue;
                m = month;
                var rect = _grid.GetCellBounds(r, 1);
                rect.Height = rect.Height * 2;//TODO: 適当に広げている
                _grid.DrawString(month.ToString() + "/", rect);
            }
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                var rect = _grid.GetCellBounds(r, 2);
                _grid.DrawString(_rowToDay[r].Day.ToString(), rect);
            }
        }
        
        private void DrawTeamMembers()
        {
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                var rect = _grid.GetCellBounds(0, c);
                _grid.DrawString(_colToMember[c].Company, rect);
            }
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                var rect = _grid.GetCellBounds(1, c);
                _grid.DrawString(_colToMember[c].DisplayName, rect);
            }
        }

        private void DrawTeamMembersOutOfTaskArea(Graphics g, Point panelLocation, float offsetFromHiddenWidth)
        {
            var companyHight = _grid.GetCellBounds(0, 0).Height;
            var nameHeight = _grid.GetCellBounds(1, 0).Height;
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                var rect = _grid.GetCellBounds(0, c);
                g.DrawString(_colToMember[c].Company, _grid.Font, Brushes.Black, rect.X + panelLocation.X - offsetFromHiddenWidth, panelLocation.Y - (companyHight + nameHeight));
            }
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                var rect = _grid.GetCellBounds(1, c);
                g.DrawString(_colToMember[c].DisplayName, _grid.Font, Brushes.Black, rect.X + panelLocation.X - offsetFromHiddenWidth, panelLocation.Y - nameHeight);
            }
        }

        internal void DrawAlwaysFrame(ViewData viewData, Graphics g, Point panelLocation, Point offsetFromHiddenLocation, WorkItem draggingItem)
        {
            DrawCallenderDaysOutOfTaskArea(g, panelLocation, offsetFromHiddenLocation.Y);
            DrawTeamMembersOutOfTaskArea(g, panelLocation, offsetFromHiddenLocation.X);
            DrawWorkItems(viewData, draggingItem);
        }

        private void DrawWorkItems(ViewData viewData, WorkItem draggingItem)
        {
            foreach (var wi in viewData.GetFilteredWorkItems())
            {
                DrawWorkItem(viewData, wi);
            }
            if (draggingItem != null) DrawWorkItem(viewData, draggingItem);

            if (viewData.Selected != null)
            {
                var bounds = GetWorkItemVisibleBounds(viewData.Selected, viewData.Filter);
                _grid.Graphics.DrawRectangle(Pens.LightGreen, Rectangle.Round(bounds));
                DrawTopDragBar(bounds);
                DrawBottomDragBar(bounds);
            }
        }

        private void DrawWorkItem(ViewData viewData, WorkItem wi)
        {
            var bounds = GetWorkItemVisibleBounds(wi, viewData.Filter);
            var colorContidion = _colorConditions.GetMatchColorCondition(wi.ToString(viewData.Original.Callender));
            if (colorContidion != null) _grid.Graphics.FillRectangle(new SolidBrush(colorContidion.BackColor), Rectangle.Round(bounds));
            var front = colorContidion == null ? Color.Black : colorContidion.ForeColor;
            _grid.DrawString(wi.ToDrawString(viewData.Original.Callender), bounds, front);
            _grid.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(bounds));
        }

        private RectangleF GetWorkItemVisibleBounds(WorkItem w, Filter filter)
        {
            var period = GetVisiblePeriod(filter, w);
            var col = _memberToCol[w.AssignedMember];
            var rowTop = _dayToRow[period.From];
            var rowBottom = _dayToRow[period.To];
            var top = _grid.GetCellBounds(rowTop, col);
            var bottom = _grid.GetCellBounds(rowBottom, col);
            return new RectangleF(top.Location, new SizeF(top.Width, bottom.Y - top.Y + top.Height));
        }

        private void DrawBottomDragBar(RectangleF bounds)
        {
            var rect = WorkItemDragService.GetBottomBarRect(bounds, _grid.RowHeight(2));// TODO (2)はやめる
            var points = WorkItemDragService.GetBottomBarLine(bounds, _grid.RowHeight(2));
            _grid.Graphics.FillRectangle(Brushes.DarkBlue, rect);
            _grid.Graphics.DrawLine(Pens.White, points.Item1, points.Item2);
        }

        private void DrawTopDragBar(RectangleF bounds)
        {
            var rect = WorkItemDragService.GetTopBarRect(bounds, _grid.RowHeight(2));
            var points = WorkItemDragService.GetTopBarLine(bounds, _grid.RowHeight(2));
            _grid.Graphics.FillRectangle(Brushes.DarkBlue, rect);
            _grid.Graphics.DrawLine(Pens.White, points.Item1, points.Item2);
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