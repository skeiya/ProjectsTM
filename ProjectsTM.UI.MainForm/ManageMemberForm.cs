using ProjectsTM.Model;
using ProjectsTM.UI.Common;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.MainForm
{
    public partial class ManageMemberForm : Form
    {
        private readonly AppData _appData;

        public ManageMemberForm(AppData appData)
        {
            InitializeComponent();
            _appData = appData;
            listBox1.DisplayMember = "NaturalString";
            UpdateList();
        }

        private void ButtonUp_Click(object sender, EventArgs e)
        {
            var m = listBox1.SelectedItem as Member;
            if (m == null) return;
            var index = listBox1.SelectedIndex;
            _appData.Members.Up(m);
            UpdateList();
            listBox1.SelectedIndex = index == 0 ? index : index - 1;
        }

        private void UpdateList()
        {
            listBox1.Items.Clear();
            foreach (var m in _appData.Members)
            {
                listBox1.Items.Add(m);
            }
        }

        private void ButtonDown_Click(object sender, EventArgs e)
        {
            var m = listBox1.SelectedItem as Member;
            if (m == null) return;
            var index = listBox1.SelectedIndex;
            _appData.Members.Down(m);
            UpdateList();
            listBox1.SelectedIndex = listBox1.Items.Count == index + 1 ? index : index + 1;
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            var m = listBox1.SelectedItem as Member;
            if (m == null) return;
            using (var dlg = new EditMemberForm(m.Clone()))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var state = (Member.MemberState)dlg.Selected;
                var after = Member.Parse(dlg.EditText, state);
                if (after == null) return;
                foreach (var w in _appData.WorkItems)
                {
                    if (m.Equals(w.AssignedMember)) w.AssignedMember = after;
                }
                m.EditApply(dlg.EditText, state);
            }
            UpdateList();
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Edit();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new EditMemberForm(new Member()))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var after = Member.Parse(dlg.EditText, (Member.MemberState)dlg.Selected);
                if (after == null) return;
                _appData.Members.Add(after);
            }
            UpdateList();
        }
    }
}
