using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManagement
{
    public partial class FilterForm : Form
    {
        private IPeriodCalculator _callender;

        public List<Member> FilterMembers { get; internal set; }
        public Period Period { get; internal set; }

        public FilterForm(Members members, IPeriodCalculator callender)
        {
            InitializeComponent();
            _callender = callender;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Period = new Period(new CallenderDay(textBoxFrom.Text), new CallenderDay(textBoxTo.Text), _callender);
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
