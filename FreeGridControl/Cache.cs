using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeGridControl
{
    class Cache
    {
        private Dictionary<int, int> _cacheTop = new Dictionary<int, int>();
        private Dictionary<int, int> _chacheLeft = new Dictionary<int, int>();
        private Dictionary<Tuple<int, int>, RectangleF> _chacheRect = new Dictionary<Tuple<int, int>, RectangleF>();
        public IntArrayForDesign RowHeights = new IntArrayForDesign();
        public IntArrayForDesign ColWidths = new IntArrayForDesign();

        public int GridHight => GetTop(RowHeights.Count);
        public int GridWidth => GetLeft(ColWidths.Count);

        public int FixedWidth { get; private set; }
        public int FixedHeight { get; private set; }
        public int FixedRows { get; set; }
        public int FixedCols { get; set; }

        public int GetTop(int row) => _cacheTop[row];
        public int GetLeft(int col) => _chacheLeft[col];

        public void Update()
        {
            FixedHeight = RowHeights.Sum(FixedRows);
            FixedWidth = ColWidths.Sum(FixedCols);
            _cacheTop.Clear();
            _chacheLeft.Clear();
            for (var r = 0; r <= RowHeights.Count; r++)
            {
                _cacheTop.Add(r, RowHeights.Sum(r));
            }
            for (var c = 0; c <= ColWidths.Count; c++)
            {
                _chacheLeft.Add(c, ColWidths.Sum(c));
            }
            _chacheRect.Clear();
            for (var r = 0; r < RowHeights.Count; r++)
            {
                for (var c = 0; c < ColWidths.Count; c++)
                {
                    var key = new Tuple<int, int>(r, c);
                    var value = new RectangleF(GetLeft(c), GetTop(r), GetLeft(c + 1) - GetLeft(c), GetTop(r + 1) - GetTop(r));
                    _chacheRect.Add(key, value);
                }
            }
            Updated(this, null);
        }

        public event EventHandler Updated;

        internal RectangleF GetRectangle(int r, int c)
        {
            return _chacheRect[new Tuple<int, int>(r, c)];
        }
    }
}
