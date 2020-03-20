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
                DrawGridLine(pe.Graphics);
            }
            catch
            {
                _cache.Update();
                DrawGridLine(pe.Graphics);
            }
            base.OnPaint(pe);
        }

        private void DrawGridLine(Graphics graphics)
        {
            var vOffset = (int)(vScrollBar.Maximum / (float)(vScrollBar.Maximum - vScrollBar.LargeChange) * vScrollBar.Value);
            var hOffset = (int)(hScrollBar.Maximum / (float)(hScrollBar.Maximum - hScrollBar.LargeChange) * hScrollBar.Value);
            for (var r = 0; r <= Rows; r++)
            {
                var h = _cache.GetHeight(r);
                graphics.DrawLine(Pens.Black, new Point(0 - hOffset, h - vOffset), new Point(GridWidth - hOffset, h - vOffset));
            }
            for (var c = 0; c <= Cols; c++)
            {
                var w = _cache.GetWidth(c);
                graphics.DrawLine(Pens.Black, new Point(w - hOffset, 0 - vOffset), new Point(w - hOffset, GridHeight - vOffset));
            }
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
        public int FixedRows { get; set; }
        [Category("Grid")]
        public int FixedCols { get; set; }
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
    }
}
