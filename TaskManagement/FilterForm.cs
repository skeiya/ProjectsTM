using System;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class FilterForm : Form
    {
        private IPeriodCalculator _callender;

        public Filter Filter { get; internal set; }

        public FilterForm(Members fullMembers, IPeriodCalculator callender)
        {
            InitializeComponent();
            _callender = callender;

            foreach (var m in fullMembers)
            {
                checkedListBox1.Items.Add(m);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Filter = GetFilter();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private Filter GetFilter()
        {
            return new Filter(GetWorkItemFilter(), GetPeriodFilter(), GetMembersFilter());
        }

        private string GetWorkItemFilter()
        {
            if (string.IsNullOrEmpty(textBoxWorkItem.Text)) return null;
            return textBoxWorkItem.Text;
        }

        private Period GetPeriodFilter()
        {
            var from = CallenderDay.Parse(textBoxFrom.Text);
            var to = CallenderDay.Parse(textBoxTo.Text);
            if (from == null || to == null) return null;
            return new Period(from, to, _callender);
        }

        private Members GetMembersFilter()
        {
            var result = new Members();
            foreach(var c in checkedListBox1.CheckedItems)
            {
                if (c is Member m) result.Add(m);
            }
            return result;
        }
    }
}
