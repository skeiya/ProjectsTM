using ProjectsTM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectsTM.UI.Common
{
    public partial class EditWorkItemForm : BaseForm
    {
        private readonly Callender _callender;
        private readonly IEnumerable<Member> _members;

        public EditWorkItemForm(WorkItem wi, WorkItems workItems, Callender callender, IEnumerable<Member> members)
        {
            InitializeComponent();
            if (wi == null) wi = new WorkItem();
            this._callender = callender;
            this._members = members;
            comboBoxWorkItemName.Text = wi.Name ?? string.Empty;
            comboBoxProject.Text = wi.Project == null ? string.Empty : wi.Project.ToString();
            comboBoxMember.Text = wi.AssignedMember == null ? string.Empty : wi.AssignedMember.ToSerializeString();
            textBoxFrom.Text = wi.Period == null ? string.Empty : wi.Period.From.ToString();
            textBoxTo.Text = wi.Period == null ? string.Empty : _callender.GetPeriodDayCount(wi.Period).ToString();
            textBoxTags.Text = wi.Tags == null ? string.Empty : wi.Tags.ToString();
            textBoxDescription.Text = wi.Description ?? string.Empty;
            InitDropDownList(wi.State);
            InitCombbox(members, workItems);
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

        private void InitCombbox(IEnumerable<Member> members, WorkItems workItems)
        {
            foreach (var m in members)
            {
                comboBoxMember.Items.Add(m.ToSerializeString());
            }
            comboBoxWorkItemName.Items.AddRange(GetTasks(workItems));
            comboBoxProject.Items.AddRange(GetProjects(workItems).ToArray());
        }
        private static List<Project> GetProjects(WorkItems workItems)
        {
            var result = new List<Project>();
            foreach (var wi in workItems)
            {
                if (!result.Contains(wi.Project)) result.Add(wi.Project);
            }
            return result;
        }
        private static string[] GetTasks(WorkItems workItems)
        {
            var result = new List<string>();
            foreach (var wi in workItems)
            {
                if (!result.Contains(wi.Name)) result.Add(wi.Name);
            }
            return result.ToArray();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!CheckEdit()) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonRegexEscape_Click(object sender, EventArgs e)
        {
            var wi = CreateWorkItem(_callender);
            if (wi == null) return;
            using (var dlg = new EditMemberForm(Regex.Escape(wi.ToString())))
            {
                dlg.Text = "正規表現エスケープ";
                dlg.ReadOnly = true;
                dlg.ShowDialog();
            }
        }

        private bool ValidateAssignedMember()
        {
            return _members.Contains(Member.Parse(comboBoxMember.Text));
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
            if (!TryGetPeriod(callender, textBoxFrom.Text, textBoxTo.Text, out var period)) return null;
            if (!TryGetAssignedMember(out var m)) return null;
            return new WorkItem(GetProject(), GetWorkItemName(), GetTags(), period, m, GetState(), GetDescrption());
        }

        private bool TryGetAssignedMember(out Member result)
        {
            result = Member.Parse(comboBoxMember.Text);
            return result != null;
        }

        private static bool TryGetPeriod(Callender callender, string fromText, string toText, out Period result)
        {
            result = Period.Invalid;
            var from = GetDayByDate(fromText);
            if (!TryGetDayByCount(toText, from, callender, out var to)) return false;
            if (from == null || to == null) return false;
            result = new Period(from, to);
            if (callender.GetPeriodDayCount(result) == 0) return false;
            return true;
        }

        private static CallenderDay GetDayByDate(string text)
        {
            return CallenderDay.Parse(text);
        }

        private static bool TryGetDayByCount(string countText, CallenderDay from, Callender callender, out CallenderDay result)
        {
            result = CallenderDay.Invalid;
            if (!int.TryParse(countText, out int dayCount)) return false;
            return callender.TryApplyOffset(from, dayCount - 1, out result);
        }

        private Tags GetTags()
        {
            return new Tags(textBoxTags.Text.Split('|').ToList());
        }

        private string GetWorkItemName()
        {
            return comboBoxWorkItemName.Text;
        }

        private Project GetProject()
        {
            return new Project(comboBoxProject.Text);
        }

        public bool TryGetWorkItem(out WorkItem result)
        {
            result = null;
            if (!TryGetPeriod(_callender, textBoxFrom.Text, textBoxTo.Text, out var period)) return false;
            if (!TryGetAssignedMember(out var m)) return false;
            result = new WorkItem(GetProject(), GetWorkItemName(), GetTags(), period, m, GetState(), GetDescrption());
            return true;
        }

        private string GetDescrption() { return textBoxDescription.Text; }

        private TaskState GetState()
        {
            return (TaskState)comboBoxState.SelectedItem;
        }

        private void UpdateEndDay()
        {
            if (!TryGetPeriod(_callender, textBoxFrom.Text, textBoxTo.Text, out var period)) return;
            textBoxTo.Text = _callender.GetPeriodDayCount(period).ToString();
        }
    }
}
