using FreeGridControl;
using ProjectsTM.Logic;
using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectsTM.UI
{
    public partial class TaskListGrid : FreeGridControl.GridControl
    {
        private List<TaskListItem> _listItems;
        private ViewData _viewData;
        private bool _isAudit;
        private string _pattern;
        private WorkItemEditService _editService;
        public event EventHandler ListUpdated;
        private ColIndex _sortCol = new ColIndex(6);
        private bool _isReverse = false;

        public TaskListGrid()
        {
            InitializeComponent();
            this.OnDrawNormalArea += TaskListGrid_OnDrawNormalArea;
            this.MouseDoubleClick += TaskListGrid_MouseDoubleClick;
            this.MouseClick += TaskListGrid_MouseClick;
            this.Disposed += TaskListGrid_Disposed;
            this.KeyDown += TaskListGrid_KeyDown;
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

        private void TaskListGrid_MouseClick(object sender, MouseEventArgs e)
        {
            var rawLocation = Client2Raw(ClientPoint.Create(e));
            var r = Y2Row(rawLocation.Y);
            if (r.Value < FixedRowCount)
            {
                HandleSortRequest(rawLocation);
                return;
            }
            var item = _listItems[r.Value - FixedRowCount];
            if (!item.IsMilestone)
            {
                _viewData.Selected = new WorkItems(item.WorkItem);
            }
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

        internal void Initialize(ViewData viewData, string pattern, bool isAudit)
        {
            this._isAudit = isAudit;
            this._pattern = pattern;
            this._editService = new WorkItemEditService(viewData);
            if (_viewData != null) DetatchEvents();
            this._viewData = viewData;
            AttachEvents();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            LockUpdate = true;
            UpdateListItem();
            ColCount = _isAudit ? 10 : 9;
            FixedRowCount = 1;
            RowCount = _listItems.Count + FixedRowCount;
            SetHeightAndWidth();
            LockUpdate = false;
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

        private void _viewData_SelectedWorkItemChanged(object sender, SelectedWorkItemChangedArg e)
        {
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

        private void _undoService_Changed(object sender, EditedEventArgs e)
        {
            InitializeGrid();
            this.Invalidate();
        }

        private void SetHeightAndWidth()
        {
            var font = this.Font;
            var g = this.CreateGraphics();
            var calculator = new HeightAndWidthCalcultor(font, g, _listItems, GetText, GetTitle, ColCount);
            foreach (var c in ColIndex.Range(0, ColCount))
            {
                ColWidths[c.Value] = calculator.GetWidth(c);
            }
            foreach (var r in RowIndex.Range(0, RowCount))
            {
                RowHeights[r.Value] = calculator.GetHeight(r);
            }
        }

        private void UpdateListItem()
        {
            _listItems = _isAudit ? GetAuditList() : GetFilterList();
            Sort();
            ListUpdated?.Invoke(this, null);
        }

        private List<TaskListItem> GetAuditList()
        {
            var list = OverwrapedWorkItemsGetter.Get(_viewData.Original.WorkItems).Select(w => CreateErrorItem(w, "期間重複")).ToList();
            foreach (var wi in _viewData.GetFilteredWorkItems())
            {
                if (list.Any(l => l.WorkItem.Equals(wi))) continue;
                if (IsNotStartedError(wi))
                {
                    list.Add(CreateErrorItem(wi, "未開始"));
                    continue;
                }
                if (IsTooBigError(wi))
                {
                    list.Add(CreateErrorItem(wi, "要分解"));
                    continue;
                }
            }
            if (_pattern == null) return list;
            return list.Where(l => Regex.IsMatch(l.WorkItem.ToString(), _pattern)).ToList();
        }

        private bool IsTooBigError(WorkItem wi)
        {
            if (wi.State == TaskState.Background) return false;
            if (wi.State == TaskState.Done) return false;
            if (!IsStartSoon(wi)) return false;
            return IsTooBig(wi);
        }

        private bool IsTooBig(WorkItem wi)
        {
            return 10 < _viewData.Original.Callender.GetPeriodDayCount(wi.Period);
        }

        private bool IsStartSoon(WorkItem wi)
        {
            var restPeriod = new Period(_viewData.Original.Callender.NearestFromToday, wi.Period.From);
            return _viewData.Original.Callender.GetPeriodDayCount(restPeriod) < 4;
        }

        private static bool IsNotStartedError(WorkItem wi)
        {
            if (IsStarted(wi)) return false;
            return CallenderDay.Today >= wi.Period.From;
        }

        private static bool IsStarted(WorkItem wi)
        {
            return wi.State != TaskState.New || wi.State == TaskState.Background;
        }

        private static TaskListItem CreateErrorItem(WorkItem wi, string msg)
        {
            return new TaskListItem(wi, Color.White, false, msg);
        }

        private List<TaskListItem> GetFilterList()
        {
            var list = new List<TaskListItem>();
            foreach (var wi in _viewData.GetFilteredWorkItems())
            {
                if (_pattern != null && !Regex.IsMatch(wi.ToString(), _pattern)) continue;
                list.Add(new TaskListItem(wi, GetColor(wi.State), false, string.Empty));
            }
            foreach (var ms in _viewData.Original.MileStones)
            {
                list.Add(new TaskListItem(ConvertWorkItem(ms), ms.Color, true, string.Empty));
            }
            return list;
        }

        private static WorkItem ConvertWorkItem(MileStone ms)
        {
            return new WorkItem(new Model.Project("noPrj"), ms.Name, new Tags(new List<string>()), new Period(ms.Day, ms.Day), new Member(), TaskState.Active, "");
        }

        private static Color GetColor(TaskState state)
        {
            switch (state)
            {
                case TaskState.Active:
                    return Color.White;
                case TaskState.Background:
                    return Color.LightGreen;
                case TaskState.Done:
                    return Color.LightGray;
                case TaskState.New:
                    return Color.LightBlue;
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
