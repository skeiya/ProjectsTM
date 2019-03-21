using System;
using System.Collections.Generic;
using System.Drawing;

namespace TaskManagement
{
    internal class TaskGrid
    {
        private CommonGrid _grid;
        private Dictionary<int, Member> _colToMember = new Dictionary<int, Member>();
        private Dictionary<Member, int> _memberToCol = new Dictionary<Member, int>();
        private Dictionary<CallenderDay, int> _dayToRow = new Dictionary<CallenderDay, int>();
        private Dictionary<int, CallenderDay> _rowToDay = new Dictionary<int, CallenderDay>();
        private List<WorkItem> _workItems;

        public TaskGrid(AppData appData, Graphics g, Rectangle pageBounds, Font font)
        {
            _grid = new CommonGrid(g, font);

            UpdateRowColMap(appData);

            _grid.RowCount = appData.Callender.Days.Count + 1;
            var height = (float)(pageBounds.Height) / _grid.RowCount;
            for (int r = 0; r < _grid.RowCount; r++)
            {
                _grid.SetRowHeight(r, height);
            }

            _grid.ColCount = appData.Members.Count + 1;
            var width = (float)(pageBounds.Width) / _grid.ColCount;
            for (int c = 0; c < _grid.ColCount; c++)
            {
                _grid.SetColWidth(c, width);
            }

            _workItems = appData.WorkItems;
        }

        private void UpdateRowColMap(AppData appData)
        {
            int c = 1;
            foreach (var m in appData.Members)
            {
                _colToMember.Add(c, m);
                _memberToCol.Add(m, c);
                c++;
            }

            int r = 1;
            foreach (var d in appData.Callender.Days)
            {
                _dayToRow.Add(d, r);
                _rowToDay.Add(r, d);
                r++;
            }
        }

        internal RectangleF GetBounds(Period period, Member assignedMember)
        {
            var col = _memberToCol[assignedMember];
            var rowTop = _dayToRow[period.From];
            var rowBottom = _dayToRow[period.To];
            var top = _grid.GetCellBounds(rowTop, col);
            var bottom = _grid.GetCellBounds(rowBottom, col);
            return new RectangleF(top.Location, new SizeF(top.Width, bottom.Y - top.Y + top.Height));
        }

        internal void Draw()
        {
            DrawCallenderDays();
            DrawTeamMembers();
            DrawWorkItems();
        }

        private void DrawCallenderDays()
        {
            for (int r = 1; r < _grid.RowCount; r++)
            {
                var rect = _grid.GetCellBounds(r, 0);
                _grid.Graphics.DrawString(_rowToDay[r].ToString(), _grid.Font, Brushes.Black, rect);
            }
        }

        private void DrawTeamMembers()
        {
            for (int c = 1; c < _grid.ColCount; c++)
            {
                var rect = _grid.GetCellBounds(0, c);
                _grid.Graphics.DrawString(_colToMember[c].ToString(), _grid.Font, Brushes.Black, rect);
            }
        }

        private void DrawWorkItems()
        {
            foreach (var wi in _workItems)
            {
                var bounds = GetBounds(wi.Period, wi.AssignedMember);
                _grid.Graphics.DrawString(wi.ToString(), _grid.Font, Brushes.Black, bounds);
                _grid.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(bounds));
            }
        }
    }
}