using System.Drawing;

namespace FreeGridControl
{
    public class DrawCellEventArgs
    {
        public DrawCellEventArgs(RowIndex r, ColIndex c, RectangleF rect, Graphics graphics)
        {
            RowIndex = r;
            ColIndex = c;
            Rect = rect;
            Graphics = graphics;
        }

        public RowIndex RowIndex { get; }
        public ColIndex ColIndex { get; }
        public RectangleF Rect { get; }
        public Graphics Graphics { get; }
    }
}