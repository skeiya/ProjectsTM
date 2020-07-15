using FreeGridControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ProjectsTM.Model;
using ProjectsTM.UI;
using ProjectsTM.ViewModel;

namespace ProjectsTM.Service
{
    class DrawService : IDisposable
    {
        private ViewData _viewData;

        private bool _redrawLock = false;
        internal void Lock(bool redrawLock)
        {
            _redrawLock = redrawLock;
        }

        private readonly Func<bool> _isDragActive;
        private ImageBuffer _imageBuffer;
        private IWorkItemGrid _grid;

        public DrawService(
            ViewData viewData,
            IWorkItemGrid grid,
            Func<bool> IsDragActive,
            Font font)
        {
            this._viewData = viewData;
            _isDragActive = IsDragActive;
            _imageBuffer = new ImageBuffer((int)grid.FullSize.Width, (int)grid.FullSize.Height);
            this._grid = grid;
            this._font = font;
        }

        internal void Draw(Graphics g, bool isPrint)
        {
            if (_redrawLock) return;
            DrawImageBufferBase();
            using (var transferImage = new Bitmap(_grid.VisibleSize.Width, _grid.VisibleSize.Height))
            {
                var transferGraphics = Graphics.FromImage(transferImage);
                TransferImage(transferGraphics);
                DrawAroundAndOverlay(isPrint, transferGraphics);
                TransferScale(g, transferImage);
            }
        }

        private void TransferScale(Graphics g, Image transferImage)
        {
            var dst = new RectangleF(0, 0, _grid.VisibleSize.Width, _grid.VisibleSize.Height);// GetVisibleNormalRect();
            var src = dst;
            g.DrawImage(transferImage, dst, src, GraphicsUnit.Pixel);
        }

        private void DrawAroundAndOverlay(bool isPrint, Graphics transferGraphics)
        {
            var g = transferGraphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            var font = FontCache.GetFont(_font.FontFamily, _viewData.FontSize, false);
            DrawCalender(font, g);
            DrawMember(font, g);
            DrawEdgeWorkItems(font, g, isPrint);
            DrawMileStones(font, g, GetMileStonesWithToday(_viewData));
            DrawSelectedWorkItemBound(g, font);
            DrawRangeSelectBound(g);
        }

        private void DrawRangeSelectBound(Graphics g)
        {
            var rect = _grid.GetRangeSelectBound();
            if (!rect.HasValue) return;
            using (var pen = new Pen(Color.Red, 2))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                pen.DashOffset += 10;
                g.DrawRectangle(pen, rect.Value);
            }
        }

        private bool SelectedIsSame(WorkItem wi)
        {
            if (_viewData.Selected == null) return false;
            if (_viewData.Selected.Count() != 1) return false;
            if (_viewData.Selected.Unique.Equals(wi)) return false;
            return _viewData.Selected.Unique.Name == wi.Name;
        }

        private void DrawEdgeWorkItems(Font font, Graphics g, bool isPrint)
        {
            var range = _grid.VisibleRowColRange;
            var members = _viewData.GetFilteredMembers();
            foreach (var c in range.Cols)
            {
                var m = _grid.Col2Member(c);
                foreach (var wi in GetVisibleWorkItems(m, range.TopRow, GetRowCount(range, c, isPrint)))
                {
                    DrawWorkItem(wi, Pens.Black, font, g, members, true);
                }
            }
        }

        internal void Clear()
        {
            var size = _imageBuffer.Image.Size;
            _imageBuffer.Dispose();
            _imageBuffer = new ImageBuffer(size.Width, size.Height);
        }

        private static int GetRowCount(RowColRange range, ColIndex c, bool isPrint)
        {
            if (isPrint) return range.RowCount;
            return c.Equals(range.LeftCol) ? range.RowCount : 1;
        }

        private void TransferImage(Graphics transferGraphics)
        {
            var dst = GetVisibleNormalRect();
            var src = dst;
            src.Offset(_grid.ScrollOffset);
            transferGraphics.DrawImage(_imageBuffer.Image, dst, src, GraphicsUnit.Pixel);
        }

        private RectangleF GetVisibleNormalRect()
        {
            var visibleSize = _grid.VisibleSize;
            var fixedSize = _grid.FixedSize;
            return new RectangleF(fixedSize.Width, fixedSize.Height, visibleSize.Width - fixedSize.Width, visibleSize.Height - fixedSize.Height);
        }

        private void DrawImageBufferBase()
        {
            var g = _imageBuffer.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            var font = FontCache.GetFont(_font.FontFamily, _viewData.FontSize, false);
            var range = _grid.VisibleRowColRange;
            var members = _viewData.GetFilteredMembers();
            foreach (var c in range.Cols)
            {
                var m = _grid.Col2Member(c);
                foreach (var wi in GetVisibleWorkItems(m, range.TopRow, range.RowCount))
                {
                    if (_viewData.Selected != null && _viewData.Selected.Contains(wi)) continue;
                    if (_imageBuffer.IsValid(wi)) continue;
                    _imageBuffer.Validate(wi);
                    DrawWorkItem(wi, Pens.Black, font, g, members, false);
                }
            }
        }

