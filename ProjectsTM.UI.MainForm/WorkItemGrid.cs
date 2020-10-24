using FreeGridControl;
using ProjectsTM.Logic;
using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.UI.Common;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjectsTM.UI.MainForm
{
    public class WorkItemGrid : FreeGridControl.GridControl, IWorkItemGrid
    {
        private ViewData _viewData;
        private WorkItemDragService _workItemDragService = new WorkItemDragService();
        private WorkItemEditService _editService;
        private DrawService _drawService = new DrawService();
        private KeyAndMouseHandleService _keyAndMouseHandleService;
        private RowColResolver _rowColResolver;

        public WorkItemEditService EditService => _editService;

        public Size FullSize => new Size(GridWidth, GridHeight);

        public Size VisibleSize => new Size(Width, Height);

        public Size FixedSize => new Size(FixedWidth, FixedHeight);

        public Point ScrollOffset => new Point(HOffset, VOffset);

        public event EventHandler<IEditedEventArgs> UndoChanged;
        public event EventHandler<float> RatioChanged;
        public event EventHandler<WorkItem> HoveringTextChanged;
        public WorkItemGrid() { }

        internal void Initialize(ViewData viewData)
        {
            LockUpdate = true;
            if (_viewData != null) DetatchEvents();
            this._viewData = viewData;
            AttachEvents();
            this.FixedRowCount = WorkItemGridConstants.FixedRows;
            this.FixedColCount = WorkItemGridConstants.FixedCols;
            this.RowCount = _viewData.GetFilteredDays().Count + this.FixedRowCount;
            this.ColCount = _viewData.GetFilteredMembers().Count() + this.FixedColCount;
            _rowColResolver = new RowColResolver(this, _viewData);
            if (_keyAndMouseHandleService != null) _keyAndMouseHandleService.Dispose();
            _keyAndMouseHandleService = new KeyAndMouseHandleService(_viewData, this, _workItemDragService, _drawService, _editService, this);
            _keyAndMouseHandleService.HoveringTextChanged += _keyAndMouseHandleService_HoveringTextChanged;
            ApplyDetailSetting();
            _editService = new WorkItemEditService(_viewData);
            LockUpdate = false;
            if (_drawService != null) _drawService.Dispose();
            _drawService.Initialize(
                _viewData,
                this,
                () => _workItemDragService.IsActive(),
                this.Font);
        }

        private void _keyAndMouseHandleService_HoveringTextChanged(object sender, WorkItem e)
        {
            this.HoveringTextChanged?.Invoke(sender, e);
        }

        public IEnumerable<Member> GetNeighbers(IEnumerable<Member> members)
        {
            var neighbers = new HashSet<Member>();
            foreach (var m in members)
            {
                var c = Member2Col(m);
                var l = Col2Member(new ColIndex(c.Value - 1));
                var r = Col2Member(new ColIndex(c.Value + 1));

                neighbers.Add(m);
                if (l != null) neighbers.Add(l);
                if (r != null) neighbers.Add(r);
            }
            return neighbers;
        }

        private void ApplyDetailSetting()
        {
            var font = FontCache.GetFont(this.Font.FontFamily, _viewData.FontSize, false);
            var g = this.CreateGraphics();
            var calWidth = (int)Math.Ceiling(g.MeasureString("2000A12A31", font).Width);
            var memberHeight = (int)Math.Ceiling(g.MeasureString("NAME", font).Height);
            var height = memberHeight;
            var width = (int)(Math.Ceiling(this.CreateGraphics().MeasureString("ABCDEF", font).Width) + 1);
            this.ColWidths[WorkItemGridConstants.YearCol.Value] = (int)(calWidth / 2f) + 1;
            this.ColWidths[WorkItemGridConstants.MonthCol.Value] = (int)(calWidth / 4f) + 1;
            this.ColWidths[WorkItemGridConstants.DayCol.Value] = (int)(calWidth / 4f) + 1;
            for (var c = FixedColCount; c < ColCount; c++)
            {
                this.ColWidths[c] = width;
            }
            this.RowHeights[WorkItemGridConstants.CompanyRow.Value] = memberHeight;
            this.RowHeights[WorkItemGridConstants.LastNameRow.Value] = memberHeight;
            this.RowHeights[WorkItemGridConstants.FirstNameRow.Value] = memberHeight;
            for (var r = FixedRowCount; r < RowCount; r++)
            {
                this.RowHeights[r] = height;
            }
        }

        private void _undoService_Changed(object sender, IEditedEventArgs e)
        {
            UndoChanged?.Invoke(this, e);
            _drawService.InvalidateMembers(e.UpdatedMembers);
            this.Invalidate();
        }

        public ColIndex Member2Col(Member m)
        {
            return Member2Col(m, _viewData.GetFilteredMembers());
        }

        public Rectangle? GetMemberDrawRect(Member m)
        {
            var col = Member2Col(m, _viewData.GetFilteredMembers());
            var rect = GetRectRaw(col, VisibleNormalTopRow, 1);
            if (!rect.HasValue) return null;
            return new Rectangle(rect.Value.X, FixedHeight, ColWidths[col.Value], GridHeight);
        }

        private void AttachEvents()
        {
            this._viewData.SelectedWorkItemChanged += _viewData_SelectedWorkItemChanged;
            this.OnDrawNormalArea += WorkItemGrid_OnDrawNormalArea;
            this.MouseDown += WorkItemGrid_MouseDown;
            this.MouseUp += WorkItemGrid_MouseUp;
            this.MouseDoubleClick += WorkItemGrid_MouseDoubleClick;
            this.MouseWheel += WorkItemGrid_MouseWheel;
            this._viewData.UndoService.Changed += _undoService_Changed;
            this.MouseMove += WorkItemGrid_MouseMove;
            this.KeyDown += WorkItemGrid_KeyDown;
            this.KeyUp += WorkItemGrid_KeyUp;
        }

        private void DetatchEvents()
        {
            this._viewData.SelectedWorkItemChanged -= _viewData_SelectedWorkItemChanged;
            this.OnDrawNormalArea -= WorkItemGrid_OnDrawNormalArea;
            this.MouseDown -= WorkItemGrid_MouseDown;
            this.MouseUp -= WorkItemGrid_MouseDown;
            this.MouseDoubleClick -= WorkItemGrid_MouseDoubleClick;
            this.MouseWheel -= WorkItemGrid_MouseWheel;
            this._viewData.UndoService.Changed -= _undoService_Changed;
            this.MouseMove -= WorkItemGrid_MouseMove;
            this.KeyDown -= WorkItemGrid_KeyDown;
            this.KeyUp -= WorkItemGrid_KeyUp;
        }

        private void WorkItemGrid_MouseWheel(object sender, MouseEventArgs e)
        {
            _keyAndMouseHandleService.MouseWheel(e);
        }

        private void WorkItemGrid_KeyDown(object sender, KeyEventArgs e)
        {
            _keyAndMouseHandleService.KeyDown(e);
            this.Invalidate();
        }

        private void WorkItemGrid_KeyUp(object sender, KeyEventArgs e)
        {
            _keyAndMouseHandleService.KeyUp(e);
            this.Invalidate();
        }

        private void WorkItemGrid_MouseUp(object sender, MouseEventArgs e)
        {
            _keyAndMouseHandleService.MouseUp();
        }

        public ClientRectangle? GetRangeSelectBound()
        {
            if (_workItemDragService.State != DragState.RangeSelect) return null;
            var p1 = this.PointToClient(Cursor.Position);
            var p2 = Raw2Client(_workItemDragService.DragedLocation);
            return Point2Rect.GetRectangle(p1, p2);
        }

        internal WorkItem GetUniqueSelect()
        {
            if (_viewData.Selected == null) return null;
            if (_viewData.Selected.Count() != 1) return null;
            return _viewData.Selected.Unique;
        }

        internal void Divide()
        {
            var selected = GetUniqueSelect();
            if (selected == null) return;
            var count = _viewData.Original.Callender.GetPeriodDayCount(selected.Period);
            using (var dlg = new DivideWorkItemForm(count))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                EditService.Divide(selected, dlg.Divided, dlg.Remain);
            }
        }

        private void WorkItemGrid_MouseMove(object sender, MouseEventArgs e)
        {
            _keyAndMouseHandleService.MouseMove(ClientPoint.Create(e), this);
            this.Invalidate();
        }

        private void WorkItemGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _keyAndMouseHandleService.DoubleClick(e);
        }

        public void AddNewWorkItem(WorkItem proto)
        {
            using (var dlg = new EditWorkItemForm(proto, _viewData.Original.WorkItems, _viewData.Original.Callender, _viewData.GetFilteredMembers()))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var wi = dlg.GetWorkItem();
                _viewData.UpdateCallenderAndMembers(wi);
                _editService.Add(wi);
                _viewData.UndoService.Push();
            }
        }

        public void EditSelectedWorkItem()
        {
            var wi = GetUniqueSelect();
            if (wi == null) return;
            using (var dlg = new EditWorkItemForm(wi.Clone(), _viewData.Original.WorkItems, _viewData.Original.Callender, _viewData.GetFilteredMembers()))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var newWi = dlg.GetWorkItem();
                _viewData.UpdateCallenderAndMembers(newWi);
                _editService.Replace(wi, newWi);
                _viewData.Selected = new WorkItems(newWi);
            }
        }
        private WorkItem _copiedWorkItem;

        public void CopyWorkItem()
        {
            if (_viewData.Selected == null) return;
            if (_viewData.Selected.Count() != 1) return;

            _copiedWorkItem = _viewData.Selected.Unique.Clone();
        }
        public void PasteWorkItem()
        {
            if (_copiedWorkItem == null) return;

            var selectedDay = CurrentCallenderDay();
            if (selectedDay == null) return;

            var selectedMember = CurrentMember();
            if (selectedMember == null) return;

            var copyItem = _copiedWorkItem.Clone();
            var offset = _viewData.Original.Callender.GetOffset(copyItem.Period.From, selectedDay);
            copyItem.Period = copyItem.Period.ApplyOffset(offset, _viewData.Original.Callender);
            if (copyItem.Period == null) return;

            copyItem.AssignedMember = selectedMember;

            _viewData.UpdateCallenderAndMembers(copyItem);
            _editService.Add(copyItem);
            _viewData.UndoService.Push();
        }

        public RawPoint CurrentRawPoint()
        {
            var point = PointToClient(Cursor.Position);
            var client = new ClientPoint(point);

            return Client2Raw(client);
        }

        public CallenderDay CurrentCallenderDay()
        {
            var point = PointToClient(Cursor.Position);
            var client = new ClientPoint(point);
            if (IsFixedArea(client)) return null;

            var rawPoint = Client2Raw(client);
            return Y2Day(rawPoint.Y);
        }

        public Member CurrentMember()
        {
            var point = PointToClient(Cursor.Position);
            var client = new ClientPoint(point);
            if (IsFixedArea(client)) return null;

            var rawPoint = Client2Raw(client);
            return X2Member(rawPoint.X);
        }


        private void _viewData_SelectedWorkItemChanged(object sender, SelectedWorkItemChangedArg e)
        {
            _drawService.InvalidateMembers(e.UpdatedMembers);
            var wi = GetUniqueSelect();
            if (wi != null)
            {
                var rowRange = GetRowRange(wi);
                MoveVisibleRowColRange(rowRange.Row, rowRange.Count, Member2Col(wi.AssignedMember, _viewData.GetFilteredMembers()));
            }
            this.Invalidate();
        }

        private void MoveVisibleDayAndMember(CallenderDay day, Member m)
        {
            if (day == null || m == null) return;
            var row = Day2Row(day);
            if (row == null) return;
            MoveVisibleRowCol(row, Member2Col(m, _viewData.GetFilteredMembers()));
        }

        private void WorkItemGrid_MouseDown(object sender, MouseEventArgs e)
        {
            _keyAndMouseHandleService.MouseDown(e);
        }

        public CallenderDay Y2Day(int y)
        {
            if (GridHeight < y) return null;
            var r = Y2Row(y);
            return Row2Day(r);
        }

        public CallenderDay Row2Day(RowIndex r)
        {
            return _rowColResolver.Row2Day(r);
        }

        public Member X2Member(int x)
        {
            if (GridWidth < x) return null;
            var c = X2Col(x);
            return Col2Member(c);
        }

        public Member Col2Member(ColIndex c)
        {
            return _rowColResolver.Col2Member(c);
        }

        internal void MoveToToday()
        {
            var wi = GetUniqueSelect();
            var m = wi != null ? wi.AssignedMember : X2Member(FixedWidth);
            var now = DateTime.Now;
            var today = new CallenderDay(now.Year, now.Month, now.Day);
            MoveVisibleDayAndMember(today, m);
        }

        private void WorkItemGrid_OnDrawNormalArea(object sender, DrawNormalAreaEventArgs e)
        {
            _drawService.Draw(e.Graphics, e.IsAllDraw);
        }

        public void DecRatio()
        {
            _viewData.DecRatio();
            RatioChanged?.Invoke(this, _viewData.Detail.ViewRatio);
        }

        public void IncRatio()
        {
            _viewData.IncRatio();
            RatioChanged?.Invoke(this, _viewData.Detail.ViewRatio);
        }

        public RawRectangle? GetWorkItemDrawRectRaw(WorkItem wi, IEnumerable<Member> members)
        {
            var rowRange = GetRowRange(wi);
            if (rowRange.Row == null) return null;
            return GetRectRaw(Member2Col(wi.AssignedMember, members), rowRange.Row, rowRange.Count);
        }

        public ClientRectangle? GetWorkItemDrawRectClient(WorkItem wi, IEnumerable<Member> members)
        {
            var rowRange = GetRowRange(wi);
            if (rowRange.Row == null) return null;
            return GetRectClient(Member2Col(wi.AssignedMember, members), rowRange.Row, rowRange.Count, GetVisibleRect(false, false));
        }

        private ColIndex Member2Col(Member m, IEnumerable<Member> members)
        {
            return _rowColResolver.Member2Col(m, members);
        }

        private RowRange GetRowRange(WorkItem wi)
        {
            RowIndex row = null;
            int count = 0;
            foreach (var d in _viewData.Original.Callender.GetPediodDays(wi.Period))
            {
                if (!_viewData.GetFilteredDays().Contains(d)) continue;
                if (row == null)
                {
                    row = Day2Row(d);
                }
                count++;
            }
            return new RowRange(row, count);
        }

        internal void Redo()
        {
            _viewData.UndoService.Redo(_viewData);
        }

        internal void Undo()
        {
            _viewData.UndoService.Undo(_viewData);
        }

        private RowIndex Day2Row(CallenderDay day)
        {
            return _rowColResolver.Day2Row(day);
        }

        public bool IsSelected(Member m)
        {
            if (_viewData.Selected == null) return false;
            return _viewData.Selected.Any(w => w.AssignedMember.Equals(m));
        }

        public bool IsSelected(CallenderDay d)
        {
            if (_viewData.Selected == null) return false;
            return _viewData.Selected.Any(w => w.Period.Contains(d));
        }

        public WorkItem PickWorkItemFromPoint(RawPoint location)
        {
            var m = X2Member(location.X);
            var d = Y2Day(location.Y);
            if (m == null || d == null) return null;
            return _viewData.PickFilterdWorkItem(m, d);
        }

        public WorkItem EmptyWorkItem(RawPoint location)
        {
            var m = X2Member(location.X);
            var d = Y2Day(location.Y);
            if (m == null || d == null) return null;
            return WorkItem.CreateProto(new Period(d, d), m);
        }
    }
}