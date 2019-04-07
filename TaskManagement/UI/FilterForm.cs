using System;
using System.Linq;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class FilterForm : Form
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

            if (viewData.Filter == null || viewData.Filter.Period == null)
            {
                ClearPeriodFilter();
            }
            else
            {
                textBoxFrom.Text = viewData.Filter.Period.From.ToString();
                textBoxTo.Text = viewData.Filter.Period.To.ToString();
            }

            if (viewData.Filter != null && !string.IsNullOrEmpty(viewData.Filter.WorkItem))
            {
                textBoxWorkItem.Text = viewData.Filter.WorkItem;
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
            if (GetPeriodFilter() == null) return;
            _viewData.SetFilter(GetFilter());
            Close();
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
            if (!_viewData.Original.Callender.Days.Contains(from)) return null;
            if (!_viewData.Original.Callender.Days.Contains(to)) return null;
            return new Period(from, to);
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

        private void buttonClearWorkItem_Click(object sender, EventArgs e)
        {
            textBoxWorkItem.Text = string.Empty;
        }

        private void buttonClearPeriod_Click(object sender, EventArgs e)
        {
            ClearPeriodFilter();
        }

        private void ClearPeriodFilter()
        {
            var days = _viewData.Original.Callender.Days;
            textBoxFrom.Text = days.First().ToString();
            textBoxTo.Text = days.Last().ToString();
        }

        private void buttonClearMembers_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                checkedListBox1.SetItemChecked(index, true);
            }
        }
    }
}
