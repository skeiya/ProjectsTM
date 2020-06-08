using System;
using System.Windows.Forms;
using TaskManagement.Model;

namespace TaskManagement.Service
{
    public class ToolTipService
    {
        private ToolTip _toolTip = null;

        public ToolTipService(ToolTip toolTip)
        {
            if (toolTip == null) return;
            _toolTip = toolTip;
        }

        public void Update(Control c, WorkItem wi)
        {
            if (c == null) return;
            if (wi == null) { this.Hide(c); return; }
            string s =
                "名前:" + wi.Name + Environment.NewLine
                + "物件:" + wi.Project.ToString() + Environment.NewLine
                + "担当:" + wi.AssignedMember.ToString() + Environment.NewLine
                + "タグ:" + wi.Tags.ToString() + Environment.NewLine
                + "状態:" + wi.State.ToString() + Environment.NewLine + Environment.NewLine
                + "開始:" + wi.Period.From.ToString() + Environment.NewLine
                + "終了:" + wi.Period.To.ToString() + Environment.NewLine;
            if (!s.Equals(_toolTip.GetToolTip(c))) _toolTip.SetToolTip(c, s);
        }

        public void Hide(Control c) { _toolTip.Hide(c); }
    }
}
