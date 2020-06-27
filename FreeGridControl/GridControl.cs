using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Threading;

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

        public void MoveVisibleRowColRange(RowIndex row, int count, ColIndex col)
        {
            if (IsVisibleRange(row, count, col)) return;
            MoveVisibleRowCol(row, col);
        }

        public void MoveVisibleRowCol(RowIndex row, ColIndex col)
        {
            if (!IsVisible(row))
            {
                var targetHeight = _cache.GetTop(row) - _cache.FixedHeight;
                this.vScrollBar.Value = (int)((targetHeight / (float)_cache.GridHeight) * (this.vScrollBar.Maximum - this.vScrollBar.LargeChange));
            }
            if (!IsVisible(col))
            {
                var targetWidth = _cache.GetLeft(col.Offset(1)) - _cache.FixedWidth;
                this.hScrollBar.Value = (int)((targetWidth / (float)_cache.GridWidth) * (this.hScrollBar.Maximum - this.hScrollBar.LargeChange));
            }
            Thread.Sleep(250);
        }

        private bool IsVisibleRange(RowIndex row, int count, ColIndex col)
        {
            if (row == null) return false;
            if (col == null) return false;
            if (row.Value + count - 1 < VisibleNormalTopRow.Value) return false;
            if (VisibleNormalButtomRow.Value < row.Value) return false;
            if (col.Value < VisibleNormalLeftCol.Value) return false;
            if (VisibleNormalRightCol.Value < col.Value) return false;
            return true;
        }

        private bool IsVisible(RowIndex row, ColIndex col)
        {
            return IsVisibleRange(row, 1, col);
        }

        private bool IsVisible(RowIndex row)
        {
            return IsVisibleRange(row, 1, VisibleNormalLeftCol);
        }

        private bool IsVisible(ColIndex col)
        {
            return IsVisibleRange(VisibleNormalTopRow, 1, col);
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
            var delta = -(e.Delta / 120) * vScrollBar.SmallChange * 2 * 10;
            var offset = Math.Min(Math.Max(vScrollBar.Value + delta, vScrollBar.Minimum), maximum);

            vScrollBar.Value = offset;
        }
        public bool IsControlDown()
        {
            return (Control.ModifierKeys & Keys.Control) == Keys.Control;
        }

        private void _cache_Updated(object sender, System.EventArgs e)
        {
            vScrollBar.LargeChange = this.Height;
            hScrollBar.LargeChange = this.Width;
            this.vScrollBar.Minimum = 0;
            var vMax = (int)Math.Max(0, _cache.GridHeight - this.Height + hScrollBar.Height + vScrollBar.LargeChange);
            if (this.vScrollBar.Value > vMax) this.vScrollBar.Value = 0;
            this.vScrollBar.Maximum = vMax;
            this.hScrollBar.Minimum = 0;
            var hMax = (int)Math.Max(0, _cache.GridWidth - this.Width + vScrollBar.Width + hScrollBar.LargeChange);
            if (this.hScrollBar.Value > hMax) this.hScrollBar.Value = 0;
            this.hScrollBar.Maximum = hMax;
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
            VisibleNormalTopRow = Y2Row(Client2Raw(new Point(0, (int)(_cache.FixedHeight + 1))).Y);
            VisibleNormalButtomRow = (_cache.GridHeight <= this.Height) ? new RowIndex(RowCount - 1) : Y2Row(Client2Raw(new Point(0, (int)(this.Height - 1))).Y);
            VisibleNormalRowCount = RowCount == FixedRowCount ? 0 : VisibleNormalButtomRow.Value - VisibleNormalTopRow.Value + 1;

            VisibleNormalLeftCol = X2Col(Client2Raw(new Point((int)(_cache.FixedWidth + 1), 0)).X);
            VisibleNormalRightCol = (_cache.GridWidth <= this.Width) ? X2Col(_cache.GridWidth - 1) : X2Col(Client2Raw(new Point((int)(this.Width - 1), 0)).X);
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

        public RectangleF? GetRect(ColIndex col, RowIndex r, int rowCount, bool isFixedRow, bool isFixedCol, bool isFrontView)
        {
            var top = _cache.GetTop(r);
            var left = _cache.GetLeft(col);
            var width = _cache.GetLeft(col.Offset(1)) - left;
            var height = _cache.GetTop(r.Offset(rowCount)) - top;
            var result = new RectangleF(left, top, width, height);
            if (isFrontView)
            {
                result.Offset(isFixedCol ? 0 : -HOffset, isFixedRow ? 0 : -VOffset);
                var visible = GetVisibleRect(isFixedRow, isFixedCol);
                if (!result.IntersectsWith(visible)) return null;
                result.Intersect(visible);
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
        public ColIndex VisibleNormalLeftCol { get; private set; }
        public ColIndex VisibleNormalRightCol { get; private set; }
        public int VisibleNormalColCount { get; private set; }

        public void Print(Graphics graphics)
        {
            DrawGrid(graphics, true);
        }

        public ColIndex X2Col(float x)
        {
            foreach (var c in ColIndex.Range(0, ColCount))
            {
                if (_cache.GetLeft(c) <= x && x < _cache.GetRight(c)) return c;
            }
            return new ColIndex(ColCount - 1);
        }

        public Point Raw2Client(RawPoint raw)
        {
            return new Point(raw.X - HOffset, raw.Y - VOffset);
        }

        public RawPoint Client2Raw(Point client)
        {
            return new RawPoint(client.X + HOffset, client.Y + VOffset);
        }

        public bool IsFixedArea(Point cur)
        {
            if (cur.X < _cache.FixedWidth || cur.Y < _cache.FixedHeight) return true;
            return false;
        }

        public RowIndex Y2Row(float y)
        {
            return Y2NormalRow(y, new RowIndex(0), new RowIndex(RowCount - 1));
        }

        private RowIndex Y2NormalRow(float y, RowIndex low, RowIndex up)
        {
            if (low.Value < 0) return low;
            if (up.Value < 0) return up;
            if (y >= _cache.GetTop(new RowIndex(RowCount))) return new RowIndex(RowCount - 1);

            if (low.Equals(up))
            {
                if (_cache.GetTop(low) <= y && y < _cache.GetTop(new RowIndex(low.Value + 1))) return low;
                return new RowIndex(-1);
            }

            var mid = new RowIndex((low.Value + up.Value) / 2);
            var midButtom = _cache.GetTop(new RowIndex(mid.Value + 1));
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
