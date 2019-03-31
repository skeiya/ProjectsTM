using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Text.RegularExpressions;

namespace TaskManagement
{
    public partial class SearchWorkitemForm : Form
    {
        private readonly ViewData _viewData;
        private System.Timers.Timer _timer = new System.Timers.Timer(100);
        private int _tickCount = 0;

        public SearchWorkitemForm(ViewData viewData)
        {
            InitializeComponent();

            foreach (var wi in viewData.GetFilteredWorkItems())
            {
                listBox1.Items.Add(wi);
            }

            this._viewData = viewData;
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                if (_tickCount > 5)
                {
                    _tickCount = 0;
                    _timer.Enabled = false;

                    listBox1.Items.Clear();
                    if (string.IsNullOrEmpty(textBoxPattern.Text))
                    {
                        foreach (var wi in _viewData.GetFilteredWorkItems())
                        {
                            listBox1.Items.Add(wi);
                        }
                    }
                    else
                    {
                        try
                        {
                            foreach (var wi in _viewData.GetFilteredWorkItems())
                            {
                                if (!Regex.IsMatch(wi.ToString(), textBoxPattern.Text)) continue;
                                listBox1.Items.Add(wi);
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    _tickCount++;
                }
            }
            ));
        }

        private void TextBoxPattern_TextChanged(object sender, EventArgs e)
        {
            _timer.Enabled = true;
            _tickCount = 0;

        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var wi = listBox1.SelectedItem as WorkItem;
            if (wi == null) return;
            using(var dlg = new EditWorkItemForm(wi, _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.UpdateCallenderAndMembers(wi);
            }
        }
    }
}
