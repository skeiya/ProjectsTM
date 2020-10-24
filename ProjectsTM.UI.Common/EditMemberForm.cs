﻿using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.Common
{
    public partial class EditMemberForm : BaseForm
    {
        public EditMemberForm(string value)
        {
            InitializeComponent();
            textBox1.Text = value;
        }

        public string EditText => textBox1.Text;

        public bool ReadOnly
        {
            set
            {
                textBox1.ReadOnly = value;
            }
            get
            {
                return textBox1.ReadOnly;
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
