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

        public void MoveVisibleRowCol(RowIndex row, ColIndex col)
        {
            if (IsVisible(row, col)) return;
            var targetHeight = _cache.GetTop(row) - _cache.FixedHeight;
            this.vScrollBar.Value = (int)((targetHeight / (float)_cache.GridHight) * (this.vScrollBar.Maximum - this.vScrollBar.LargeChange));
            var targetWidth = _cache.GetLeft(col.Offset(1)) - _cache.FixedWidth;
            this.hScrollBar.Value = (int)((targetWidth / (float)_cache.GridWidth) * (this.hScrollBar.Maximum - this.hScrollBar.LargeChange));
        }

        private bool IsVisible(RowIndex row, ColIndex col)
        {
            if (row.Value < VisibleTopRow.Value) return false;
            if (VisibleButtomRow.Value < row.Value) return false;
            if (col.Value < VisibleLeftCol.Value) return false;
            if (VisibleRightCol.Value < col.Value) return false;
            return true;
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
            this.vScrollBar.Maximum = Math.Max(0, _cache.GridHight - this.Height + hScrollBar.Height + vScrollBar.LargeChange);
            this.hScrollBar.Minimum = 0;
            this.hScrollBar.Maximum = Math.Max(0, _cache.GridWidth - this.Width + vScrollBar.Width + hScrollBar.LargeChange);

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

        private int VOffset => vScrollBar.Value;
        private int HOffset => hScrollBar.Value;
        private void DrawGrid(Graphics graphics)
        {
            DrawCell(graphics);
        }

        private void DrawCell(Graphics graphics)
        {
            foreach (var r in RowIndex.Range(0, RowCount))
            {
                foreach (var c in ColIndex.Range(0, ColCount))
                {
                    var rect = GetDrawRect(r, c, VOffset, HOffset);
                    if (rect.IsEmpty) continue;
                    OnDrawCell?.Invoke(this, new DrawCellEventArgs(r, c, rect, graphics));
                }
            }
            OnDrawNormalArea?.Invoke(this, new DrawNormalAreaEventArgs(graphics));
        }

        public RectangleF GetRect(ColIndex col, (RowIndex r, int count) topAndHeight)
        {
            var top = _cache.GetTop(topAndHeight.r);
            var left = _cache.GetLeft(col);
            var width = _cache.GetLeft(col.Offset(1)) - left;
            var height = _cache.GetTop(topAndHeight.r.Offset(topAndHeight.count)) - top;
            var result = new RectangleF(left, top, width, height);
            result.Offset(-HOffset, -VOffset);
            result.Intersect(GetVisibleRect(false, false));
            return result;
        }

        private RectangleF GetDrawRect(RowIndex r, ColIndex c, int vOffset, int hOffset)
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

        private bool IsFixedRow(RowIndex r)
        {
            return r.Value < FixedRowCount;
        }

        private bool IsFixedCol(ColIndex c)
        {
            return c.Value < FixedColCount;
        }

        [Category("Grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RowCount
        {
            get => _cache.RowHeights.Count;
            set
            {
                if (RowCount == value) return;
                _cache.RowHeights.SetCount(value);
                _cache.Update();
            }
        }
        [Category("Grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ColCount
        {
            get => _cache.ColWidths.Count;
            set
            {
                if (ColCount == value) return;
                _cache.ColWidths.SetCount(value);
                _cache.Update();
            }
        }
        [Category("Grid")]
        public int FixedRowCount
        {
            get => _cache.FixedRows; set
            {
                _cache.FixedRows = value;
                _cache.Update();
            }
        }
        [Category("Grid")]
        public int FixedColCount
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

        public RowIndex VisibleTopRow => Y2Row(_cache.FixedHeight + 1);
        public RowIndex VisibleButtomRow => (_cache.GridHight <= this.Height) ? new RowIndex(RowCount - 1) : Y2Row(this.Height - 1);
        public int VisibleRowCount => VisibleButtomRow.Value - VisibleTopRow.Value + 1;
        public ColIndex VisibleLeftCol => X2Col(_cache.FixedWidth + 1);
        public ColIndex VisibleRightCol => (_cache.GridWidth <= this.Width) ? new ColIndex(ColCount - 1) : X2Col(this.Width - 1);
        public int VisibleColCount => VisibleRightCol.Value - VisibleLeftCol.Value + 1;

        public void Print(Graphics graphics)
        {
            DrawGrid(graphics);
        }

        public ColIndex X2Col(int x)
        {
            foreach (var c in ColIndex.Range(0, FixedColCount))
            {
                if (x < _cache.GetLeft(c)) return c.Offset(-1);
            }
            foreach (var c in ColIndex.Range(FixedColCount, ColCount - FixedColCount + 1))
            {
                if (x < _cache.GetLeft(c) - HOffset) return c.Offset(-1);
            }
            return new ColIndex(ColCount - 1);
        }

        public RowIndex Y2Row(int y)
        {
            foreach (var r in RowIndex.Range(0, FixedRowCount))
            {
                if (y < _cache.GetTop(r)) return r.Offset(-1);
            }
            foreach (var r in RowIndex.Range(FixedRowCount, RowCount - FixedColCount + 1))
            {
                if (y < _cache.GetTop(r) - VOffset) return r.Offset(-1);
            }
            return new RowIndex(RowCount - 1);
        }

        public bool Row2Y(RowIndex r, out int y)
        {
            var rect = GetVisibleRect(false, false);
            y = (int)(_cache.GetTop(r.Offset(FixedRowCount)) - VOffset);
            if (y < rect.Top) return false;
            if (rect.Bottom < y) return false;
            return true;
        }
    }
}
