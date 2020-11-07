using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProjectsTM.UI.MainForm
{
    public partial class TrendChart : BaseForm
    {
        private readonly WorkItems _workItems;
        private readonly Callender _callender;
        private readonly string _filePath;

        private Dictionary<DateTime, int> _manDays;
        private static readonly DateTime _invalidDate = new DateTime(1000, 1, 1);

        public TrendChart(WorkItems workItems, Callender callender, string filePath)
        {
            _workItems = workItems;
            _callender = callender;
            _filePath = filePath;
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
            var manDaysPoint = 0.0;
            foreach (var pair in _manDays.OrderBy(pair => pair.Key))
            {
                var dateTime = pair.Key;
                if (checkBox1.Checked) manDaysPoint = pair.Value;
                else manDaysPoint += pair.Value;
                chart1.Series[legend].Points.AddXY(dateTime.ToOADate(), manDaysPoint/20);
            }
        }

        private DateTime Callender2DataTime(CallenderDay callenderDay)
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
            _manDays = new Dictionary<DateTime, int>();
            if (checkBox1.Checked) SetTotalTransition(proj);
            else SetConsumptionTransition(proj);
        }

        private void SetTotalTransition(Project proj)
        {
            for (int monthsAgo = 0; monthsAgo < 12; monthsAgo++)
            {
                var workItems = GetOldWorkItems(monthsAgo, proj);
                if (!workItems.Any()) return;
                var total = CalcTotal(workItems);
                _manDays.Add(DateTime.Today.AddMonths(-monthsAgo), total);
            }
        }

        private int CalcTotal(IEnumerable<WorkItem> ws)
        {
            var total = 0;
            foreach (var w in ws) total += _callender.GetPediodDays(w.Period).Count;          
            return total;
        }

        private IEnumerable<WorkItem> GetOldWorkItems(int monthsAgo, Project proj)
        {
            var oldFile = GitRepositoryService.GetOldFileSomeMonthsAgo(_filePath, monthsAgo);
            if (string.IsNullOrEmpty(oldFile)) return new List<WorkItem>();
            var oldAppData = AppDataSerializeService.Deserialize(oldFile);
            File.Delete(oldFile);
            return oldAppData.WorkItems.Where(w => w.Project.Equals(proj));
        }

        private void SetConsumptionTransition(Project proj)
        {
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
                var dateTime = Callender2DataTime(d);
                if (dateTime == _invalidDate) continue;
                if (_manDays.TryGetValue(dateTime, out int value)) _manDays[dateTime]++;
                else _manDays.Add(dateTime, 1);
            }
        }

        private void SetChartStyle(string legend)
        {
            chart1.Series.Add(legend);
            chart1.Series[legend].ChartType = SeriesChartType.Line;
            chart1.Series[legend].MarkerStyle = MarkerStyle.Circle;
            chart1.Series[legend].MarkerSize = 10;
            chart1.Series[legend].XValueType = ChartValueType.Date;
            chart1.ChartAreas["ChartArea1"].AxisY.Title = checkBox1.Checked ? "過去の最終到達予想工数[人月]" : "工数消費ペース[人月]";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitChart();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            InitChart();
        }
    }
}
