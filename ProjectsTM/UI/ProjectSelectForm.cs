using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectsTM.UI
{
    public partial class ProjectSelectForm : Form
    {
        public ProjectSelectForm()
        {
            InitializeComponent();
        }

        public IEnumerable<string> Projects { get; internal set; }
        public string Selected
        {
            get
            {
                return this.comboBox1.SelectedItem.ToString();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ProjectSelectForm_Shown(object sender, EventArgs e)
        {
            foreach (var p in Projects)
            {
                this.comboBox1.Items.Add(p);
            }
            this.comboBox1.SelectedIndex = 0;
        }
    }
}
