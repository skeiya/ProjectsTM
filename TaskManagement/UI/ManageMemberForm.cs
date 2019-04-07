using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManagement.UI
{
    public partial class ManageMemberForm : Form
    {
        private readonly AppData _appData;

        public ManageMemberForm(AppData appData)
        {
            InitializeComponent();
            _appData = appData;
            UpdateList();
        }

        private void ButtonUp_Click(object sender, EventArgs e)
        {
            var m = listBox1.SelectedItem as Member;
            if (m == null) return;
            _appData.Members.Up(m);
            UpdateList();
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
            _appData.Members.Down(m);
            UpdateList();
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
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Edit();
        }
    }
}
