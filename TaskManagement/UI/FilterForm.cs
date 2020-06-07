using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Serialization;
using TaskManagement.Logic;
using TaskManagement.Model;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public partial class FilterForm : Form
    {
        private Members _members;
        private Members _originalMembers;
        private Filter _filter;
        private Callender _callender;
        private WorkItems _workItems;
        private Func<Member, string, bool> IsMemberMatchText;

        public FilterForm(Members members, Filter filter, Callender callender, WorkItems workItems, Func<Member, string, bool> isMemberMatchText)
        {
            InitializeComponent();
            _originalMembers = members.Clone();
            _members = members.Clone();
            _filter = filter;
            _callender = callender;
            _workItems = workItems;
            UpdateAllField();
            this.IsMemberMatchText = isMemberMatchText;
        }

        private void UpdateAllField()
        {
            UpdateMembersCheck();
            UpdatePeriodText();
            UpdateWorkItemText();
        }

        private void UpdateMembersCheck()
        {
            checkedListBox1.Items.Clear();
            checkedListBox1.DisplayMember = "NaturalString";
            foreach (var m in _members)
            {
                var check = _filter.HideMembers == null ? true : !_filter.HideMembers.Contain(m);
                checkedListBox1.Items.Add(m, check);
            }
        }

        private void UpdateWorkItemText()
        {
            textBoxWorkItem.Text = _filter.WorkItem == null ? string.Empty : _filter.WorkItem;
        }

        private void UpdatePeriodText()
        {
            textBoxFrom.Text = _filter.Period == null ? string.Empty : _filter.Period.From.ToString();
            textBoxTo.Text = _filter.Period == null ? string.Empty : _filter.Period.To.ToString();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateValue();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ValidateValue()
        {
            var from = textBoxFrom.Text;
            var to = textBoxTo.Text;
            if (string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to)) return;

            var dayErrorMsg = "稼働日が存在しません。：";
            var fromDay = CallenderDay.Parse(textBoxFrom.Text);
            if (fromDay == null || !_callender.Days.Contains(fromDay)) throw new Exception(dayErrorMsg + textBoxFrom.Text);

            var toDay = CallenderDay.Parse(textBoxTo.Text);
            if (toDay == null || !_callender.Days.Contains(toDay)) throw new Exception(dayErrorMsg + textBoxTo.Text);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public Filter GetFilter()
        {
            return new Filter(GetWorkItemFilter(), GetPeriodFilter(), GetHiddenMembers());
        }

        private string GetWorkItemFilter()
        {
            if (string.IsNullOrEmpty(textBoxWorkItem.Text)) return null;
            return textBoxWorkItem.Text;
        }

        private Period GetPeriodFilter()
        {
            var from = CallenderDay.Parse(textBoxFrom.Text);
            var to = CallenderDay.Parse(textBoxTo.Text);
            if (from == null || to == null) return null;
            return new Period(from, to);
        }

        private Members GetHiddenMembers()
        {
            var result = new Members();
            foreach (var c in checkedListBox1.Items)
            {
                var m = (Member)c;
                if (!GetCheckedMembers().Contains(m)) result.Add(m);
            }
            return result;
        }

        private Members GetCheckedMembers()
        {
            var result = new Members();
            foreach (var c in checkedListBox1.CheckedItems) result.Add((Member)c);
            return result;
        }

        private void buttonClearWorkItem_Click(object sender, EventArgs e)
        {
            textBoxWorkItem.Text = string.Empty;
        }

        private void buttonClearPeriod_Click(object sender, EventArgs e)
        {
            ClearPeriodFilter();
        }

        private void ClearPeriodFilter()
        {
            var days = _callender.Days;
            if (days.Count == 0) return;
            textBoxFrom.Text = days.First().ToString();
            textBoxTo.Text = days.Last().ToString();
        }

        private void buttonClearMembers_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                checkedListBox1.SetItemChecked(index, true);
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                using (var reader = StreamFactory.CreateReader(dlg.FileName))
                {
                    var s = new XmlSerializer(typeof(Filter));
                    _filter = (Filter)s.Deserialize(reader);
                    UpdateAllField();
                }
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateValue();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                return;
            }
            _filter = GetFilter();
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                using (var writer = StreamFactory.CreateWriter(dlg.FileName))
                {
                    var s = new XmlSerializer(typeof(Filter));
                    s.Serialize(writer, _filter);
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using (var dlg = new EazyRegexForm())
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                textBoxWorkItem.Text = GetRegex(dlg.TaskName, dlg.ProjectName, dlg.MemberName, dlg.TagText);
            }
        }

        private string GetRegex(string taskName, string projectName, string memberName, string tagText)
        {
            var result = @"^\[";
            result += string.IsNullOrEmpty(taskName) ? ".*" : ".*" + taskName + ".*";
            result += @"\]\[";
            result += string.IsNullOrEmpty(projectName) ? ".*" : ".*" + projectName + ".*";
            result += @"\]\[";
            result += string.IsNullOrEmpty(memberName) ? ".*" : ".*" + memberName + ".*";
            result += @"\]\[";
            result += string.IsNullOrEmpty(tagText) ? ".*" : ".*" + tagText + ".*";
            result += @"\]$";
            return result;
        }

        private void CheckBoxSort_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSort.Checked)
            {
                _members.SortByCompany();
            }
            else
            {
                _members = _originalMembers.Clone();
            }
            _filter = GetFilter();
            UpdateAllField();
        }

        private void ButtonAllOff_Click(object sender, EventArgs e)
        {
            AllOff();
        }

        private void AllOff()
        {
            for (var idx = 0; idx < checkedListBox1.Items.Count; idx++)
            {
                checkedListBox1.SetItemCheckState(idx, CheckState.Unchecked);
            }
        }

        private void buttonGenerateFromProject_Click(object sender, EventArgs e)
        {
            using (var dlg = new ProjectSelectForm())
            {
                dlg.Projects = _workItems.Select(w => w.Project.ToString()).Distinct();
                if (dlg.ShowDialog() != DialogResult.OK) return;
                AllOff();
                CheckOnProject(dlg.Selected);
            }
        }

        private void CheckOnProject(string selected)
        {
            CheckByTextMatch(@"^\[.*?\]\[" + selected + @"\]");
        }

        private Member GetMember(string v)
        {
            return _members.FirstOrDefault(m => m.NaturalString.Equals(v));
        }

        private void buttonGenerateFromWorkItems_Click(object sender, EventArgs e)
        {
            using(var dlg = new EditMemberForm(""))
            {
                dlg.Text = "作業項目の正規表現";
                if (dlg.ShowDialog() != DialogResult.OK) return;
                AllOff();
                CheckByTextMatch(dlg.EditText);
            }
        }

        private void CheckByTextMatch(string editText)
        {
            for (var idx = 0; idx < checkedListBox1.Items.Count; idx++)
            {
                var m = GetMember(checkedListBox1.Items[idx].ToString());
                var state = IsMemberMatchText(m, editText) ? CheckState.Checked : CheckState.Unchecked;
                checkedListBox1.SetItemCheckState(idx, state);
            }
        }
    }
}
