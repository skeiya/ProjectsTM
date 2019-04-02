using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TaskManagement.UI
{
    public partial class ManagementWokingDaysForm : Form
    {
        private Callender _callender;

        public ManagementWokingDaysForm(Callender callender)
        {
            InitializeComponent();
            this._callender = callender;
            UpdateListView();
        }

        private void UpdateListView()
        {
            listView1.Items.Clear();
            foreach (var d in _callender.Days)
            {
                listView1.Items.Add(d.ToString());
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            foreach(var d in GetSelectedDays())
            {
                _callender.Delete(d);
            }
            UpdateListView();
        }

        private IEnumerable<CallenderDay> GetSelectedDays()
        {
            var result = new List<CallenderDay>();
            foreach(int index in listView1.SelectedIndices)
            {
                result.Add(CallenderDay.Parse(listView1.Items[index].Text));
            }
            return result;
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            var d = CallenderDay.Parse(textBox1.Text);
            if (d == null) return;
            _callender.Days.Add(d);
            _callender.Days.Sort();
            UpdateListView();
        }
    }
}
