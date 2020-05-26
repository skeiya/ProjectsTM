using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TaskManagement.Logic;
using TaskManagement.Model;
using TaskManagement.Service;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public partial class SearchWorkitemForm : Form
    {
        private readonly ViewData _viewData;
        private readonly WorkItemEditService _editService;
        private List<WorkItem> _list = new List<WorkItem>();

        public SearchWorkitemForm(ViewData viewData, WorkItemEditService editService)
        {
            InitializeComponent();
            this._viewData = viewData;
            this._editService = editService;
            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var found = _viewData.GetFilteredWorkItems().FirstOrDefault(w => w.Equals(_list[listBox1.SelectedIndex]));
            _viewData.Selected = found == null ? null : new WorkItems(found);
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var wi = _list[listBox1.SelectedIndex];
            using (var dlg = new EditWorkItemForm(wi.Clone(), _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var newWi = dlg.GetWorkItem();
                _editService.Replace(wi, newWi);
                _viewData.Selected = new WorkItems(newWi);
                _viewData.UpdateCallenderAndMembers(wi);
            }
        }

        private void CheckBoxOverwrapPeriod_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPattern.Enabled = !checkBoxOverwrapPeriod.Checked;
            if (!checkBoxOverwrapPeriod.Checked)
            {
                return;
            }

            textBoxPattern.Text = string.Empty;
            _list = OverwrapedWorkItemsGetter.Get(_viewData.Original.WorkItems);
            UpdateListView();
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (IsSearchOverwrap())
            {
                CheckBoxOverwrapPeriod_CheckedChanged(null, null);
                return;
            }
            _list.Clear();
            try
            {
                foreach (var wi in _viewData.GetFilteredWorkItems())
                {
                    if (!Regex.IsMatch(wi.ToString(), textBoxPattern.Text)) continue;
                    _list.Add(wi);
                }
            }
            catch { }
            UpdateListView();
        }

        private bool IsSearchOverwrap()
        {
            return checkBoxOverwrapPeriod.Checked;
        }

        private void UpdateListView()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(_list.Select((i) => i.ToString()).ToArray());
        }
    }
}
