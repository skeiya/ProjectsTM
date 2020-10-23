using ProjectsTM.Model;
using ProjectsTM.UI.Common;
using ProjectsTM.ViewModel;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.MainForm
{
    public partial class ManageMileStoneForm : BaseForm
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
            using (var dlg = new EditMileStoneForm(_callender, null, MileStones.GetMileStoneFilters()))
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
                var item = new ListViewItem(new string[] { m.Name, m.Project.ToString(), m.Day.ToString(), m.MileStoneFilterName, m.State.ToString() });
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
                using (var dlg = new EditMileStoneForm(_callender, m.Clone(), MileStones.GetMileStoneFilters()))
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

        private void listView1_Resize(object sender, EventArgs e)
        {
            listView1.Columns[listView1.Columns.Count - 1].Width = -2; // "-2"はListViewの仕様マジックナンバー(これでFillの幅になる)
        }
    }
}
