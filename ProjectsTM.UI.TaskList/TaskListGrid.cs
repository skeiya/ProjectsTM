using FreeGridControl;
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
        private bool _isShowMS = true;
        private string _pattern;
        private WorkItemEditService _editService;
        private List<int> _taskListColWidths;

        public event EventHandler ListUpdated;
        private ColIndex _sortCol = new ColIndex(6);
        private bool _isReverse = false;
        private RowIndex _lastSelect;
        private WidthAdjuster _widthAdjuster;

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
            if (r.Value < FixedRowCount)
            {
                HandleSortRequest(rawLocation);
                return;
            }
            SelectItems(r);
        }

        private void TaskListGrid_MouseDown(object sender, MouseEventArgs e)
        {
            var col = GetAdjustCol(e.Location);
            if (col == null) return;
            var width = ColWidths[col.Value];
            _widthAdjuster.Start(e.Location, width, GetWidthAdjuster(col));
        }

        private Action<int> GetWidthAdjuster(ColIndex col)
        {
            return new Action<int>((w) => ColWidths[col.Value] = w);
        }

        private void TaskListGrid_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor = _widthAdjuster.Update(e.Location);
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
            }else if(KeyState.IsControlDown && (e.KeyCode == Keys.A))
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
            InitializeGrid();
        }

        private void Sort()
        {
            if (IsDayCountCol(_sortCol))
            {
                _listItems = _listItems.OrderBy(l => _viewData.Original.Callender.GetPeriodDayCount(l.WorkItem.Period)).ToList();
            }
            else
            {
                _listItems = _listItems.OrderBy(l => GetText(l, _sortCol)).ToList();
            }
            if (_isReverse)
            {
                _listItems.Reverse();
            }
        }

        private static bool IsDayCountCol(ColIndex c)
        {
            return c.Value == 7;
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
            if (item.IsMilestone) return;
            using (var dlg = new EditWorkItemForm(item.WorkItem.Clone(), _viewData.Original.Callender, _viewData.GetFilteredMembers()))
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

        internal void Initialize(ViewData viewData, string pattern, List<int> taskListColWidths, bool isShowMS)
        {
            this._pattern = pattern;
            this._editService = new WorkItemEditService(viewData);
            this._taskListColWidths = taskListColWidths;
            if (_viewData != null) DetatchEvents();
            this._viewData = viewData;
            this._isShowMS = isShowMS;
            AttachEvents();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            LockUpdate = true;
            UpdateListItem();
            ColCount = 10;
            FixedRowCount = 1;
            RowCount = _listItems.Count + FixedRowCount;
            SetHeightAndWidth();
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
            InitializeGrid();
            this.Invalidate();
        }

        private readonly ColIndex AutoExtendCol = new ColIndex(8);

        private void UpdateExtendColWidth()
        {
            if (ColWidths.Count <= AutoExtendCol.Value) return;
            LockUpdate = true;
            var g = this.CreateGraphics();
            var unit = Size.Round(g.MeasureString("あ", Font));
            ColWidths[AutoExtendCol.Value] = GetWidth(AutoExtendCol, unit);
            LockUpdate = false;
        }

        private void SetHeightAndWidth()
        {
            var g = this.CreateGraphics();
            var unit = Size.Round(g.MeasureString("あ", Font));
            foreach (var c in ColIndex.Range(0, ColCount))
            {
                ColWidths[c.Value] = GetWidth(c, unit);
            }
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

        private int GetWidth(ColIndex c, Size unit)
        {
            if (c.Equals(AutoExtendCol))
            {
                var w = this.Width - this.VScrollBarWidth;
                foreach (var col in ColIndex.Range(0, ColCount))
                {
                    if (!col.Equals(AutoExtendCol)) w -= GetWidth(col, unit);
                }
                return Math.Max(w, unit.Width * 5);
            }
            if (c.Value < _taskListColWidths.Count) return _taskListColWidths[c.Value];
            return unit.Width * 5;
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
            OverwrapedWorkItemsCollectService.Get(_viewData.Original.WorkItems).ForEach(w => result.Add(w, "期間重複"));
            var soon = _viewData.Original.Callender.ApplyOffset(_viewData.Original.Callender.NearestFromToday, 5);
            foreach (var wi in _viewData.GetFilteredWorkItems())
            {
                if (result.TryGetValue(wi, out var dummy)) continue;
                if (IsNotEndError(wi))
                {
                    result.Add(wi, "未終了");
                    continue;
                }
                if (IsTooBigError(wi, soon))
                {
                    result.Add(wi, "要分解");
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
            return 10 < _viewData.Original.Callender.GetPeriodDayCount(wi.Period);
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
                if (_pattern != null && !Regex.IsMatch(wi.ToString(), _pattern)) continue;
                audit.TryGetValue(wi, out string error);
                list.Add(new TaskListItem(wi, GetColor(wi.State, error), false, error));
            }
            if (_isShowMS)
            {
                foreach (var ms in _viewData.Original.MileStones)
                {
                    var wi = ConvertWorkItem(ms);
                    if (_pattern != null && !Regex.IsMatch(wi.ToString(), _pattern)) continue;
                    list.Add(new TaskListItem(wi, ms.Color, true, string.Empty));
                }
            }
            return list;
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
                var text = GetText(item, c);
                var rect = res.Value;
                rect.Y += 1;
                g.DrawString(text, this.Font, Brushes.Black, rect.Value, format);

            }
            if (_viewData.Selected != null && _viewData.Selected.Contains(item.WorkItem))
            {
                var res = GetRectClient(new ColIndex(0), r, 1, visibleArea);
                if (!res.HasValue) return;
                var rect = new Rectangle(0, res.Value.Top, GridWidth, res.Value.Height);
                g.DrawRectangle(PenCache.GetPen(Color.DarkBlue, 3), rect);
            }
        }

        private string GetText(TaskListItem item, ColIndex c)
        {
            var colIndex = c.Value;
            var wi = item.WorkItem;
            if (colIndex == 0)
            {
                return wi.Name;
            }
            else if (colIndex == 1)
            {
                return wi.Project.ToString();
            }
            else if (colIndex == 2)
            {
                return wi.AssignedMember.ToString();
            }
            else if (colIndex == 3)
            {
                return wi.Tags.ToString();
            }
            else if (colIndex == 4)
            {
                return wi.State.ToString();
            }
            else if (colIndex == 5)
            {
                return wi.Period.From.ToString();
            }
            else if (colIndex == 6)
            {
                return wi.Period.To.ToString();
            }
            else if (colIndex == 7)
            {
                return _viewData.Original.Callender.GetPeriodDayCount(wi.Period).ToString();
            }
            else if (colIndex == 8)
            {
                return wi.Description;
            }
            else if (colIndex == 9)
            {
                return item.ErrMsg;
            }
            return string.Empty;
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
                    g.DrawString(GetTitle(c), this.Font, Brushes.Black, rect.Value);
                    if (c.Equals(_sortCol)) g.DrawString(_isReverse ? "▼" : "▲", this.Font, Brushes.Black, rect.Value, format);
                }
            }
        }

        private static string GetTitle(ColIndex c)
        {
            string[] titles = new string[] { "名前", "プロジェクト", "担当", "タグ", "状態", "開始", "終了", "人日", "備考", "エラー" };
            return titles[c.Value];
        }
    }
}
