using System;
using System.Windows.Forms;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public partial class ViewDetailSettingForm : Form
    {
        public ViewDetailSettingForm(Detail p)
        {
            InitializeComponent();
            textBoxCompany.Text = p.CompanyHeightCore.ToString();
            textBoxName.Text = p.NameHeightCore.ToString();
            textBoxRow.Text = p.RowHeightCore.ToString();
            textBoxDate.Text = p.DateWidthCore.ToString();
            textBoxCol.Text = p.ColWidthCore.ToString();
        }

        public Detail Detail { get; private set; }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            Detail = CreateDetail();
            if (Detail == null) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        private Detail CreateDetail()
        {
            var result = new Detail();
            try
            {
                result.CompanyHeightCore = int.Parse(textBoxCompany.Text);
                result.NameHeightCore = int.Parse(textBoxName.Text);
                result.RowHeightCore = int.Parse(textBoxRow.Text);
                result.DateWidthCore = int.Parse(textBoxDate.Text);
                result.ColWidthCore = int.Parse(textBoxCol.Text);
            }
            catch
            {
                return null;
            }
            return result;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
