using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeGridControl
{
    class Cache
    {
        private Dictionary<int, int> _cacheHeight = new Dictionary<int, int>();
        private Dictionary<int, int> _chacheWidth = new Dictionary<int, int>();
        public IntArrayForDesign RowHeights = new IntArrayForDesign();
        public IntArrayForDesign ColWidths = new IntArrayForDesign();

        public int GridHight => GetHeight(RowHeights.Count);
        public int GridWidth => GetWidth(ColWidths.Count);

        public int GetHeight(int row) => _cacheHeight[row];
        public int GetWidth(int col) => _chacheWidth[col];

        public void Update()
        {
            _cacheHeight.Clear();
            _chacheWidth.Clear();
            for (var r = 0; r <= RowHeights.Count; r++)
            {
                _cacheHeight.Add(r, RowHeights.Sum(r));
            }
            for (var c = 0; c <= ColWidths.Count; c++)
            {
                _chacheWidth.Add(c, ColWidths.Sum(c));
            }
            Updated(this, null);
        }

        public event EventHandler Updated;
    }
}
