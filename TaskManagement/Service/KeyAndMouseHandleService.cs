using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagement.Logic;
using TaskManagement.Model;
using TaskManagement.UI;
using TaskManagement.ViewModel;
using static FreeGridControl.GridControl;

namespace TaskManagement.Service
{
    class KeyAndMouseHandleService : IDisposable
    {
        private readonly ViewData _viewData;
        private readonly IWorkItemGrid _grid;
        private readonly WorkItemDragService _workItemDragService;
        private DrawService _drawService;
        private WorkItemEditService _editService;
        private Cursor _originalCursor;

        public event EventHandler<string> HoveringTextChanged;
        private ToolTipService _toolTipService;
        private bool disposedValue;

        public KeyAndMouseHandleService(ViewData viewData, IWorkItemGrid grid, WorkItemDragService workItemDragService,DrawService drawService, WorkItemEditService editService, Control parentControl)
        {
            this._viewData = viewData;
            this._grid = grid;
            this._workItemDragService = workItemDragService;
            this._drawService = drawService;
            this._editService = editService;
            this._toolTipService = new ToolTipService(parentControl);
        }

        public void MouseDown(MouseEventArgs e)
        {
            if (_grid.IsFixedArea(e.Location)) return;
            RawPoint curOnRaw = _grid.Client2Raw(e.Location);

            if (e.Button == MouseButtons.Right)
            {
                _workItemDragService.StartRangeSelect(curOnRaw);
            }

            if (e.Button == MouseButtons.Left)
            {
                if (IsWorkItemExpandArea(_viewData, e.Location))
                {
                    _workItemDragService.StartExpand(GetExpandDirection(_viewData, e.Location), _viewData.Selected, _grid.Y2Day(curOnRaw.Y));
                    return;
                }
            }

            var wi = _grid.PickWorkItemFromPoint(curOnRaw);
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
                if (e.Button == MouseButtons.Left && KeyState.IsControlDown)
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
                if (KeyState.IsControlDown)
                {
                    _workItemDragService.StartCopy(_viewData, curOnRaw, _grid.Y2Day(curOnRaw.Y), _drawService.InvalidateMembers);
                }
                else
                {
                    _workItemDragService.StartMove(_viewData.Selected, curOnRaw, _grid.Y2Day(curOnRaw.Y));
                }
            }
        }

        private int GetExpandDirection(ViewData viewData, Point location)
        {
            if (viewData.Selected == null) return 0;
            foreach (var w in viewData.Selected)
            {
                var bounds = _grid.GetWorkItemDrawRect(w, viewData.GetFilteredMembers(), true);
                if (!bounds.HasValue) return 0;
                if (IsTopBar(bounds.Value, location)) return +1;
                if (IsBottomBar(bounds.Value, location)) return -1;
            }
            return 0;
        }

        public void MouseMove(MouseEventArgs e, Control control)
        {
            UpdateHoveringText(e);
            _workItemDragService.UpdateDraggingItem(_grid, _grid.Client2Raw(e.Location), _viewData);
            if (IsWorkItemExpandArea(_viewData, e.Location))
            {
                if (control.Cursor != Cursors.SizeNS)
                {
                    _originalCursor = control.Cursor;
                    control.Cursor = Cursors.SizeNS;
                }
            }
            else
            {
                if (control.Cursor == Cursors.SizeNS)
                {
                    control.Cursor = _originalCursor;
                }
            }
        }


        private bool IsWorkItemExpandArea(ViewData viewData, Point location)
        {
            if (viewData.Selected == null) return false;
            return null != PickExpandingWorkItem(location);
        }

        public WorkItem PickExpandingWorkItem(Point location)
        {
            if (_viewData.Selected == null) return null;
            foreach (var w in _viewData.Selected)
            {
                var bounds = _grid.GetWorkItemDrawRect(w, _viewData.GetFilteredMembers(), true);
                if (!bounds.HasValue) continue;
                if (IsTopBar(bounds.Value, location)) return w;
                if (IsBottomBar(bounds.Value, location)) return w;
            }
            return null;
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

        private void UpdateHoveringText(MouseEventArgs e)
        {
            if (_workItemDragService.IsActive()) return;
            if (_grid.IsFixedArea(e.Location)) { _toolTipService.Hide(); return; }
            RawPoint cur = _grid.Client2Raw(e.Location);
            var wi = _viewData.PickFilterdWorkItem(_grid.X2Member(cur.X), _grid.Y2Day(cur.Y));
            HoveringTextChanged?.Invoke(this, wi == null ? string.Empty : wi.ToString());
            if (wi == null)
            {
                _toolTipService.Hide();
                return;
            }
            _toolTipService.Update(wi, _viewData.Original.Callender.GetPeriodDayCount(wi.Period));
        }

        public void DoubleClick(MouseEventArgs e)
        {
            if (_grid.IsFixedArea(e.Location)) return;
            RawPoint curOnRaw = _grid.Client2Raw(e.Location);

            if (_viewData.Selected != null)
            {
                _grid.EditSelectedWorkItem();
                return;
            }
            var day = _grid.Y2Day(curOnRaw.Y);
            var member = _grid.X2Member(curOnRaw.X);
            if (day == null || member == null) return;
            var proto = new WorkItem(new Project(""), "", new Tags(new List<string>()), new Period(day, day), member, TaskState.Active, string.Empty);
            _grid.AddNewWorkItem(proto);
        }

        internal void KeyDown(KeyEventArgs e)
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
        }

        internal void KeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (_viewData.Selected == null) return;
                _editService.Delete();
            }
            if (e.KeyCode == Keys.ControlKey)
            {
                _workItemDragService.ToMoveMode(_viewData.Original.WorkItems, _drawService.InvalidateMembers);
            }
        }

        internal void MouseWheel(MouseEventArgs e)
        {
            if (KeyState.IsControlDown)
            {
                if (e.Delta > 0)
                {
                    _grid.IncRatio();
                }
                else
                {
                    _grid.DecRatio();
                }
            }
        }

        internal void MouseUp()
        {
            using (new RedrawLock(_drawService, () => _grid.Invalidate()))
            {
                _workItemDragService.End(_editService, _viewData, false, RangeSelect);
            }
        }

        void RangeSelect()
        {
            var range = _grid.GetRangeSelectBound();
            if (!range.HasValue) return;
            var members = _viewData.GetFilteredMembers();
            var selected = new WorkItems();
            foreach (var c in _grid.VisibleRowColRange.Cols)
            {
                var m = _grid.Col2Member(c);
                foreach (var w in _viewData.GetFilteredWorkItemsOfMember(m))
                {
                    var rect = _grid.GetWorkItemDrawRect(w, members, true);
                    if (!rect.HasValue) continue;
                    if (range.Value.Contains(Rectangle.Round(rect.Value))) selected.Add(w);
                }
            }
            _viewData.Selected = selected;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _toolTipService.Dispose();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~KeyAndMouseHandleService()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
