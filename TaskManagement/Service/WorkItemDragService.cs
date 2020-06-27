using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TaskManagement.Model;
using TaskManagement.UI;
using TaskManagement.ViewModel;

namespace TaskManagement.Service
{
    using RawPoint = FreeGridControl.GridControl.RawPoint;

    class WorkItemDragService
    {
        private WorkItems _backup;
        private RawPoint _draggedLocation;
        private CallenderDay _draggedDay = null;
        private int _expandDirection = 0;

        private DragState _state = DragState.None;
        public DragState State
        {
            get
            {
                return _state;
            }
            private set
            {
                _state = value;
            }
        }

        public static float ExpandHeight => 5;

        public RawPoint DragedLocation => _draggedLocation;

        public void UpdateDraggingItem(IWorkItemGrid grid,  RawPoint curLocation, ViewData viewData)
        {
            var callender = viewData.Original.Callender;

            switch (State)
            {
                case DragState.BeforeMoving:
                    {
                        var enoughDistance = Math.Pow(_draggedLocation.X - curLocation.X, 2) + Math.Pow(_draggedLocation.Y - curLocation.Y, 2) > 25;
                        if (enoughDistance) State = DragState.Moving;
                        break;
                    }
                case DragState.BeforeExpanding:
                    {
                        var enoughDistance = Math.Pow(_draggedLocation.X - curLocation.X, 2) + Math.Pow(_draggedLocation.Y - curLocation.Y, 2) > 25;
                        if (enoughDistance) State = DragState.Expanding;
                        break;
                    }
                case DragState.BeforeRangeSelect:
                    {
                        var enoughDistance = Math.Pow(_draggedLocation.X - curLocation.X, 2) + Math.Pow(_draggedLocation.Y - curLocation.Y, 2) > 25;
                        if (enoughDistance) State = DragState.RangeSelect;
                        break;
                    }
            }

            switch (State)
            {
                case DragState.Expanding:
                    UpdateExpand(viewData, grid, curLocation, callender);
                    return;
                case DragState.Moving:
                case DragState.Copying:
                    UpdateMoving(grid, curLocation, callender, viewData);
                    return;
                default:
                    return;
            }
        }

        private void UpdateMoving(IWorkItemGrid grid, RawPoint curLocation, Callender callender, ViewData viewData)
        {
            var mOffset = GetMemberOffset(grid, curLocation);
            var pOffset = GetPeriodOffset(grid, curLocation);
            var result = _backup.Clone();
            foreach (var w in result)
            {
                if (null == GetNewMember(grid, w.AssignedMember, mOffset)) return;
                if (null == GetNewPeriod(w.Period, pOffset, callender)) return;
            }
            foreach (var w in result)
            {
                var member = GetNewMember(grid, w.AssignedMember, mOffset);
                if (member != null)
                {
                    w.AssignedMember = member;
                }
                var period = GetNewPeriod(w.Period, pOffset, callender);
                if (period != null)
                {
                    w.Period = period;
                }
            }
            viewData.Original.WorkItems.Remove(viewData.Selected);
            viewData.Original.WorkItems.Add(result);
            viewData.Selected = result;
        }

        private static Period GetNewPeriod(Period period, int pOffset, Callender cal)
        {
            return period.ApplyOffset(pOffset, cal);
        }

        private int GetPeriodOffset(IWorkItemGrid grid, RawPoint curLocation)
        {
            if (IsOnlyMoveHorizontal(curLocation)) return 0;
            var dragged = grid.Y2Row(_draggedLocation.Y);
            var cur = grid.Y2Row(curLocation.Y);
            return cur.Value - dragged.Value;
        }

        private int GetMemberOffset(IWorkItemGrid grid, RawPoint curLocation)
        {
            if (IsOnlyMoveVirtical(curLocation)) return 0;
            var dragged = grid.X2Col(_draggedLocation.X);
            var cur = grid.X2Col(curLocation.X);
            return cur.Value - dragged.Value;
        }

        private static Member GetNewMember(IWorkItemGrid grid, Member before, int offset)
        {
            var newCol = grid.Member2Col(before).Offset(offset);
            return grid.Col2Member(newCol);
        }

