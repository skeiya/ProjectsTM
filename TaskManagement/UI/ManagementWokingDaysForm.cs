using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TaskManagement.UI
{
    public partial class ManagementWokingDaysForm : Form
    {
        private Callender _callender;
        private readonly WorkItems _workItems;

        public ManagementWokingDaysForm(Callender callender, WorkItems workItems)
        {
            InitializeComponent();
            this._callender = callender;
            this._workItems = workItems;
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
            var selectedDay = GetSelectedDay();
            if (selectedDay == null) return;
            if (!Deletable(selectedDay)) return;
            _callender.Delete(selectedDay);
            UpdateListView();
        }

        private bool Deletable(CallenderDay selectedDay)
        {
            foreach(var w in _workItems)
            {
                if (w.Period.From.Equals(selectedDay)) return false;
                if (w.Period.To.Equals(selectedDay)) return false;
            }
            return true;
        }

        private CallenderDay GetSelectedDay()
        {
            foreach (int index in listView1.SelectedIndices)
            {
                return CallenderDay.Parse(listView1.Items[index].Text);
            }
            return null;
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
