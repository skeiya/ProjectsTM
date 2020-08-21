using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace FreeGridControl
{
    public partial class GridControl : UserControl
    {
        private Cache _cache = new Cache();

        public event EventHandler<DrawNormalAreaEventArgs> OnDrawNormalArea;

        public int VScrollBarWidth => this.vScrollBar.Width;
        public int HScrollBarHeight => this.hScrollBar.Height;

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
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
        }
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down) return true;
            if (keyData == Keys.Left || keyData == Keys.Right) return true;
            return base.IsInputKey(keyData);
        }
        protected override void OnEnter(EventArgs e)
        {
            this.Invalidate();
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            this.Invalidate();
            base.OnLeave(e);
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
            var vMax = Math.Max(0, _cache.GridHeight - this.Height + hScrollBar.Height + vScrollBar.LargeChange);
            if (this.vScrollBar.Value > vMax) this.vScrollBar.Value = 0;
            this.vScrollBar.Maximum = vMax;
            this.hScrollBar.Minimum = 0;
            var hMax = Math.Max(0, _cache.GridWidth - this.Width + vScrollBar.Width + hScrollBar.LargeChange);
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
            VisibleNormalTopRow = Y2Row(Client2Raw(new ClientPoint(0, _cache.FixedHeight + 1)).Y);
            VisibleNormalButtomRow = (_cache.GridHeight <= this.Height) ? new RowIndex(RowCount - 1) : Y2Row(Client2Raw(new ClientPoint(0, this.Height - 1 - HScrollBarHeight)).Y);
            VisibleNormalRowCount = RowCount == FixedRowCount ? 0 : VisibleNormalButtomRow.Value - VisibleNormalTopRow.Value + 1;

            VisibleNormalLeftCol = X2Col(Client2Raw(new ClientPoint(_cache.FixedWidth + 1, 0)).X);
            VisibleNormalRightCol = (_cache.GridWidth <= this.Width) ? X2Col(_cache.GridWidth - 1) : X2Col(Client2Raw(new ClientPoint(this.Width - 1 - VScrollBarWidth, 0)).X);
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

        public int VOffset
        {
            get { return vScrollBar.Value; }
            set { vScrollBar.Value = value; }
        }
        public int HOffset
        {
            get { return hScrollBar.Value; }
            set { hScrollBar.Value = value; }
        }

        private void DrawGrid(Graphics graphics, bool isPrint)
        {
            OnDrawNormalArea?.Invoke(this, new DrawNormalAreaEventArgs(graphics, isPrint));
        }

        public RawRectangle? GetRectRaw(ColIndex col, RowIndex r, int rowCount)
        {
            var top = _cache.GetTop(r);
            var left = _cache.GetLeft(col);
            var width = _cache.GetLeft(col.Offset(1)) - left;
            var height = _cache.GetTop(r.Offset(rowCount)) - top;
            var result = new RawRectangle(left, top, width, height);
            if (result.IsEmpty) return null;
            return result;
        }

        public ClientRectangle? GetRectClient(ColIndex col, RowIndex r, int rowCount, ClientRectangle visibleArea)
        {
            var raw = GetRectRaw(col, r, rowCount);
            if (!raw.HasValue) return null;
            var location = Raw2Client(raw.Value.Location);
            var result = new ClientRectangle(location.X, location.Y, raw.Value.Width, raw.Value.Height);
            result.Intersect(visibleArea);
            if (result.IsEmpty) return null;
            return result;
        }

        public ClientRectangle GetVisibleRect(bool isFixedRow, bool isFixedCol)
        {
            if (!isFixedRow && !isFixedCol)
            {
                return new ClientRectangle(_cache.FixedWidth, _cache.FixedHeight, Width - _cache.FixedWidth, Height - _cache.FixedHeight);
            }
            else if (isFixedRow && !isFixedCol)
            {
                return new ClientRectangle(_cache.FixedWidth, 0, Width - _cache.FixedWidth, _cache.FixedHeight);
            }
            else if (!isFixedRow && isFixedCol)
            {
                return new ClientRectangle(0, _cache.FixedHeight, _cache.FixedWidth, Height - _cache.FixedHeight);
            }
            return new ClientRectangle(0, 0, _cache.FixedWidth, _cache.FixedHeight);
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
        public int GridHeight => _cache.GridHeight;

        public RowIndex VisibleNormalTopRow { get; private set; }
        public RowIndex VisibleNormalButtomRow { get; private set; }
        public int VisibleNormalRowCount { get; private set; }
        public ColIndex VisibleNormalLeftCol { get; private set; }
        public ColIndex VisibleNormalRightCol { get; private set; }
        public int VisibleNormalColCount { get; private set; }

        public RowColRange VisibleRowColRange => new RowColRange(VisibleNormalLeftCol, VisibleNormalTopRow, VisibleNormalColCount, VisibleNormalRowCount);

        public void Print(Graphics graphics)
        {
            DrawGrid(graphics, true);
        }

        public ColIndex X2Col(int x)
        {
            foreach (var c in ColIndex.Range(0, ColCount))
            {
                if (_cache.GetLeft(c) <= x && x < _cache.GetRight(c)) return c;
            }
            return new ColIndex(ColCount - 1);
        }

        public ColIndex GetAdjustCol(Point location)
        {
            if (FixedRowCount == 0) return null;
            if (FixedHeight < location.Y) return null;
            foreach (var col in ColIndex.Range(0, ColCount - 1))
            {
                var center = _cache.GetRight(col);
                if (center - 2 < location.X && location.X < center + 2) return col;
            }
            return null;
        }

        public Point Raw2Client(RawPoint raw)
        {
            var x = IsFixedCol(raw.X) ? raw.X : raw.X - HOffset;
            var y = IsFixedRow(raw.Y) ? raw.Y : raw.Y - VOffset;
            return new Point(x, y);
        }

        public RawPoint Client2Raw(ClientPoint client)
        {
            var x = IsFixedCol(client.X) ? client.X : client.X + HOffset;
            var y = IsFixedRow(client.Y) ? client.Y : client.Y + VOffset;
            return new RawPoint(x, y);
        }

        public bool IsFixedArea(ClientPoint cur)
        {
            if (IsFixedCol(cur.X) || IsFixedRow(cur.Y)) return true;
            return false;
        }

        private bool IsFixedRow(int y)
        {
            return y < _cache.FixedHeight;
        }

        private bool IsFixedCol(int x)
        {
            return x < _cache.FixedWidth;
        }

        public RowIndex Y2Row(int y)
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

        protected int FixedWidth => _cache.FixedWidth;
        protected int FixedHeight => _cache.FixedHeight;
    }
}
