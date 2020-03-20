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
            _cache.Update();
        }

        private void _colWidths_ItemChanged(object sender, System.EventArgs e)
        {
            _cache.Update();
            this.Refresh();
        }

        private void _rowHeights_ItemChanged(object sender, System.EventArgs e)
        {
            _cache.Update();
            this.Refresh();
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
            for (var r = 0; r <= Rows; r++)
            {
                var h = _cache.GetHeight(r);
                graphics.DrawLine(Pens.Black, new Point(0, h), new Point(GridWidth, h));
            }
            for (var c = 0; c <= Cols; c++)
            {
                var w = _cache.GetWidth(c);
                graphics.DrawLine(Pens.Black, new Point(w, 0), new Point(w, GridHeight));
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
