using FreeGridControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TaskManagement.Model;
using TaskManagement.Service;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public class WorkItemGrid : FreeGridControl.GridControl
    {
        private ViewData _viewData;

        private WorkItemDragService _workItemDragService = new WorkItemDragService();
        private UndoService _undoService = new UndoService();
        private WorkItemEditService _editService;
        private Cursor _originalCursor;

        public WorkItemEditService EditService => _editService;

        public event EventHandler<EditedEventArgs> UndoChanged;

        public event EventHandler<string> HoveringTextChanged;
        public event EventHandler<float> RatioChanged;

        private Dictionary<CallenderDay, RowIndex> _day2RowCache = new Dictionary<CallenderDay, RowIndex>();
        private Dictionary<RowIndex, CallenderDay> _row2DayChache = new Dictionary<RowIndex, CallenderDay>();

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
            UpdateDayRowCache();
        }

        private void UpdateDayRowCache()
        {
            _day2RowCache.Clear();
            _row2DayChache.Clear();
        }

        private void _viewData_FilterChanged(object sender, EventArgs e)
        {
            UpdateDayRowCache();
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
            this.Refresh();
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
            this.Refresh();
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
                _workItemDragService.ToCopyMode(_viewData.Original.WorkItems);
            }
            if (e.KeyCode == Keys.Escape)
            {
                _workItemDragService.End(_editService, _viewData, true);
                _viewData.Selected = null;
            }
            this.Invalidate();
        }

        private void WorkItemGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (_viewData.Selected == null) return;
                _editService.Delete(_viewData.Selected);
                _viewData.Selected = null;
            }
            if (e.KeyCode == Keys.ControlKey)
            {
                _workItemDragService.ToMoveMode(_viewData.Original.WorkItems);
            }
            this.Invalidate();
        }

        private void WorkItemGrid_MouseUp(object sender, MouseEventArgs e)
        {
            _workItemDragService.End(_editService, _viewData, false);
        }

        private void WorkItemGrid_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateHoveringText(e);
            _workItemDragService.UpdateDraggingItem(X2Member, Y2Day, e.Location, _viewData);
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
            if (_workItemDragService.IsMoving()) return;
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
            var proto = new WorkItem(new Project(""), "", new Tags(new List<string>()), new Period(day, day), member);
            AddNewWorkItem(proto);
        }

        public void AddNewWorkItem(WorkItem proto)
        {
            using (var dlg = new EditWorkItemForm(proto, _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var wi = dlg.GetWorkItem(_viewData.Original.Callender);
                _viewData.UpdateCallenderAndMembers(wi);
                _editService.Add(wi);
                _undoService.Push();
            }
        }

        public void EditSelectedWorkItem()
        {
            var wi = _viewData.Selected;
            if (wi == null) return;
            using (var dlg = new EditWorkItemForm(wi.Clone(), _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var newWi = dlg.GetWorkItem(_viewData.Original.Callender);
                _viewData.UpdateCallenderAndMembers(newWi);
                _editService.Replace(wi, newWi);
                _viewData.Selected = newWi;
            }
        }

        private void _viewData_SelectedWorkItemChanged(object sender, EventArgs e)
        {
            if (_viewData.Selected != null) MoveVisibleRowCol(GetRowRange(_viewData.Selected).row, Member2Col(_viewData.Selected.AssignedMember, _viewData.GetFilteredMembers()));
            this.Invalidate();
        }

        private void MoveVisibleDayAndMember(CallenderDay day, Member m)
        {
            if (day == null || m == null) return;
            MoveVisibleRowCol(Day2Row(day), Member2Col(m, _viewData.GetFilteredMembers()));
        }

        private void WorkItemGrid_MouseDown(object sender, MouseEventArgs e)
        {
            this.ActiveControl = null;

            if (IsWorkItemExpandArea(_viewData, e.Location))
            {
                if (e.Button != MouseButtons.Left) return;
                _workItemDragService.StartExpand(GetExpandDirection(_viewData, e.Location), _viewData.Selected);
                return;
            }

            var wi = PickWorkItemFromPoint(e.Location);
            _viewData.Selected = wi;// _viewData.IsFilteredWorkItem(wi) ? null : wi;
            _workItemDragService.StartMove(_viewData.Selected, e.Location, Y2Day(e.Location.Y));
        }

        private int GetExpandDirection(ViewData viewData, Point location)
        {
            if (viewData.Selected == null) return 0;
            var bounds = GetDrawRect(viewData.Selected, viewData.GetFilteredMembers());
            if (!bounds.HasValue) return 0;
            if (IsTopBar(bounds.Value, location)) return +1;
            if (IsBottomBar(bounds.Value, location)) return -1;
            return 0;
        }

        private bool IsWorkItemExpandArea(ViewData viewData, Point location)
        {
            if (viewData.Selected == null) return false;
            var bounds = GetDrawRect(viewData.Selected, viewData.GetFilteredMembers());// (viewData.Selected, viewData.Filter);
            if (!bounds.HasValue) return false;
            return IsTopBar(bounds.Value, location) || IsBottomBar(bounds.Value, location);
        }

        internal bool IsTopBar(RectangleF workItemBounds, PointF point)
        {
            var topBar = WorkItemDragService.GetTopBarRect(workItemBounds);
            return topBar.Contains(point);
        }

        internal bool IsBottomBar(RectangleF workItemBounds, PointF point)
        {
            var bottomBar = WorkItemDragService.GetBottomBarRect(workItemBounds);
            return bottomBar.Contains(point);
        }

        private CallenderDay Y2Day(int y)
        {
            if (GridHeight < y) return null;
            var r = Y2Row(y);
            return Row2Day(r);
        }

        private CallenderDay Row2Day(RowIndex r)
        {
            if (_row2DayChache.TryGetValue(r, out var day)) return day;
            if (r == null) return null;
            var days = _viewData.GetFilteredDays();
            if (r.Value - FixedRowCount < 0 || days.Count <= r.Value - FixedRowCount) return null;
            day = days.ElementAt(r.Value - FixedRowCount);
            _row2DayChache.Add(r, day);
            return day;
        }

        private Member X2Member(int x)
        {
            if (GridWidth < x) return null;
            var c = X2Col(x);
            return Col2Member(c);
        }

        private Member Col2Member(ColIndex c)
        {
            if (c == null) return null;
            var members = _viewData.GetFilteredMembers();
            if (c.Value - FixedColCount < 0 || members.Count <= c.Value - FixedColCount) return null;
            return _viewData.GetFilteredMembers().ElementAt(c.Value - FixedColCount);
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
            var m = _viewData.Selected == null ? X2Member(0) : _viewData.Selected.AssignedMember;
            var now = DateTime.Now;
            var today = new CallenderDay(now.Year, now.Month, now.Day);
            MoveVisibleDayAndMember(today, m);
        }

        private Font CreateFont()
        {
            return new Font(this.Font.FontFamily, _viewData.FontSize);
        }

        private void WorkItemGrid_OnDrawNormalArea(object sender, DrawNormalAreaEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (var font = CreateFont())
            {
                foreach (var c in VisibleLeftCol.Range(VisibleColCount))
                {
                    var members = _viewData.GetFilteredMembers();
                    var m = members.ElementAt(c.Value - FixedColCount);
                    DrawMember(c, m, font, e.Graphics);
                    foreach (var wi in GetVisibleWorkItems(m, VisibleTopRow, VisibleRowCount))
                    {
                        var colorCondition = _viewData.Original.ColorConditions.GetMatchColorCondition(wi.ToString());
                        var brush = colorCondition == null ? null : new SolidBrush(colorCondition.BackColor);
                        var color = colorCondition == null ? Color.Black : colorCondition.ForeColor;
                        DrawWorkItem(wi, brush, color, Pens.Black, font, e.Graphics, members);
                    }
                }
                DrawCalender(font, e.Graphics);
                DrawMileStones(font, e.Graphics, GetMileStonesWithToday(_viewData));
                DrawSelectedWorkItemBound(e, font);
            }
        }

        private void DrawCalender(Font font, Graphics g)
        {
            var year = 0;
            var month = 0;
            var day = 0;
            foreach (var r in VisibleTopRow.Range(VisibleRowCount))
            {
                var d = Row2Day(r);
                if (year != d.Year)
                {
                    var rectYear = GetRect(new ColIndex(0), r, false, true);
                    year = d.Year;
                    rectYear.Offset(0, _viewData.Detail.RowHeight);
                    rectYear.Inflate(0, _viewData.Detail.RowHeight);
                    g.DrawString(year.ToString(), font, Brushes.Black, rectYear);
                }
                if (month != d.Month)
                {
                    var rectMonth = GetRect(new ColIndex(1), r, false, true);
                    month = d.Month;
                    rectMonth.Offset(0, _viewData.Detail.RowHeight);
                    rectMonth.Inflate(0, _viewData.Detail.RowHeight);
                    g.DrawString(month.ToString(), font, Brushes.Black, rectMonth);
                }
                if (day != d.Day)
                {
                    var rectDay = GetRect(new ColIndex(2), r, false, true);
                    day = d.Day;
                    g.DrawString(day.ToString(), font, Brushes.Black, rectDay);
                }
            }
        }

        private void DrawMember(ColIndex c, Member m, Font font, Graphics g)
        {
            var rectCompany = GetRect(c, new RowIndex(0), true, false);
            g.DrawString(m.Company, font, Brushes.Black, rectCompany);
            var firstName = GetRect(c, new RowIndex(1), true, false);
            g.DrawString(m.FirstName, font, Brushes.Black, firstName);
            var lastName = GetRect(c, new RowIndex(2), true, false);
            g.DrawString(m.LastName, font, Brushes.Black, lastName);
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

        private void DrawWorkItem(WorkItem wi, SolidBrush fillBrush, Color fore, Pen edge, Font font, Graphics g, Members members)
        {
            var rect = GetDrawRect(wi, members);
            if (!rect.HasValue) return;
            if (fillBrush != null) g.FillRectangle(fillBrush, Rectangle.Round(rect.Value));
            var front = fore == null ? Color.Black : fore;
            g.DrawString(wi.ToDrawString(_viewData.Original.Callender), font, BrushCache.GetBrush(front), rect.Value);
            g.DrawRectangle(edge, Rectangle.Round(rect.Value));
        }

        private RectangleF? GetDrawRect(WorkItem wi, Members members)
        {
            var rowRange = GetRowRange(wi);
            if (rowRange.row == null) return null;
            return GetRect(Member2Col(wi.AssignedMember, members), rowRange.row, false, false);
        }

        private void DrawSelectedWorkItemBound(DrawNormalAreaEventArgs e, Font font)
        {
            if (_viewData.Selected != null)
            {
                DrawWorkItem(_viewData.Selected, null, Color.Black, Pens.LightGreen, font, e.Graphics, _viewData.GetFilteredMembers());

                if (!_workItemDragService.IsActive())
                {
                    var rect = GetDrawRect(_viewData.Selected, _viewData.GetFilteredMembers());
                    if (rect.HasValue)
                    {
                        DrawTopDragBar(e.Graphics, rect.Value);
                        DrawBottomDragBar(e.Graphics, rect.Value);
                    }
                }
            }
        }

        private void DrawBottomDragBar(Graphics g, RectangleF bounds)
        {
            var rect = WorkItemDragService.GetBottomBarRect(bounds);
            var points = WorkItemDragService.GetBottomBarLine(bounds);
            g.FillRectangle(Brushes.DarkBlue, rect);
            g.DrawLine(Pens.White, points.Item1, points.Item2);
        }

        private void DrawTopDragBar(Graphics g, RectangleF bounds)
        {
            var rect = WorkItemDragService.GetTopBarRect(bounds);
            var points = WorkItemDragService.GetTopBarLine(bounds);
            g.FillRectangle(Brushes.DarkBlue, rect);
            g.DrawLine(Pens.White, points.Item1, points.Item2);
        }

        //private RowIndex Day2Row(CallenderDay day)
        //{
        //    foreach (var r in RowIndex.Range(FixedRowCount, RowCount - FixedRowCount))
        //    {
        //        if (_viewData.GetFilteredDays().ElementAt(r.Value - FixedRowCount).Equals(day)) return r;
        //    }
        //    return null;
        //}

        private ColIndex Member2Col(Member m, Members members)
        {
            foreach (var c in ColIndex.Range(0, ColCount))
            {
                if (members.ElementAt(c.Value).Equals(m)) return c.Offset(FixedColCount);
            }
            Debug.Assert(false);
            return null;
        }

        private static MileStones GetMileStonesWithToday(ViewData viewData)
        {
            var result = viewData.Original.MileStones.Clone();
            var date = DateTime.Now;
            var today = new CallenderDay(date.Year, date.Month, date.Day);
            if (viewData.Original.Callender.Days.Contains(today))
            {
                result.Add(new MileStone("Today", today, Color.Red));
            }
            return result;
        }

        private void DrawMileStones(Font font, Graphics g, MileStones mileStones)
        {
            foreach (var m in mileStones)
            {
                if (!Day2Y(m.Day, out var y)) continue;
                using (var brush = new SolidBrush(m.Color))
                {
                    g.FillRectangle(brush, 0, y, Width, 1);
                    g.DrawString(m.Name, font, brush, 0, y - 10);
                }
            }
        }

        private bool Day2Y(CallenderDay day, out float y)
        {
            var r = Day2Row(day);
            return Row2Y(r, out y);
        }

        private (RowIndex row, int count) GetRowRange(WorkItem wi)
        {
            var visibleTopDay = Row2Day(VisibleTopRow);
            var visibleButtomDay = Row2Day(VisibleButtomRow);
            var visiblePeriod = new Period(visibleTopDay, visibleButtomDay);
            if (!visiblePeriod.HasInterSection(wi.Period)) return (null, 0);
            if (wi.Period.Contains(visibleTopDay) && wi.Period.Contains(visibleButtomDay)) return (VisibleTopRow, VisibleRowCount);
            if (wi.Period.Contains(visibleTopDay) && !wi.Period.Contains(visibleButtomDay)) return (VisibleTopRow, Day2Row(wi.Period.To).Value - VisibleTopRow.Value + 1);
            if (!wi.Period.Contains(visibleTopDay) && !wi.Period.Contains(visibleButtomDay)) return (Day2Row(wi.Period.From), Day2Row(wi.Period.To).Value - Day2Row(wi.Period.From).Value + 1);
            return (Day2Row(wi.Period.From), VisibleButtomRow.Value - Day2Row(wi.Period.From).Value + 1);
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
            foreach (var r in VisibleTopRow.Range(VisibleRowCount))
            {
                if (_viewData.GetFilteredDays().ElementAt(r.Value - FixedRowCount).Equals(day))
                {
                    _day2RowCache.Add(day, r);
                    return r;
                }
            }
            return null;
        }

        private IEnumerable<WorkItem> GetVisibleWorkItems(Member m, RowIndex top, int count)
        {
            if (count <= 0) yield break;
            var topDay = _viewData.GetFilteredDays().ElementAt(top.Value - FixedRowCount);
            var buttomDay = _viewData.GetFilteredDays().ElementAt(top.Value + count - 1 - FixedRowCount);
            foreach (var wi in _viewData.GetFilteredWorkItemsOfMember(m))
            {
                if (!wi.Period.HasInterSection(new Period(topDay, buttomDay))) continue;
                yield return wi;
            }
        }
    }
}