        private IEnumerable<WorkItem> GetVisibleWorkItems(Member m, RowIndex top, int count)
        {
            if (count <= 0) yield break;
            var topDay = _grid.Row2Day(top);
            if (topDay == null) yield break;
            var buttomDay = _grid.Row2Day(top.Offset(count - 1));
            foreach (var wi in _viewData.GetFilteredWorkItemsOfMember(m))
            {
                if (!wi.Period.HasInterSection(new Period(topDay, buttomDay))) continue;
                yield return wi;
            }
        }

        private void DrawWorkItem(WorkItem wi, Pen edge, Font font, Graphics g, IEnumerable<Member> members, bool isFrontView)
        {
            var cond = _viewData.Original.ColorConditions.GetMatchColorCondition(wi.ToString());
            var fillBrush = cond == null ? BrushCache.GetBrush(Control.DefaultBackColor) : BrushCache.GetBrush(cond.BackColor);
            var front = cond == null ? Color.Black : cond.ForeColor;
            var res = _grid.GetWorkItemDrawRect(wi, members, isFrontView);
            if (!res.HasValue) return;
            if (res.Value.IsEmpty) return;
            if (wi.State == TaskState.Done)
            {
                font = FontCache.GetFont(_font.FontFamily, _viewData.FontSize, true);
            }
            var rect = res.Value;
            rect.Intersect(new RectangleF(0, 0, _grid.FullSize.Width - 1, _grid.FullSize.Height - 1));
            g.FillRectangle(fillBrush, rect);
            var isAppendDays = IsAppendDays(g, font, rect);
            g.DrawString(wi.ToDrawString(_viewData.Original.Callender, isAppendDays), font, BrushCache.GetBrush(front), rect);
            g.DrawRectangle(edge, rect.X, rect.Y, rect.Width, rect.Height);
        }

        private static bool IsAppendDays(Graphics g, Font f, RectangleF rect)
        {
            var min = g.MeasureString("5d", f);
            if (rect.Height < min.Height) return false;
            return min.Width < rect.Width;
        }

        private static void DrawBottomDragBar(Graphics g, RectangleF bounds)
        {
            var rect = WorkItemDragService.GetBottomBarRect(bounds);
            var points = WorkItemDragService.GetBottomBarLine(bounds);
            g.FillRectangle(Brushes.DarkBlue, rect);
            g.DrawLine(Pens.White, points.Item1, points.Item2);
        }

        private static void DrawTopDragBar(Graphics g, RectangleF bounds)
        {
            var rect = WorkItemDragService.GetTopBarRect(bounds);
            var points = WorkItemDragService.GetTopBarLine(bounds);
            g.FillRectangle(Brushes.DarkBlue, rect);
            g.DrawLine(Pens.White, points.Item1, points.Item2);
        }

        private static MileStones GetMileStonesWithToday(ViewData viewData)
        {
            var result = viewData.Original.MileStones.Clone();
            var today = CallenderDay.Today;
            if (viewData.Original.Callender.Days.Contains(today))
            {
                result.Add(new MileStone("Today", today, Color.Red, null));
            }
            return result;
        }

        private bool FilterdProjectsContain(MileStone m, IEnumerable<Project> projects)
        {
            foreach (var project in projects)
            {
                if (m.Project.ToString() == project.ToString()) return true;
            }
            return false;
        }

        private bool FilteredMileStonesContain(MileStone m, IEnumerable<Project> projects)
        {
            if (m == null) return false;
            if (m.Project == null) { m.Project = new Project("ALL"); return true; }
            if (m.Project.ToString() == "ALL") return true;
            if (FilterdProjectsContain(m, projects)) return true;
            return false;
        }

        private void DrawMileStones(Font font, Graphics g, MileStones mileStones)
        {
            var range = _grid.VisibleRowColRange;
            var projects = _viewData.GetFilteredWorkItems().Select(w => w.Project).Distinct();
            foreach (var r in range.Rows)
            {
                var m = mileStones.FirstOrDefault((i) => i.Day.Equals(_grid.Row2Day(r)));           
                if (!FilteredMileStonesContain(m, projects)) continue;
                var rect = _grid.GetRect(range.LeftCol, r, 0, false, false, true);
                if (!rect.HasValue) continue;
                using (var brush = new SolidBrush(m.Color))
                {
                    g.FillRectangle(brush, 0, rect.Value.Y, _grid.VisibleSize.Width, 1);
                    g.DrawString(m.Name, font, brush, 0, rect.Value.Y - 10);
                }
            }
        }

