using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FreeGridControl
{
    public partial class GridControl : UserControl
    {
        private Cache _cache = new Cache();

        public event EventHandler<DrawCellEventArgs> OnDrawCell;
        public event EventHandler<DrawNormalAreaEventArgs> OnDrawNormalArea;

        public bool LockUpdate { set { _cache.LockUpdate = value; } get { return _cache.LockUpdate; } }

        public GridControl()
        {
            InitializeComponent();
            _cache.RowHeights.ItemChanged += _rowHeights_ItemChanged;
            _cache.ColWidths.ItemChanged += _colWidths_ItemChanged;
            this.DoubleBuffered = true;
            _cache.Updated += _cache_Updated;
            _cache.Update();
            this.vScrollBar.Value = 0;
            this.hScrollBar.Value = 0;
            this.vScrollBar.Scroll += ScrollBar_Scroll;
            this.hScrollBar.Scroll += ScrollBar_Scroll;
            this.MouseWheel += GridControl_MouseWheel;
            this.SizeChanged += GridControl_SizeChanged;
        }

        private void GridControl_SizeChanged(object sender, EventArgs e)
        {
            _cache_Updated(null, null);
        }

        private void GridControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Math.Abs(e.Delta) < 120) return;

            var maximum = 1 + vScrollBar.Maximum - vScrollBar.LargeChange;
            var delta = -(e.Delta / 120) * vScrollBar.SmallChange * 2;
            var offset = Math.Min(Math.Max(vScrollBar.Value + delta, vScrollBar.Minimum), maximum);

            vScrollBar.Value = offset;
            this.Refresh();
        }

        private void _cache_Updated(object sender, System.EventArgs e)
        {
            this.vScrollBar.Minimum = 0;
            this.vScrollBar.Maximum = Math.Max(0, _cache.GridHight - Height + hScrollBar.Height);
            this.hScrollBar.Minimum = 0;
            this.hScrollBar.Maximum = Math.Max(0, _cache.GridWidth - Width + vScrollBar.Width);

            this.Refresh();
        }

        private void ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.Refresh();
        }

        private void _colWidths_ItemChanged(object sender, System.EventArgs e)
        {
            _cache.Update();
        }

        private void _rowHeights_ItemChanged(object sender, System.EventArgs e)
        {
            _cache.Update();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            try
            {
                DrawGrid(pe.Graphics);
            }
            catch
            {
                _cache.Update();
                DrawGrid(pe.Graphics);
            }
            base.OnPaint(pe);
        }

        private float VOffset => vScrollBar.Maximum / (float)(vScrollBar.Maximum - vScrollBar.LargeChange) * vScrollBar.Value;
        private float HOffset => hScrollBar.Maximum / (float)(hScrollBar.Maximum - hScrollBar.LargeChange) * hScrollBar.Value;
        private void DrawGrid(Graphics graphics)
        {
            var vOffset = (int)(vScrollBar.Maximum / (float)(vScrollBar.Maximum - vScrollBar.LargeChange) * vScrollBar.Value);
            var hOffset = (int)(hScrollBar.Maximum / (float)(hScrollBar.Maximum - hScrollBar.LargeChange) * hScrollBar.Value);
            //DrawFixedLine(graphics, vOffset, hOffset);
            //DrawNormalLine(graphics, vOffset, hOffset);
            DrawCell(graphics, vOffset, hOffset);
        }

        private void DrawNormalLine(Graphics graphics, int vOffset, int hOffset)
        {
            for (var r = FixedRows + 1; r <= Rows; r++)
            {
                var h = _cache.GetTop(r) - vOffset;
                if (h <= FixedHight) continue;
                graphics.DrawLine(Pens.Black, new Point(0 - hOffset, h), new Point(GridWidth - hOffset, h));
            }
            for (var c = FixedCols + 1; c <= Cols; c++)
            {
                var w = _cache.GetLeft(c) - hOffset;
                if (w <= FixedWidth) continue;
                graphics.DrawLine(Pens.Black, new Point(w, 0 - vOffset), new Point(w, GridHeight - vOffset));
            }
        }

        private void DrawFixedLine(Graphics graphics, int vOffset, int hOffset)
        {
            for (var r = 0; r <= FixedRows; r++)
            {
                var h = _cache.GetTop(r);
                graphics.DrawLine(Pens.Blue, new Point(0, h), new Point(GridWidth, h));
            }
            for (var c = 0; c <= FixedCols; c++)
            {
                var w = _cache.GetLeft(c);
                graphics.DrawLine(Pens.Blue, new Point(w, 0), new Point(w, GridHeight));
            }
        }

        private void DrawCell(Graphics graphics, int vOffset, int hOffset)
        {
            for (var r = 0; r < Rows; r++)
            {
                for (var c = 0; c < Cols; c++)
                {
                    var rect = GetDrawRect(r, c, vOffset, hOffset);
                    if (rect.IsEmpty) continue;
                    OnDrawCell?.Invoke(this, new DrawCellEventArgs(r, c, rect, graphics));
                }
            }
            var visibleRowColRect = GetVisibleRowColRect(vOffset, hOffset);
            OnDrawNormalArea?.Invoke(this, new DrawNormalAreaEventArgs(visibleRowColRect, graphics, GetRect));
        }

        private RectangleF GetRect(int col, Tuple<int, int> topAndHeight)
        {
            var left = _cache.GetLeft(col + FixedCols);
            var top = _cache.GetTop(topAndHeight.Item1 + FixedRows);
            var width = _cache.ColWidths[col + FixedCols];
            var height = _cache.GetTop(topAndHeight.Item1 + topAndHeight.Item2 - 1 + FixedRows) + _cache.RowHeights[topAndHeight.Item1 + topAndHeight.Item2 - 1 + FixedRows] - top;
            var result = new RectangleF(left, top, width, height);
            result.Offset(-HOffset, -VOffset);
            result.Intersect(GetVisibleRect(false, false));
            return result;
        }

        private Rectangle? GetVisibleRowColRect(int vOffset, int hOffset)
        {
            var leftTop = GetVisibleRowColLeftTop(vOffset, hOffset);
            var rightButtom = GetVisibleRowColRightButtom(vOffset, hOffset);
            if (leftTop == null || rightButtom == null) return null;
            return new Rectangle(leftTop.Value.X - FixedCols, leftTop.Value.Y - FixedRows, rightButtom.Value.X - leftTop.Value.X + 1, rightButtom.Value.Y - leftTop.Value.Y + 1);
        }

        private Point? GetVisibleRowColRightButtom(int vOffset, int hOffset)
        {
            for (var r = Rows - 1; r >= FixedRows; r--)
            {
                for (var c = Cols - 1; c >= FixedCols; c--)
                {
                    var rect = GetDrawRect(r, c, vOffset, hOffset);
                    if (!rect.IsEmpty) return new Point(c, r);
                }
            }
            return null;
        }

        private Point? GetVisibleRowColLeftTop(int vOffset, int hOffset)
        {
            for (var r = FixedRows; r < Rows; r++)
            {
                for (var c = FixedCols; c < Cols; c++)
                {
                    var rect = GetDrawRect(r, c, vOffset, hOffset);
                    if (!rect.IsEmpty) return new Point(c, r);
                }
            }
            return null;
        }

        private RectangleF GetDrawRect(int r, int c, int vOffset, int hOffset)
        {
            var isFixedRow = IsFixedRow(r);
            var isFixedCol = IsFixedCol(c);
            var rect = _cache.GetRectangle(r, c);
            rect.Offset(isFixedCol ? 0 : -hOffset, isFixedRow ? 0 : -vOffset);
            rect.Intersect(GetVisibleRect(isFixedRow, isFixedCol));
            return rect;
        }

        RectangleF GetVisibleRect(bool isFixedRow, bool isFixedCol)
        {
            if (!isFixedRow && !isFixedCol)
            {
                return new RectangleF(_cache.FixedWidth, _cache.FixedHeight, Width - _cache.FixedWidth, Height - _cache.FixedHeight);
            }
            else if (isFixedRow && !isFixedCol)
            {
                return new RectangleF(_cache.FixedWidth, 0, Width - _cache.FixedWidth, _cache.FixedHeight);
            }
            else if (!isFixedRow && isFixedCol)
            {
                return new RectangleF(0, _cache.FixedHeight, _cache.FixedWidth, Height - _cache.FixedHeight);
            }
            return new RectangleF(0, 0, _cache.FixedWidth, _cache.FixedHeight);
        }

        private bool IsFixedRow(int r)
        {
            return r < FixedRows;
        }

        private bool IsFixedCol(int c)
        {
            return c < FixedCols;
        }

        [Category("Grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Rows
        {
            get => _cache.RowHeights.Count;
            set
            {
                if (Rows == value) return;
                _cache.RowHeights.SetCount(value);
                _cache.Update();
            }
        }
        [Category("Grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Cols
        {
            get => _cache.ColWidths.Count;
            set
            {
                if (Cols == value) return;
                _cache.ColWidths.SetCount(value);
                _cache.Update();
            }
        }
        [Category("Grid")]
        public int FixedRows
        {
            get => _cache.FixedRows; set
            {
                _cache.FixedRows = value;
                _cache.Update();
            }
        }
        [Category("Grid")]
        public int FixedCols
        {
            get => _cache.FixedCols; set
            {
                _cache.FixedCols = value;
                _cache.Update();
            }
        }
        [Category("Grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IntArrayForDesign RowHeights
        {
            get => _cache.RowHeights;
            set
            {
                ;
            }
        }
        [Category("Grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IntArrayForDesign ColWidths
        {
            get => _cache.ColWidths;
            set
            {
                ;
            }
        }

        public int GridWidth => _cache.GridWidth;
        public int GridHeight => _cache.GridHight;

        public int FixedHight => _cache.FixedHeight;
        public int FixedWidth => _cache.FixedWidth;
    }
}
