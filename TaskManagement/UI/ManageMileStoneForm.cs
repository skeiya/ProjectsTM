using System;
using System.Windows.Forms;
using TaskManagement.Model;

namespace TaskManagement.UI
{
    public partial class ManageMileStoneForm : Form
    {
        private readonly MileStones _mileStones;
        private readonly Callender _callender;

        public ManageMileStoneForm(MileStones mileStones, Callender callender)
        {
            InitializeComponent();
            this._mileStones = mileStones;
            this._callender = callender;
            UpdateList();
        }

        public MileStones MileStones => _mileStones;

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new EditMileStoneForm(_callender, null))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _mileStones.Add(dlg.MileStone);
                UpdateList();
            }
        }

        private void UpdateList()
        {
            listView1.Items.Clear();
            foreach (var m in _mileStones)
            {
                var item = new ListViewItem(new string[] { m.Name, m.Day.ToString() });
                item.Tag = m;
                item.BackColor = m.Color;
                listView1.Items.Add(item);
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            try
            {
                var m = (MileStone)listView1.SelectedItems[0].Tag;
                using (var dlg = new EditMileStoneForm(_callender, m.Clone()))
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    _mileStones.Replace(m, dlg.MileStone);
                }
            }
            catch
            {
                return;
            }
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var m = (MileStone)listView1.SelectedItems[0].Tag;
                _mileStones.Delete(m);
                UpdateList();
            }
            catch
            {
                return;
            }
        }
    }
}
