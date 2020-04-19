using System;
using System.Drawing;
using System.Windows.Forms;
using TaskManagement.Model;
using TaskManagement.ViewModel;
using TaskManagement.UI;

namespace TaskManagement.Service
{
    class WorkItemDragService
    {
        private WorkItem _beforeWorkItem;
        WorkItem _draggingWorkItem = null;
        private Point _draggedLocation;
        CallenderDay _draggedDay = null;
        private int _expandDirection = 0;
        bool _isCopying = false;

        private ScrollByDragServicce _scrollByDragService = new ScrollByDragServicce();

        public WorkItem CopyingItem => _isCopying ? _draggingWorkItem : null;

        public static float ExpandHeight => 5;

        public bool IsMoving()
        {
            return _draggingWorkItem != null;
        }

        private bool IsExpanding()
        {
            return _expandDirection != 0;
        }

        public void UpdateDraggingItem(Func<int, Member> x2Member, Func<int, CallenderDay> y2Day, Point curLocation, ViewData viewData)
        {
            var callender = viewData.Original.Callender;

            if (IsExpanding())
            {
                UpdateExpand(viewData.Selected, y2Day, curLocation, callender);
                return;
            }

            if (IsMoving())
            {
                UpdateMoving(x2Member, y2Day, curLocation, callender);
                return;
            }

        }

        private void UpdateMoving(Func<int, Member> x2Member, Func<int, CallenderDay> y2Day, Point curLocation, Callender callender)
        {
            var member = GetNewMember(x2Member, curLocation);
            if (member != null)
            {
                _draggingWorkItem.AssignedMember = member;
            }
            var period = GetNewPeriod(y2Day, curLocation, callender);
            if (period != null)
            {
                _draggingWorkItem.Period = period;
            }
        }

        private Member GetNewMember(Func<int, Member> x2Member, Point curLocation)
        {
            if (IsOnlyMoveVirtical(curLocation)) return _beforeWorkItem.AssignedMember;
            return x2Member(curLocation.X);
        }

        private Period GetNewPeriod(Func<int, CallenderDay> y2Day, Point curLocation, Callender callender)
        {
            if (IsOnlyMoveHorizontal(curLocation)) return _beforeWorkItem.Period;
            var curDay = y2Day(curLocation.Y);
            if (curDay == null) return null;
            var draggedPediod = _beforeWorkItem.Period;
            var offset = callender.GetOffset(_draggedDay, curDay);
            return draggedPediod.ApplyOffset(offset, callender);
        }

        private void UpdateExpand(WorkItem selected, Func<int, CallenderDay> y2Day, Point curLocation, Callender callender)
        {
            var curDay = y2Day(curLocation.Y);
            var draggedDay = _expandDirection > 0 ? selected.Period.From : selected.Period.To;
            var otherSideDay = _expandDirection > 0 ? selected.Period.To : selected.Period.From;
            int offset = GetOffset(callender, curDay, draggedDay);
            var d = callender.ApplyOffset(draggedDay, offset + _expandDirection);
            if (d == null || IsUpsideDown(callender, otherSideDay, d)) return;
            draggedDay.CopyFrom(d);
        }

        private int GetOffset(Callender callender, CallenderDay curDay, CallenderDay draggedDay)
        {
            if (curDay == null)
            {
                return _expandDirection > 0 ? -2 : +2;
            }
            return callender.GetOffset(draggedDay, curDay);
        }

        private bool IsUpsideDown(Callender callender, CallenderDay otherSideDay, CallenderDay d)
        {
            return _expandDirection * callender.GetOffset(d, otherSideDay) < 0;
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

        internal void StartExpand(int direction, WorkItem selected)
        {
            _beforeWorkItem = selected.Clone();
            _expandDirection = direction;
        }

        internal void StartMove(WorkItem wi, Point location, CallenderDay draggedDay)
        {
            if (wi == null) return;
            _beforeWorkItem = wi.Clone();
            _draggingWorkItem = wi;
            _draggedLocation = location;
            _draggedDay = draggedDay;
        }

        public bool IsActive()
        {
            if (IsMoving()) return true;
            if (IsExpanding()) return true;
            return false;
        }

        internal void End(WorkItemEditService editService, ViewData viewData, bool isCancel)
        {
            try
            {
                if (!ExistsEdit(viewData)) return;
                var edit = BackupEdit(viewData);
                ClearEdit(viewData);
                if (isCancel) return;
                ApplyEdit(editService, viewData, edit);
            }
            finally
            {
                ClearDragState();
            }
        }

        private void ClearDragState()
        {
            _isCopying = false;
            _draggingWorkItem = null;
            _expandDirection = 0;
        }

        private void ApplyEdit(WorkItemEditService editService, ViewData viewData, WorkItem edit)
        {
            if (IsExpanding() || !_isCopying)
            {
                editService.Replace(viewData.Selected, edit);
            }
            else
            {
                editService.Add(edit);
            }
            viewData.Selected = edit;
        }

        private void ClearEdit(ViewData viewData)
        {
            if (_isCopying) viewData.Original.WorkItems.Remove(_beforeWorkItem);
            viewData.Selected.AssignedMember = _beforeWorkItem.AssignedMember;
            viewData.Selected.Period = _beforeWorkItem.Period;
        }

        private static WorkItem BackupEdit(ViewData viewData)
        {
            return viewData.Selected.Clone();
        }

        private bool ExistsEdit(ViewData viewData)
        {
            if (!IsActive()) return false;
            return !_beforeWorkItem.Equals(viewData.Selected);
        }

        internal static Tuple<PointF, PointF> GetBottomBarLine(RectangleF bounds)
        {
            var bar = GetBottomBarRect(bounds);
            return GetMidLine(bar);
        }

        internal static Tuple<PointF, PointF> GetTopBarLine(RectangleF bounds)
        {
            var bar = GetTopBarRect(bounds);
            return GetMidLine(bar);
        }

        private static Tuple<PointF, PointF> GetMidLine(RectangleF bar)
        {
            var y = (bar.Bottom + bar.Top) / 2;
            var p1 = new PointF(bar.X + bar.Width / 4, y);
            var p2 = new PointF(bar.X + bar.Width * 3 / 4, y);
            return new Tuple<PointF, PointF>(p1, p2);
        }

        internal static RectangleF GetBottomBarRect(RectangleF bounds)
        {
            return new RectangleF(bounds.X, bounds.Bottom, bounds.Width, ExpandHeight);
        }

        internal static RectangleF GetTopBarRect(RectangleF bounds)
        {
            return new RectangleF(bounds.X, bounds.Top - ExpandHeight, bounds.Width, ExpandHeight);
        }

        internal void ToCopyMode(WorkItems workItems)
        {
            if (IsExpanding()) return;
            if (!IsActive()) return;
            if (_isCopying) return;
            _isCopying = true;
            workItems.Add(_beforeWorkItem);
        }

        internal void ToMoveMode(WorkItems workItems)
        {
            if (!IsActive()) return;
            if (!_isCopying) return;
            _isCopying = false;
            workItems.Remove(_beforeWorkItem);
        }

        public bool Scroll(Point mouseLocationOnTaskGrid, WorkItemGrid wig)
        {
            return _scrollByDragService.Scroll(mouseLocationOnTaskGrid, wig);
        }
    }
}
