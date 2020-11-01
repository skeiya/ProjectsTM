using ProjectsTM.Model;
using System.Windows.Forms;

namespace ProjectsTM.UI.MainForm
{
    public partial class ManageMySettingForm : Form
    {
        private readonly Members _members;
        public string Selected => comboBox1.SelectedItem.ToString();

        public ManageMySettingForm(Members members, string userName)
        {
            _members = members;
            InitializeComponent();
            InitCombo(userName);
        }

        private void InitCombo(string userName)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("未設定");
            bool found = false;
            foreach(var m in _members)
            {
                comboBox1.Items.Add(m.ToString());
                if (!userName.Equals(m.ToString())) return;
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                found = true;
            }
            if (!found) comboBox1.SelectedIndex = 0;
        }
    }
}
