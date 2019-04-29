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
            using (var dlg = new EditMileStoneForm(_callender))
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
                item.BackColor = m.Color;
                listView1.Items.Add(item);
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
