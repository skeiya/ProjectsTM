using System;
using System.Drawing;

namespace FreeGridControl
{
    public class DrawNormalAreaEventArgs
    {
        public Rectangle? VisibleRowColRect { get; private set; }

        public DrawNormalAreaEventArgs(Rectangle? visibleRowColRect, Graphics graphics, Func<int, Tuple<int, int>, Rectangle> getRect)
        {
            this.VisibleRowColRect = visibleRowColRect;
            Graphics = graphics;
            GetRect = getRect;
        }
        public Graphics Graphics { get; set; }

        public Func<int, Tuple<int, int>, Rectangle> GetRect { get; private set; }
    }
}