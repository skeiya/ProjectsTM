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
        private IntArrayForDesign _rowHeights = new IntArrayForDesign();
        private IntArrayForDesign _colWidths = new IntArrayForDesign();

        public GridControl()
        {
            InitializeComponent();
            _rowHeights.ItemChanged += _rowHeights_ItemChanged;
            _colWidths.ItemChanged += _colWidths_ItemChanged;
        }

        private void _colWidths_ItemChanged(object sender, System.EventArgs e)
        {
            this.Refresh();
        }

        private void _rowHeights_ItemChanged(object sender, System.EventArgs e)
        {
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            DrawGridLine(pe.Graphics);
            base.OnPaint(pe);
        }

        private void DrawGridLine(Graphics graphics)
        {
            for (var r = 0; r <= Rows; r++)
            {
                var h = _rowHeights.Sum(r);
                graphics.DrawLine(Pens.Black, new Point(0, h), new Point(this.Width, h));
            }
            for (var c = 0; c <= Cols; c++)
            {
                var w = _colWidths.Sum(c);
                graphics.DrawLine(Pens.Black, new Point(w, 0), new Point(w, this.Height));
            }
        }

        [Category("Grid")]
        public int Rows
        {
            get => _rowHeights.Count;
            set
            {
                if (_rowHeights.Count == value) return;
                _rowHeights.SetCount(value);
                this.Invalidate();
            }
        }
        [Category("Grid")]
        public int Cols
        {
            get => _colWidths.Count;
            set
            {
                if (_colWidths.Count == value) return;
                _colWidths.SetCount(value);
                this.Invalidate();
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
            get => _rowHeights;
            set
            {
                ;
            }
        }
        [Category("Grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IntArrayForDesign ColWidths
        {
            get => _colWidths;
            set
            {
                ;
            }
        }
    }
}
