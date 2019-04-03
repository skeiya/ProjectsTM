using System;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class ColorManagementForm : Form
    {
        private readonly ColorConditions _colorConditions;

        public ColorManagementForm(ColorConditions colorConditions)
        {
            InitializeComponent();
            this._colorConditions = colorConditions;
            UpdateList();
        }

        private void UpdateList()
        {
            this.listView1.Items.Clear();
            foreach (var cond in _colorConditions)
            {
                var i = new ListViewItem(cond.Pattern);
                i.BackColor = cond.Color;
                this.listView1.Items.Add(i);
            }
        }

        private void bottonAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorConditionEditorForm())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _colorConditions.Add(dlg.ColorCondition);
            }
            UpdateList();
        }

        private void bottonDelete_Click(object sender, EventArgs e)
        {
            var c = GetSelectedCondition();
            if (c == null) return;
            _colorConditions.Remove(c);
            UpdateList();
        }

        private ColorCondition GetSelectedCondition()
        {
            foreach(int i in listView1.SelectedIndices)
            {
                return _colorConditions.At(i);
            }
            return null;
        }
    }
}
