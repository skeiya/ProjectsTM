using System;
using System.Text;
using System.Windows.Forms;
using ProjectsTM.Model;
using ProjectsTM.ViewModel;

namespace ProjectsTM.Service
{
    public class ToolTipService : IDisposable
    {
        private ToolTip _toolTip = new ToolTip();
        private bool disposedValue;
        private readonly ViewData _viewData;
        private readonly Control _parentControl;

        public ToolTipService(Control c, ViewData viewData)
        {
            this._toolTip.ShowAlways = true;
            this._parentControl = c;
            this._viewData = viewData;
        }

        private string GetDescrptionFromOtherWorkItem(WorkItem hoveringWorkItem)
        {
            foreach (var w in _viewData.GetFilteredWorkItems())
            {
                if (w.Name != hoveringWorkItem.Name) continue;
                if (w.Equals(hoveringWorkItem)) continue;
                if (string.IsNullOrEmpty(w.Description)) continue;

                return w.Description +
                    Environment.NewLine +
                    "※同名作業項目のメモ※";
            }
            return null;
        }

        private string CreateDescriptionContent(WorkItem hoveringWorkItem)
        {
            if (!string.IsNullOrEmpty(hoveringWorkItem.Description)) return hoveringWorkItem.Description;
            return GetDescrptionFromOtherWorkItem(hoveringWorkItem);
        }

        private static void SetDescriptionContent(StringBuilder allStrForTooltip, string strDescription)
        {
            if (strDescription == null) return;
            allStrForTooltip.AppendLine();
            allStrForTooltip.AppendLine("---作業項目メモ---");
            allStrForTooltip.AppendLine(strDescription);
        }

        private void AddDescription(StringBuilder allStringForTooltip, WorkItem hoveringWorkItem)
        {
            string result = CreateDescriptionContent(hoveringWorkItem);
            SetDescriptionContent(allStringForTooltip, result);
        }

        private string CreateStrForTooltip(WorkItem wi, int days)
        {
            StringBuilder s = new StringBuilder();
            s.Append("名前:"); s.AppendLine(wi.Name);
            s.Append("物件:"); s.AppendLine(wi.Project.ToString());
            s.Append("担当:"); s.AppendLine(wi.AssignedMember.ToString());
            s.Append("タグ:"); s.AppendLine(wi.Tags.ToString());
            s.Append("状態:"); s.AppendLine(wi.State.ToString());
            s.AppendLine();
            s.Append("開始:"); s.AppendLine(wi.Period.From.ToString());
            s.Append("終了:"); s.AppendLine(wi.Period.To.ToString());
            if (days > 0) { s.Append("人日:"); s.AppendLine(days.ToString()); }
            AddDescription(s, wi);

            return s.ToString();
        }

        public void Update(WorkItem wi, int days)
        {
            if (wi == null) { this.Hide(); return; }
            string s = CreateStrForTooltip(wi, days);
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
