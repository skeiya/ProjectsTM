using FreeGridControl;
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

        public TaskListGrid()
        {
            InitializeComponent();
            this.OnDrawNormalArea += TaskListGrid_OnDrawNormalArea;
            this.MouseDoubleClick += TaskListGrid_MouseDoubleClick;
            this.MouseClick += TaskListGrid_MouseClick;
            this.Disposed += TaskListGrid_Disposed;
        }

        private void TaskListGrid_MouseClick(object sender, MouseEventArgs e)
        {
            var r = Y2Row(Client2Raw(e.Location).Y);
            if (r.Value < FixedRowCount) return;
            var item = _listItems[r.Value - FixedRowCount];
            if (!item.IsMilestone)
            {
                _viewData.Selected = new WorkItems(item.WorkItem);
            }
        }

        private void TaskListGrid_Disposed(object sender, EventArgs e)
        {
            DetatchEvents();
        }

        private void TaskListGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var r = Y2Row(Client2Raw(e.Location).Y);
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
            this.Invalidate();
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
            var calculator = new HeightAndWidthCalcultor(font, g, _listItems, GetText, ColCount);
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
            var list = _isAudit ? GetAuditList() : GetFilterList();
            _listItems = list.OrderBy(l => l.WorkItem.Period.To).ToList();

        }

        private List<TaskListItem> GetAuditList()
        {
            var list = new List<TaskListItem>();
            foreach (var wi in _viewData.GetFilteredWorkItems())
            {
                if (IsOverwrapError(wi))
                {
                    list.Add(CreateErrorItem(wi, "期間重複"));
                    continue;
                }
                if (IsNotStartedError(wi))
                {
                    list.Add(CreateErrorItem(wi, "未開始"));
                    continue;
                }
            }
            return list;
        }

        private static bool IsNotStartedError(WorkItem wi)
        {
            if (IsStarted(wi)) return false;
            return CallenderDay.ToDay() >= wi.Period.From;
        }

        private static bool IsStarted(WorkItem wi)
        {
            return wi.State != TaskState.New || wi.State == TaskState.Background;
        }

        private bool IsOverwrapError(WorkItem wi)
        {
            foreach (var other in _viewData.GetFilteredWorkItems())
            {
                if (!other.AssignedMember.Equals(wi.AssignedMember)) continue;
                if (other.Equals(wi)) continue;
                if (other.Period.HasInterSection(wi.Period)) return true;
            }
            return false;
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
            return new WorkItem(new Project("noPrj"), ms.Name, new Tags(new List<string>()), new Period(ms.Day, ms.Day), new Member(), TaskState.Active, "");
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
            var g = e.Graphics;
            var range = this.VisibleRowColRange;
            DrawTitleRow(g);
            foreach (var r in RowIndex.Range(VisibleNormalTopRow.Value, VisibleNormalRowCount))
            {
                DrawItemRow(g, r);
            }
        }

        private void DrawItemRow(Graphics g, RowIndex r)
        {
            var item = _listItems[r.Value - FixedRowCount];
            foreach (var c in ColIndex.Range(VisibleNormalLeftCol.Value, VisibleNormalColCount))
            {
                var res = GetRect(c, r, 1, false, false, true);
                if (!res.HasValue) continue;
                g.FillRectangle(BrushCache.GetBrush(item.Color), res.Value);
                g.DrawRectangle(Pens.Black, Rectangle.Round(res.Value));
                var text = GetText(item, c);
                g.DrawString(text, this.Font, Brushes.Black, res.Value);

            }
            if (_viewData.Selected != null && _viewData.Selected.Contains(item.WorkItem))
            {
                var res = GetRect(new ColIndex(0), r, 1, false, false, true);
                if (!res.HasValue) return;
                var rect = new Rectangle(0, (int)res.Value.Top, (int)GridWidth, (int)res.Value.Height);
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
            foreach (var c in ColIndex.Range(VisibleNormalLeftCol.Value, VisibleNormalColCount))
            {
                var res = GetRect(c, new RowIndex(0), 1, true, false, true);
                if (!res.HasValue) return;
                g.FillRectangle(Brushes.Gray, res.Value);
                g.DrawRectangle(Pens.Black, Rectangle.Round(res.Value));
                g.DrawString(GetTitle(c), this.Font, Brushes.Black, res.Value);
            }
        }

        private static string GetTitle(ColIndex c)
        {
            string[] titles = new string[] { "名前", "プロジェクト", "担当", "タグ", "状態", "開始", "終了", "人日", "備考", "エラー" };
            return titles[c.Value];
        }
    }
}
