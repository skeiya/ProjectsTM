using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public partial class ViewDetailSettingForm : Form
    {
        public ViewDetailSettingForm(Detail p)
        {
            InitializeComponent();
            textBoxCompany.Text = p.CompanyHeight.ToString();
            textBoxName.Text = p.NameHeight.ToString();
            textBoxRow.Text = p.RowHeight.ToString();
            textBoxDate.Text = p.DateWidth.ToString();
            textBoxCol.Text = p.ColWidth.ToString();
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
                result.CompanyHeight = int.Parse(textBoxCompany.Text);
                result.NameHeight = int.Parse(textBoxName.Text);
                result.RowHeight = int.Parse(textBoxRow.Text);
                result.DateWidth = int.Parse(textBoxDate.Text);
                result.ColWidth = int.Parse(textBoxCol.Text);
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
