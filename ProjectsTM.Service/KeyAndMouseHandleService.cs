using FreeGridControl;
using ProjectsTM.Logic;
using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProjectsTM.Service
{
    public class KeyAndMouseHandleService : IDisposable
    {
        private readonly ViewData _viewData;
        private readonly IWorkItemGrid _grid;
        private readonly WorkItemDragService _workItemDragService;
        private DrawService _drawService;
        private WorkItemEditService _editService;
        private Cursor _originalCursor;

        public event EventHandler<WorkItem> HoveringTextChanged;
        private ToolTipService _toolTipService;
        private bool disposedValue;

        public KeyAndMouseHandleService(ViewData viewData, IWorkItemGrid grid, WorkItemDragService workItemDragService, DrawService drawService, WorkItemEditService editService, Control parentControl)
        {
            this._viewData = viewData;
            this._grid = grid;
            this._workItemDragService = workItemDragService;
            this._drawService = drawService;
            this._editService = editService;
            this._toolTipService = new ToolTipService(parentControl, _viewData);
            HoveringTextChanged += KeyAndMouseHandleService_HoveringTextChanged;
        }

        private void KeyAndMouseHandleService_HoveringTextChanged(object sender, WorkItem e)
        {
            if (e == null)
            {
                _toolTipService.Hide();
                return;
            }
            _toolTipService.Update(e, _viewData.Original.Callender.GetPeriodDayCount(e.Period));
        }

        public void MouseDown(MouseEventArgs e)
        {
            var location = ClientPoint.Create(e);
            Console.WriteLine("location:" + location.X);
            Console.WriteLine("FixedWidth:" + _grid.FixedSize.Width);
            if (_grid.IsFixedArea(location)) return;
            var curOnRaw = _grid.Client2Raw(location);
            Console.WriteLine("curOnRaw:" + curOnRaw.X);
            if (e.Button == MouseButtons.Right)
            {
                _workItemDragService.StartRangeSelect(curOnRaw);
            }

            if (e.Button == MouseButtons.Left)
            {
                if (IsWorkItemExpandArea(_viewData, location))
                {
                    _workItemDragService.StartExpand(GetExpandDirection(_viewData, location), _viewData.Selected, _grid.Y2Day(curOnRaw.Y));
                    return;
                }
            }

            var wi = _grid.PickWorkItemFromPoint(curOnRaw);

            if (wi == null)
            {
                Console.WriteLine("wi.Null");
            }
            else
            {
                Console.WriteLine("wi:" + wi.Name);
            }
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
                    _workItemDragService.StartCopy(_viewData, curOnRaw, _grid.Y2Day(curOnRaw.Y), _drawService.InvalidateMembers);
                }
                else
                {
                    if (!_viewData.Selected.Contains(wi))
                    {
                        _viewData.Selected = new WorkItems(wi);
                    }
                    _workItemDragService.StartMove(_viewData.Selected, curOnRaw, _grid.Y2Day(curOnRaw.Y));
                }
            }
        }

        private int GetExpandDirection(ViewData viewData, ClientPoint location)
        {
            if (viewData.Selected == null) return 0;
            foreach (var w in viewData.Selected)
            {
                var bounds = _grid.GetWorkItemDrawRectClient(w, viewData.GetFilteredMembers());
                if (!bounds.HasValue) return 0;
                if (IsTopBar(bounds.Value, location)) return +1;
                if (IsBottomBar(bounds.Value, location)) return -1;
            }
            return 0;
        }

        public void MouseMove(ClientPoint location, Control control)
        {
            UpdateHoveringText(location);
            _workItemDragService.UpdateDraggingItem(_grid, _grid.Client2Raw(location), _viewData);
            if (IsWorkItemExpandArea(_viewData, location))
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


        private bool IsWorkItemExpandArea(ViewData viewData, ClientPoint location)
        {
            if (viewData.Selected == null) return false;
            return null != PickExpandingWorkItem(location);
        }

        public WorkItem PickExpandingWorkItem(ClientPoint location)
        {
            if (_viewData.Selected == null) return null;
            foreach (var w in _viewData.Selected)
            {
                var bounds = _grid.GetWorkItemDrawRectClient(w, _viewData.GetFilteredMembers());
                if (!bounds.HasValue) continue;
                if (IsTopBar(bounds.Value, location)) return w;
                if (IsBottomBar(bounds.Value, location)) return w;
            }
            return null;
        }

        internal static bool IsTopBar(ClientRectangle workItemBounds, ClientPoint point)
        {
            var topBar = WorkItemDragService.GetTopBarRect(workItemBounds);
            return topBar.Contains(point);
        }

        internal static bool IsBottomBar(ClientRectangle workItemBounds, ClientPoint point)
        {
            var bottomBar = WorkItemDragService.GetBottomBarRect(workItemBounds);
            return bottomBar.Contains(point);
        }

        private void UpdateHoveringText(ClientPoint location)
        {
            if (_workItemDragService.IsActive()) return;
            if (_grid.IsFixedArea(location)) { _toolTipService.Hide(); return; }
            RawPoint cur = _grid.Client2Raw(location);
            var wi = _viewData.PickFilterdWorkItem(_grid.X2Member(cur.X), _grid.Y2Day(cur.Y));
            HoveringTextChanged?.Invoke(this, wi);
        }

        public void DoubleClick(MouseEventArgs e)
        {
            var locaion = ClientPoint.Create(e);
            if (_grid.IsFixedArea(locaion)) return;
            RawPoint curOnRaw = _grid.Client2Raw(locaion);

            if (_viewData.Selected != null)
            {
                _grid.EditSelectedWorkItem();
                return;
            }
            var day = _grid.Y2Day(curOnRaw.Y);
            var member = _grid.X2Member(curOnRaw.X);
            if (day == null || member == null) return;
            var proto = WorkItem.CreateProto(new Period(day, day), member);
            _grid.AddNewWorkItem(proto);
        }

        public void KeyDown(KeyEventArgs e)
        {
            var ctrl = (e.Modifiers & Keys.Control) == Keys.Control;
            var shift = (e.Modifiers & Keys.Shift) == Keys.Shift;
            
            if (ctrl && shift && e.KeyCode == Keys.Up)
            {
                _editService.ExpandDays(-1);
                return;
            }
            if (ctrl && e.KeyCode == Keys.Up)
            {
                _editService.ShiftDays(-1);
                return;
            }

            if (ctrl && shift && e.KeyCode == Keys.Down)
            {
                _editService.ExpandDays(1);
                return;
            }
            if (ctrl && e.KeyCode == Keys.Down)
            {
                _editService.ShiftDays(1);
                return;
            }
            
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

        public void KeyUp(KeyEventArgs e)
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

        public void MouseWheel(MouseEventArgs e)
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

        public void MouseUp()
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
                    var rect = _grid.GetWorkItemDrawRectClient(w, members);
                    if (!rect.HasValue) continue;
                    if (range.Value.Contains(rect.Value)) selected.Add(w);
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
