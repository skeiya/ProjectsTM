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
    public partial class CsvImportSelectForm : Form
    {
        public CsvImportSelectForm(bool workItemSelectable)
        {
            InitializeComponent();
            radioButtonWorkItems.Enabled = !workItemSelectable;
        }

        public CsvImportType ImportType
        {
            get
            {
                if (radioButtonWorkingDays.Checked) return CsvImportType.WorkingDays;
                if (radioButtonMembers.Checked) return CsvImportType.Members;
                return CsvImportType.WorkItems;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
