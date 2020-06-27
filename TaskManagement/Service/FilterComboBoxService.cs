using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using TaskManagement.Logic;
using TaskManagement.ViewModel;

namespace TaskManagement.Service
{
    class FilterComboBoxService
    {
        private ViewData _viewData;
        private ToolStripComboBox toolStripComboBoxFilter;
        private static string DirPath => "./filters";
        private List<string> _allPaths = new List<string>();

        public string Text
        {
            get
            {
                return toolStripComboBoxFilter.Text;
            }
            set
            {
                toolStripComboBoxFilter.Text = value;
            }
        }


        public FilterComboBoxService(ViewData viewData, ToolStripComboBox toolStripComboBoxFilter)
        {
            _viewData = viewData;
            this.toolStripComboBoxFilter = toolStripComboBoxFilter;
            this.toolStripComboBoxFilter.GotFocus += ToolStripComboBoxFilter_GotFocus;
        }

        private void ToolStripComboBoxFilter_GotFocus(object sender, EventArgs e)
        {
            Initialize();
        }

        internal void Initialize()
        {
            var selectedIndex = toolStripComboBoxFilter.SelectedIndex;
            if (toolStripComboBoxFilter.Items.Count != 0) DetachEvent();
            toolStripComboBoxFilter.Items.Clear();
            toolStripComboBoxFilter.Items.Add("ALL");
            _allPaths.Clear();
            _allPaths.AddRange(Directory.GetFiles(DirPath));
            foreach (var f in _allPaths)
            {
                toolStripComboBoxFilter.Items.Add(Path.GetFileNameWithoutExtension(f));
            }
            toolStripComboBoxFilter.SelectedIndex = selectedIndex < 0 ? 0 : selectedIndex;
            AttachEvent();
        }

        private void AttachEvent()
        {
            toolStripComboBoxFilter.SelectedIndexChanged += ToolStripComboBoxFilter_SelectedIndexChanged;
        }

        private void DetachEvent()
        {
            toolStripComboBoxFilter.SelectedIndexChanged -= ToolStripComboBoxFilter_SelectedIndexChanged;
        }

        private void ToolStripComboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _viewData.Selected = null;
            var idx = toolStripComboBoxFilter.SelectedIndex;
            if (idx == 0)
            {
                _viewData.SetFilter(null);
                return;
            }
            var path = _allPaths[idx - 1];
            if (!File.Exists(path)) return;
            using (var rs = StreamFactory.CreateReader(path))
            {
                var x = new XmlSerializer(typeof(Filter));
                var filter = (Filter)x.Deserialize(rs);
                _viewData.SetFilter(filter);
            }
        }
    }
}
