using FreeGridControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectsTM.UI
{
    internal class HeightAndWidthCalcultor
    {
        private Font _font;
        private Graphics _graphics;
        private Func<TaskListItem, ColIndex, string> _getText;
        private int _colCount;
        private readonly List<TaskListItem> _listItems;
        private Dictionary<RowIndex, float> _heights = new Dictionary<RowIndex, float>();
        private Dictionary<ColIndex, float> _widthds = new Dictionary<ColIndex, float>();

        public HeightAndWidthCalcultor(Font font, Graphics g, List<TaskListItem> listItems, Func<TaskListItem, ColIndex, string> getText, int colCount)
        {
            this._font = font;
            this._graphics = g;
            this._listItems = listItems;
            this._getText = getText;
            this._colCount = colCount;
            Caluculate();
        }

        internal float GetWidth(ColIndex c)
        {
            if (!_widthds.ContainsKey(c)) return 0;
            return _widthds[c];
        }

        internal float GetHeight(RowIndex r)
        {
            if (!_heights.ContainsKey(r)) return 0;
            return _heights[r];
        }

        private void Caluculate()
        {
            _heights[new RowIndex(0)] = (float)Math.Ceiling(_graphics.MeasureString("NAM", _font).Height);
            var t = Task.Run(() =>
            {
                Parallel.ForEach(
                    ColIndex.Range(0, _colCount),
                    (c) =>
                    {
                        using (var bmp = new Bitmap(1, 1))
                        {
                            var g = Graphics.FromImage(bmp);
                            foreach (var r in RowIndex.Range(1, _listItems.Count))
                            {
                                var tmp = g.MeasureString(_getText(_listItems[r.Value - 1], c), _font);
                                _widthds[c] = (float)Math.Ceiling(Math.Max(GetWidth(c), tmp.Width + 10));
                                _heights[r] = (float)Math.Ceiling(Math.Max(GetHeight(r), tmp.Height));
                            }
                        }
                    }
                    );
            });
            while (!t.IsCompleted) Thread.Sleep(0); // こうやって待たないとOnPaintが走って落ちる
        }
    }
}