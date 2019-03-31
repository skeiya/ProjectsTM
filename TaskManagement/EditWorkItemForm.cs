using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            return _wi;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!CheckEdit()) return;
            _wi.Edit(GetProject(), GetWorkItemName(), GetPeriod(_callender), GetAssignedMember(), GetTags());
            DialogResult = DialogResult.OK;
            Close();
        }

        bool CheckEdit()
        {
            return CreateWorkItem(null) != null;
        }

        private WorkItem CreateWorkItem(Callender callender)
        {
            var p = GetProject();
            if (p == null) return null;
            var w = GetWorkItemName();
            if (w == null) return null;
            var period = GetPeriod(callender);
            if (period == null) return null;
            var m = GetAssignedMember();
            if (m == null) return null;
            return new WorkItem(p, w, GetTags(), period, m);
        }

        private Member GetAssignedMember()
        {
            return Member.Parse(textBoxMember.Text);
        }

        private Period GetPeriod(Callender callender)
        {
            var from = CallenderDay.Parse(textBoxFrom.Text);
            var to = CallenderDay.Parse(textBoxTo.Text);
            if (from == null || to == null) return null;
            return new Period(from, to, callender);
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

        private void Button4_Click(object sender, EventArgs e)
        {

        }
    }
}
