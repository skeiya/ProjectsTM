using FreeGridControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TaskManagement.Model;
using TaskManagement.UI;
using TaskManagement.ViewModel;

namespace TaskManagement.Service
{
    class DrawService : IDisposable
    {
        private ViewData _viewData;

        private Func<Size> GetVisibleSize { get; }
        private bool _redrawLock = false;
        internal void Lock(bool redrawLock)
        {
            _redrawLock = redrawLock;
        }

        private Func<Point> GetScrollOffset { get; }
        private Func<RowColRange> GetVisibleNormalRowColRange { get; }

        private readonly SizeF _fixedSize;
        private readonly Func<bool> _isDragActive;
        private ImageBuffer _imageBuffer;
        private Func<Member, RectangleF> getMemberDrawRect;
        private Func<ColIndex, Member> col2Member;
        private Func<IEnumerable<Member>, IEnumerable<Member>> GetNeighbers;
        private Func<RowIndex, CallenderDay> row2Day;

        public DrawService(
            ViewData viewData,
            Size fullSize,
            Func<Size> GetVisibleSize,
            SizeF fixedSize,
            Func<Point> GetScrollOffset,
            Func<bool> IsDragActive,
            Func<RowColRange> GetVisibleNormalRowColRange,
            Func<Member, RectangleF> getMemberDrawRect,
            Func<ColIndex, Member> col2Member,
            Func<IEnumerable<Member>, IEnumerable<Member>> GetNeighbers,
            Func<RowIndex, CallenderDay> row2Day,
            Func<ColIndex, RowIndex, int, bool, bool, bool, RectangleF> getRect,
            Func<WorkItem, Members, bool, RectangleF?> getDrawRect,
            Font font)
        {
            this._viewData = viewData;
            this.GetVisibleSize = GetVisibleSize;
            this._fixedSize = fixedSize;
            this.GetScrollOffset = GetScrollOffset;
            _isDragActive = IsDragActive;
            this.GetVisibleNormalRowColRange = GetVisibleNormalRowColRange;
            _imageBuffer = new ImageBuffer(fullSize.Width, fullSize.Height);
            this.getMemberDrawRect = getMemberDrawRect;
            this.col2Member = col2Member;
            this.GetNeighbers = GetNeighbers;
            this.row2Day = row2Day;
            this.getRect = getRect;
            this.getDrawRect = getDrawRect;
            this._font = font;
        }

        internal void Draw(Graphics g)
        {
            if (_redrawLock) return;
            DrawWorkItemAreaBase(g);
            DrawAroundAndOverlay(g);
        }

        private void DrawAroundAndOverlay(Graphics g)
        {
            using (var font = CreateFont())
            {
                DrawCalender(font, g);
                DrawMember(font, g);
                DrawMileStones(font, g, GetMileStonesWithToday(_viewData));
                DrawSelectedWorkItemBound(g, font);
            }
        }

        private void DrawWorkItemAreaBase(Graphics g)
        {
            var image = DrawImageBuffer();
            TransferImage(g, image);
        }

        private void TransferImage(Graphics g, Image image)
        {
            var dst = GetVisibleNormalRect();
            var src = dst;
            src.Offset(GetScrollOffset());
            g.DrawImage(image, dst, src, GraphicsUnit.Pixel);
        }

        private RectangleF GetVisibleNormalRect()
        {
            var visibleSize = GetVisibleSize();
            return new RectangleF(_fixedSize.Width, _fixedSize.Height, visibleSize.Width - _fixedSize.Width, visibleSize.Height - _fixedSize.Height);
        }

        private Image DrawImageBuffer()
        {
            var g = _imageBuffer.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            using (var font = CreateFont())
            {
                var range = GetVisibleNormalRowColRange();
                var members = _viewData.GetFilteredMembers();
                foreach (var c in range.Cols)
                {
                    var m = col2Member(c);
                    foreach (var wi in GetVisibleWorkItems(m, range.TopRow, range.RowCount))
                    {
                        if (_viewData.Selected != null && _viewData.Selected.Equals(wi)) continue;
                        if (_imageBuffer.IsValid(wi)) continue;
                        _imageBuffer.Validate(wi);
                        var colorCondition = _viewData.Original.ColorConditions.GetMatchColorCondition(wi.ToString());
                        var brush = colorCondition == null ? null : new SolidBrush(colorCondition.BackColor);
                        var color = colorCondition == null ? Color.Black : colorCondition.ForeColor;
                        DrawWorkItem(wi, brush, color, Pens.Black, font, g, members, false);
                    }
                }
            }
            return _imageBuffer.Image;
        }

        private IEnumerable<WorkItem> GetVisibleWorkItems(Member m, RowIndex top, int count)
        {
            if (count <= 0) yield break;
            var topDay = row2Day(top);
            var buttomDay = row2Day(top.Offset(count - 1));
            foreach (var wi in _viewData.GetFilteredWorkItemsOfMember(m))
            {
                if (!wi.Period.HasInterSection(new Period(topDay, buttomDay))) continue;
                yield return wi;
            }
        }

