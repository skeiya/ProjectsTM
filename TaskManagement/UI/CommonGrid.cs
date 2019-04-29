using System;
using System.Collections.Generic;
using System.Drawing;
using TaskManagement.UI;

namespace TaskManagement.UI
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

        public void SetRowHeight(int r, float height)
        {
            _rowToHeight[r] = height;
        }

        public void SetColWidth(int c, float width)
        {
            _colToWidth[c] = width;
        }

        public SizeF MeasureString(string s)
        {
            return Graphics.MeasureString(s, Font, 100, StringFormat.GenericTypographic);
        }

        public void DrawString(string s, RectangleF rect)
        {
            DrawString(s, rect, Color.Black);
        }

        internal void DrawString(string s, RectangleF rect, Color c)
        {
            var deflate = rect;
            deflate.X += 1;
            deflate.Y += 1;
            Graphics.DrawString(s, Font, BrushCache.GetBrush(c), deflate, StringFormat.GenericTypographic);
        }

        internal void DrawMileStoneLine(float bottom, Color color)
        {
            using (var brush = new SolidBrush(color))
            {
                var height = 5f;
                var width = GetFullWidth();
                var x = 0f;
                var y = bottom - height;
                Graphics.FillRectangle(brush, x, y, width, height);
            }
        }

        private float GetFullWidth()
        {
            var result = 0f;
            for (var c = 0; c < ColCount; c++)
            {
                result += ColWidth(c);
            }
            return result;
        }
    }
}
