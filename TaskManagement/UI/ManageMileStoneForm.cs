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
    public partial class ManageMileStoneForm : Form
    {
        public ManageMileStoneForm()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            using(var dlg = new EditMileStoneForm())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
            }
        }
    }
}
