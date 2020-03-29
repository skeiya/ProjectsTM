using FreeGridControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public WorkItemGrid() { }

        internal void Initialize(ViewData viewData)
        {
            LockUpdate = true;
            if (_viewData != null) DetatchEvents();
            this._viewData = viewData;
            AttachEvents();
            var fixedRows = 2;
            var fixedCols = 3;
            this.RowCount = _viewData.GetFilteredDays().Count + fixedRows;
            this.ColCount = _viewData.GetFilteredMembers().Count + fixedCols;
            this.FixedRowCount = fixedRows;
            this.FixedColCount = fixedCols;

            ApplyDetailSetting(_viewData.Detail);

            _editService = new WorkItemEditService(_viewData, _undoService);
            _undoService.Changed += _undoService_Changed1;

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
            for (var r = FixedRowCount; r < RowCount; r++)
            {
                this.RowHeights[r] = detail.RowHeight;
            }
        }

        private void _undoService_Changed1(object sender, EditedEventArgs e)
        {
            UndoChanged?.Invoke(this, e);
        }

        private void AttachEvents()
        {
            this._viewData.SelectedWorkItemChanged += _viewData_SelectedWorkItemChanged;
            this.OnDrawCell += WorkItemGrid_OnDrawCell;
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
            this.OnDrawCell -= WorkItemGrid_OnDrawCell;
            this.OnDrawNormalArea -= WorkItemGrid_OnDrawNormalArea;
            this.MouseDown -= WorkItemGrid_MouseDown;
            this.MouseUp -= WorkItemGrid_MouseDown;
            this.MouseDoubleClick -= WorkItemGrid_MouseDoubleClick;
            this.MouseWheel -= WorkItemGrid_MouseWheel;
            this._undoService.Changed -= _undoService_Changed;
            this.MouseMove -= WorkItemGrid_MouseMove;
            this.KeyDown -= WorkItemGrid_KeyDown;
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
            //@@@taskDrawArea.Invalidate();
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
        }

        private void WorkItemGrid_MouseUp(object sender, MouseEventArgs e)
        {
            _workItemDragService.End(_editService, _viewData, false);
        }

        private void WorkItemGrid_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateHoveringText(e);
            _workItemDragService.UpdateDraggingItem(X2Member, Y2Day, e.Location, _viewData);
            if (/*_grid.IsWorkItemExpandArea(_viewData, e.Location)*/false)
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

        private void _undoService_Changed(object sender, EditedEventArgs e)
        {
            this.Refresh();
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
            if (_viewData.Selected != null) MoveVisibleDayAndMember(_viewData.Selected.Period.From, _viewData.Selected.AssignedMember);
            this.Invalidate();
        }

        private void MoveVisibleDayAndMember(CallenderDay day, Member m)
        {
            if (day == null || m == null) return;
            MoveVisibleRowCol(Day2Row(day), Member2Col(m));
        }

        private void WorkItemGrid_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.ActiveControl = null;

            //@@@if (_grid.IsWorkItemExpandArea(_viewData, e.Location))
            //{
            //    if (e.Button != MouseButtons.Left) return;
            //    _workItemDragService.StartExpand(_grid.GetExpandDirection(_viewData, e.Location), _viewData.Selected);
            //    return;
            //}

            var wi = PickWorkItemFromPoint(e.Location);
            _viewData.Selected = wi;// _viewData.IsFilteredWorkItem(wi) ? null : wi;
            _workItemDragService.StartMove(_viewData.Selected, e.Location, Y2Day(e.Location.Y));
        }

        private CallenderDay Y2Day(int y)
        {
            var r = Y2Row(y);
            return Row2Day(r);
        }

        private CallenderDay Row2Day(RowIndex r)
        {
            if (r == null) return null;
            var days = _viewData.GetFilteredDays();
            if (r.Value - FixedRowCount < 0 || days.Count <= r.Value - FixedRowCount) return null;
            return days.ElementAt(r.Value - FixedRowCount);
        }

        private Member X2Member(int x)
        {
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

        private void WorkItemGrid_OnDrawCell(object sender, FreeGridControl.DrawCellEventArgs e)
        {
            var memberIndex = e.ColIndex.Value - this.FixedColCount;
            if (0 <= memberIndex)
            {
                var member = _viewData.GetFilteredMembers().ElementAt(memberIndex);
                if (e.RowIndex.Value == 0 && this.FixedColCount <= e.ColIndex.Value)
                {
                    e.Graphics.DrawString(member.Company, this.Font, Brushes.Black, e.Rect);
                }
                if (e.RowIndex.Value == 1 && this.FixedColCount <= e.ColIndex.Value)
                {
                    e.Graphics.DrawString(member.DisplayName, this.Font, Brushes.Black, e.Rect);
                }
            }

            var dayIndex = e.RowIndex.Value - this.FixedRowCount;
            if (0 <= dayIndex)
            {
                var day = _viewData.GetFilteredDays().ElementAt(dayIndex);
                if (this.FixedRowCount <= e.RowIndex.Value && e.ColIndex.Value == 0)
                {
                    e.Graphics.DrawString(day.Year.ToString(), this.Font, Brushes.Black, e.Rect);
                }
                if (this.FixedRowCount <= e.RowIndex.Value && e.ColIndex.Value == 1)
                {
                    e.Graphics.DrawString(day.Month.ToString(), this.Font, Brushes.Black, e.Rect);
                }
                if (this.FixedRowCount <= e.RowIndex.Value && e.ColIndex.Value == 2)
                {
                    e.Graphics.DrawString(day.Day.ToString(), this.Font, Brushes.Black, e.Rect);
                }
            }
        }

        private void WorkItemGrid_OnDrawNormalArea(object sender, DrawNormalAreaEventArgs e)
        {
            foreach (var c in VisibleLeftCol.Range(VisibleColCount))
            {
                var m = _viewData.GetFilteredMembers().ElementAt(c.Value - FixedColCount);
                foreach (var wi in GetVisibleWorkItems(m, VisibleTopRow, VisibleRowCount))
                {
                    var colorCondition = _viewData.Original.ColorConditions.GetMatchColorCondition(wi.ToString());
                    var brush = colorCondition == null ? null : new SolidBrush(colorCondition.BackColor);
                    var color = colorCondition == null ? Color.Black : colorCondition.ForeColor;
                    DrawWorkItem(wi, brush, color, Pens.Black, e.Graphics);
                }
            }

            DrawSelectedWorkItemBound(e);

            DrawMileStones(e.Graphics, GetMileStonesWithToday(_viewData));
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

        private void DrawWorkItem(WorkItem wi, SolidBrush fillBrush, Color fore, Pen edge, Graphics g)
        {
            var rowRange = GetRowRange(wi);
            if (rowRange.row == null) return;
            var rect = GetDrawRect(wi);
            if (!rect.HasValue) return;
            if (fillBrush != null) g.FillRectangle(fillBrush, Rectangle.Round(rect.Value));
            var front = fore == null ? Color.Black : fore;
            g.DrawString(wi.ToDrawString(_viewData.Original.Callender), this.Font, BrushCache.GetBrush(front), rect.Value);
            g.DrawRectangle(edge, Rectangle.Round(rect.Value));
        }

        private RectangleF? GetDrawRect(WorkItem wi)
        {
            var rowRange = GetRowRange(wi);
            if (rowRange.row == null) return null;
            return GetRect(Member2Col(wi.AssignedMember), rowRange);
        }

        private void DrawSelectedWorkItemBound(DrawNormalAreaEventArgs e)
        {
            if (_viewData.Selected != null)
            {
                DrawWorkItem(_viewData.Selected, null, Color.Black, Pens.LightGreen, e.Graphics);

                //@@@if(!isDragging) {
                var rect = GetDrawRect(_viewData.Selected);
                if (rect.HasValue)
                {
                    DrawTopDragBar(e.Graphics, rect.Value);
                    DrawBottomDragBar(e.Graphics, rect.Value);
                }
                //}
            }
        }

        private void DrawBottomDragBar(Graphics g, RectangleF bounds)
        {
            var rect = WorkItemDragService.GetBottomBarRect(bounds, 5);// @@@TODO (5)はやめる
            var points = WorkItemDragService.GetBottomBarLine(bounds, 5);
            g.FillRectangle(Brushes.DarkBlue, rect);
            g.DrawLine(Pens.White, points.Item1, points.Item2);
        }

        private void DrawTopDragBar(Graphics g, RectangleF bounds)
        {
            var rect = WorkItemDragService.GetTopBarRect(bounds, 5);
            var points = WorkItemDragService.GetTopBarLine(bounds, 5);
            g.FillRectangle(Brushes.DarkBlue, rect);
            g.DrawLine(Pens.White, points.Item1, points.Item2);
        }

        private RectangleF WorkItem2Rect(WorkItem wi)
        {
            var col = Member2Col(wi.AssignedMember);
            var rowRange = Period2RowRange(wi.Period);
            return GetRect(col, rowRange);
        }

        private (RowIndex row, int count) Period2RowRange(Period period)
        {
            var fromRow = Day2Row(period.From);
            var toRow = Day2Row(period.To);
            return (fromRow, toRow.Value - fromRow.Value + 1);
        }

        private RowIndex Day2Row(CallenderDay day)
        {
            foreach (var r in RowIndex.Range(FixedRowCount, RowCount - FixedRowCount))
            {
                if (_viewData.GetFilteredDays().ElementAt(r.Value - FixedRowCount).Equals(day)) return r;
            }
            return null;
        }

        private ColIndex Member2Col(Member m)
        {
            foreach (var c in ColIndex.Range(0, ColCount))
            {
                if (_viewData.GetFilteredMembers().ElementAt(c.Value).Equals(m)) return c.Offset(FixedColCount);
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

        private void DrawMileStones(Graphics g, MileStones mileStones)
        {
            foreach (var m in mileStones)
            {
                if (!Day2Y(m.Day, out int y)) continue;
                using (var brush = new SolidBrush(m.Color))
                {
                    g.FillRectangle(brush, 0, y, Width, 5);
                }
            }
        }

        private bool Day2Y(CallenderDay day, out int y)
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
            if (wi.Period.Contains(visibleTopDay) && !wi.Period.Contains(visibleButtomDay)) return (VisibleTopRow, GetRow(wi.Period.To).Value - VisibleTopRow.Value + 1);
            if (!wi.Period.Contains(visibleTopDay) && !wi.Period.Contains(visibleButtomDay)) return (GetRow(wi.Period.From), GetRow(wi.Period.To).Value - GetRow(wi.Period.From).Value + 1);
            return (GetRow(wi.Period.From), VisibleButtomRow.Value - GetRow(wi.Period.From).Value);
        }

        internal void Redo()
        {
            _undoService.Redo(_viewData);
        }

        internal void Undo()
        {
            _undoService.Undo(_viewData);
        }

        private RowIndex GetRow(CallenderDay day)
        {
            foreach (var r in VisibleTopRow.Range(VisibleRowCount))
            {
                if (_viewData.GetFilteredDays().ElementAt(r.Value - FixedRowCount).Equals(day)) return r;
            }
            Debug.Assert(false);
            return null;
        }

        private IEnumerable<Member> GetVisibleMembers(int left, int count)
        {
            for (var index = left; index < left + count; index++)
            {
                yield return _viewData.GetFilteredMembers().ElementAt(index);
            }
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
