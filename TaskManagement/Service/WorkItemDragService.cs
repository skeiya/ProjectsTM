using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskManagement.Service
{
    class WorkItemDragService
    {
        WorkItem _draggingWorkItem = null;
        private Point _draggedLocation;
        CallenderDay _draggedDay = null;
        Period _draggedPeriod = null;
        private Member _draggedMember;
        private int _expandDirection = 0;

        public bool IsDragging()
        {
            return _draggingWorkItem != null;
        }

        public void UpdateDraggingItem(TaskGrid grid, Point curLocation, ViewData viewData)
        {
            var callender = viewData.Original.Callender;

            if (viewData.Selected != null && _expandDirection != 0)
            {
                UpdateExpand(viewData.Selected, grid, curLocation, callender);
                return;
            }

            if (!IsDragging()) return;
            var member = grid.GetMemberFromX(curLocation.X);
            if (member == null) return;
            var curDay = grid.GetDayFromY(curLocation.Y);
            if (curDay == null) return;

            if (IsOnlyMoveHorizontal(curLocation))
            {
                _draggingWorkItem.AssignedMember = member;
                _draggingWorkItem.Period = _draggedPeriod;
            }
            else if (IsOnlyMoveVirtical(curLocation))
            {
                _draggingWorkItem.AssignedMember = _draggedMember;
                var offset = callender.GetOffset(_draggedDay, curDay);
                _draggingWorkItem.Period = _draggedPeriod.ApplyOffset(offset, callender);
            }
            else
            {
                _draggingWorkItem.AssignedMember = member;
                var offset = callender.GetOffset(_draggedDay, curDay);
                _draggingWorkItem.Period = _draggedPeriod.ApplyOffset(offset, callender);
            }

        }

        private void UpdateExpand(WorkItem selected, TaskGrid grid, Point curLocation, Callender callender)
        {
            var curDay = grid.GetDayFromY(curLocation.Y);
            if (curDay == null) return;

            var draggedDay = _expandDirection > 0 ? selected.Period.From : selected.Period.To;
            var offset = callender.GetOffset(draggedDay, curDay);
            if(_expandDirection > 0)
            {
                selected.Period.From = callender.ApplyOffset(selected.Period.From, offset + 1);
            }
            else if(_expandDirection < 0)
            {
                selected.Period.To = callender.ApplyOffset(selected.Period.To, offset - 1);
            }
        }

        private bool IsOnlyMoveHorizontal(Point curLocation)
        {
            if (!IsShiftDown()) return false;
            return !IsVirticalLong(_draggedLocation, curLocation);
        }

        private bool IsOnlyMoveVirtical(Point curLocation)
        {
            if (!IsShiftDown()) return false;
            return IsVirticalLong(_draggedLocation, curLocation);

        }

        private bool IsVirticalLong(Point a, Point b)
        {
            var h = Math.Abs(a.Y - b.Y);
            var w = Math.Abs(a.X - b.X);
            return w < h;
        }

        private bool IsShiftDown()
        {
            return (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
        }

        internal void Start(WorkItem wi, Point location, TaskGrid grid)
        {
            _draggingWorkItem = wi;
            _draggedLocation = location;
            _draggedPeriod = wi.Period.Clone();
            _draggedMember = wi.AssignedMember;
            _draggedDay = grid.GetDayFromY(location.Y);
        }

        internal void End()
        {
            _draggingWorkItem = null;
            _expandDirection = 0;
        }

        internal static Tuple<PointF, PointF> GetBottomBarLine(RectangleF bounds, float height)
        {
            var bar = GetBottomBarRect(bounds, height);
            return GetMidLine(bar);
        }

        internal static Tuple<PointF, PointF> GetTopBarLine(RectangleF bounds, float height)
        {
            var bar = GetTopBarRect(bounds, height);
            return GetMidLine(bar);
        }

        private static Tuple<PointF, PointF> GetMidLine(RectangleF bar)
        {
            var y = (bar.Bottom + bar.Top) / 2;
            var p1 = new PointF(bar.X + bar.Width / 4, y);
            var p2 = new PointF(bar.X + bar.Width * 3 / 4, y);
            return new Tuple<PointF, PointF>(p1, p2);
        }

        internal static RectangleF GetBottomBarRect(RectangleF bounds, float height)
        {
            return new RectangleF(bounds.X, bounds.Bottom, bounds.Width, height);
        }

        internal static RectangleF GetTopBarRect(RectangleF bounds, float height)
        {
            return new RectangleF(bounds.X, bounds.Top - height, bounds.Width, height);
        }

        internal void StartExpand(int direction)
        {
            _expandDirection = direction;
        }
    }
}
