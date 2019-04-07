using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class SearchWorkitemForm : Form
    {
        private readonly ViewData _viewData;
        private System.Timers.Timer _timer = new System.Timers.Timer(100);
        private int _tickCount = 0;
        private WorkItem[] _list = null;

        public SearchWorkitemForm(ViewData viewData)
        {
            InitializeComponent();
            this._viewData = viewData;

            var tmp = new List<WorkItem>();
            foreach (var wi in viewData.GetFilteredWorkItems())
            {
                tmp.Add(wi);
            }
            _list = tmp.ToArray();
            UpdateListBox();

            _timer.Elapsed += _timer_Elapsed;
            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
        }

        private void UpdateListBox()
        {
            listBox1.Items.Clear();
            foreach (var l in _list)
            {
                listBox1.Items.Add(l.ToString(_viewData.Original.Callender));
            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _viewData.Selected = _list[listBox1.SelectedIndex];
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                if (_tickCount <= 5)
                {
                    _tickCount++;
                    return;
                }

                _tickCount = 0;
                _timer.Enabled = false;

                UpdateFilteredList();
                UpdateListBox();
            }
            ));
        }

        private void UpdateFilteredList()
        {
            var tmp = new List<WorkItem>();
            if (string.IsNullOrEmpty(textBoxPattern.Text))
            {
                foreach (var wi in _viewData.GetFilteredWorkItems())
                {
                    tmp.Add(wi);
                }
            }
            else
            {
                try
                {
                    foreach (var wi in _viewData.GetFilteredWorkItems())
                    {
                        if (!Regex.IsMatch(wi.ToString(_viewData.Original.Callender), textBoxPattern.Text)) continue;
                        tmp.Add(wi);
                    }
                }
                catch { }
            }
            _list = tmp.ToArray();
        }

        private void TextBoxPattern_TextChanged(object sender, EventArgs e)
        {
            _timer.Enabled = true;
            _tickCount = 0;
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var wi = _list[listBox1.SelectedIndex];
            using (var dlg = new EditWorkItemForm(wi, _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _viewData.UpdateCallenderAndMembers(wi);
            }
        }
    }
}
