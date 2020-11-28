﻿using FreeGridControl;
using ProjectsTM.Logic;
using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.UI.Common;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectsTM.UI.TaskList
{
    public partial class TaskListGrid : FreeGridControl.GridControl
    {
        private List<TaskListItem> _listItems;
        private ViewData _viewData;
        public TaskListOption Option = new TaskListOption();
        private WorkItemEditService _editService;

        public event EventHandler ListUpdated;
        private ColIndex _sortCol = ColDefinition.InitialSortCol;
        private bool _isReverse = false;
        private RowIndex _lastSelect;
        private WidthAdjuster _widthAdjuster;
        private Point _mouseDownPoint;
        private readonly int MAX_SORTABLE_DISTANCE = 20;
        
        public TaskListGrid()
        {
            InitializeComponent();
            this.OnDrawNormalArea += TaskListGrid_OnDrawNormalArea;
            this.MouseDoubleClick += TaskListGrid_MouseDoubleClick;
            this.MouseMove += TaskListGrid_MouseMove;
            this.MouseDown += TaskListGrid_MouseDown;
            this.MouseUp += TaskListGrid_MouseUp;
            this.Disposed += TaskListGrid_Disposed;
            this.KeyDown += TaskListGrid_KeyDown;
            this.Resize += TaskListGrid_Resize;
            _widthAdjuster = new WidthAdjuster(GetAdjustCol);
        }

        internal int GetErrorCount()
        {
            return _listItems.Count(l => !string.IsNullOrEmpty(l.ErrMsg));
        }

        private void TaskListGrid_Resize(object sender, EventArgs e)
        {
            UpdateExtendColWidth();
        }

        private void TaskListGrid_MouseUp(object sender, MouseEventArgs e)
        {
            if (_widthAdjuster.IsActive)
            {
                _widthAdjuster.End();
                return;
            }
            var rawLocation = Client2Raw(ClientPoint.Create(e));
            var r = Y2Row(rawLocation.Y);
            var mouseUpPoint = e.Location;
            if (r.Value < FixedRowCount)
            {
                if(CalcDistace(_mouseDownPoint, mouseUpPoint) <= MAX_SORTABLE_DISTANCE)
                {
                    HandleSortRequest(rawLocation);
                }
                return;
            }
            SelectItems(r);
        }

        private double CalcDistace(Point downPoint, Point upPoint)
        {
            var deltaX = upPoint.X - downPoint.X;
            var deltaY = upPoint.Y - downPoint.Y;

            return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
        }

        private void TaskListGrid_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDownPoint = e.Location;
            var rawPoint = this.Client2Raw(new ClientPoint(e.Location));
            var col = GetAdjustCol(rawPoint);
            if (col == null) return;
            var width = ColWidths[col.Value];
            _widthAdjuster.Start(rawPoint, width, GetWidthAdjuster(col));
        }

        private Action<int> GetWidthAdjuster(ColIndex col)
        {
            return new Action<int>(
                (w) =>
                {
                    ColWidths[col.Value] = Math.Max(w, 35);
                    UpdateExtendColWidth();
                }
            );
        }

        private void TaskListGrid_MouseMove(object sender, MouseEventArgs e)
        {
            var rawPoint = this.Client2Raw(new ClientPoint(e.Location));
            Cursor = _widthAdjuster.Update(rawPoint);
        }

        private void TaskListGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                MoveSelect(+1);
            }
            else if (e.KeyCode == Keys.Up)
            {
                MoveSelect(-1);
            }
            else if (KeyState.IsControlDown && (e.KeyCode == Keys.C))
            {
                CopyToClipboard();
            }
            else if (KeyState.IsControlDown && (e.KeyCode == Keys.A))
            {
                SelectAll();
            }
        }

        private void SetStrOneLine(StringBuilder copyData, WorkItem w)
        {
            const string DOUBLE_Q = "\"";
            const string TAB = "\t";
            copyData.Append(w.Name.ToString()); copyData.Append(TAB);
            copyData.Append(w.Project.ToString()); copyData.Append(TAB);
            copyData.Append(w.AssignedMember.ToString()); copyData.Append(TAB);
            copyData.Append(w.Tags.ToString()); copyData.Append(TAB);
            copyData.Append(w.State); copyData.Append(TAB);
            copyData.Append(w.Period.From.ToString()); copyData.Append(TAB);
            copyData.Append(w.Period.To.ToString()); copyData.Append(TAB);
            copyData.Append(_viewData.Original.Callender.GetPeriodDayCount(w.Period).ToString());
            copyData.Append(TAB);
            copyData.Append(DOUBLE_Q); copyData.Append(w.Description); copyData.Append(DOUBLE_Q);
            copyData.AppendLine(TAB);
        }

        private void CopyToClipboard()
        {
            if (_viewData.Selected == null) return;
            var workItems = _viewData.Selected;
            StringBuilder copyData = new StringBuilder(string.Empty);
            foreach (var w in workItems) { SetStrOneLine(copyData, w); }
            Clipboard.SetData(DataFormats.Text, copyData.ToString());
        }

        private void MoveSelect(int offset)
        {
            if (_viewData.Selected == null) return;
            if (_viewData.Selected.Count() != 1) return;
            var idx = _listItems.FindIndex(l => l.WorkItem.Equals(_viewData.Selected.Unique));
            var oneStep = offset / Math.Abs(offset);
            while (true)
            {
                idx += oneStep;
                if (idx < 0 || _listItems.Count <= idx) return;
                if (_listItems.ElementAt(idx).IsMilestone) continue;
                offset -= oneStep;
                if (offset != 0) continue;
                _viewData.Selected = new WorkItems(_listItems.ElementAt(idx).WorkItem);
            }
        }

        private void SelectItems(RowIndex r)
        {
            var from = r.Value - FixedRowCount;
            var to = r.Value - FixedRowCount;
            if (IsMultiSelect()) from = _lastSelect.Value - FixedRowCount;
            SelectRange(from, to);
        }

        private bool IsMultiSelect()
        {
            if (!KeyState.IsShiftDown) return false;
            if (_lastSelect == null) return false;
            return true;
        }

        private void SelectRange(int from, int to)
        {
            SwapIfUpsideDown(ref from, ref to);
            var selects = new WorkItems();
            for (var idx = from; idx <= to; idx++)
            {
                var l = _listItems[idx];
                if (l.IsMilestone) continue;
                selects.Add(l.WorkItem);
            }
            _viewData.Selected = selects;
        }
        private void SelectAll()
        {
            SelectRange(0, _listItems.Count - 1);
        }

        private void SwapIfUpsideDown(ref int from, ref int to)
        {
            if (from <= to) return;
            int buf = from;
            from = to;
            to = buf;
        }

        private void HandleSortRequest(RawPoint rawLocation)
        {
            var c = X2Col(rawLocation.X);
            if (_sortCol.Equals(c))
            {
                _isReverse = !_isReverse;
            }
            else
            {
                _isReverse = false;
            }
            _sortCol = c;
            UpdateView();
        }

        private void Sort()
        {
            ColDefinition.Sort(_sortCol, ref _listItems, _viewData);
            if (_isReverse)
            {
                _listItems.Reverse();
            }
        }

        private void TaskListGrid_Disposed(object sender, EventArgs e)
        {
            DetatchEvents();
        }

        private void TaskListGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var r = Y2Row(Client2Raw(ClientPoint.Create(e)).Y);
            if (r.Value < FixedRowCount) return;
            var item = _listItems[r.Value - FixedRowCount];

            if (item.IsMilestone)
            {
                var selectMs = item.MileStone;
                using (var dlg = new EditMileStoneForm(_viewData.Original.Callender, selectMs, _viewData.Original.MileStones.GetMileStoneFilters()))
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    _viewData.Original.MileStones.Replace(selectMs, dlg.MileStone);
                    var newWi = ConvertWorkItem(dlg.MileStone);
                    _listItems[r.Value - FixedRowCount].WorkItem = newWi;
                    UpdateView();
                    return;
                }
            }
            using (var dlg = new EditWorkItemForm(item.WorkItem.Clone(), _viewData.Original.WorkItems, _viewData.Original.Callender, _viewData.GetFilteredMembers()))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var newWi = dlg.GetWorkItem();
                _viewData.UpdateCallenderAndMembers(newWi);
                _editService.Replace(item.WorkItem, newWi);
                _viewData.Selected = new WorkItems(newWi);
            }
        }

        internal int GetDayCount()
        {
            return _listItems.Where(l => !l.IsMilestone).Sum(l => _viewData.Original.Callender.GetPeriodDayCount(l.WorkItem.Period));
        }

        internal void Initialize(ViewData viewData)
        {
            this._editService = new WorkItemEditService(viewData);
            if (_viewData != null) DetatchEvents();
            this._viewData = viewData;
            AttachEvents();
            InitializeGrid();
        }

        public void UpdateView()
        {
            LockUpdate = true;
            UpdateListItem();
            RowCount = _listItems.Count + FixedRowCount;
            InitializeRowHeight();
            LockUpdate = false;
            UpdateLastSelect();
        }

        private void InitializeGrid()
        {
            LockUpdate = true;
            UpdateListItem();
            ColCount = ColDefinition.Count;
            FixedRowCount = 1;
            RowCount = _listItems.Count + FixedRowCount;
            InitializeColWidth();
            InitializeRowHeight();
            LockUpdate = false;
            UpdateLastSelect();
        }

        private void AttachEvents()
        {
            _viewData.UndoService.Changed += _undoService_Changed;
            _viewData.SelectedWorkItemChanged += _viewData_SelectedWorkItemChanged;
        }

        private void DetatchEvents()
        {
            _viewData.UndoService.Changed -= _undoService_Changed;
            _viewData.SelectedWorkItemChanged -= _viewData_SelectedWorkItemChanged;
        }

        private void UpdateLastSelect()
        {
            if (_viewData.Selected == null ||
                _viewData.Selected.Count() == 0) { _lastSelect = null; return; }

            if (_viewData.Selected.Count() == 1)
            {
                var idx = _listItems.FindIndex(l => l.WorkItem.Equals(_viewData.Selected.Unique));
                _lastSelect = new RowIndex(idx + FixedRowCount);
            }
        }

        private void _viewData_SelectedWorkItemChanged(object sender, SelectedWorkItemChangedArg e)
        {
            UpdateLastSelect();
            MoveSelectedVisible();
            this.Invalidate();
        }

        private void MoveSelectedVisible()
        {
            if (_viewData.Selected == null) return;
            if (_viewData.Selected.Count() != 1) return;
            var listIdx = _listItems.FindIndex(l => l.WorkItem.Equals(_viewData.Selected.Unique));
            if (listIdx == -1) return;
            MoveVisibleRowCol(new RowIndex(listIdx + FixedRowCount), new ColIndex(0)); // TODO グリッドの上側に移動してしまう。下側にはみ出ていた時は下のままにする。
        }

        private void _undoService_Changed(object sender, IEditedEventArgs e)
        {
            UpdateView();
            this.Invalidate();
        }

        private readonly ColIndex AutoExtendCol = ColDefinition.AutoExtendCol;

        private void UpdateExtendColWidth()
        {
            if (ColWidths.Count <= AutoExtendCol.Value) return;
            LockUpdate = true;
            var g = this.CreateGraphics();
            ColWidths[AutoExtendCol.Value] = GetWidth(AutoExtendCol);
            LockUpdate = false;
        }

        private void InitializeColWidth()
        {
            foreach (var c in ColIndex.Range(0, ColCount))
            {
                ColWidths[c.Value] = GetWidth(c);
            }
        }

        private void InitializeRowHeight()
        {
            var g = this.CreateGraphics();
            var unit = Size.Round(g.MeasureString("あ", Font));

            foreach (var r in RowIndex.Range(0, RowCount))
            {
                RowHeights[r.Value] = GetHeight(r, unit);
            }
        }

        int GetStringLineCount(string s)
        {
            int n = 1;
            foreach (var c in s)
            {
                if (c == '\n') n++;
            }
            return n;
        }

        private int GetHeight(RowIndex r, Size unit)
        {
            if (r.Value == 0) return unit.Height;
            return unit.Height * GetStringLineCount(_listItems[r.Value - FixedRowCount].WorkItem.Description);
        }

        private int GetWidth(ColIndex c)
        {
            if (c.Equals(AutoExtendCol))
            {
                var w = this.Width - this.VScrollBarWidth;
                foreach (var col in ColIndex.Range(0, ColCount))
                {
                    if (!col.Equals(AutoExtendCol)) w -= GetWidth(col);
                }
                return Math.Max(w, 35);
            }
            return this.ColWidths[c.Value];
        }

        private void UpdateListItem()
        {
            _listItems = GetFilterList();
            Sort();
            ListUpdated?.Invoke(this, null);
        }

        private Dictionary<WorkItem, string> GetAuditList()
        {
            var result = new Dictionary<WorkItem, string>();
            OverwrapedWorkItemsCollectService.Get(_viewData.Original.WorkItems).ForEach(w => result.Add(w, "衝突"));
            CallenderDay soon = null;
            for (int i = 5; i >= 0; i--)
            {
                soon = _viewData.Original.Callender.ApplyOffset(_viewData.Original.Callender.NearestFromToday, i);
                if (soon != null) break;
            }
            foreach (var wi in _viewData.GetFilteredWorkItems())
            {
                if (result.TryGetValue(wi, out var dummy)) continue;
                if (IsNotEndError(wi))
                {
                    result.Add(wi, "未完");
                    continue;
                }
                if (IsTooBigError(wi, soon))
                {
                    result.Add(wi, "過大");
                    continue;
                }
            }
            return result;
        }

        private bool IsNotEndError(WorkItem wi)
        {
            if (wi.State == TaskState.Done) return false;
            if (wi.State == TaskState.Background) return false;
            return wi.Period.To < CallenderDay.Today;
        }

        private bool IsTooBigError(WorkItem wi, CallenderDay soon)
        {
            if (wi.State == TaskState.Background) return false;
            if (wi.State == TaskState.Done) return false;
            if (!IsStartSoon(wi, soon)) return false;
            return IsTooBig(wi);
        }

        private bool IsTooBig(WorkItem wi)
        {
            return ColDefinition.Count < _viewData.Original.Callender.GetPeriodDayCount(wi.Period);
        }

        private bool IsStartSoon(WorkItem wi, CallenderDay soon)
        {
            return wi.Period.From <= soon;
        }

        private List<TaskListItem> GetFilterList()
        {
            var list = new List<TaskListItem>();
            var audit = GetAuditList();
            foreach (var wi in _viewData.GetFilteredWorkItems())
            {
                if (!IsMatchPattern(wi.ToString())) continue;
                audit.TryGetValue(wi, out string error);
                list.Add(new TaskListItem(wi, GetColor(wi.State, error), error));
            }
            if (Option.IsShowMS)
            {
                foreach (var ms in _viewData.Original.MileStones)
                {
                    var wi = ConvertWorkItem(ms);
                    if (!IsMatchPattern(wi.ToString())) continue;
                    list.Add(new TaskListItem(wi, ms, ms.Color, string.Empty));
                }
            }
            return list;
        }

        private bool IsMatchPattern(string target)
        {
            if (!string.IsNullOrEmpty(Option.Pattern) && !Regex.IsMatch(target, Option.Pattern)) return false;
            if (!string.IsNullOrEmpty(Option.AndPattern) && !Regex.IsMatch(target, Option.AndPattern)) return false;
            return true;
        }

        private static WorkItem ConvertWorkItem(MileStone ms)
        {
            return new WorkItem(ms.Project, ms.Name, new Tags(new List<string>() { "MS" }), new Period(ms.Day, ms.Day), new Member(), ms.State, "");
        }

        private static Color GetColor(TaskState state, string error)
        {
            if (!string.IsNullOrEmpty(error)) return Color.Red;
            switch (state)
            {
                case TaskState.Done:
                    return Color.LightGray;
                default:
                    return Color.White;
            }
        }

        private void TaskListGrid_OnDrawNormalArea(object sender, FreeGridControl.DrawNormalAreaEventArgs e)
        {
            using (var format = new StringFormat() { LineAlignment = StringAlignment.Center })
            {
                var g = e.Graphics;
                DrawTitleRow(g);
                foreach (var r in RowIndex.Range(VisibleNormalTopRow.Value, VisibleNormalRowCount))
                {
                    DrawItemRow(g, r, format);
                }
            }
        }

        private void DrawItemRow(Graphics g, RowIndex r, StringFormat format)
        {
            var item = _listItems[r.Value - FixedRowCount];
            var visibleArea = GetVisibleRect(false, false);
            foreach (var c in ColIndex.Range(VisibleNormalLeftCol.Value, VisibleNormalColCount))
            {
                var res = GetRectClient(c, r, 1, visibleArea);
                if (!res.HasValue) continue;
                g.FillRectangle(BrushCache.GetBrush(item.Color), res.Value.Value);
                g.DrawRectangle(Pens.Black, Rectangle.Round(res.Value.Value));
                var text = ColDefinition.GetText(item, c, _viewData);
                var rect = res.Value;
                rect.Y += 1;
                g.DrawString(text, this.Font, Brushes.Black, rect.Value, format);

            }
            if (_viewData.Selected != null && _viewData.Selected.Contains(item.WorkItem))
            {
                var res = GetRectClient(VisibleNormalLeftCol, r, 1, visibleArea);
                if (!res.HasValue) return;
                var rect = new Rectangle(0, res.Value.Top, GridWidth, res.Value.Height);
                g.DrawRectangle(PenCache.GetPen(Color.DarkBlue, 3), rect);
            }
        }


        private void DrawTitleRow(Graphics g)
        {
            using (var format = new StringFormat() { Alignment = StringAlignment.Far })
            {
                var visibleArea = GetVisibleRect(true, false);
                foreach (var c in ColIndex.Range(VisibleNormalLeftCol.Value, VisibleNormalColCount))
                {
                    var res = GetRectClient(c, new RowIndex(0), 1, visibleArea);
                    if (!res.HasValue) return;
                    g.FillRectangle(Brushes.Gray, res.Value.Value);
                    g.DrawRectangle(Pens.Black, res.Value.Value);
                    var rect = res.Value;
                    rect.Y += 1;
                    g.DrawString(ColDefinition.GetTitle(c), this.Font, Brushes.Black, rect.Value);
                    if (c.Equals(_sortCol)) g.DrawString(_isReverse ? "▼" : "▲", this.Font, Brushes.Black, rect.Value, format);
                }
            }
        }
    }
}
