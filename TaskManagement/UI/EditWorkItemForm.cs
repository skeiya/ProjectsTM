using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TaskManagement.Model;

namespace TaskManagement
{
    public partial class EditWorkItemForm : Form
    {
        private readonly WorkItem _wi;
        private readonly Callender _callender;

        public EditWorkItemForm(WorkItem wi, Callender callender)
        {
            InitializeComponent();
            if (wi == null) wi = new WorkItem();
            textBoxWorkItemName.Text = wi.Name == null ? string.Empty : wi.Name;
            textBoxProject.Text = wi.Project == null ? string.Empty : wi.Project.ToString();
            textBoxMember.Text = wi.AssignedMember == null ? string.Empty : wi.AssignedMember.ToSerializeString();
            textBoxFrom.Text = wi.Period == null ? string.Empty : wi.Period.From.ToString();
            textBoxTo.Text = wi.Period == null ? string.Empty : wi.Period.To.ToString();
            textBoxTags.Text = wi.Tags == null ? string.Empty : wi.Tags.ToString();
            this._wi = wi;
            this._callender = callender;
        }

        public WorkItem GetWorkItem(Callender callender)
        {
            var period = GetPeriod(_callender, textBoxFrom.Text, textBoxTo.Text, radioButtonDayCount.Checked);
            return new WorkItem(GetProject(), GetWorkItemName(), GetTags(), period, GetAssignedMember());
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!CheckEdit()) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        bool CheckEdit()
        {
            return CreateWorkItem(_callender) != null;
        }

        private WorkItem CreateWorkItem(Callender callender)
        {
            var p = GetProject();
            if (p == null) return null;
            var w = GetWorkItemName();
            if (w == null) return null;
            var period = GetPeriod(callender, textBoxFrom.Text, textBoxTo.Text, radioButtonDayCount.Checked);
            if (period == null) return null;
            var m = GetAssignedMember();
            if (m == null) return null;
            return new WorkItem(p, w, GetTags(), period, m);
        }

        private Member GetAssignedMember()
        {
            return Member.Parse(textBoxMember.Text);
        }

        private static Period GetPeriod(Callender callender, string fromText, string toText, bool isToCount)
        {
            var from = GetDayByDate(fromText);
            var to = isToCount ? GetDayByCount(toText, from, callender) : GetDayByDate(toText);
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

        private void RadioButtonDayCount_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEndDay();
        }

        private void UpdateEndDay()
        {
            if (radioButtonDayCount.Checked)
            {
                var period = GetPeriod(_callender, textBoxFrom.Text, textBoxTo.Text, false);
                textBoxTo.Text = period == null ? string.Empty : _callender.GetPeriodDayCount(period).ToString();
            }
            else
            {
                var period = GetPeriod(_callender, textBoxFrom.Text, textBoxTo.Text, true);
                textBoxTo.Text = period == null ? string.Empty : period.To.ToString();
            }
        }
    }
}
