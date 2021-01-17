using ProjectsTM.Model;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ProjectsTM.UI.Main
{
    public partial class TrendChartBackgroundWorkForm : Form
    {
        private bool BackgroudWorkDone { get; set; } = false;

        public TrendChartBackgroundWorkForm(Action<Project, BackgroundWorker, DoWorkEventArgs> collectWorkItems, Project proj)
        {
            InitializeComponent();
            backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;

            backgroundWorker1.RunWorkerAsync(new Tuple<Action<Project, BackgroundWorker, DoWorkEventArgs>, Project>(collectWorkItems, proj));
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled) MessageBox.Show("キャンセルされました");
            BackgroudWorkDone = true;
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var arg = (Tuple<Action<Project, BackgroundWorker, DoWorkEventArgs>, Project>)e.Argument;
            var collectWorkItems = arg.Item1;
            var proj = arg.Item2;
            collectWorkItems?.Invoke(proj, (BackgroundWorker)sender, e);
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = e.ProgressPercentage.ToString() + "% 完了";
            progressBar1.Value = e.ProgressPercentage;
        }

        private void TrendChartBackgroundWorkForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (BackgroudWorkDone) return;
            backgroundWorker1.CancelAsync();
            e.Cancel = true; // バックグランドキャンセルしてからフォームキャンセルしないと、途中状態が描画されることがある
        }
    }
}
