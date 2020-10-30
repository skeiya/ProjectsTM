using ProjectsTM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProjectsTM.UI.MainForm
{
    public partial class TrendChart : Form
    {
        private readonly WorkItems _workItems;
        private readonly Callender _callender;

        private Dictionary<CallenderDay, int> _manDays;
        private static readonly DateTime _invalidDate = new DateTime(1000, 1, 1);

        public TrendChart(WorkItems workItems, Callender callender)
        {
            _workItems = workItems;
            _callender = callender;
            InitializeComponent();
            InitCombo();
            InitChart();
        }

        private void InitCombo()
        {
            foreach (var ws in _workItems.EachMembers)
            {
                foreach (var proj in ws.GetProjects())
                {
                    if (!comboBox1.Items.Contains(proj)) comboBox1.Items.Add(proj);
                }
            }
            if (comboBox1.Items.Count != 0) comboBox1.SelectedIndex = 0;
        }

        private void InitChart()
        {
            var projName = comboBox1.SelectedItem?.ToString();
            if (projName == null) return;
            UpdateChart(new Project(projName));
        }

        private void UpdateChart(Project proj)
        {
            chart1.Series.Clear();
            string legend = "総工数";
            SetChartStyle(legend);
            UpdateManDays(proj);
            DrawChart(legend);
        }

        private void DrawChart(string legend)
        {
            var accumulation = 0;
            foreach (var pair in _manDays.OrderBy(pair => pair.Key))
            {
                var dateTime = ParseDateTime(pair.Key);
                if (dateTime == _invalidDate) continue;
                accumulation += pair.Value;
                chart1.Series[legend].Points.AddXY(dateTime.ToOADate(), accumulation);
            }
        }

        private DateTime ParseDateTime(CallenderDay callenderDay)
        {
            try
            {
                return new DateTime(callenderDay.Year, callenderDay.Month, callenderDay.Day);
            }
            catch
            {
                return _invalidDate;
            }
        }

        private void UpdateManDays(Project proj)
        {
            _manDays = new Dictionary<CallenderDay, int>();
            foreach (var w in _workItems)
            {
                if (!w.Project.Equals(proj)) continue;
                var days = _callender.GetPediodDays(w.Period);
                AddToManDays(days);
            }
        }

        private void AddToManDays(List<CallenderDay> days)
        {
            foreach(var d in days)
            {
                if (_manDays.TryGetValue(d, out int value)) _manDays[d]++;
                else _manDays.Add(d, 1);
            }
        }

        private void SetChartStyle(string legend)
        {
            chart1.Series.Add(legend);
            chart1.Series[legend].ChartType = SeriesChartType.Line;
            chart1.Series[legend].MarkerStyle = MarkerStyle.Circle;
            chart1.Series[legend].MarkerSize = 10;
            chart1.Series[legend].XValueType = ChartValueType.Date;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitChart();
        }
    }
}
