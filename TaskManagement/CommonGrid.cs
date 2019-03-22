using System.Collections.Generic;
using System.Drawing;

namespace TaskManagement
{
    class CommonGrid
    {
        private Dictionary<int, float> _rowToHeight = new Dictionary<int, float>();
        private Dictionary<int, float> _colToWidth = new Dictionary<int, float>();

        public CommonGrid(Graphics g, Font font)
        {
            Graphics = g;
            Font = font;
        }

        public int RowCount { set; get; }
        public int ColCount { set; get; }
        public Graphics Graphics { get; }

        public Font Font { get; }

        public RectangleF GetCellBounds(int row, int col)
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

        public float RowHeight(int row)
        {
            return _rowToHeight[row];
        }

        public float ColWidth(int col)
        {
            return _colToWidth[col];
        }

        internal void SetRowHeight(int r, float height)
        {
            _rowToHeight[r] = height;
        }

        internal void SetColWidth(int c, float width)
        {
            _colToWidth[c] = width;
        }

        internal SizeF MeasureString(string s)
        {
            return Graphics.MeasureString(s, Font, 100, StringFormat.GenericTypographic);
        }

        internal void DrawString(string s, RectangleF rect)
        {
            Graphics.DrawString(s, Font, Brushes.Black, rect, StringFormat.GenericTypographic);
        }
    }
}
