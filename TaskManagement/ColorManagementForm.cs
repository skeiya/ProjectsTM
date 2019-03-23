using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class ColorManagementForm : Form
    {
        private readonly ColorConditions colorConditions;

        public ColorManagementForm(ColorConditions colorConditions)
        {
            InitializeComponent();
            this.colorConditions = colorConditions;

            foreach (var cond in colorConditions)
            {
                var i = new ListViewItem(cond.Regex);
                i.BackColor = cond.Color;
                this.listView1.Items.Add(i);
            }
        }

        private void bottonAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorConditionEditorForm())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;

            }
        }
    }
}
