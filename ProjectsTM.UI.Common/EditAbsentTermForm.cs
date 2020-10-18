using ProjectsTM.Model;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.Common
{
    public partial class EditAbsentTermForm : Form
    {
        private readonly Member _member;
        private readonly AbsentTerm _absentTerm;
        private readonly Callender _callender;

        public AbsentTerm EditAbsentTerm => CreateAbsentTerm(_member, _callender);

        public EditAbsentTermForm(Member member, AbsentTerm absentTerm, Callender callender)
        {
            InitializeComponent();
            this._member = member;
            if (absentTerm == null) absentTerm = new AbsentTerm(member, new Period());
            this._absentTerm = absentTerm;
            this._callender = callender;
            textBoxFrom.Text = absentTerm.Period?.From == null ? string.Empty : absentTerm.Period.From.ToString();
            textBoxTo.Text = absentTerm.Period?.To == null ? string.Empty : absentTerm.Period.To.ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!CheckEdit()) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        bool CheckEdit()
        {
            return CreateAbsentTerm(_member, _callender) != null;
        }

        private AbsentTerm CreateAbsentTerm(Member member, Callender callender)
        {
            var period = GetPeriod(callender, textBoxFrom.Text, textBoxTo.Text);
            if (period == null) return null;
            return new AbsentTerm(member, period);
        }

        private Period GetPeriod(Callender callender, string fromText, string toText)
        {
            var from = GetDayByDate(fromText);
            var to = GetDayByDate(toText);
            if (from == null || to == null) return null;
            var result = new Period(from, to);
            if (callender.GetPeriodDayCount(result) == 0) return null;
            return result;
        }

        private CallenderDay GetDayByDate(string text)
        {
            return CallenderDay.Parse(text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
