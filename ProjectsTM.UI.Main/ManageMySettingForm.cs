using ProjectsTM.Model;
using ProjectsTM.UI.Common;
using ProjectsTM.UI.TaskList;
using System.Linq;

namespace ProjectsTM.UI.Main
{
    public partial class ManageMySettingForm : BaseForm
    {
        private readonly Members _members;
        public Member Selected
        {
            get
            {
                if (comboBox1.SelectedIndex <= 0) return null;
                return _members.ToArray().ElementAt(comboBox1.SelectedIndex - 1);
            }
        }

        public bool HideSetting => checkBox1.Checked;

        public ManageMySettingForm(Members members, Member me, bool hideSetting)
        {
            InitializeComponent();
            comboBox1.SelectedValueChanged += ComboBox1_SelectedValueChanged;
            _members = members;
            checkBox1.Checked = hideSetting;
            InitCombo(me);
        }

        private void ComboBox1_SelectedValueChanged(object sender, System.EventArgs e)
        {
            checkBox1.Enabled = Selected == null;
        }

        private void InitCombo(Member me)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("未設定");
            bool found = false;
            foreach (var m in _members)
            {
                comboBox1.Items.Add(m.ToString());
                if (me == null || !me.Equals(m)) continue;
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                found = true;
            }
            if (!found) comboBox1.SelectedIndex = 0;
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            NotifyMySettingChanged();
            this.Close();
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void NotifyMySettingChanged()
        {
            TaskListForm.ChangeMySetting?.Invoke(this, new MySettingChangeEventArgs(Selected));
        }
    }
}
