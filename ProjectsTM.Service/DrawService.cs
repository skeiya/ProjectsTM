﻿using FreeGridControl;
using ProjectsTM.Logic;
using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjectsTM.Service
{
    public class DrawService : IDisposable
    {
        private readonly MainViewData _viewData;

        private bool _redrawLock = false;
        internal void Lock(bool redrawLock)
        {
            _redrawLock = redrawLock;
        }

        private readonly Func<bool> _isDragActive;
        private readonly Func<bool> _isDragMoving;
        private readonly Func<DragStartInfo> _dragStartInfo;
        private ImageBuffer _imageBuffer;
        private readonly IWorkItemGrid _grid;

        public DrawService(IWorkItemGrid grid)
        {
            _grid = grid;
        }

        public DrawService(
            MainViewData viewData,
            IWorkItemGrid grid,
            Func<bool> isDragActive,
            Func<bool> isDragMoving,
            Func<DragStartInfo> dragStartInfo,
            Font font)
        {
            this._viewData = viewData;
            _isDragActive = isDragActive;
            _isDragMoving = isDragMoving;
            _dragStartInfo = dragStartInfo;
            this._grid = grid;
            this._font = font;
            ClearBuffer();
        }

        public void ClearBuffer()
        {
            _imageBuffer?.Dispose();
            _imageBuffer = new ImageBuffer(_grid.FullSize.Width, _grid.FullSize.Height);
        }

        public void Draw(Graphics g, bool isAllDraw)
        {
            if (_redrawLock) return;
            DrawImageBufferBase();
            using (var transferImage = new Bitmap(_grid.VisibleSize.Width, _grid.VisibleSize.Height))
            {
                var transferGraphics = Graphics.FromImage(transferImage);
                TransferImage(transferGraphics);
                DrawAroundAndOverlay(isAllDraw, transferGraphics);
                TransferScale(g, transferImage);
            }
        }

        private void TransferScale(Graphics g, Image transferImage)
        {
            var dst = new RectangleF(0, 0, _grid.VisibleSize.Width, _grid.VisibleSize.Height);// GetVisibleNormalRect();
            var src = dst;
            g.DrawImage(transferImage, dst, src, GraphicsUnit.Pixel);
        }

        private void DrawAroundAndOverlay(bool isAllDraw, Graphics transferGraphics)
        {
            var g = transferGraphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            var font = FontCache.GetFont(_font.FontFamily, _viewData.FontSize, false);
            DrawAbsentArea(g, _viewData.Original.AbsentInfo);
            DrawCalender(font, g);
            DrawMember(font, g);
            DrawEdgeWorkItems(font, g, isAllDraw);
            DrawCursorPosition(g, font);
            DrawMileStones(font, g, GetMileStonesWithToday(_viewData.Core));
            DrawSelectedWorkItemBound(g, font);
            DrawRangeSelectBound(g);
        }

        private void DrawAbsentArea(Graphics g, AbsentInfo absentInfo)
        {
            var range = _grid.VisibleRowColRange;
            foreach (var c in range.Cols)
            {
                var absentTerms = absentInfo.GetAbsentTerms(_grid.Col2Member(c));
                foreach (var a in absentTerms)
                {
                    foreach (var r in range.Rows)
                    {
                        var d = _grid.Row2Day(r);
                        if (!a.Period.Contains(d)) continue;
                        var rect = _grid.GetRectClient(c, r, 1, _grid.GetVisibleRect(false, false)).Value;
                        g.FillRectangle(Brushes.LightGray, rect);
                    }
                }
            }
        }

        private void DrawRangeSelectBound(Graphics g)
        {
            var rect = _grid.GetRangeSelectBound();
            if (rect.IsEmpty) return;
            using (var pen = new Pen(Color.Red, 2))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                pen.DashOffset += 10;
                g.DrawRectangle(pen, rect.Value);
            }
        }

        private void DrawEdgeWorkItems(Font font, Graphics g, bool isAllDraw)
        {
            var range = _grid.VisibleRowColRange;
            var members = _viewData.FilteredItems.Members;

            foreach (var c in range.Cols)
            {
                var m = _grid.Col2Member(c);
                foreach (var wi in GetVisibleWorkItems(m, range.TopRow, GetRowCount(range, c, isAllDraw)))
                {
                    DrawWorkItemClient(wi, Pens.Black, font, g, members);
                }
            }
        }

        private void DrawCursorPosition(Graphics g, Font font)
        {
            var curOnRaw = _grid.Global2Raw(Cursor.Position);
            if (_grid.PickWorkItemFromPoint(curOnRaw, out var curWi))
            {
                var members = _viewData.FilteredItems.Members;
                DrawCursorWorkItem(font, g, members, curWi);
            }
            else
            {
                DrawCursorBackgroundRectangle(g, curOnRaw);
            }
        }

        private void DrawCursorBackgroundRectangle(Graphics g, RawPoint curOnRaw)
        {
            var c = _grid.X2Col(curOnRaw.X);
            var r = _grid.Y2Row(curOnRaw.Y);
            if (!_grid.VisibleRowColRange.Contains(r, c)) return;
            var reternRect = _grid.GetRectClient(c, r, 1, _grid.GetVisibleRect(false, false));
            if (reternRect.IsEmpty) return;
            var rect = reternRect.Value;
            var pen = PenCache.GetPen(Color.LightGray, 3f);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        private void DrawCursorWorkItem(Font font, Graphics g, IEnumerable<Member> members, WorkItem wi)
        {
            var pen = PenCache.GetPen(Color.Red, 3f);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            DrawWorkItemClient(wi, pen, font, g, members);
        }

        private static int GetRowCount(RowColRange range, ColIndex c, bool isAllDraw)
        {
            if (isAllDraw) return range.RowCount;
            return c.Equals(range.LeftCol) || c.Equals(range.Cols.Last()) ? range.RowCount : 1;
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
            var members = _viewData.FilteredItems.Members;
            foreach (var c in range.Cols)
            {
                var m = _grid.Col2Member(c);
                foreach (var wi in GetVisibleWorkItems(m, range.TopRow, range.RowCount))
                {
                    if (_viewData.Selected.Contains(wi)) continue;
                    if (_imageBuffer.IsValid(wi)) continue;
                    _imageBuffer.Validate(wi);
                    DrawWorkItemRaw(wi, Pens.Black, font, g, members);
                }
            }
        }

        private IEnumerable<WorkItem> GetVisibleWorkItems(Member m, RowIndex top, int count)
        {
            if (count <= 0) yield break;
            var topDay = _grid.Row2Day(top);
            if (topDay == null) yield break;
            var buttomDay = _grid.Row2Day(top.Offset(count - 1));
            foreach (var wi in _viewData.FilteredItems.GetWorkItemsOfMember(m))
            {
                if (!wi.Period.HasInterSection(new Period(topDay, buttomDay))) continue;
                yield return wi;
            }
        }

        private void DrawWorkItemRaw(WorkItem wi, Pen edge, Font font, Graphics g, IEnumerable<Member> members)
        {
            var res = _grid.GetWorkItemDrawRectRaw(wi, members);
            if (res.IsEmpty) return;
            DrawWorkItemCore(wi, edge, font, g, res.Value);
        }

        private void DrawWorkItemClient(WorkItem wi, Pen edge, Font font, Graphics g, IEnumerable<Member> members)
        {
            var res = _grid.GetWorkItemDrawRectClient(wi, members);
            if (res.IsEmpty) return;
            DrawWorkItemCore(wi, edge, font, g, res.Value);
        }

        private void DrawWorkItemCore(WorkItem wi, Pen edge, Font font, Graphics g, Rectangle rect)
        {
            var cond = _viewData.Original.ColorConditions.GetMatchColorCondition(wi.ToString(), Control.DefaultBackColor);
            var fillBrush = BrushCache.GetBrush(cond.BackColor);
            var front = cond.ForeColor;
            if (wi.State == TaskState.Done)
            {
                font = FontCache.GetFont(_font.FontFamily, _viewData.FontSize, true);
            }
            g.FillRectangle(fillBrush, rect);
            var isAppendDays = IsAppendDays(g, font, rect);
            g.DrawString(wi.ToDrawString(_viewData.Original.Callender, isAppendDays), font, BrushCache.GetBrush(front), rect);
            g.DrawRectangle(edge, rect.X, rect.Y, rect.Width, rect.Height);
        }

        private static bool IsAppendDays(Graphics g, Font f, Rectangle rect)
        {
            var min = g.MeasureString("5d", f);
            if (rect.Height < min.Height) return false;
            return min.Width < rect.Width;
        }

        private static void DrawBottomDragBar(Graphics g, ClientRectangle bounds)
        {
            var rect = WorkItemDragService.GetBottomBarRect(bounds);
            var points = WorkItemDragService.GetBottomBarLine(bounds);
            g.FillRectangle(Brushes.DarkBlue, rect.Value);
            g.DrawLine(Pens.White, points.Item1, points.Item2);
        }

        private static void DrawTopDragBar(Graphics g, ClientRectangle bounds)
        {
            var rect = WorkItemDragService.GetTopBarRect(bounds);
            var points = WorkItemDragService.GetTopBarLine(bounds);
            g.FillRectangle(Brushes.DarkBlue, rect.Value);
            g.DrawLine(Pens.White, points.Item1, points.Item2);
        }

        private static MileStones GetMileStonesWithToday(ViewData viewData)
        {
            var result = new MileStones();
            var today = viewData.Original.Callender.NearestFromToday;
            if (viewData.Original.Callender.Contains(today))
            {
                result.Add(new MileStone("Today", new Project("Pro1"), today, Color.Red, null, TaskState.Active));
            }
            foreach (var m in viewData.Original.MileStones) result.Add(m.Clone());
            return result;
        }

        private bool DoesFilterSuppressMileStoneDraw(MileStone m)
        {
            if (m == null) return true;
            if (m.Name.Equals("Today")) return false;
            return !m.IsMatchFilter(_viewData.Filter.MSFilterSearchPattern);
        }

        private void DrawMileStones(Font font, Graphics g, MileStones mileStones)
        {
            var range = _grid.VisibleRowColRange;
            var visibleArea = _grid.GetVisibleRect(false, false);
            foreach (var r in range.Rows)
            {
                var mSs = mileStones.Where((i) => i.Day.Equals(_grid.Row2Day(r)));
                int x = 0;
                foreach (var m in mSs)
                {
                    if (DoesFilterSuppressMileStoneDraw(m)) continue;
                    var rect = _grid.GetRectClient(range.LeftCol, r, 1, visibleArea);
                    if (rect.IsEmpty) continue;
                    using (var brush = new SolidBrush(m.Color))
                    {
                        var y = m.Name.Equals("Today") ? rect.Value.Top : rect.Value.Bottom;
                        g.FillRectangle(brush, 0, y, _grid.VisibleSize.Width, 1);
                        g.DrawString(m.Name, font, brush, x, y - 10);
                        if (!m.Name.Equals("Today")) x += (int)g.MeasureString(m.Name.ToString(), font).Width;
                    }
                }
            }
        }

        private void DrawSameNameWorkItem(Graphics g, Font font)
        {
            var range = _grid.VisibleRowColRange;
            var members = _viewData.FilteredItems.Members;
            foreach (var c in range.Cols)
            {
                var m = _grid.Col2Member(c);
                foreach (var wi in GetVisibleWorkItems(m, range.TopRow, range.RowCount))
                {
                    if (!_viewData.Selected.IsSameName(wi)) continue;
                    DrawWorkItemClient(wi, Pens.LightGreen, font, g, members);
                }
            }
        }

        private void DrawTemporaryWorkItem(WorkItem wi, ClientRectangle rect, Pen edge, Font font, Graphics g, IEnumerable<Member> members)
        {
            var res = _grid.GetWorkItemDrawRectClient(wi, members);
            if (res.IsEmpty) return;
            g.FillRectangle(BrushCache.GetBrush(Control.DefaultBackColor), res.Value);

            if (rect.IsEmpty) return;
            var moveX = _grid.Global2Raw(Cursor.Position).X - _dragStartInfo().Location.X;
            var moveY = _grid.Global2Raw(Cursor.Position).Y - _dragStartInfo().Location.Y;
            var tempRect = new Rectangle(rect.Value.X + moveX, rect.Value.Y + moveY, res.Value.Width, res.Value.Height);
            DrawWorkItemCore(wi, edge, font, g, tempRect);
        }

        private void DrawSelectedWorkItemBound(Graphics g, Font font)
        {
            for (int i = 0; i < _viewData.Selected.Count(); i++)
            {
                if (_isDragMoving() && !WorkItemDragService.IsCurLocationOnHitArea(_grid, _grid.Global2Raw(Cursor.Position)))
                {
                    DrawTemporaryWorkItem(_viewData.Selected.ElementAt(i), _dragStartInfo().Rects.ElementAt(i), Pens.LightGreen, font, g, _viewData.FilteredItems.Members);
                }
                else
                {
                    DrawWorkItemClient(_viewData.Selected.ElementAt(i), Pens.LightGreen, font, g, _viewData.FilteredItems.Members);
                }
            }

            DrawSameNameWorkItem(g, font);

            if (!_isDragActive())
            {
                foreach (var w in _viewData.Selected)
                {
                    var rect = _grid.GetWorkItemDrawRectClient(w, _viewData.FilteredItems.Members);
                    if (rect.IsEmpty) continue;
                    DrawTopDragBar(g, rect);
                    DrawBottomDragBar(g, rect);
                }
            }
        }

        private void DrawCalender(Font font, Graphics g)
        {
            var year = 0;
            var month = 0;
            var day = 0;
            var range = _grid.VisibleRowColRange;
            var rowHeight = (int)g.MeasureString("A", font).Height;
            var visibleArea = _grid.GetVisibleRect(false, true);
            foreach (var r in range.Rows)
            {
                var d = _grid.Row2Day(r);
                if (_grid.IsSelected(d))
                {
                    var rectYear = _grid.GetRectClient(WorkItemGridConstants.YearCol, r, 1, visibleArea);
                    var rectMonth = _grid.GetRectClient(WorkItemGridConstants.MonthCol, r, 1, visibleArea);
                    var rectDay = _grid.GetRectClient(WorkItemGridConstants.DayCol, r, 1, visibleArea);
                    var rect = new RectangleF(rectYear.Value.Left, rectYear.Value.Top, rectYear.Value.Width + rectMonth.Value.Width + rectDay.Value.Width, rectYear.Value.Height);
                    g.FillRectangle(BrushCache.GetBrush(Color.LightSkyBlue), rect);
                }
                if (year != d.Year)
                {
                    var rectYear = _grid.GetRectClient(WorkItemGridConstants.YearCol, r, 1, visibleArea);
                    if (!rectYear.IsEmpty)
                    {
                        month = 0;
                        day = 0;
                        year = DrawYear(font, g, d, rectYear, rowHeight);
                    }
                }
                if (month != d.Month)
                {
                    var rectMonth = _grid.GetRectClient(WorkItemGridConstants.MonthCol, r, 1, visibleArea);
                    if (!rectMonth.IsEmpty)
                    {
                        day = 0;
                        month = DrawMonth(font, g, d, rectMonth, rowHeight);
                    }
                }
                if (day != d.Day)
                {
                    var rectDay = _grid.GetRectClient(WorkItemGridConstants.DayCol, r, 1, visibleArea);
                    if (!rectDay.IsEmpty)
                    {
                        day = d.Day;
                        g.DrawString(day.ToString(), font, Brushes.Black, rectDay.Value);
                    }
                }
            }
        }

        public void InvalidateMembers(IEnumerable<Member> updatedMembers)
        {
            _imageBuffer.Invalidate(updatedMembers, _grid);
        }

        private static int DrawMonth(Font font, Graphics g, CallenderDay d, ClientRectangle rectMonth, int height)
        {
            int month = d.Month;
            rectMonth.Offset(0, height);
            rectMonth.Inflate(0, height);
            g.DrawString(month.ToString(), font, Brushes.Black, rectMonth.Value);
            return month;
        }

        private static int DrawYear(Font font, Graphics g, CallenderDay d, ClientRectangle rectYear, int height)
        {
            int year = d.Year;
            rectYear.Offset(0, height);
            rectYear.Inflate(0, height);
            g.DrawString(year.ToString(), font, Brushes.Black, rectYear.Value);
            return year;
        }

        private void DrawMember(Font font, Graphics g)
        {
            var range = _grid.VisibleRowColRange;
            var visibleAread = _grid.GetVisibleRect(true, false);
            foreach (var c in range.Cols)
            {
                var m = _grid.Col2Member(c);
                var rectCompany = _grid.GetRectClient(c, WorkItemGridConstants.CompanyRow, 1, visibleAread);
                var rectLastName = _grid.GetRectClient(c, WorkItemGridConstants.LastNameRow, 1, visibleAread);
                var rectFirstName = _grid.GetRectClient(c, WorkItemGridConstants.FirstNameRow, 1, visibleAread);
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
                    _imageBuffer?.Dispose();
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
