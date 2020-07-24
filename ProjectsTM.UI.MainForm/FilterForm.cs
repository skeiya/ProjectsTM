using ProjectsTM.Logic;
using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ProjectsTM.UI
{
    public partial class FilterForm : Form
    {
        private Members _members;
        private Members _originalMembers;
        private Filter _filter;
        private Callender _callender;
        private IEnumerable<WorkItem> _workItems;
        private Func<Member, string, bool> IsMemberMatchText;
        private PatternHistory _history;
        private MileStones _mileStones;

        public FilterForm(Members members, Filter filter, Callender callender, IEnumerable<WorkItem> workItems, Func<Member, string, bool> isMemberMatchText, PatternHistory patternHistory, MileStones mileStones)
        {
            InitializeComponent();
            _originalMembers = members.Clone();
            _members = members.Clone();
            _filter = filter;
            _callender = callender;
            _workItems = workItems;
            _history = patternHistory;
            _mileStones = mileStones;
            UpdateAllField();
            this.IsMemberMatchText = isMemberMatchText;
            checkedListBox1.CheckOnClick = true;
            checkedListBox_msFilters.CheckOnClick = true;
            buttonFromTodayToSpecialDay.Text += SpecialDay;
        }

        private void UpdateAllField()
        {
            UpdateMembersCheck();
            UpdateMileStonesCheck();
            UpdatePeriodText();
            UpdateWorkItemText();
        }

        private void UpdateMembersCheck()
        {
            checkedListBox1.Items.Clear();
            checkedListBox1.DisplayMember = "NaturalString";
            foreach (var m in _members)
            {
                var check = _filter.HideMembers == null ? true : !_filter.HideMembers.Contains(m);
                checkedListBox1.Items.Add(m, check);
            }

            checkBox_IsFreeTimeMemberShow.Checked = _filter == null ? false : _filter.IsFreeTimeMemberShow;
        }

        private void UpdateMileStonesCheck()
        {
            checkedListBox_msFilters.Items.Clear();
            checkedListBox_msFilters.DisplayMember = "NaturalString";
            if (_mileStones == null) return;
            foreach (var ms in _mileStones)
            {
                if (ms == null || 
                    ms.MileStoneFilter == null || 
                    ms.MileStoneFilter.Name == null) continue;
                if (checkedListBox_msFilters.Items.Contains(ms.MileStoneFilter.Name)) continue;
                var check = _filter.MileStoneFilters == null ? true : _filter.MileStoneFilters.Contains(ms.MileStoneFilter);
                checkedListBox_msFilters.Items.Add(ms.MileStoneFilter.Name, check);
            }
        }

        private void UpdateWorkItemText()
        {
            comboBoxPattern.Text = _filter.WorkItem == null ? string.Empty : _filter.WorkItem;
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
            return new Filter(GetWorkItemFilter(), GetPeriodFilter(), GetHiddenMembers(), checkBox_IsFreeTimeMemberShow.Checked, GetCheckedMileStoneFilters());
        }

        private string GetWorkItemFilter()
        {
            if (string.IsNullOrEmpty(comboBoxPattern.Text)) return null;
            return comboBoxPattern.Text;
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

        private MileStoneFilters GetCheckedMileStoneFilters()
        {
            var result = new MileStoneFilters();
            foreach (var c in checkedListBox_msFilters.CheckedItems) result.Add(new MileStoneFilter((string)c));
            return result;
        }

        private void buttonClearWorkItem_Click(object sender, EventArgs e)
        {
            comboBoxPattern.Text = string.Empty;
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
                comboBoxPattern.Text = dlg.RegexPattern;
            }
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
            using (var dlg = new EditMemberForm(""))
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

        private static string SpecialDay => "2021/3/31";
        private void buttonFromTodayToSpecialDay_Click(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            textBoxFrom.Text = now.Year.ToString() + "/" + now.Month.ToString() + "/" + now.Day.ToString();
            textBoxTo.Text = SpecialDay;
        }

        private void comboBoxPattern_DropDown(object sender, EventArgs e)
        {
            comboBoxPattern.Items.Clear();
            comboBoxPattern.Items.AddRange(_history.Items.ToArray());
        }
    }
}
