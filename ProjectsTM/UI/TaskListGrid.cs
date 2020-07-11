using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectsTM.ViewModel;
using FreeGridControl;
using ProjectsTM.Model;
using ProjectsTM.Service;

namespace ProjectsTM.UI
{
    public partial class TaskListGrid : FreeGridControl.GridControl
    {
        private List<TaskListItem> _listItems;
        private ViewData _viewData;
        private WorkItemEditService _editService;

        public TaskListGrid()
        {
            InitializeComponent();
            this.OnDrawNormalArea += TaskListGrid_OnDrawNormalArea;
            this.MouseDoubleClick += TaskListGrid_MouseDoubleClick;
            this.Disposed += TaskListGrid_Disposed;
        }

        private void TaskListGrid_Disposed(object sender, EventArgs e)
        {
            DetatchEvents();
        }

        private void TaskListGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var r = Y2Row(e.Y);
            var item = _listItems[r.Value - FixedRowCount];
            if (r.Value < FixedRowCount) return; using (var dlg = new EditWorkItemForm(item.WorkItem.Clone(), _viewData.Original.Callender, _viewData.GetFilteredMembers()))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var newWi = dlg.GetWorkItem();
                _viewData.UpdateCallenderAndMembers(newWi);
                _editService.Replace(item.WorkItem, newWi);
                _viewData.Selected = new WorkItems(newWi);
            }
        }

        internal void Initialize(ViewData viewData)
        {
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
            ColCount = 9;
            FixedRowCount = 1;
            RowCount = _listItems.Count + FixedRowCount;
            SetHeightAndWidth();
            LockUpdate = false;
        }

        private void AttachEvents()
        {
            _viewData.UndoService.Changed += _undoService_Changed;
        }

        private void DetatchEvents()
        {
            _viewData.UndoService.Changed -= _undoService_Changed;
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
            var list = new List<TaskListItem>();
            foreach (var wi in _viewData.GetFilteredWorkItems())
            {
                list.Add(new TaskListItem(wi, GetColor(wi.State)));
            }
            foreach (var ms in _viewData.Original.MileStones)
            {
                list.Add(new TaskListItem(ConvertWorkItem(ms), ms.Color));
            }
            _listItems = list.OrderBy(l => l.WorkItem.Period.To).ToList();
        }

        private WorkItem ConvertWorkItem(MileStone ms)
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
                var text = GetText(item.WorkItem, c);
                g.DrawString(text, this.Font, Brushes.Black, res.Value);
            }
        }

        private string GetText(WorkItem item, ColIndex c)
        {
            var colIndex = c.Value;
            if (colIndex == 0)
            {
                return item.Name;
            }
            else if (colIndex == 1)
            {
                return item.Project.ToString();
            }
            else if (colIndex == 2)
            {
                return item.AssignedMember.ToString();
            }
            else if (colIndex == 3)
            {
                return item.Tags.ToString();
            }
            else if (colIndex == 4)
            {
                return item.State.ToString();
            }
            else if (colIndex == 5)
            {
                return item.Period.From.ToString();
            }
            else if (colIndex == 6)
            {
                return item.Period.To.ToString();
            }
            else if (colIndex == 7)
            {
                return _viewData.Original.Callender.GetPeriodDayCount(item.Period).ToString();
            }
            else if (colIndex == 8)
            {
                return item.Description;
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
            string[] titles = new string[] { "名前", "プロジェクト", "担当", "タグ", "状態", "開始", "終了", "人日", "備考" };
            return titles[c.Value];
        }
    }
}
