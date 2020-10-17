using ProjectsTM.Logic;
using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ProjectsTM.Service
{
    public class FilterComboBoxService
    {
        private ViewData _viewData;
        private ToolStripComboBox _toolStripComboBoxFilter;
        private string DirPath => Path.Combine(Path.GetDirectoryName(_filepPath), "filters");
        private List<string> _allPaths = new List<string>();
        private Func<Member, string, bool> IsMemberMatchText;

        private readonly string FilePrefix = "file:";
        private readonly string CompanyPrefix = "company:";
        private readonly string ProjectPrefix = "project:";
        private readonly string AllKeyword = "ALL";

        public string Text
        {
            get
            {
                return _toolStripComboBoxFilter.Text;
            }
            set
            {
                _toolStripComboBoxFilter.Text = value;
            }
        }


        public FilterComboBoxService(ViewData viewData, ToolStripComboBox toolStripComboBoxFilter, Func<Member, string, bool> isMemberMatchText)
        {
            _viewData = viewData;
            this._toolStripComboBoxFilter = toolStripComboBoxFilter;
            this._toolStripComboBoxFilter.Items.Add(AllKeyword);
            this.IsMemberMatchText = isMemberMatchText;
            this._toolStripComboBoxFilter.DropDown += ToolStripComboBoxFilter_DropDown;
        }

        private void ToolStripComboBoxFilter_DropDown(object sender, EventArgs e)
        {
            UpdateFilePart(_filepPath);
        }

        private string _filepPath = string.Empty;

        public void UpdateFilePart(string filePath)
        {
            _filepPath = filePath;
            var selectedItem = _toolStripComboBoxFilter.SelectedItem;
            var selectedText = selectedItem == null ? null : _toolStripComboBoxFilter.SelectedItem.ToString();
            if (_toolStripComboBoxFilter.Items.Count != 0) DetachEvent();
            UpdateByFiles();
            _toolStripComboBoxFilter.SelectedIndex = GetIndexBinder(selectedText);
            AttachEvent();
        }

        public void UpdateAppDataPart()
        {
            var selectedItem = _toolStripComboBoxFilter.SelectedItem;
            var selectedText = selectedItem == null ? null : _toolStripComboBoxFilter.SelectedItem.ToString();
            UpdateByProjects();
            UpdateByCompany();
            _toolStripComboBoxFilter.SelectedIndex = GetIndexBinder(selectedText);
        }

        private void UpdateByCompany()
        {
            PartClear(CompanyPrefix);
            var insertIdx = GetCompanyTopIndex();
            foreach (var com in GetCompanies())
            {
                var members = GetMembersConcerningWithCompany(com);
                _toolStripComboBoxFilter.Items.Insert(insertIdx, CompanyPrefix + com + "(" + members.Count.ToString() + ")");
            }
        }

        private void PartClear(string prefix)
        {
            for (var idx = _toolStripComboBoxFilter.Items.Count - 1; idx >= 0; idx--)
            {
                if (!_toolStripComboBoxFilter.Items[idx].ToString().StartsWith(prefix)) continue;
                _toolStripComboBoxFilter.Items.RemoveAt(idx);
            }
        }

        private int GetCompanyTopIndex()
        {
            for (int idx = 0; idx < _toolStripComboBoxFilter.Items.Count; idx++)
            {
                if (!_toolStripComboBoxFilter.Items[idx].ToString().StartsWith(CompanyPrefix)) continue;
                return idx + 1;
            }
            return GetProjectTopIndex();
        }

        private IEnumerable<string> GetCompanies()
        {
            return _viewData.Original.WorkItems.Select(w => w.AssignedMember.Company).Distinct();
        }

        private void UpdateByProjects()
        {
            PartClear(ProjectPrefix);
            var insertIdx = GetProjectTopIndex();
            foreach (var pro in GetProjects())
            {
                _toolStripComboBoxFilter.Items.Insert(insertIdx, ProjectPrefix + pro);
            }
        }

        private int GetProjectTopIndex()
        {
            for (var idx = 0; idx < _toolStripComboBoxFilter.Items.Count; idx++)
            {
                if (!_toolStripComboBoxFilter.Items[idx].ToString().StartsWith(FilePrefix)) continue;
                return idx + 1;
            }
            return GetFileTopIndex();
        }

        private IEnumerable<Project> GetProjects()
        {
            return _viewData.Original.WorkItems.Select(w => w.Project).Distinct();
        }

        private int GetIndexBinder(string selectedText)
        {
            try
            {
                var idx = _toolStripComboBoxFilter.Items.IndexOf(selectedText);
                if (idx < 0) idx = 0;
                return idx;
            }
            catch
            {
                return 0;
            }
        }

        private void UpdateByFiles()
        {
            PartClear(FilePrefix);
            _allPaths.Clear();
            if (string.IsNullOrEmpty(_filepPath)) return;
            if (!Directory.Exists(DirPath)) return;
            _allPaths.AddRange(Directory.GetFiles(DirPath));
            int insertIdx = GetFileTopIndex();
            foreach (var f in _allPaths)
            {
                _toolStripComboBoxFilter.Items.Insert(insertIdx++, FilePrefix + Path.GetFileNameWithoutExtension(f));
            }
        }

        private int GetFileTopIndex()
        {
            return _toolStripComboBoxFilter.Items.IndexOf(AllKeyword) + 1;
        }

        private void AttachEvent()
        {
            _toolStripComboBoxFilter.SelectedIndexChanged += ToolStripComboBoxFilter_SelectedIndexChanged;
        }

        private void DetachEvent()
        {
            _toolStripComboBoxFilter.SelectedIndexChanged -= ToolStripComboBoxFilter_SelectedIndexChanged;
        }

        public void ToolStripComboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _viewData.Selected = null;
            var idx = _toolStripComboBoxFilter.SelectedIndex;
            if (idx == 0)
            {
                _viewData.SetFilter(Filter.All(_viewData));
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
            var company = companies.ElementAt(idx);
            var members = GetMembersConcerningWithCompany(company);
            return new Filter(null, null, members, false, company, false);
        }

        private Members GetMembersConcerningWithCompany(string com)
        {
            var members = new Members();
            foreach (var m in _viewData.Original.Members)
            {
                if (IsMemberMatchText(m, @"^\[.*?]\[.*?]\[.*?\(" + com + @"\)]\[.*?]\[.*?]")) members.Add(m);
            }

            return members;
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
            foreach (var m in _viewData.Original.Members)
            {
                if (IsMemberMatchText(m, @"^\[.*?\]\[" + pro.ToString() + @"\]")) members.Add(m);
            }
            return new Filter(null, null, members, false, pro.ToString(), false);
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
                Filter filter = (Filter)x.Deserialize(rs);
                filter.SetShowMemersFromHideMembers(_viewData.CreateAllMembersList());
                return filter;
            }
        }
    }
}
