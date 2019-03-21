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
            _grid = new CommonGrid(g, new Font(font.FontFamily, (float)4));

            UpdateRowColMap(appData);

            _grid.RowCount = appData.Callender.Days.Count + Members.RowCount;
            SetRowHeight(pageBounds);

            _grid.ColCount = appData.Members.Count + Callender.ColCount;
            SetColWidth(g, pageBounds);

            _workItems = appData.WorkItems;
        }

        private void SetColWidth(Graphics g, Rectangle pageBounds)
        {
            var year = g.MeasureString("0000/", _grid.Font, 100, StringFormat.GenericTypographic).Width;
            var month = g.MeasureString("00/", _grid.Font, 100, StringFormat.GenericTypographic).Width;
            var day = g.MeasureString("00", _grid.Font, 100, StringFormat.GenericTypographic).Width;
            var member = ((float)(pageBounds.Width) - year - month - day) / (_grid.ColCount - Callender.ColCount);
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                _grid.SetColWidth(c, member);
            }
            _grid.SetColWidth(0, year);
            _grid.SetColWidth(1, month);
            _grid.SetColWidth(2, day);
        }

        private void SetRowHeight(Rectangle pageBounds)
        {
            var member = _grid.Graphics.MeasureString("AAA", _grid.Font, 100, StringFormat.GenericTypographic).Height;
            var height = ((float)(pageBounds.Height) - member) / (_grid.RowCount - Members.RowCount);
            for (int r = Members.RowCount; r < _grid.RowCount; r++)
            {
                _grid.SetRowHeight(r, height);
            }
            _grid.SetRowHeight(0, member);
        }

        private void UpdateRowColMap(AppData appData)
        {
            int c = Callender.ColCount;
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
            int y = 0;
            int m = 0;
            for (int r = 1; r < _grid.RowCount; r++)
            {
                var year = _rowToDay[r].Year;
                if (y == year) continue;
                y = year;
                var rect = _grid.GetCellBounds(r, 0);
                rect.Height = rect.Height * 2;//TODO: 適当に広げている
                _grid.Graphics.DrawString(year.ToString() + "/", _grid.Font, Brushes.Black, rect, StringFormat.GenericTypographic);
            }
            for (int r = 1; r < _grid.RowCount; r++)
            {
                var month = _rowToDay[r].Month;
                if (m == month) continue;
                m = month;
                var rect = _grid.GetCellBounds(r, 1);
                rect.Height = rect.Height * 2;//TODO: 適当に広げている
                _grid.Graphics.DrawString(month.ToString() + "/", _grid.Font, Brushes.Black, rect, StringFormat.GenericTypographic);
            }
            for (int r = 1; r < _grid.RowCount; r++)
            {
                var rect = _grid.GetCellBounds(r, 2);
                _grid.Graphics.DrawString(_rowToDay[r].Day.ToString(), _grid.Font, Brushes.Black, rect, StringFormat.GenericTypographic);
            }
        }

        private void DrawTeamMembers()
        {
            for (int c = Callender.ColCount; c < _grid.ColCount; c++)
            {
                var rect = _grid.GetCellBounds(0, c);
                _grid.Graphics.DrawString(_colToMember[c].ToString(), _grid.Font, Brushes.Black, rect, StringFormat.GenericTypographic);
            }
        }

        private void DrawWorkItems()
        {
            foreach (var wi in _workItems)
            {
                var bounds = GetBounds(wi.Period, wi.AssignedMember);
                _grid.Graphics.DrawString(wi.ToString(), _grid.Font, Brushes.Black, bounds, StringFormat.GenericTypographic);
                _grid.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(bounds));
            }
        }
    }
}