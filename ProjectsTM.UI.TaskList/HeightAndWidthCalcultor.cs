using FreeGridControl;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectsTM.UI.TaskList
{
    internal class HeightAndWidthCalcultor
    {
        private Font _font;
        private Graphics _graphics;
        private Func<TaskListItem, ColIndex, string> _getText;
        private Func<ColIndex, string> _getTitle;
        private int _colCount;
        private readonly List<TaskListItem> _listItems;
        private int[] _heights;
        private int[] _widths;

        public HeightAndWidthCalcultor(Font font, Graphics g, List<TaskListItem> listItems, Func<TaskListItem, ColIndex, string> getText, Func<ColIndex, string> getTitle, int colCount)
        {
            this._font = font;
            this._graphics = g;
            this._listItems = listItems;
            this._getText = getText;
            this._getTitle = getTitle;
            this._colCount = colCount;
            Caluculate();
        }

        internal int GetWidth(ColIndex c)
        {
            return _widths[c.Value];
        }

        internal int GetHeight(RowIndex r)
        {
            return _heights[r.Value];
        }

        private void Caluculate()
        {
            _heights = new int[_listItems.Count + 1];
            _heights[0] = (int)Math.Ceiling(_graphics.MeasureString("NAM", _font).Height);
            _widths = new int[_colCount];
            var t = Task.Run(() =>
            {
                Parallel.ForEach(
                    ColIndex.Range(0, _colCount),
                    (c) =>
                    {
                        using (var bmp = new Bitmap(1, 1))
                        {
                            var g = Graphics.FromImage(bmp);
                            {
                                var tmp = g.MeasureString(_getTitle(c), _font);
                                _widths[c.Value] = (int)Math.Ceiling(Math.Max(GetWidth(c), tmp.Width + 10));
                            }
                            foreach (var r in RowIndex.Range(1, _listItems.Count))
                            {
                                var tmp = g.MeasureString(_getText(_listItems[r.Value - 1], c), _font);
                                _widths[c.Value] = (int)Math.Ceiling(Math.Max(GetWidth(c), tmp.Width + 10));
                                _heights[r.Value] = (int)Math.Ceiling(Math.Max(GetHeight(r), tmp.Height));
                            }
                        }
                    }
                    );
            });
            while (!t.IsCompleted) Thread.Sleep(0); // こうやって待たないとOnPaintが走って落ちる
        }
    }
}