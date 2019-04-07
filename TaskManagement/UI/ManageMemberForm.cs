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
        private readonly Members _members;

        public ManageMemberForm(Members members)
        {
            InitializeComponent();
            _members = members;
            UpdateList();
        }

        private void ButtonUp_Click(object sender, EventArgs e)
        {
            var m = listBox1.SelectedItem as Member;
            if (m == null) return;
            _members.Up(m);
            UpdateList();
        }

        private void UpdateList()
        {
            listBox1.Items.Clear();
            foreach (var m in _members)
            {
                listBox1.Items.Add(m);
            }
        }

        private void ButtonDown_Click(object sender, EventArgs e)
        {
            var m = listBox1.SelectedItem as Member;
            if (m == null) return;
            _members.Down(m);
            UpdateList();
        }
    }
}