        private void DrawWorkItem(WorkItem wi, SolidBrush fillBrush, Color fore, Pen edge, Font font, Graphics g, Members members, bool isFrontView)
        {
            var rect = getDrawRect(wi, members, isFrontView);
            if (!rect.HasValue) return;
            if (rect.Value.IsEmpty) return;
            if (fillBrush != null) g.FillRectangle(fillBrush, Rectangle.Round(rect.Value));
            var front = fore == null ? Color.Black : fore;
            var isAppendDays = IsAppendDays(g, font, rect.Value);
            g.DrawString(wi.ToDrawString(_viewData.Original.Callender, isAppendDays), font, BrushCache.GetBrush(front), rect.Value);
            g.DrawRectangle(edge, Rectangle.Round(rect.Value));
        }

        private bool IsAppendDays(Graphics g, Font f, RectangleF rect)
        {
            var min = g.MeasureString("5d", f);
            if (rect.Height < min.Height) return false;
            return min.Width < rect.Width;
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
            var range = GetVisibleNormalRowColRange();
            foreach (var r in range.Rows)
            {
                var m = mileStones.FirstOrDefault((i) => i.Day.Equals(row2Day(r)));
                if (m == null) continue;
                var y = getRect(range.LeftCol, r, 0, false, false, true).Y;
                using (var brush = new SolidBrush(m.Color))
                {
                    g.FillRectangle(brush, 0, y, GetVisibleSize().Width, 1);
                    g.DrawString(m.Name, font, brush, 0, y - 10);
                }
            }
        }


        private void DrawSelectedWorkItemBound(Graphics g, Font font)
        {
            if (_viewData.Selected != null)
            {
                DrawWorkItem(_viewData.Selected, null, Color.Black, Pens.LightGreen, font, g, _viewData.GetFilteredMembers(), true);

                if (!_isDragActive())
                {
                    var rect = getDrawRect(_viewData.Selected, _viewData.GetFilteredMembers(), true);
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
            var range = GetVisibleNormalRowColRange();
            foreach (var r in range.Rows)
            {
                var d = row2Day(r);
                if (year != d.Year)
                {
                    var rectYear = getRect(new ColIndex(0), r, 1, false, true, true);
                    if (!rectYear.IsEmpty)
                    {
                        month = 0;
                        day = 0;
                        year = DrawYear(font, g, d, rectYear);
                    }
                }
                if (month != d.Month)
                {
                    var rectMonth = getRect(new ColIndex(1), r, 1, false, true, true);
                    if (!rectMonth.IsEmpty)
                    {
                        day = 0;
                        month = DrawMonth(font, g, d, rectMonth);
                    }
                }
                if (day != d.Day)
                {
                    var rectDay = getRect(new ColIndex(2), r, 1, false, true, true);
                    if (!rectDay.IsEmpty)
                    {
                        day = d.Day;
                        g.DrawString(day.ToString(), font, Brushes.Black, rectDay);
                    }
                }
            }
        }

        internal void InvalidateMembers(List<Member> updatedMembers)
        {
            _imageBuffer.Invalidate(updatedMembers, getMemberDrawRect, col2Member, GetNeighbers);
        }

        private int DrawMonth(Font font, Graphics g, CallenderDay d, RectangleF rectMonth)
        {
            int month = d.Month;
            rectMonth.Offset(0, _viewData.Detail.RowHeight);
            rectMonth.Inflate(0, _viewData.Detail.RowHeight);
            g.DrawString(month.ToString(), font, Brushes.Black, rectMonth);
            return month;
        }

        private int DrawYear(Font font, Graphics g, CallenderDay d, RectangleF rectYear)
        {
            int year = d.Year;
            rectYear.Offset(0, _viewData.Detail.RowHeight);
            rectYear.Inflate(0, _viewData.Detail.RowHeight);
            g.DrawString(year.ToString(), font, Brushes.Black, rectYear);
            return year;
        }

        private void DrawMember(Font font, Graphics g)
        {
            var range = GetVisibleNormalRowColRange();
            foreach (var c in range.Cols)
            {
                var m = col2Member(c);
                var rectCompany = getRect(c, new RowIndex(0), 1, true, false, true);
                g.DrawString(m.Company, font, Brushes.Black, rectCompany);
                var firstName = getRect(c, new RowIndex(1), 1, true, false, true);
                g.DrawString(m.FirstName, font, Brushes.Black, firstName);
                var lastName = getRect(c, new RowIndex(2), 1, true, false, true);
                g.DrawString(m.LastName, font, Brushes.Black, lastName);
            }
        }

        private Font CreateFont()
        {
            return new Font(_font.FontFamily, _viewData.FontSize);
        }


        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには
        private Func<ColIndex, RowIndex, int, bool, bool, bool, RectangleF> getRect;
        private Func<WorkItem, Members, bool, RectangleF?> getDrawRect;
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
