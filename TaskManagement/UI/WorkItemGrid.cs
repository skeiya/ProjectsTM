using FreeGridControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TaskManagement.Model;
using TaskManagement.Service;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public class WorkItemGrid : FreeGridControl.GridControl, IWorkItemGrid
    {
        private ViewData _viewData;

        private WorkItemDragService _workItemDragService = new WorkItemDragService();
        private UndoService _undoService = new UndoService();
        private WorkItemEditService _editService;
        private Cursor _originalCursor;

        public WorkItemEditService EditService => _editService;

        public SizeF FullSize => new SizeF(GridWidth, GridHeight);

        public Size VisibleSize => new Size(Width, Height);

        public SizeF FixedSize => new SizeF(FixedWidth, FixedHeight);

        public Point ScrollOffset => new Point(HOffset, VOffset);

        public RowColRange VisibleRowColRange => new RowColRange(VisibleNormalLeftCol, VisibleNormalTopRow, VisibleNormalColCount, VisibleNormalRowCount);

        public event EventHandler<EditedEventArgs> UndoChanged;

        public event EventHandler<string> HoveringTextChanged;
        public event EventHandler<float> RatioChanged;

        private Dictionary<CallenderDay, RowIndex> _day2RowCache = new Dictionary<CallenderDay, RowIndex>();
        private Dictionary<RowIndex, CallenderDay> _row2DayChache = new Dictionary<RowIndex, CallenderDay>();
        private Dictionary<Member, ColIndex> _member2ColChache = new Dictionary<Member, ColIndex>();
        private Dictionary<ColIndex, Member> _col2MemberChache = new Dictionary<ColIndex, Member>();
        public WorkItemGrid() { }

        internal void Initialize(ViewData viewData)
        {
            LockUpdate = true;
            if (_viewData != null) DetatchEvents();
            this._viewData = viewData;
            AttachEvents();
            var fixedRows = 3;
            var fixedCols = 3;
            this.RowCount = _viewData.GetFilteredDays().Count + fixedRows;
            this.ColCount = _viewData.GetFilteredMembers().Count + fixedCols;
            this.FixedRowCount = fixedRows;
            this.FixedColCount = fixedCols;

            ApplyDetailSetting(_viewData.Detail);

            _editService = new WorkItemEditService(_viewData, _undoService);

            LockUpdate = false;
            UpdateCache();
        }

        DrawService _drawService;
        private void UpdateCache()
        {
            _day2RowCache.Clear();
            _row2DayChache.Clear();
            _member2ColChache.Clear();
            _col2MemberChache.Clear();
            RefreshDraw();
        }

        public void RefreshDraw()
        {
            if (_drawService != null)
            {
                _drawService.Dispose();
                _drawService = null;
            }
            _drawService = new DrawService(
                _viewData,
                this,
                () => _workItemDragService.IsActive(),
                this.Font);
            this.Invalidate();
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

        private void _viewData_FilterChanged(object sender, EventArgs e)
        {
            UpdateCache();
        }

        internal void AdjustForPrint(Rectangle printRect)
        {
            var vRatio = printRect.Height / (float)GridHeight;
            var hRatio = printRect.Width / (float)GridWidth;
            LockUpdate = true;
            for (var c = 0; c < ColCount; c++)
            {
                ColWidths[c] = (ColWidths[c] * hRatio);
            }
            for (var r = 0; r < RowCount; r++)
            {
                RowHeights[r] = (RowHeights[r] * vRatio);
            }
            LockUpdate = false;
        }

        private void ApplyDetailSetting(Detail detail)
        {
            this.ColWidths[0] = detail.DateWidth / 2;
            this.ColWidths[1] = detail.DateWidth / 4;
            this.ColWidths[2] = detail.DateWidth / 4;
            for (var c = FixedColCount; c < ColCount; c++)
            {
                this.ColWidths[c] = detail.ColWidth;
            }
            this.RowHeights[0] = detail.CompanyHeight;
            this.RowHeights[1] = detail.NameHeight;
            this.RowHeights[2] = detail.NameHeight;
            for (var r = FixedRowCount; r < RowCount; r++)
            {
                this.RowHeights[r] = detail.RowHeight;
            }
        }

        private void _undoService_Changed(object sender, EditedEventArgs e)
        {
            UndoChanged?.Invoke(this, e);
            _drawService.InvalidateMembers(e.UpdatedMembers);
            this.Invalidate();
        }

        public ColIndex Member2Col(Member m)
        {
            return Member2Col(m, _viewData.GetFilteredMembers());
        }

        public RectangleF? GetMemberDrawRect(Member m)
        {
            var col = Member2Col(m, _viewData.GetFilteredMembers());
            var rect = GetRect(col, VisibleNormalTopRow, 1, false, false, false);
            if (!rect.HasValue) return null;
            return new RectangleF(rect.Value.X, FixedHeight, ColWidths[col.Value], GridHeight);
        }

        private void AttachEvents()
        {
            this._viewData.SelectedWorkItemChanged += _viewData_SelectedWorkItemChanged;
            this._viewData.FontChanged += _viewData_FontChanged;
            this._viewData.FilterChanged += _viewData_FilterChanged;
            this.OnDrawNormalArea += WorkItemGrid_OnDrawNormalArea;
            this.MouseDown += WorkItemGrid_MouseDown;
            this.MouseUp += WorkItemGrid_MouseUp;
            this.MouseDoubleClick += WorkItemGrid_MouseDoubleClick;
            this.MouseWheel += WorkItemGrid_MouseWheel;
            this._undoService.Changed += _undoService_Changed;
            this.MouseMove += WorkItemGrid_MouseMove;
            this.KeyDown += WorkItemGrid_KeyDown;
            this.KeyUp += WorkItemGrid_KeyUp;
        }

        private void DetatchEvents()
        {
            this._viewData.SelectedWorkItemChanged -= _viewData_SelectedWorkItemChanged;
            this._viewData.FontChanged -= _viewData_FontChanged;
            this._viewData.FilterChanged -= _viewData_FilterChanged;
            this.OnDrawNormalArea -= WorkItemGrid_OnDrawNormalArea;
            this.MouseDown -= WorkItemGrid_MouseDown;
            this.MouseUp -= WorkItemGrid_MouseDown;
            this.MouseDoubleClick -= WorkItemGrid_MouseDoubleClick;
            this.MouseWheel -= WorkItemGrid_MouseWheel;
            this._undoService.Changed -= _undoService_Changed;
            this.MouseMove -= WorkItemGrid_MouseMove;
            this.KeyDown -= WorkItemGrid_KeyDown;
        }

        private void _viewData_FontChanged(object sender, EventArgs e)
        {
            RefreshDraw();
        }

        private void WorkItemGrid_MouseWheel(object sender, MouseEventArgs e)
        {
            if (IsControlDown())
            {
                if (e.Delta > 0)
                {
                    IncRatio();
                }
                else
                {
                    DecRatio();
                }
            }
        }

        private void WorkItemGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _workItemDragService.ToCopyMode(_viewData.Original.WorkItems, _drawService.InvalidateMembers);
            }
            if (e.KeyCode == Keys.Escape)
            {
                _workItemDragService.End(_editService, _viewData, true, null);
                _viewData.Selected = null;
            }
            this.Invalidate();
        }

        private void WorkItemGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (_viewData.Selected == null) return;
                _editService.Delete();
                _viewData.Selected = null;
            }
            if (e.KeyCode == Keys.ControlKey)
            {
                _workItemDragService.ToMoveMode(_viewData.Original.WorkItems, _drawService.InvalidateMembers);
            }
            this.Invalidate();
        }

        private void WorkItemGrid_MouseUp(object sender, MouseEventArgs e)
        {
            using (new RedrawLock(_drawService, () => this.Invalidate()))
            {
                _workItemDragService.End(_editService, _viewData, false, RangeSelect);
            }
        }

        public Rectangle? GetRangeSelectBound()
        {
            if (_workItemDragService.State != DragState.RangeSelect) return null;
            var p1 = this.PointToClient(Cursor.Position);
            var p2 = _workItemDragService.DragedLocation;
            return GetRectangle(p1, p2);
        }

        void RangeSelect()
        {
            var range = GetRangeSelectBound();
            if (!range.HasValue) return;
            var members = _viewData.GetFilteredMembers();
            var selected = new WorkItems();
            foreach (var c in VisibleRowColRange.Cols)
            {
                var m = Col2Member(c);
                foreach (var w in _viewData.GetFilteredWorkItemsOfMember(m))
                {
                    var rect = GetWorkItemDrawRect(w, members, true);
                    if (!rect.HasValue) continue;
                    if (range.Value.Contains(Rectangle.Round(rect.Value))) selected.Add(w);
                }
            }
            _viewData.Selected = selected;
        }

        private static Rectangle GetRectangle(Point p1, Point p2)
        {
            var x = Math.Min(p1.X, p2.X);
            var w = Math.Abs(p1.X - p2.X);
            var y = Math.Min(p1.Y, p2.Y);
            var h = Math.Abs(p1.Y - p2.Y);
            return new Rectangle(x, y, w, h);
        }

        internal bool ScrollOneStep(ScrollDirection direction)
        {
            const int scrollCellCount = 5;
            switch (direction)
            {
                case ScrollDirection.RIGHT: ScrollHorizontal(_viewData.Detail.ColWidth * scrollCellCount); break;
                case ScrollDirection.LEFT: ScrollHorizontal(-_viewData.Detail.ColWidth * scrollCellCount); break;
                case ScrollDirection.LOWER: ScrollVertical(_viewData.Detail.RowHeight * scrollCellCount); break;
                case ScrollDirection.UPPER: ScrollVertical(-_viewData.Detail.RowHeight * scrollCellCount); break;
                default: return false;
            }
            return true;
        }


        private void ScrollByDragToOutsideOfPanel(Point mouseLocationOnTaskGrid)
        {
            if (!WorkItemDragService.Scroll(mouseLocationOnTaskGrid, this)) return;
            Thread.Sleep(500);
        }

        private void WorkItemGrid_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateHoveringText(e);
            _workItemDragService.UpdateDraggingItem(this, e.Location, _viewData);
            if (_workItemDragService.IsActive()) ScrollByDragToOutsideOfPanel(e.Location);
            if (IsWorkItemExpandArea(_viewData, e.Location))
            {
                if (this.Cursor != Cursors.SizeNS)
                {
                    _originalCursor = this.Cursor;
                    this.Cursor = Cursors.SizeNS;
                }
            }
            else
            {
                if (this.Cursor == Cursors.SizeNS)
                {
                    this.Cursor = _originalCursor;
                }
            }
            this.Invalidate();
        }

        private void UpdateHoveringText(MouseEventArgs e)
        {
            if (_workItemDragService.IsActive()) return;
            var wi = _viewData.PickFilterdWorkItem(X2Member(e.X), Y2Day(e.Y));
            HoveringTextChanged?.Invoke(this, wi == null ? string.Empty : wi.ToString());
        }

        private void WorkItemGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_viewData.Selected != null)
            {
                EditSelectedWorkItem();
                return;
            }
            var day = Y2Day(e.Location.Y);
            var member = X2Member(e.Location.X);
            if (day == null || member == null) return;
            var proto = new WorkItem(new Project(""), "", new Tags(new List<string>()), new Period(day, day), member, TaskState.Active);
            AddNewWorkItem(proto);
        }

        public void AddNewWorkItem(WorkItem proto)
        {
            using (var dlg = new EditWorkItemForm(proto, _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var wi = dlg.GetWorkItem();
                _viewData.UpdateCallenderAndMembers(wi);
                _editService.Add(wi);
                _undoService.Push();
            }
        }

        public void EditSelectedWorkItem()
        {
            if (_viewData.Selected == null) return;
            if (_viewData.Selected.Count() != 1) return;
            var wi = _viewData.Selected.Unique;
            if (wi == null) return;
            using (var dlg = new EditWorkItemForm(wi.Clone(), _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var newWi = dlg.GetWorkItem();
                _viewData.UpdateCallenderAndMembers(newWi);
                _editService.Replace(wi, newWi);
                _viewData.Selected = new WorkItems(newWi);
            }
        }

        private void _viewData_SelectedWorkItemChanged(object sender, SelectedWorkItemChangedArg e)
        {
            _drawService.InvalidateMembers(e.UpdatedMembers);
            if (_viewData.Selected != null && _viewData.Selected.Count() == 1)
            {
                var rowRange = GetRowRange(_viewData.Selected.Unique);
                MoveVisibleRowColRange(rowRange.row, rowRange.count, Member2Col(_viewData.Selected.Unique.AssignedMember, _viewData.GetFilteredMembers()));
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
            if (e.Button == MouseButtons.Right)
            {
                _workItemDragService.StartRangeSelect(e.Location);
            }

            if (e.Button == MouseButtons.Left)
            {
                if (IsWorkItemExpandArea(_viewData, e.Location))
                {
                    _workItemDragService.StartExpand(GetExpandDirection(_viewData, e.Location), _viewData.Selected, Y2Day(e.Location.Y));
                    return;
                }
            }

            var wi = PickWorkItemFromPoint(e.Location);
            if (wi == null)
            {
                _viewData.Selected = null;
                return;
            }
            if (_viewData.Selected == null)
            {
                _viewData.Selected = new WorkItems(wi);
            }
            else
            {
                if (e.Button == MouseButtons.Left && IsControlDown())
                {
                    if (!_viewData.Selected.Contains(wi))
                    {
                        _viewData.Selected.Add(wi);
                    }
                    else
                    {
                        _viewData.Selected.Remove(wi);
                    }
                }
                else
                {
                    if (!_viewData.Selected.Contains(wi))
                    {
                        _viewData.Selected = new WorkItems(wi);
                    }
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                _workItemDragService.StartMove(_viewData.Selected, e.Location, Y2Day(e.Location.Y),this);
            }
        }

        private int GetExpandDirection(ViewData viewData, Point location)
        {
            if (viewData.Selected == null) return 0;
            foreach (var w in viewData.Selected)
            {
                var bounds = GetWorkItemDrawRect(w, viewData.GetFilteredMembers(), true);
                if (!bounds.HasValue) return 0;
                if (IsTopBar(bounds.Value, location)) return +1;
                if (IsBottomBar(bounds.Value, location)) return -1;
            }
            return 0;
        }

        private bool IsWorkItemExpandArea(ViewData viewData, Point location)
        {
            if (viewData.Selected == null) return false;
            return null != PickExpandingWorkItem(location);
        }

        internal static bool IsTopBar(RectangleF workItemBounds, PointF point)
        {
            var topBar = WorkItemDragService.GetTopBarRect(workItemBounds);
            return topBar.Contains(point);
        }

        internal static bool IsBottomBar(RectangleF workItemBounds, PointF point)
        {
            var bottomBar = WorkItemDragService.GetBottomBarRect(workItemBounds);
            return bottomBar.Contains(point);
        }

        public CallenderDay Y2Day(int y)
        {
            if (GridHeight < y) return null;
            var r = Y2Row(y);
            return Row2Day(r);
        }

        public CallenderDay Row2Day(RowIndex r)
        {
            if (_row2DayChache.TryGetValue(r, out var day)) return day;
            if (r == null) return null;
            var days = _viewData.GetFilteredDays();
            if (r.Value - FixedRowCount < 0 || days.Count <= r.Value - FixedRowCount) return null;
            day = days.ElementAt(r.Value - FixedRowCount);
            _row2DayChache.Add(r, day);
            return day;
        }

        public Member X2Member(int x)
        {
            if (GridWidth < x) return null;
            var c = X2Col(x);
            return Col2Member(c);
        }

        public Member Col2Member(ColIndex c)
        {
            if (_col2MemberChache.TryGetValue(c, out var member)) return member;
            if (c == null) return null;
            var members = _viewData.GetFilteredMembers();
            if (c.Value - FixedColCount < 0 || members.Count <= c.Value - FixedColCount) return null;
            var result = _viewData.GetFilteredMembers().ElementAt(c.Value - FixedColCount);
            _col2MemberChache.Add(c, result);
            return result;
        }

        private WorkItem PickWorkItemFromPoint(Point location)
        {
            var m = X2Member(location.X);
            var d = Y2Day(location.Y);
            if (m == null || d == null) return null;
            return _viewData.PickFilterdWorkItem(m, d);
        }

        internal void MoveToToday()
        {
            var m = (_viewData.Selected != null && _viewData.Selected.Count() == 1) ? _viewData.Selected.Unique.AssignedMember : X2Member(0);
            var now = DateTime.Now;
            var today = new CallenderDay(now.Year, now.Month, now.Day);
            MoveVisibleDayAndMember(today, m);
        }

        private void WorkItemGrid_OnDrawNormalArea(object sender, DrawNormalAreaEventArgs e)
        {
            _drawService.Draw(e.Graphics, e.IsPrint);
        }

        internal void DecRatio()
        {
            _viewData.DecRatio();
            RatioChanged?.Invoke(this, _viewData.Detail.ViewRatio);
        }

        internal void IncRatio()
        {
            _viewData.IncRatio();
            RatioChanged?.Invoke(this, _viewData.Detail.ViewRatio);
        }

        public RectangleF? GetWorkItemDrawRect(WorkItem wi, Members members, bool isFrontView)
        {
            var rowRange = GetRowRange(wi);
            if (rowRange.row == null) return null;
            return GetRect(Member2Col(wi.AssignedMember, members), rowRange.row, rowRange.count, false, false, isFrontView);
        }

        private ColIndex Member2Col(Member m, Members members)
        {
            if (_member2ColChache.TryGetValue(m, out var col)) return col;
            foreach (var c in ColIndex.Range(0, ColCount))
            {
                if (members.ElementAt(c.Value).Equals(m))
                {
                    var result = c.Offset(FixedColCount);
                    _member2ColChache.Add(m, result);
                    return result;
                }
            }
            Debug.Assert(false);
            return null;
        }

        private (RowIndex row, int count) GetRowRange(WorkItem wi)
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
            return (row, count);
        }

        internal void Redo()
        {
            _undoService.Redo(_viewData);
        }

        internal void Undo()
        {
            _undoService.Undo(_viewData);
        }

        private RowIndex Day2Row(CallenderDay day)
        {
            if (_day2RowCache.TryGetValue(day, out var row)) return row;
            foreach (var r in RowIndex.Range(FixedRowCount, RowCount - FixedRowCount))
            {
                if (_viewData.GetFilteredDays().ElementAt(r.Value - FixedRowCount).Equals(day))
                {
                    _day2RowCache.Add(day, r);
                    return r;
                }
            }
            return null;
        }

        public WorkItem PickExpandingWorkItem(Point location)
        {
            if (_viewData.Selected == null) return null;
            foreach (var w in _viewData.Selected)
            {
                var bounds = GetWorkItemDrawRect(w, _viewData.GetFilteredMembers(), true);
                if (!bounds.HasValue) continue;
                if (IsTopBar(bounds.Value, location)) return w;
                if (IsBottomBar(bounds.Value, location)) return w;
            }
            return null;
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
    }
}