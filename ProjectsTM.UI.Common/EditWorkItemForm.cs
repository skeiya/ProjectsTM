using ProjectsTM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectsTM.UI.Common
{
    public partial class EditWorkItemForm : Form
    {
        private readonly WorkItem _wi;
        private readonly Callender _callender;
        private readonly IEnumerable<Member> _members;

        public EditWorkItemForm(WorkItem wi, Callender callender, IEnumerable<Member> members)
        {
            InitializeComponent();
            if (wi == null) wi = new WorkItem();
            this._wi = wi;
            this._callender = callender;
            this._members = members;
            textBoxWorkItemName.Text = wi.Name == null ? string.Empty : wi.Name;
            textBoxProject.Text = wi.Project == null ? string.Empty : wi.Project.ToString();
            textBoxMember.Text = wi.AssignedMember == null ? string.Empty : wi.AssignedMember.ToSerializeString();
            textBoxFrom.Text = wi.Period == null ? string.Empty : wi.Period.From.ToString();
            textBoxTo.Text = wi.Period == null ? string.Empty : _callender.GetPeriodDayCount(wi.Period).ToString();
            textBoxTags.Text = wi.Tags == null ? string.Empty : wi.Tags.ToString();
            textBoxDescription.Text = wi.Description == null ? string.Empty : wi.Description;
            InitDropDownList(wi.State);
            UpdateEndDay();
        }

        private void InitDropDownList(TaskState state)
        {
            comboBoxState.Items.Clear();
            foreach (TaskState e in Enum.GetValues(typeof(TaskState)))
            {
                if (e == TaskState.New) continue;
                comboBoxState.Items.Add(e);
            }
            comboBoxState.SelectedItem = state;
        }

        public WorkItem GetWorkItem()
        {
            var period = GetPeriod(_callender, textBoxFrom.Text, textBoxTo.Text);
            return new WorkItem(GetProject(), GetWorkItemName(), GetTags(), period, GetAssignedMember(), GetState(), GetDescrption());
        }

        private string GetDescrption() { return textBoxDescription.Text; }

        private TaskState GetState()
        {
            return (TaskState)comboBoxState.SelectedItem;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!CheckEdit()) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateAssignedMember()
        {
            return _members.Contains(Member.Parse(textBoxMember.Text));
        }

        bool CheckEdit()
        {
            if (!ValidateAssignedMember())
            {
                MessageBox.Show("担当者が存在しません。", "不正な入力", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return CreateWorkItem(_callender) != null;
        }

        private WorkItem CreateWorkItem(Callender callender)
        {
            var p = GetProject();
            if (p == null) return null;
            var w = GetWorkItemName();
            if (w == null) return null;
            var period = GetPeriod(callender, textBoxFrom.Text, textBoxTo.Text);
            if (period == null) return null;
            var m = GetAssignedMember();
            if (m == null) return null;
            return new WorkItem(p, w, GetTags(), period, m, GetState(), GetDescrption());
        }

        private Member GetAssignedMember()
        {
            return Member.Parse(textBoxMember.Text);
        }

        private static Period GetPeriod(Callender callender, string fromText, string toText)
        {
            var from = GetDayByDate(fromText);
            var to = GetDayByCount(toText, from, callender);
            if (from == null || to == null) return null;
            var result = new Period(from, to);
            if (callender.GetPeriodDayCount(result) == 0) return null;
            return result;
        }

        private static CallenderDay GetDayByDate(string text)
        {
            return CallenderDay.Parse(text);
        }

        private static CallenderDay GetDayByCount(string countText, CallenderDay from, Callender callender)
        {
            var dayCount = 0;
            if (!int.TryParse(countText, out dayCount)) return null;
            return callender.ApplyOffset(from, dayCount - 1);
        }

        private Tags GetTags()
        {
            return new Tags(textBoxTags.Text.Split('|').ToList());
        }

        private string GetWorkItemName()
        {
            if (string.IsNullOrEmpty(textBoxWorkItemName.Text)) return null;
            return textBoxWorkItemName.Text;
        }

        private Project GetProject()
        {
            //return comboBoxProject.SelectedItem as Project;
            return new Project(textBoxProject.Text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void UpdateEndDay()
        {
            var period = GetPeriod(_callender, textBoxFrom.Text, textBoxTo.Text);
            textBoxTo.Text = period == null ? string.Empty : _callender.GetPeriodDayCount(period).ToString();
        }

        private void buttonRegexEscape_Click(object sender, EventArgs e)
        {
            var wi = CreateWorkItem(_callender);
            if (wi == null) return;
            using (var dlg = new EditStringForm(Regex.Escape(wi.ToString())))
            {
                dlg.Text = "正規表現エスケープ";
                dlg.ReadOnly = true;
                dlg.ShowDialog();
            }
        }
    }
}
