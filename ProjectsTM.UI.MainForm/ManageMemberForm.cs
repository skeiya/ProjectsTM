using ProjectsTM.Model;
using ProjectsTM.Service;
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
            UpdateDisplay();
        }

        private void ButtonUp_Click(object sender, EventArgs e)
        {
            var m = listBox1.SelectedItem as Member;
            if (m == null) return;
            var index = listBox1.SelectedIndex;
            _appData.Members.Up(m);
            UpdateList();
            listBox1.SelectedIndex = index == 0 ? index : index - 1;
            UpdateDisplay();
        }

        private void ButtonDown_Click(object sender, EventArgs e)
        {
            var m = listBox1.SelectedItem as Member;
            if (m == null) return;
            var index = listBox1.SelectedIndex;
            _appData.Members.Down(m);
            UpdateList();
            listBox1.SelectedIndex = listBox1.Items.Count == index + 1 ? index : index + 1;
            UpdateDisplay();
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            var m = listBox1.SelectedItem as Member;
            if (m == null) return;
            using (var dlg = new EditMemberForm(m.ToSerializeString()))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var after = Member.Parse(dlg.EditText);
                if (after == null) return;
                foreach (var w in _appData.WorkItems)
                {
                    if (m.Equals(w.AssignedMember)) w.AssignedMember = after;
                }
                m.EditApply(dlg.EditText);
            }
            UpdateList();
            UpdateDisplay();
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Edit();
        }
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new EditMemberForm((new Member()).ToSerializeString()))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var after = Member.Parse(dlg.EditText);
                if (after == null) return;
                _appData.Members.Add(after);
            }
            UpdateList();
            UpdateDisplay();
        }
        private void UpdateList()
        {
            listBox1.Items.Clear();
            foreach (var m in _appData.Members)
            {
                listBox1.Items.Add(m);
            }
        }
        
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            var m = listBox1.SelectedItem as Member;
            if (m == null)
            {
                _labelMenmberNum.Text = "Total:" + _appData.Members.Count.ToString();
                return;
            }
            _labelMenmberNum.Text = CountMemberNumService.GetCountStr(_appData.Members, m.Company);
        }
    }
}
