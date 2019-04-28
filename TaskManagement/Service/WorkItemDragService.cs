using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TaskManagement.Service
{
    class WorkItemDragService
    {
        private string _beforeWorkItem;
        WorkItem _draggingWorkItem = null;
        private Point _draggedLocation;
        CallenderDay _draggedDay = null;
        Period _draggedPeriod = null;
        private Member _draggedMember;
        private int _expandDirection = 0;

        public WorkItem CopyingItem => _draggingWorkItem;

        public bool IsDragging()
        {
            return _draggingWorkItem != null;
        }

        public void UpdateDraggingItem(TaskGrid grid, Point curLocation, ViewData viewData)
        {
            var callender = viewData.Original.Callender;

            if (IsExpanding())
            {
                UpdateExpand(viewData.Selected, grid, curLocation, callender);
                return;
            }

            if (IsDragging())
            {
                UpdateDragging(grid, curLocation, callender);
                return;
            }

        }

        private void UpdateDragging(TaskGrid grid, Point curLocation, Callender callender)
        {
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
            if (_expandDirection > 0)
            {
                var d = callender.ApplyOffset(selected.Period.From, offset + 1);
                if (d == null || callender.GetOffset(d, selected.Period.To) < 0) return;
                selected.Period.From = d;
            }
            else if (_expandDirection < 0)
            {
                var d = callender.ApplyOffset(selected.Period.To, offset - 1);
                if (d == null || callender.GetOffset(selected.Period.From, d) < 0) return;
                selected.Period.To = d;
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

        internal void StartDrag(WorkItem wi, Point location, TaskGrid grid)
        {
            _beforeWorkItem = wi.Serialize();
            _draggingWorkItem = wi.Clone();
            _draggedLocation = location;
            _draggedPeriod = wi.Period.Clone();
            _draggedMember = wi.AssignedMember;
            _draggedDay = grid.GetDayFromY(location.Y);
        }

        internal void End(UndoService undo, WorkItem selected)
        {
            if (!IsDragging() && !IsExpanding()) return;
            undo.Push(_beforeWorkItem, selected.Serialize());
            _draggingWorkItem = null;
            _expandDirection = 0;
        }

        private bool IsExpanding()
        {
            return _expandDirection != 0;
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

        internal void StartExpand(int direction, WorkItem selected)
        {
            _beforeWorkItem = selected.Serialize();
            _expandDirection = direction;
        }
    }
}
