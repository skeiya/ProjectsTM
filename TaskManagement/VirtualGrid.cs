using System;
using System.Collections.Generic;
using System.Drawing;

namespace TaskManagement
{
    internal class VirtualGrid
    {
        private Dictionary<int, Member> _colToMember = new Dictionary<int, Member>();
        private Dictionary<Member, int> _memberToCol = new Dictionary<Member, int>();
        private float _rowHeight;
        private int _colCount;
        private float _colWidth;
        private Dictionary<CallenderDay, int> _dayToRow = new Dictionary<CallenderDay, int>();
        private Dictionary<int, CallenderDay> _rowToDay = new Dictionary<int, CallenderDay>();
        private Font _font;
        private List<WorkItem> _workItems;
        private readonly int _rowCount;
        private readonly Graphics _graphics;

        public VirtualGrid(AppData appData, Graphics g, Font font)
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

            _rowCount = appData.Callender.Days.Count + 1;
            _rowHeight = g.VisibleClipBounds.Height / _rowCount;

            _colCount = appData.Members.Count + 1;
            _colWidth = g.VisibleClipBounds.Width / _colCount;
            this._graphics = g;
            _font = font;

            _workItems = appData.WorkItems;
        }

        internal RectangleF GetBounds(Period period, Member assignedMember)
        {
            var col = _memberToCol[assignedMember];
            var rowTop = _dayToRow[period.From];
            var rowBottom = _dayToRow[period.To];
            var top = GetCellBounds(rowTop, col);
            var bottom = GetCellBounds(rowBottom, col);
            return new RectangleF(top.Location, new SizeF(top.Width, bottom.Y - top.Y + top.Height));
        }

        private RectangleF GetCellBounds(int row, int col)
        {
            var result = new RectangleF();

            for (int r = 0; r < row; r++)
            {
                result.Y += RowHeight(r);
            }
            result.Height = RowHeight(row);

            for (int c = 0; c < col; c++)
            {
                result.X += ColWidth(c);
            }
            result.Width = ColWidth(col);

            return result;
        }

        internal void Draw()
        {
            DrawCallenderDays();
            DrawTeamMembers();
            DrawWorkItems();
        }

        int RowCount => _rowCount;

        private float RowHeight(int row)
        {
            return _rowHeight;
        }

        private float ColWidth(int col)
        {
            return _colWidth;
        }

        private void DrawCallenderDays()
        {
            for (int r = 1; r < _rowCount; r++)
            {
                var rect = GetCellBounds(r, 0);
                _graphics.DrawString(_rowToDay[r].ToString(), _font, Brushes.Black, rect);
            }
        }

        private void DrawTeamMembers()
        {
            for (int c = 1; c < _colCount; c++)
            {
                var rect = GetCellBounds(0, c);
                _graphics.DrawString(_colToMember[c].ToString(), _font, Brushes.Black, rect);
            }
        }

        private void DrawWorkItems()
        {
            foreach (var wi in _workItems)
            {
                var bounds = GetBounds(wi.Period, wi.AssignedMember);
                _graphics.DrawString(wi.ToString(), _font, Brushes.Black, bounds);
                _graphics.DrawRectangle(Pens.Black, Rectangle.Round(bounds));
            }
        }
    }
}