using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;
using TaskManagement.Service;

namespace TaskManagement
{
    public partial class SearchWorkitemForm : Form
    {
        private readonly ViewData _viewData;
        private readonly UndoService _undoService;
        private System.Timers.Timer _timer = new System.Timers.Timer(100);
        private int _tickCount = 0;
        private List<WorkItem> _list = new List<WorkItem>();

        public SearchWorkitemForm(ViewData viewData, UndoService undoService)
        {
            InitializeComponent();
            this._viewData = viewData;
            this._undoService = undoService;
            UpdateFilteredList();
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
            _list.Clear();
            if (string.IsNullOrEmpty(textBoxPattern.Text))
            {
                foreach (var wi in _viewData.GetFilteredWorkItems())
                {
                    _list.Add(wi);
                }
            }
            else
            {
                try
                {
                    foreach (var wi in _viewData.GetFilteredWorkItems())
                    {
                        if (!Regex.IsMatch(wi.ToString(_viewData.Original.Callender), textBoxPattern.Text)) continue;
                        _list.Add(wi);
                    }
                }
                catch { }
            }
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
                var newWi = dlg.GetWorkItem(_viewData.Original.Callender);
                _undoService.Push(wi.Serialize(), newWi.Serialize());
                wi.Apply(newWi);
                _viewData.UpdateCallenderAndMembers(wi);
            }

            UpdateFilteredList();
            UpdateListBox();
        }

        private void CheckBoxOverwrapPeriod_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPattern.Enabled = !checkBoxOverwrapPeriod.Checked;
            if (!checkBoxOverwrapPeriod.Checked)
            {
                UpdateFilteredList();
                UpdateListBox();
                return;
            }

            textBoxPattern.Text = string.Empty;
            _tickCount = 0;
            _timer.Enabled = false; _list.Clear();
            foreach (var src in _viewData.Original.WorkItems)
            {
                foreach (var dst in _viewData.Original.WorkItems)
                {
                    if (src.Equals(dst))
                    {
                        continue;
                    }
                    if (!src.AssignedMember.Equals(dst.AssignedMember)) continue;
                    if (src.Period.HasInterSection(dst.Period)) _list.Add(src);
                }
            }

            listBox1.Items.Clear();
            foreach (var l in _list)
            {
                listBox1.Items.Add(l.ToString(_viewData.Original.Callender));
            }
        }
    }
}
