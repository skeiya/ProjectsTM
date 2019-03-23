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
        public ColorManagementForm()
        {
            InitializeComponent();
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
