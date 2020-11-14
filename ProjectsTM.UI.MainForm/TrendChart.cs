using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.UI.Common;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProjectsTM.UI.MainForm
{
    public partial class TrendChart : BaseForm
    {
        private readonly ViewData _viewData;
        private readonly string _filePath;
        private readonly FilterComboBoxService _filterComboBoxService;

        private Dictionary<DateTime, int> _manDays;
        private static readonly DateTime _invalidDate = new DateTime(1000, 1, 1);

        public TrendChart(AppData appData, string filePath,Func<Member, string, bool> isMemberMatchText)
        {
            _viewData = new ViewData(appData, null);
            _filePath = filePath;
            InitializeComponent();
            _filterComboBoxService = new FilterComboBoxService(_viewData, toolStripComboBox1, isMemberMatchText);
            _filterComboBoxService.UpdateFilePart(filePath);
            InitCombo();
            if (comboBox1.Items.Count != 0) comboBox1.SelectedIndex = 0;
        }

        private void InitCombo()
        {
            foreach (var ws in _viewData.Original.WorkItems.EachMembers)
            {
                foreach (var proj in ws.GetProjects())
                {
                    if (!comboBox1.Items.Contains(proj)) comboBox1.Items.Add(proj);
                }
            }
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
                chart1.Series[legend].Points.AddXY(dateTime.ToOADate(), manDaysPoint / 20);
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
            Action<Project, BackgroundWorker, DoWorkEventArgs> CollectWorkItems;
            if (checkBox1.Checked) CollectWorkItems = CollectOldTotalWorkItems;
            else CollectWorkItems = CollectConsumedWorkItems;
            using (var dlg = new TrendChartBackgroundWorkForm(CollectWorkItems, proj)) dlg.ShowDialog();
        }

        private void CollectOldTotalWorkItems(Project proj, BackgroundWorker worker, DoWorkEventArgs e)
        {
            var monthsCount = 12;
            for (int monthsAgo = 0; monthsAgo < monthsCount; monthsAgo++)
            {
                if (worker.CancellationPending) { CancellCollectWorkItems(e); return; }
                worker.ReportProgress((int)(monthsAgo * 100 / monthsCount));
                var workItems = GetOldWorkItems(monthsAgo, proj);
                if (!workItems.Any()) return;
                var total = CalcTotal(workItems);
                _manDays.Add(DateTime.Today.AddMonths(-monthsAgo), total);
            }
        }

        private void CancellCollectWorkItems(DoWorkEventArgs e)
        {
            _manDays = new Dictionary<DateTime, int>();
            e.Cancel = true; 
        }

        private int CalcTotal(IEnumerable<WorkItem> ws)
        {
            var total = 0;
            foreach (var w in ws) total += _viewData.Original.Callender.GetPediodDays(w.Period).Count;
            return total;
        }

        private IEnumerable<WorkItem> GetOldWorkItems(int monthsAgo, Project proj)
        {
            var oldAppData = GetOldAppData(monthsAgo);
            if(oldAppData == null) return new List<WorkItem>();
            var oldViewData = new ViewData(oldAppData, null);
            oldViewData.SetFilter(_viewData.Filter);
            return oldViewData.GetFilteredWorkItems().Where(w => w.Project.Equals(proj));
        }

        public AppData GetOldAppData(int monthsAgo)
        {
            var oldFile = GitRepositoryService.GetOldFileSomeMonthsAgo(_filePath, monthsAgo);
            if (string.IsNullOrEmpty(oldFile)) return null;
            var oldAppData = AppDataSerializeService.Deserialize(oldFile);
            File.Delete(oldFile);
            return oldAppData;
        }

        private void CollectConsumedWorkItems(Project proj, BackgroundWorker worker, DoWorkEventArgs e)
        {
            var counter = 0;
            var workItems = _viewData.GetFilteredWorkItems();
            foreach (var w in workItems)
            {
                if (worker.CancellationPending) { CancellCollectWorkItems(e); return; }
                counter++;
                worker?.ReportProgress((int)(counter * 100 / workItems.Count()));
                if (!w.Project.Equals(proj)) continue;
                var days = _viewData.Original.Callender.GetPediodDays(w.Period);
                AddToManDays(days);
            }
        }

        private void AddToManDays(List<CallenderDay> days)
        {
            foreach (var d in days)
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

        private void button1_Click(object sender, EventArgs e)
        {
            InitChart();
        }
    }
}