        private void UpdateExpand(ViewData viewData, IWorkItemGrid grid, RawPoint curLocation, Callender callender)
        {
            var curDay = grid.Y2Day(curLocation.Y);
            var isExpandingFrom = _expandDirection > 0;
            int offset = GetOffset(callender, curDay, _draggedDay);
            var result = _backup.Clone();
            foreach (var w in result)
            {
                var manipDay = isExpandingFrom ? w.Period.From : w.Period.To;
                var newDay = callender.ApplyOffset(manipDay, offset);
                if (null == newDay) return;
                if (IsUpsideDown(callender, isExpandingFrom ? w.Period.To : w.Period.From, newDay)) return;
            }
            foreach (var w in result)
            {
                var manipDay = isExpandingFrom ? w.Period.From : w.Period.To;
                var newDay = callender.ApplyOffset(manipDay, offset);
                manipDay.CopyFrom(newDay);
            }
            viewData.Original.WorkItems.Remove(viewData.Selected);
            viewData.Original.WorkItems.Add(result);
            viewData.Selected = result;
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

        private bool IsOnlyMoveHorizontal(RawPoint curLocation)
        {
            if (!IsShiftDown()) return false;
            return !IsVirticalLong(_draggedLocation, curLocation);
        }

        private bool IsOnlyMoveVirtical(RawPoint curLocation)
        {
            if (!IsShiftDown()) return false;
            return IsVirticalLong(_draggedLocation, curLocation);
        }

        private static bool IsVirticalLong(RawPoint a, RawPoint b)
        {
            var h = Math.Abs(a.Y - b.Y);
            var w = Math.Abs(a.X - b.X);
            return w < h;
        }

        private static bool IsShiftDown()
        {
            return (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
        }

        internal void StartExpand(int direction, WorkItems selected, CallenderDay callenderDay)
        {
            _backup = selected.Clone();
            _expandDirection = direction;
            _draggedDay = callenderDay;
            State = DragState.BeforeExpanding;
        }

        internal void StartMove(WorkItems selected, RawPoint location, CallenderDay draggedDay)
        {
            if (selected == null) return;
            _backup = selected.Clone();
            _draggedLocation = location;
            _draggedDay = draggedDay;
            State = DragState.BeforeMoving;
        }

        internal void StartRangeSelect(RawPoint location)
        {
            _draggedLocation = location;
            State = DragState.BeforeRangeSelect;
        }

        public bool IsActive()
        {
            return State != DragState.None;
        }

        internal void End(WorkItemEditService editService, ViewData viewData, bool isCancel, Action RangeSelect, WorkItemGrid grid = null)
        {
            switch (State)
            {
                case DragState.None:
                case DragState.BeforeExpanding:
                case DragState.BeforeMoving:
                case DragState.BeforeRangeSelect:
                    ClearDragState();
                    return;
                case DragState.RangeSelect:
                    if (!isCancel && RangeSelect != null)
                    {
                        RangeSelect();
                    }
                    ClearDragState();
                    return;
            }
            try
            {
                if (!ExistsEdit(viewData)) return;
                var edit = BackupEdit(viewData);
                ClearEdit(viewData, grid);
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
            _expandDirection = 0;
            State = DragState.None;
        }

        private void ApplyEdit(WorkItemEditService editService, ViewData viewData, WorkItems edit)
        {
            switch (State)
            {
                case DragState.Expanding:
                case DragState.Moving:
                    editService.Replace(_backup, edit);
                    break;
                case DragState.Copying:
                    editService.Add(edit);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            viewData.Selected = edit;
        }

        private void ClearEdit(ViewData viewData, WorkItemGrid grid)
        {
            switch (State)
            {
                case DragState.Copying:
                    viewData.Original.WorkItems.Remove(_backup);
                    break;
                default:
                    break;
            }
            viewData.Original.WorkItems.Remove(viewData.Selected);
            viewData.Original.WorkItems.Add(_backup);
        }

        private static WorkItems BackupEdit(ViewData viewData)
        {
            return viewData.Selected.Clone();
        }

        private bool ExistsEdit(ViewData viewData)
        {
            if (!IsActive()) return false;
            return !_backup.Equals(viewData.Selected);
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

        internal void ToCopyMode(WorkItems workItems, Action<IEnumerable<Member>> invalidateMembers)
        {
            if (State != DragState.Moving) return;
            State = DragState.Copying;
            workItems.Add(_backup);
            invalidateMembers(_backup.Select(w => w.AssignedMember));
        }

        internal void ToMoveMode(WorkItems workItems, Action<IEnumerable<Member>> invalidateMembers)
        {
            if (State != DragState.Copying) return;
            State = DragState.Moving;
            workItems.Remove(_backup);
            invalidateMembers(_backup.Select(w => w.AssignedMember));
        }

        internal void StartCopy(ViewData viewData, RawPoint location, CallenderDay draggedDay, Action<IEnumerable<Member>> invalidateMembers)
        {
            StartMove(viewData.Selected, location, draggedDay);
            ToCopyMode(viewData.Original.WorkItems, invalidateMembers);
        }
    }
}
