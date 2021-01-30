using ProjectsTM.Model;
using ProjectsTM.UI.Common;
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

        public ManageMySettingForm(Members members, Member me)
        {
            _members = members;
            InitializeComponent();
            InitCombo(me);
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
    }
}
