using System;
using System.Threading;
using System.Threading.Tasks;

namespace FreeGridControl
{
    class Cache
    {
        private int[] _cacheTop = new int[1] { 0 };
        private int[] _cacheLeft = new int[1] { 0 };
        public IntArrayForDesign RowHeights = new IntArrayForDesign();
        public IntArrayForDesign ColWidths = new IntArrayForDesign();
        private bool _lockUpdate = true;

        public int GridHeight => GetTop(new RowIndex(RowHeights.Count));
        public int GridWidth => GetLeft(new ColIndex(ColWidths.Count));

        public int FixedWidth { get; private set; }
        public int FixedHeight { get; private set; }
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

        public int GetTop(RowIndex row) => _cacheTop[row.Value];
        public int GetLeft(ColIndex col) => _cacheLeft[col.Value];
        public int GetRight(ColIndex col) => _cacheLeft[col.Value] + ColWidths[col.Value];

        public void Update()
        {
            if (_lockUpdate) return;
            var virtical = Task.Run(() =>
            {
                FixedHeight = RowHeights.Sum(FixedRows);
                _cacheTop = new int[RowHeights.Count + 1];
                var height = 0;
                _cacheTop[0] = height;
                for (var idx = 0; idx < RowHeights.Count; idx++)
                {
                    height += RowHeights[idx];
                    _cacheTop[idx + 1] = height;
                }
            });
            var horizontal = Task.Run(() =>
            {
                FixedWidth = ColWidths.Sum(FixedCols);
                _cacheLeft = new int[ColWidths.Count + 1];
                var width = 0;
                _cacheLeft[0] = width;
                for (var idx = 0; idx < ColWidths.Count; idx++)
                {
                    width += ColWidths[idx];
                    _cacheLeft[idx + 1] = width;
                }
            });
            while (!virtical.IsCompleted || !horizontal.IsCompleted) Thread.Sleep(0); // Waitで待つとmessage loop回って、再入発生して落ちる。
            Updated(this, null);
        }

        public event EventHandler Updated;
    }
}
