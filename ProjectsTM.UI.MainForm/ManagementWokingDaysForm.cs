using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProjectsTM.UI.MainForm
{
    public partial class ManagementWokingDaysForm : BaseForm
    {
        private List<CallenderDay> _days;
        private event EventHandler<IEnumerable<CallenderDay>> UpdateWokingDays;
        private Func<CallenderDay, bool> IsDeletableWokingDay;

        public ManagementWokingDaysForm(
            Callender callender, 
            EventHandler<IEnumerable<CallenderDay>> updateWokinggDays, 
            Func<CallenderDay, bool> isDeletableWokingDay)
        {
            InitializeComponent();
            this._days = new List<CallenderDay>();
            foreach (var d in callender.Days) _days.Add(d);
            this.UpdateWokingDays += updateWokinggDays;
            this.IsDeletableWokingDay += isDeletableWokingDay;
            ApplyEdit();
        }

        private void ApplyEdit()
        {
            _days = _days.Distinct().ToList();
            _days.Sort();
            UpdateWokingDays?.Invoke(this, _days);
            UpdateListView();
        }

        private void UpdateListView()
        {
            listView1.Items.Clear();
            foreach (var d in _days)
            {
                listView1.Items.Add(d.ToString());
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            var selectedDays = GetSelectedDays();
            if (selectedDays.Count == 0) return;

            foreach (var d in selectedDays)
            {
                if (!IsDeletableWokingDay(d)) return;
                _days.Remove(d);
            }
            ApplyEdit();
        }

        private List<CallenderDay> GetSelectedDays()
        {
            var selectedDays = new List<CallenderDay>();
            foreach (int index in listView1.SelectedIndices)
            {
                selectedDays.Add(CallenderDay.Parse(listView1.Items[index].Text));
            }
            return selectedDays;
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            var input = dateTimePicker1.Value;
            var seqDays = new List<DateTime>();
            for (var i = 0; i < numericUpDownSeq.Value; i++)
            {
                DateTime item = input.AddDays(i);
                if (!checkBoxContainsWeekEnd.Checked)
                {
                    if (item.DayOfWeek == DayOfWeek.Saturday || item.DayOfWeek == DayOfWeek.Sunday)
                    {
                        continue;
                    }
                }
                seqDays.Add(item);
            }

            foreach (var day in seqDays)
            {
                var d = CallenderDay.Parse(day.ToShortDateString());
                if (d == null) continue;
                _days.Add(d);
            }
            ApplyEdit();
        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var cal = CsvReadService.ReadWorkingDays(dlg.FileName);
                foreach (var d in cal.Days)
                {
                    _days.Add(d);
                }
            }
            ApplyEdit();
        }
    }
}
