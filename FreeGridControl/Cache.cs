using System;
using System.Collections.Generic;
using System.Drawing;

namespace FreeGridControl
{
    class Cache
    {
        private Dictionary<RowIndex, float> _cacheTop = new Dictionary<RowIndex, float>();
        private Dictionary<ColIndex, float> _cacheLeft = new Dictionary<ColIndex, float>();
        private Dictionary<Tuple<RowIndex, ColIndex>, RectangleF> _chacheRect = new Dictionary<Tuple<RowIndex, ColIndex>, RectangleF>();
        public FloatArrayForDesign RowHeights = new FloatArrayForDesign();
        public FloatArrayForDesign ColWidths = new FloatArrayForDesign();
        private bool _lockUpdate = true;

        public float GridHight => GetTop(new RowIndex(RowHeights.Count));
        public float GridWidth => GetLeft(new ColIndex(ColWidths.Count));

        public float FixedWidth { get; private set; }
        public float FixedHeight { get; private set; }
        public int FixedRows { get; set; }
        public int FixedCols { get; set; }
        public bool LockUpdate
        {
            get => _lockUpdate; internal set
            {
                var org = _lockUpdate;
                _lockUpdate = value;
                if (org && !_lockUpdate) Update();
            }
        }

        public float GetTop(RowIndex row) => _cacheTop.Count == 0 ? 0 : _cacheTop[row];
        public float GetLeft(ColIndex col) => _cacheLeft.Count == 0 ? 0 : _cacheLeft[col];

        public void Update()
        {
            if (_lockUpdate) return;
            FixedHeight = RowHeights.Sum(FixedRows);
            FixedWidth = ColWidths.Sum(FixedCols);
            _cacheTop.Clear();
            _cacheLeft.Clear();
            foreach (var r in RowIndex.Range(0, RowHeights.Count + 1))
            {
                _cacheTop.Add(r, RowHeights.Sum(r.Value));
            }
            foreach (var c in ColIndex.Range(0, ColWidths.Count + 1))
            {
                _cacheLeft.Add(c, ColWidths.Sum(c.Value));
            }
            _chacheRect.Clear();
            foreach (var r in RowIndex.Range(0, RowHeights.Count))
            {
                foreach (var c in ColIndex.Range(0, ColWidths.Count))
                {
                    var key = new Tuple<RowIndex, ColIndex>(r, c);
                    var value = new RectangleF(GetLeft(c), GetTop(r), GetLeft(c.Offset(1)) - GetLeft(c), GetTop(r.Offset(1)) - GetTop(r));
                    _chacheRect.Add(key, value);
                }
            }
            Updated(this, null);
        }

        public event EventHandler Updated;

        internal RectangleF GetRectangle(RowIndex r, ColIndex c)
        {
            return _chacheRect[new Tuple<RowIndex, ColIndex>(r, c)];
        }
    }
}
