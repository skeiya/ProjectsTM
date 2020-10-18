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
            Period p = absentTerm.Period;
            textBoxFrom.Text = (p?.From == null || p.From == AbsentTerm.UnlimitedFrom ) ? string.Empty :  p.From.ToString();
            textBoxTo.Text = (p?.To == null || p.To == AbsentTerm.UnlimitedTo) ? string.Empty : absentTerm.Period.To.ToString();
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
            var from = fromText == string.Empty ? AbsentTerm.UnlimitedFrom : GetDayByDate(fromText);
            var to = toText == string.Empty ? AbsentTerm.UnlimitedTo : GetDayByDate(toText);
            if (!CheckAbsentPeriod(callender, from, to)) return null;
            var result = new Period(from, to);
            return result;
        }

        private bool CheckAbsentPeriod(Callender callender, CallenderDay from, CallenderDay to)
        {
            if (from == null || to == null) return false;
            if (from == AbsentTerm.UnlimitedFrom && to == AbsentTerm.UnlimitedTo) return false;
            if (from != AbsentTerm.UnlimitedFrom && !callender.Days.Contains(from)) return false;
            if (to != AbsentTerm.UnlimitedTo && !callender.Days.Contains(to)) return false;
            if (from >= to) return false;
            return true;
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
