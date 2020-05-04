using System.Drawing;
using TaskManagement.UI;

namespace TaskManagement.Service
{
    public enum ScrollDirection
    {
        RIGHT = 0,
        LEFT,
        UPPER,
        LOWER,
        NONE,
    }

    class ScrollByDragServicce
    {
        private static ScrollDirection GetDirection(Point cursorPtOnWorkItemGrid, WorkItemGrid wig)
        {
            var gridLeft = wig.Left;
            for (int i = 0; i < wig.FixedColCount; i++) gridLeft += (int)wig.ColWidths[i];
            if (cursorPtOnWorkItemGrid.X < gridLeft) return ScrollDirection.LEFT;
            if (cursorPtOnWorkItemGrid.X > wig.Right) return ScrollDirection.RIGHT;

            var gridTop = wig.Top;
            for (int i = 0; i < wig.FixedRowCount; i++) gridTop += (int)wig.RowHeights[i];
            if (cursorPtOnWorkItemGrid.Y < gridTop) return ScrollDirection.UPPER;
            if (cursorPtOnWorkItemGrid.Y > wig.Bottom) return ScrollDirection.LOWER;

            return ScrollDirection.NONE;
        }

        public static bool Scroll(Point mouseLocationOnTaskGrid, WorkItemGrid wig)
        {
            var direction = GetDirection(mouseLocationOnTaskGrid, wig);
            if (direction == ScrollDirection.NONE) return false;
            return wig.ScrollAndUpdate(direction);
        }
    }
}
