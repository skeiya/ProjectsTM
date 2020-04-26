using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FreeGridControl
{
    public partial class GridControl : UserControl
    {
        private Cache _cache = new Cache();

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
            this.vScrollBar.ValueChanged += ScrollBar_ValueChanged;
            this.hScrollBar.ValueChanged += ScrollBar_ValueChanged;
            this.MouseWheel += GridControl_MouseWheel;
            this.SizeChanged += GridControl_SizeChanged;
        }

        protected void ScrollHorizontal(int moveDistance)
        {
            var currentWidth = this.hScrollBar.Value * _cache.GridWidth / (this.hScrollBar.Maximum - this.hScrollBar.LargeChange);
            var targetWidth = currentWidth + moveDistance;
            if (targetWidth > _cache.GridWidth) targetWidth = _cache.GridWidth;
            if (targetWidth < 0) targetWidth = 0;
            this.hScrollBar.Value = (int)((targetWidth / (float)_cache.GridWidth) * (this.hScrollBar.Maximum - this.hScrollBar.LargeChange));
        }

        protected void ScrollVertical(int moveDistance)
        {
            var currentHeight = this.vScrollBar.Value * _cache.GridHeight / (this.vScrollBar.Maximum - this.vScrollBar.LargeChange);
            var targetHeight = currentHeight + moveDistance;
            if (targetHeight > _cache.GridHeight) targetHeight = _cache.GridHeight;
            if (targetHeight < 0) targetHeight = 0;
            this.vScrollBar.Value = (int)((targetHeight / (float)_cache.GridHeight) * (this.vScrollBar.Maximum - this.vScrollBar.LargeChange));
        }

        public void MoveVisibleRowCol(RowIndex row, ColIndex col)
        {
            if (IsVisible(row, col)) return;
            if (row != null)
            {
                var targetHeight = _cache.GetTop(row) - _cache.FixedHeight;
                this.vScrollBar.Value = (int)((targetHeight / (float)_cache.GridHeight) * (this.vScrollBar.Maximum - this.vScrollBar.LargeChange));
            }
            if (col != null)
            {
                var targetWidth = _cache.GetLeft(col.Offset(1)) - _cache.FixedWidth;
                this.hScrollBar.Value = (int)((targetWidth / (float)_cache.GridWidth) * (this.hScrollBar.Maximum - this.hScrollBar.LargeChange));
            }
        }

        private bool IsVisible(RowIndex row, ColIndex col)
        {
            if (row == null) return false;
            if (col == null) return false;
            if (row.Value < VisibleNormalTopRow.Value) return false;
            if (VisibleNormalButtomRow.Value < row.Value) return false;
            if (col.Value < VisibleNormalLeftCol.Value) return false;
            if (VisibleNormalRightCol.Value < col.Value) return false;
            return true;
        }

        private void GridControl_SizeChanged(object sender, EventArgs e)
        {
            _cache_Updated(null, null);
        }

        private void GridControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (IsControlDown()) return;
            if (Math.Abs(e.Delta) < 120) return;

            var maximum = 1 + vScrollBar.Maximum - vScrollBar.LargeChange;
            var delta = -(e.Delta / 120) * vScrollBar.SmallChange * 2;
            var offset = Math.Min(Math.Max(vScrollBar.Value + delta, vScrollBar.Minimum), maximum);

            vScrollBar.Value = offset;
        }
        public bool IsControlDown()
        {
            return (Control.ModifierKeys & Keys.Control) == Keys.Control;
        }

        private void _cache_Updated(object sender, System.EventArgs e)
        {
            this.vScrollBar.Minimum = 0;
            this.vScrollBar.Maximum = (int)Math.Max(0, _cache.GridHeight - this.Height + hScrollBar.Height + vScrollBar.LargeChange);
            this.hScrollBar.Minimum = 0;
            this.hScrollBar.Maximum = (int)Math.Max(0, _cache.GridWidth - this.Width + vScrollBar.Width + hScrollBar.LargeChange);
            UpdateVisibleRange();
            this.Invalidate();
        }

        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            UpdateVisibleRange();
            this.Refresh();
        }

        private void UpdateVisibleRange()
        {
            VisibleNormalTopRow = Y2Row(_cache.FixedHeight + 1);
            VisibleNormalButtomRow = (_cache.GridHeight <= this.Height) ? new RowIndex(RowCount - 1) : Y2Row(this.Height - 1);
            VisibleNormalRowCount = RowCount == FixedRowCount ? 0 : VisibleNormalButtomRow.Value - VisibleNormalTopRow.Value + 1;
            VisibleNormalColCount = ColCount == FixedColCount ? 0 : VisibleNormalRightCol.Value - VisibleNormalLeftCol.Value + 1;
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
                DrawGrid(pe.Graphics, false);
            }
            catch
            {
                _cache.Update();
                DrawGrid(pe.Graphics, false);
            }
            base.OnPaint(pe);
        }

        protected int VOffset => vScrollBar.Value;
        protected int HOffset => hScrollBar.Value;
        private void DrawGrid(Graphics graphics, bool isPrint)
        {
            OnDrawNormalArea?.Invoke(this, new DrawNormalAreaEventArgs(graphics, isPrint));
        }

        public RectangleF GetRect(ColIndex col, RowIndex r, int rowCount, bool isFixedRow, bool isFixedCol, bool isFrontView)
        {
            var top = _cache.GetTop(r);
            var left = _cache.GetLeft(col);
            var width = _cache.GetLeft(col.Offset(1)) - left;
            var height = _cache.GetTop(r.Offset(rowCount)) - top;
            var result = new RectangleF(left, top, width, height);
            if (isFrontView)
            {
                result.Offset(isFixedCol ? 0 : -HOffset, isFixedRow ? 0 : -VOffset);
                result.Intersect(GetVisibleRect(isFixedRow, isFixedCol));
            }
            return result;
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
        public FloatArrayForDesign RowHeights
        {
            get => _cache.RowHeights;
            set
            {
                ;
            }
        }

        [Category("Grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public FloatArrayForDesign ColWidths
        {
            get => _cache.ColWidths;
            set
            {
                ;
            }
        }

        public float GridWidth => _cache.GridWidth;
        public float GridHeight => _cache.GridHeight;

        public RowIndex VisibleNormalTopRow { get; private set; }
        public RowIndex VisibleNormalButtomRow { get; private set; }
        public int VisibleNormalRowCount { get; private set; }
        public int VisibleNormalColCount { get; private set; }
        public ColIndex VisibleNormalLeftCol => X2Col(_cache.FixedWidth + 1);
        public ColIndex VisibleNormalRightCol => (_cache.GridWidth <= this.Width) ? new ColIndex(ColCount - 1) : X2Col(this.Width - 1);

        public void Print(Graphics graphics)
        {
            DrawGrid(graphics, true);
        }

        public ColIndex X2Col(float x)
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

        public RowIndex Y2Row(float y)
        {
            foreach (var r in RowIndex.Range(0, FixedRowCount))
            {
                if (y < _cache.GetTop(r)) return r.Offset(-1);
            }
            return Y2NormalRow(y, new RowIndex(FixedRowCount - 1), new RowIndex(RowCount - 1));
        }

        private RowIndex Y2NormalRow(float y, RowIndex low, RowIndex up)
        {
            if (low.Value < 0) return low;
            if (up.Value < 0) return up;
            if (y >= _cache.GetTop(new RowIndex(RowCount)) - VOffset) return new RowIndex(RowCount - 1);

            if (low.Equals(up))
            {
                if (_cache.GetTop(low) - VOffset <= y && y < _cache.GetTop(new RowIndex(low.Value + 1)) - VOffset) return low;
                return new RowIndex(-1);
            }

            var mid = new RowIndex((low.Value + up.Value) / 2);
            var midButtom = _cache.GetTop(new RowIndex(mid.Value + 1)) - VOffset;
            if (y < midButtom) return Y2NormalRow(y, low, mid);
            if (up.Value - low.Value == 1) mid = up;
            return Y2NormalRow(y, mid, up);
        }

        public bool Row2Y(RowIndex r, out float y)
        {
            if (r == null)
            {
                y = -1;
                return false;
            }
            var rect = GetVisibleRect(false, false);
            y = _cache.GetTop(r) - VOffset;
            if (y < rect.Top) return false;
            if (rect.Bottom < y) return false;
            return true;
        }

        protected float FixedWidth => _cache.FixedWidth;
        protected float FixedHeight => _cache.FixedHeight;
    }
}
