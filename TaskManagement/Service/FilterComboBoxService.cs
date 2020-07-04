using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using TaskManagement.Logic;
using TaskManagement.Model;
using TaskManagement.ViewModel;

namespace TaskManagement.Service
{
    class FilterComboBoxService
    {
        private ViewData _viewData;
        private ToolStripComboBox toolStripComboBoxFilter;
        private string DirPath => Path.Combine(Path.GetDirectoryName(_filepPath), "filters");
        private List<string> _allPaths = new List<string>();
        private Func<Member, string, bool> IsMemberMatchText;

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


        public FilterComboBoxService(ViewData viewData, ToolStripComboBox toolStripComboBoxFilter, Func<Member, string, bool> isMemberMatchText)
        {
            _viewData = viewData;
            this.toolStripComboBoxFilter = toolStripComboBoxFilter;
            this.IsMemberMatchText = isMemberMatchText;
            this.toolStripComboBoxFilter.DropDown += ToolStripComboBoxFilter_DropDown;
        }

        private void ToolStripComboBoxFilter_DropDown(object sender, EventArgs e)
        {
            Initialize(_filepPath);
        }

        private string _filepPath = string.Empty;
        internal void Initialize(string filePath)
        {
            _filepPath = filePath;
            var selectedItem = toolStripComboBoxFilter.SelectedItem;
            var selectedText = selectedItem == null ? null : toolStripComboBoxFilter.SelectedItem.ToString();
            if (toolStripComboBoxFilter.Items.Count != 0) DetachEvent();
            toolStripComboBoxFilter.Items.Clear();
            toolStripComboBoxFilter.Items.Add("ALL");
            AppendByFiles();
            AppendByProjects();
            AppendByCompany();
            toolStripComboBoxFilter.SelectedIndex = GetIndexBinder(selectedText);
            AttachEvent();
        }

        private void AppendByCompany()
        {
            foreach (var com in GetCompanies())
            {
                toolStripComboBoxFilter.Items.Add("company:" + com);
            }
        }
        private IEnumerable<string> GetCompanies()
        {
            return _viewData.GetFilteredWorkItems().Select(w => w.AssignedMember.Company).Distinct();
        }

        private void AppendByProjects()
        {
            foreach (var pro in GetProjects())
            {
                toolStripComboBoxFilter.Items.Add("project:" + pro);
            }
        }

        private IEnumerable<Project> GetProjects()
        {
            return _viewData.GetFilteredWorkItems().Select(w => w.Project).Distinct();
        }

        private int GetIndexBinder(string selectedText)
        {
            try
            {
                return toolStripComboBoxFilter.Items.IndexOf(selectedText);
            }
            catch
            {
                return 0;
            }
        }

        private void AppendByFiles()
        {
            _allPaths.Clear();
            if (string.IsNullOrEmpty(_filepPath)) return;
            if (!Directory.Exists(DirPath)) return;
            _allPaths.AddRange(Directory.GetFiles(DirPath));
            foreach (var f in _allPaths)
            {
                toolStripComboBoxFilter.Items.Add("file:" + Path.GetFileNameWithoutExtension(f));
            }
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
            idx = idx - 1;
            var filter = GetFilterByFiles(ref idx);
            if (filter == null)
            {
                filter = GetFilterByProjects(ref idx);
            }
            if (filter == null)
            {
                filter = GetFilterByCompanies(ref idx);
            }
            _viewData.SetFilter(filter);
        }

        private Filter GetFilterByCompanies(ref int idx)
        {
            var companies = GetCompanies();
            if (companies.Count() <= idx)
            {
                idx -= companies.Count();
                return null;
            }
            var com = companies.ElementAt(idx);
            var members = new Members();
            var filteredMembers = _viewData.GetFilteredMembers();
            foreach (var m in _viewData.Original.Members)
            {
                if (!filteredMembers.Contains(m))
                {
                    members.Add(m);
                    continue;
                }
                if (!IsMemberMatchText(m, @"^\[.*?]\[.*?]\[.*?\(" + com + @"\)]\[.*?]\[.*?]")) members.Add(m);
            }
            return new Filter(null, null, members, _viewData.Filter.IsFreeTimeMemberShow);
        }

        private Filter GetFilterByProjects(ref int idx)
        {
            var projects = GetProjects();
            if (projects.Count() <= idx)
            {
                idx -= projects.Count();
                return null;
            }
            var pro = projects.ElementAt(idx);
            var members = new Members();
            var filteredMembers = _viewData.GetFilteredMembers();
            foreach (var m in _viewData.Original.Members)
            {
                if (!filteredMembers.Contains(m))
                {
                    members.Add(m);
                    continue;
                }
                if (!IsMemberMatchText(m, @"^\[.*?\]\[" + pro.ToString() + @"\]")) members.Add(m);
            }
            return new Filter(null, null, members, _viewData.Filter.IsFreeTimeMemberShow);
        }

        private Filter GetFilterByFiles(ref int idx)
        {
            if (_allPaths.Count <= idx)
            {
                idx -= _allPaths.Count;
                return null;
            }
            var path = _allPaths[idx];
            if (!File.Exists(path))
            {
                return null;
            }
            using (var rs = StreamFactory.CreateReader(path))
            {
                var x = new XmlSerializer(typeof(Filter));
                return (Filter)x.Deserialize(rs);
            }
        }
    }
}
