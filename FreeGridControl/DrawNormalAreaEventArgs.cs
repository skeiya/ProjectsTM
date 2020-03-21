using System;
using System.Drawing;

namespace FreeGridControl
{
    public class DrawNormalAreaEventArgs
    {
        public Rectangle? VisibleRowColRect { get; private set; }

        public DrawNormalAreaEventArgs(Rectangle? visibleRowColRect, Graphics graphics)
        {
            this.VisibleRowColRect = visibleRowColRect;
            Graphics = graphics;
        }
        public Graphics Graphics { get; set; }
    }
}