        private void DrawSameNameWorkItem(Graphics g, Font font)
        {
            var range = _grid.VisibleRowColRange;
            var members = _viewData.GetFilteredMembers();
            foreach (var c in range.Cols)
            {
                var m = _grid.Col2Member(c);
                foreach (var wi in GetVisibleWorkItems(m, range.TopRow, range.RowCount))
                {
                    if (SelectedIsSame(wi)) DrawWorkItem(wi, Pens.LightGreen, font, g, members, true);
                }
            }
        }
        private void DrawSelectedWorkItemBound(Graphics g, Font font)
        {
            if (_viewData.Selected == null) return;
            foreach (var w in _viewData.Selected)
            {
                DrawWorkItem(w, Pens.LightGreen, font, g, _viewData.GetFilteredMembers(), true);
            }

            DrawSameNameWorkItem(g, font);

            if (!_isDragActive())
            {
                foreach (var w in _viewData.Selected)
                {
                    var rect = _grid.GetWorkItemDrawRect(w, _viewData.GetFilteredMembers(), true);
                    if (rect.HasValue)
                    {
                        DrawTopDragBar(g, rect.Value);
                        DrawBottomDragBar(g, rect.Value);
                    }
                }
            }
        }

        private void DrawCalender(Font font, Graphics g)
        {
            var year = 0;
            var month = 0;
            var day = 0;
            var range = _grid.VisibleRowColRange;
            var rowHeight = g.MeasureString("A", font).Height;
            foreach (var r in range.Rows)
            {
                var d = _grid.Row2Day(r);
                if (_grid.IsSelected(d))
                {
                    var rectYear = _grid.GetRect(WorkItemGridConstants.YearCol, r, 1, false, true, true);
                    var rectMonth = _grid.GetRect(WorkItemGridConstants.MonthCol, r, 1, false, true, true);
                    var rectDay = _grid.GetRect(WorkItemGridConstants.DayCol, r, 1, false, true, true);
                    var rect = new RectangleF(rectYear.Value.Left, rectYear.Value.Top, rectYear.Value.Width + rectMonth.Value.Width + rectDay.Value.Width, rectYear.Value.Height);
                    g.FillRectangle(BrushCache.GetBrush(Color.LightSkyBlue), rect);
                }
                if (year != d.Year)
                {
                    var rectYear = _grid.GetRect(WorkItemGridConstants.YearCol, r, 1, false, true, true);
                    if (rectYear.HasValue)
                    {
                        month = 0;
                        day = 0;
                        year = DrawYear(font, g, d, rectYear.Value, rowHeight);
                    }
                }
                if (month != d.Month)
                {
                    var rectMonth = _grid.GetRect(WorkItemGridConstants.MonthCol, r, 1, false, true, true);
                    if (rectMonth.HasValue)
                    {
                        day = 0;
                        month = DrawMonth(font, g, d, rectMonth.Value, rowHeight);
                    }
                }
                if (day != d.Day)
                {
                    var rectDay = _grid.GetRect(WorkItemGridConstants.DayCol, r, 1, false, true, true);
                    if (rectDay.HasValue)
                    {
                        day = d.Day;
                        g.DrawString(day.ToString(), font, Brushes.Black, rectDay.Value);
                    }
                }
            }
        }

        internal void InvalidateMembers(IEnumerable<Member> updatedMembers)
        {
            _imageBuffer.Invalidate(updatedMembers, _grid);
        }

        private static int DrawMonth(Font font, Graphics g, CallenderDay d, RectangleF rectMonth, float height)
        {
            int month = d.Month;
            rectMonth.Offset(0, height);
            rectMonth.Inflate(0, height);
            g.DrawString(month.ToString(), font, Brushes.Black, rectMonth);
            return month;
        }

        private static int DrawYear(Font font, Graphics g, CallenderDay d, RectangleF rectYear, float height)
        {
            int year = d.Year;
            rectYear.Offset(0, height);
            rectYear.Inflate(0, height);
            g.DrawString(year.ToString(), font, Brushes.Black, rectYear);
            return year;
        }

        private void DrawMember(Font font, Graphics g)
        {
            var range = _grid.VisibleRowColRange;
            foreach (var c in range.Cols)
            {
                var m = _grid.Col2Member(c);
                var rectCompany = _grid.GetRect(c, WorkItemGridConstants.CompanyRow, 1, true, false, true);
                var rectLastName = _grid.GetRect(c, WorkItemGridConstants.LastNameRow, 1, true, false, true);
                var rectFirstName = _grid.GetRect(c, WorkItemGridConstants.FirstNameRow, 1, true, false, true);
                if (_grid.IsSelected(m))
                {
                    var rect = new RectangleF(rectCompany.Value.Left, rectCompany.Value.Top, rectCompany.Value.Width, rectCompany.Value.Height + rectLastName.Value.Height + rectFirstName.Value.Height);
                    g.FillRectangle(BrushCache.GetBrush(Color.LightSkyBlue), rect);
                }
                g.DrawString(m.Company, font, Brushes.Black, rectCompany.Value);
                g.DrawString(m.LastName, font, Brushes.Black, rectLastName.Value);
                g.DrawString(m.FirstName, font, Brushes.Black, rectFirstName.Value);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには
        private readonly Font _font;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _imageBuffer.Dispose();
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~DrawService()
        // {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
