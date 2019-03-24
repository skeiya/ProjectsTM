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
        private readonly ColorConditions _colorConditions;
        private readonly Form1 _parent;

        public ColorManagementForm(ColorConditions colorConditions, Form1 parent)
        {
            InitializeComponent();
            this._colorConditions = colorConditions;
            this._parent = parent;
            UpdateList();
        }

        private void UpdateList()
        {
            this.listView1.Items.Clear();
            foreach (var cond in _colorConditions)
            {
                var i = new ListViewItem(cond.Regex);
                i.BackColor = cond.Color;
                this.listView1.Items.Add(i);
            }
        }

        private void bottonAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorConditionEditorForm(_parent))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _colorConditions.Add(dlg.ColorCondition);
            }
            UpdateList();
        }
    }
}
