﻿using System;
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
    public partial class EditMileStoneForm : Form
    {
        public EditMileStoneForm()
        {
            InitializeComponent();
        }

        private void ButtonSelectColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                labelColor.BackColor = dlg.Color;
            }
        }
    }
}
