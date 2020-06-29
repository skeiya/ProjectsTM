using System;
using System.Windows.Forms;
using TaskManagement.Model;

namespace TaskManagement.Service
{
    public class ToolTipService : IDisposable
    {
        private ToolTip _toolTip = new ToolTip();
        private bool disposedValue;
        private readonly Control _parentControl;

        public ToolTipService(Control c)
        {
            _toolTip.ShowAlways = true;
            this._parentControl = c;
        }

        public void Update(WorkItem wi, int days)
        {
            if (wi == null) { this.Hide(); return; }
            string s =
                "名前:" + wi.Name + Environment.NewLine
                + "物件:" + wi.Project.ToString() + Environment.NewLine
                + "担当:" + wi.AssignedMember.ToString() + Environment.NewLine
                + "タグ:" + wi.Tags.ToString() + Environment.NewLine
                + "状態:" + wi.State.ToString() + Environment.NewLine + Environment.NewLine
                + "開始:" + wi.Period.From.ToString() + Environment.NewLine
                + "終了:" + wi.Period.To.ToString() + Environment.NewLine;
            if(days > 0) s += "人日:" + days.ToString() + Environment.NewLine;

            if (!string.IsNullOrEmpty(wi.Description))
            {
                s += Environment.NewLine
                    + "---作業項目メモ---" + Environment.NewLine
                    + wi.Description + Environment.NewLine;
            }
            if (!s.Equals(_toolTip.GetToolTip(_parentControl))) _toolTip.SetToolTip(_parentControl, s);
        }

        public void Hide() { _toolTip.Hide(_parentControl); }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _toolTip.Dispose();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~ToolTipService()
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
