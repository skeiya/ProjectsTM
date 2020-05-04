using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TaskManagement.Model;
using TaskManagement.UI;

namespace TaskManagement.ViewModel
{
    class ImageBuffer : IDisposable
    {
        private Bitmap _bitmap;
        private Graphics _bitmapGraphics;
        private Dictionary<Member, HashSet<WorkItem>> _validList = new Dictionary<Member, HashSet<WorkItem>>();

        public Graphics Graphics => _bitmapGraphics;

        public Image Image => _bitmap;

        internal ImageBuffer(int width, int height)
        {
            if (_bitmap == null)
            {
                _bitmap = new Bitmap(width, height);
                _bitmapGraphics = System.Drawing.Graphics.FromImage(_bitmap);
                _bitmapGraphics.Clear(Control.DefaultBackColor);
            }

        }

        internal void Validate(WorkItem wi)
        {
            if (IsValid(wi)) return;

            if (_validList.TryGetValue(wi.AssignedMember, out var workItems))
            {
                workItems.Add(wi);
                return;
            }
            _validList.Add(wi.AssignedMember, new HashSet<WorkItem>() { wi });
        }

        internal bool IsValid(WorkItem wi)
        {
            if (!_validList.TryGetValue(wi.AssignedMember, out var workItems)) return false;
            return workItems.Contains(wi);
        }

        internal void Invalidate(IEnumerable<Member> members, IWorkItemGrid grid)
        {
            //該当メンバの列を少し広めにクリアFill
            foreach (var m in members)
            {
                var rect = grid.GetMemberDrawRect(m);
                if (!rect.HasValue) continue;
                var newRect = rect.Value;
                newRect.Inflate(1, 1);
                _bitmapGraphics.FillRectangle(BrushCache.GetBrush(Control.DefaultBackColor), newRect);
            }
            foreach (var m in grid.GetNeighbers(members))
            {
                _validList.Remove(m);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _bitmapGraphics.Dispose();
                    _bitmap.Dispose();
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~InvalidArea()
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
