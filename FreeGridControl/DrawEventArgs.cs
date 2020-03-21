using System.Drawing;

namespace FreeGridControl
{
    public class DrawCellEventArgs
    {
        public DrawCellEventArgs(int r, int c, RectangleF rect, Graphics graphics)
        {
            RowIndex = r;
            ColIndex = c;
            Rect = rect;
            Graphics = graphics;
        }

        public int RowIndex { get; }
        public int ColIndex { get; }
        public RectangleF Rect { get; }
        public Graphics Graphics { get; }
    }
}