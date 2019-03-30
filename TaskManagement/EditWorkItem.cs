using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManagement
{
    internal partial class EditWorkItem : Form
    {
        public EditWorkItem(AppData appData)
        {
            InitializeComponent();
            AppData = appData;

            foreach (var p in appData.Projects)
            {
                comboBoxProject.Items.Add(p);
            }
            foreach (var m in appData.Members)
            {
                comboBoxAssignedMemer.Items.Add(m);
            }
        }

        public WorkItem WorkItem { get; private set; }
        public AppData AppData { get; }

        private void Button1_Click(object sender, EventArgs e)
        {
            var w = CreateWorkItem();
            if (w == null) return;
            WorkItem = w;
            DialogResult = DialogResult.OK;
            Close();
        }

        private WorkItem CreateWorkItem()
        {
            var p = GetProject();
            if (p == null) return null;
            var w = GetWorkItemName();
            if (w == null) return null;
            var period = GetPeriod();
            if (period == null) return null;
            var m = GetAssignedMember();
            if (m == null) return null;
            return new WorkItem(p, w, GetTags(), period, m);
        }

        private Member GetAssignedMember()
        {
            var words = comboBoxAssignedMemer.Text.Split(' ');
            if (words.Count() == 0) return null;
            if (words.Count() == 1) return new Member(words[0], string.Empty, textBoxCompany.Text);
            return new Member(words[0], words[1], textBoxCompany.Text);
        }

        private Period GetPeriod()
        {
            var from = CallenderDay.Parse(textBoxFrom.Text);
            var to = CallenderDay.Parse(textBoxTo.Text);
            if (from == null || to == null) return null;
            return new Period(from, to, AppData.Callender);
        }

        private List<string> GetTags()
        {
            return textBoxTags.Text.Split('|').ToList();
        }

        private string GetWorkItemName()
        {
            if (string.IsNullOrEmpty(textBoxWorkItemName.Text)) return null;
            return textBoxWorkItemName.Text;
        }

        private Project GetProject()
        {
            //return comboBoxProject.SelectedItem as Project;
            return new Project(comboBoxProject.Text);
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
