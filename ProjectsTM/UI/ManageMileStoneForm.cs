using System;
using System.Windows.Forms;
using ProjectsTM.Model;
using ProjectsTM.ViewModel;

namespace ProjectsTM.UI
{
    public partial class ManageMileStoneForm : Form
    {
        private readonly MileStones _mileStones;
        private readonly Callender _callender;
        private readonly ViewData _viewData;

        public ManageMileStoneForm(MileStones mileStones, Callender callender, ViewData viewData)
        {
            InitializeComponent();
            this._mileStones = mileStones;
            this._callender = callender;
            this._viewData = viewData;
            UpdateList();
        }

        public MileStones MileStones => _mileStones;

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new EditMileStoneForm(_callender, null, _viewData))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _mileStones.Add(dlg.MileStone);
                UpdateList();
            }
        }

        private void UpdateList()
        {
            listView1.Items.Clear();
            _mileStones.Sort();
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
                using (var dlg = new EditMileStoneForm(_callender, m.Clone(),_viewData))
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    _mileStones.Replace(m, dlg.MileStone);
                    UpdateList();
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
