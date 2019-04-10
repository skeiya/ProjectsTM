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

        public bool IsDragging()
        {
            return _draggingWorkItem != null;
        }

        public void UpdateDraggingItem(TaskGrid grid, Point curLocation, Callender callender)
        {
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
        }
    }
}
