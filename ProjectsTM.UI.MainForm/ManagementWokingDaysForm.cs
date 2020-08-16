using ProjectsTM.Model;
using ProjectsTM.Service;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI
{
    public partial class ManagementWokingDaysForm : Form
    {
        private Callender _callender;
        private readonly WorkItems _workItems;
        private readonly object _lockobj;

        public ManagementWokingDaysForm(Callender callender, WorkItems workItems, object lockobj)
        {
            InitializeComponent();
            this._callender = callender;
            this._workItems = workItems;
            this._lockobj = lockobj;
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
            lock (_lockobj)
            {
                _callender.Delete(selectedDay);
            }
            UpdateListView();
        }

        private bool Deletable(CallenderDay selectedDay)
        {
            foreach (var w in _workItems)
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
            lock (_lockobj)
            {
                _callender.Days.Add(d);
                _callender.Days.Sort();
            }
            UpdateListView();
        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var cal = CsvReadService.ReadWorkingDays(dlg.FileName);
                lock (_lockobj)
                {
                    foreach (var d in cal.Days)
                    {
                        _callender.Days.Add(d);
                    }
                    _callender.Days.Sort();
                }
            }
            UpdateListView();
        }
    }
}
