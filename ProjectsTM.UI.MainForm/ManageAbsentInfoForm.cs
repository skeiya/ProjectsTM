using ProjectsTM.Model;
using ProjectsTM.UI.Common;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectsTM.UI.MainForm
{
    public partial class ManageAbsentInfoForm : Form
    {
        private readonly Member _member;
        public readonly AbsentTerms _absentTerms;
        private readonly Callender _callender;

        public AbsentTerms Edited => _absentTerms;

        public ManageAbsentInfoForm(Member member, AbsentTerms absentTerms, Callender callender)
        {
            InitializeComponent();
            _member = member;
            _absentTerms = absentTerms;
            _callender = callender;
            listBox1.DisplayMember = "NaturalString";
            this.Text = _member.NaturalString + "：不在情報の編集";
            UpdateList();
        }

        private void UpdateList()
        {
            listBox1.Items.Clear();
            foreach (var a in _absentTerms)
            {
                listBox1.Items.Add($"不在期間 : {a.Period.From} - {a.Period.To}");
            }
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            string selectedItem = (string)listBox1.SelectedItem;
            if (selectedItem == null) return;
            var before = ParseAbsentTerm(selectedItem);
            if (before == null) return;
            using (var dlg = new EditAbsentTermForm(_member, before, _callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var after = dlg.EditAbsentTerm;
                if (after == null) return;
                _absentTerms.Replace(before, after);
            }
            UpdateList();
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Edit();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new EditAbsentTermForm(_member, null, _callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var after = dlg.EditAbsentTerm;
                if (after == null) return;
                _absentTerms.Add(after);
            }
            UpdateList();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            string selectedItem = (string)listBox1.SelectedItem;
            if (selectedItem == null) return;
            var absentTerm = ParseAbsentTerm(selectedItem);
            if (absentTerm == null) return;
            _absentTerms.Remove(absentTerm);
            UpdateList();
        }

        public AbsentTerm ParseAbsentTerm(string str)
        {
            var m = Regex.Match(str, @"不在期間 : (.+) - (.+)");
            if (!m.Success) return null;
            return new AbsentTerm(_member, ParsePeriod(m.Groups[1].Value, m.Groups[2].Value));
        }

        public Period ParsePeriod(string from, string to)
        {
            var f = CallenderDay.Parse(from);
            var t = CallenderDay.Parse(to);
            return new Period(f, t);
        }
    }
}
