using System;
using System.Collections.Generic;
using System.Drawing;

namespace TaskManagement.UI
{
    class CellBoundsCache
    {
        Dictionary<Tuple<int, int>, RectangleF> _data = new Dictionary<Tuple<int, int>, RectangleF>();

        internal void Set(int r, int c, RectangleF bound)
        {
            _data[new Tuple<int, int>(r, c)] = bound;
        }

        internal RectangleF Get(int r, int c)
        {
            return _data[new Tuple<int, int>(r, c)];
        }
    }
}
