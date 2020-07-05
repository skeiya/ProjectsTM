using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectsTM.UI
{
    public partial class RsExportSelectForm : Form
    {
        public bool allPeriod => radioAll.Checked;
        public string selectGetsudo => textSelectGetsudo.Text;

        public RsExportSelectForm()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!CheckRadio()) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool CheckRadio()
        {
            if (radioAll.Checked == true)
            {


                return true;
            }
            else if (radioSelect.Checked == true)
            {
                var from = textSelectGetsudo.Text;
                if (string.IsNullOrEmpty(from)) return false;




                return true;
            }
            else
            {
                Debug.Assert(false);
                return false;
            }
        }


        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
