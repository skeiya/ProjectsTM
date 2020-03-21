using System;
using System.Drawing;

namespace FreeGridControl
{
    public class DrawNormalAreaEventArgs
    {
        public Rectangle? VisibleRowColRect { get; private set; }

        public DrawNormalAreaEventArgs(Rectangle? visibleRowColRect, Graphics graphics, Func<int, Tuple<int, int>, RectangleF> getRect)
        {
            this.VisibleRowColRect = visibleRowColRect;
            Graphics = graphics;
            GetRect = getRect;
        }
        public Graphics Graphics { get; set; }

        public Func<int, Tuple<int, int>, RectangleF> GetRect { get; private set; }
    }
}