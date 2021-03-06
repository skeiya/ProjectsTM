﻿using FreeGridControl;
using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjectsTM.Service
{
    using RawPoint = FreeGridControl.RawPoint;

    public class WorkItemDragService
    {
        private SelectedWorkItems _backup;
        private DragStartInfo _dragStartInfo;
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

        public static int ExpandHeight => 5;

        public DragStartInfo DragStartInfo => _dragStartInfo;

        public void UpdateDraggingItem(IWorkItemGrid grid, RawPoint curLocation, ViewData viewData)
        {
            var callender = viewData.Original.Callender;

            switch (State)
            {
                case DragState.BeforeMoving:
                    {
                        var enoughDistance = Math.Pow(_dragStartInfo.Location.X - curLocation.X, 2) + Math.Pow(_dragStartInfo.Location.Y - curLocation.Y, 2) > 25;
                        if (enoughDistance) State = DragState.Moving;
                        break;
                    }
                case DragState.BeforeExpanding:
                    {
                        var enoughDistance = Math.Pow(_dragStartInfo.Location.X - curLocation.X, 2) + Math.Pow(_dragStartInfo.Location.Y - curLocation.Y, 2) > 25;
                        if (enoughDistance) State = DragState.Expanding;
                        break;
                    }
                case DragState.BeforeRangeSelect:
                    {
                        var enoughDistance = Math.Pow(_dragStartInfo.Location.X - curLocation.X, 2) + Math.Pow(_dragStartInfo.Location.Y - curLocation.Y, 2) > 25;
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
                if (!TryGetNewPeriod(w.Period, pOffset, callender, out var _)) return;
            }
            foreach (var w in result)
            {
                var member = GetNewMember(grid, w.AssignedMember, mOffset);
                if (member != null)
                {
                    w.AssignedMember = member;
                }
                if(TryGetNewPeriod(w.Period, pOffset, callender, out var period))
                {
                    w.Period = period;
                }
            }
            viewData.Original.WorkItems.Remove(viewData.Selected);
            viewData.Original.WorkItems.Add(result);
            viewData.Selected.Set(result);
        }

        private static bool TryGetNewPeriod(Period period, int pOffset, Callender cal, out Period result)
        {
            return period.TryApplyOffset(pOffset, cal, out result);
        }

        private int GetPeriodOffset(IWorkItemGrid grid, RawPoint curLocation)
        {
            if (IsOnlyMoveHorizontal(curLocation)) return 0;
            var dragged = grid.Y2Row(_dragStartInfo.Location.Y);
            var cur = grid.Y2Row(curLocation.Y);
            return cur.Value - dragged.Value;
        }

        private int GetMemberOffset(IWorkItemGrid grid, RawPoint curLocation)
        {
            if (IsOnlyMoveVirtical(curLocation)) return 0;
            var dragged = grid.X2Col(_dragStartInfo.Location.X);
            var cur = grid.X2Col(curLocation.X);
            return cur.Value - dragged.Value;
        }

        public static bool IsCurLocationOnHitArea(IWorkItemGrid grid, RawPoint curLocation)
        {
            var draggedCol = grid.X2Col(curLocation.X);
            var draggedRow = grid.Y2Row(curLocation.Y);
            if (draggedCol.Value < 0 || draggedRow.Value < 0) return false;

            var rect = grid.GetRectRaw(draggedCol, draggedRow, 1);
            if (curLocation.X < rect.Value.X + rect.Value.Width * 0.25) return false;
            if (curLocation.X > rect.Value.X + rect.Value.Width * 0.75) return false;
            return true;
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
                if (!callender.TryApplyOffset(manipDay, offset, out var newDay)) return;
                if (IsUpsideDown(callender, isExpandingFrom ? w.Period.To : w.Period.From, newDay)) return;
            }
            foreach (var w in result)
            {
                var manipDay = isExpandingFrom ? w.Period.From : w.Period.To;
                if (!callender.TryApplyOffset(manipDay, offset, out var newDay)) return;
                manipDay.CopyFrom(newDay);
            }
            viewData.Original.WorkItems.Remove(viewData.Selected);
            viewData.Original.WorkItems.Add(result);
            viewData.Selected.Set(result);
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
            return !IsVirticalLong(_dragStartInfo.Location, curLocation);
        }

        private bool IsOnlyMoveVirtical(RawPoint curLocation)
        {
            if (!IsShiftDown()) return false;
            return IsVirticalLong(_dragStartInfo.Location, curLocation);
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

        internal void StartExpand(int direction, SelectedWorkItems selected, CallenderDay callenderDay)
        {
            _backup = selected.Clone();
            _expandDirection = direction;
            _draggedDay = callenderDay;
            State = DragState.BeforeExpanding;
        }

        internal void StartMove(SelectedWorkItems selected, IEnumerable<ClientRectangle> rects, RawPoint location, CallenderDay draggedDay)
        {
            _backup = selected.Clone();
            _dragStartInfo = new DragStartInfo(location, rects);
            _draggedDay = draggedDay;

            State = DragState.BeforeMoving;
        }

        internal void StartRangeSelect(RawPoint location)
        {
            _dragStartInfo = new DragStartInfo(location, null);
            State = DragState.BeforeRangeSelect;
        }

        public bool IsActive()
        {
            return State != DragState.None;
        }

        public bool IsMoveing()
        {
            return State == DragState.Moving;
        }

        internal void End(WorkItemEditService editService, ViewData viewData, bool isCancel, Action rangeSelect)
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
                    if (!isCancel && rangeSelect != null)
                    {
                        rangeSelect();
                    }
                    ClearDragState();
                    return;
            }
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
            _expandDirection = 0;
            State = DragState.None;
        }

        private void ApplyEdit(WorkItemEditService editService, ViewData viewData, IEnumerable<WorkItem> edit)
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
            viewData.Selected.Set(edit);
        }

        private void ClearEdit(ViewData viewData)
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

        private static SelectedWorkItems BackupEdit(ViewData viewData)
        {
            return viewData.Selected.Clone();
        }

        private bool ExistsEdit(ViewData viewData)
        {
            if (!IsActive()) return false;
            return !_backup.Equals(viewData.Selected);
        }

        internal static Tuple<PointF, PointF> GetBottomBarLine(ClientRectangle bounds)
        {
            var bar = GetBottomBarRect(bounds);
            return GetMidLine(bar);
        }

        internal static Tuple<PointF, PointF> GetTopBarLine(ClientRectangle bounds)
        {
            var bar = GetTopBarRect(bounds);
            return GetMidLine(bar);
        }

        private static Tuple<PointF, PointF> GetMidLine(ClientRectangle bar)
        {
            var y = (bar.Bottom + bar.Top) / 2;
            var p1 = new PointF(bar.X + bar.Width / 4, y);
            var p2 = new PointF(bar.X + bar.Width * 3 / 4, y);
            return new Tuple<PointF, PointF>(p1, p2);
        }

        internal static ClientRectangle GetBottomBarRect(ClientRectangle bounds)
        {
            return new ClientRectangle(bounds.X, bounds.Bottom, bounds.Width, ExpandHeight);
        }

        internal static ClientRectangle GetTopBarRect(ClientRectangle bounds)
        {
            return new ClientRectangle(bounds.X, bounds.Top - ExpandHeight, bounds.Width, ExpandHeight);
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

        internal void StartCopy(MainViewData viewData, IEnumerable<ClientRectangle> rects, RawPoint location, CallenderDay draggedDay, Action<IEnumerable<Member>> invalidateMembers)
        {
            StartMove(viewData.Selected, rects, location, draggedDay);
            ToCopyMode(viewData.Original.WorkItems, invalidateMembers);
        }
    }
}
