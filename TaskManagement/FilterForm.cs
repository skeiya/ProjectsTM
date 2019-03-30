using System;
using System.Windows.Forms;

namespace TaskManagement
{
    internal partial class FilterForm : Form
    {
        private ViewData _viewData;

        public FilterForm(ViewData viewData)
        {
            InitializeComponent();

            _viewData = viewData;

            foreach (var m in _viewData.Original.Members)
            {
                checkedListBox1.Items.Add(m, IsContain(m));
            }
        }

        private bool IsContain(Member m)
        {
            foreach (var f in _viewData.GetFilteredMembers())
            {
                if (!m.Equals(f)) return true;
            }
            return false;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            _viewData.SetFilter(GetFilter());
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
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
            return new Period(from, to, _viewData.Original.Callender);
        }

        private Members GetMembersFilter()
        {
            var result = new Members();
            var remains = GetRemainingMemger();
            foreach (var m in _viewData.Original.Members)
            {
                if (remains.Contain(m)) continue;
                result.Add(m);
            }
            return result;
        }

        private Members GetRemainingMemger()
        {
            var result = new Members();
            foreach (var c in checkedListBox1.CheckedItems)
            {
                if (c is Member m) result.Add(m);
            }
            return result;
        }
    }
}
