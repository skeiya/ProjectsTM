using System.Collections.Generic;

namespace TaskManagement
{
    public class Period
    {
        private readonly IPeriodCalculator _periodCalculator;

        public Period(CallenderDay from, CallenderDay to, IPeriodCalculator periodCalculator)
        {
            this.From = from;
            this.To = to;
            _periodCalculator = periodCalculator;
        }

        public CallenderDay From { set; get; }
        public CallenderDay To { set; get; }
        public List<CallenderDay> Days => _periodCalculator.GetDays(From, To);

        public override string ToString()
        {
            return _periodCalculator.GetTerm(From, To).ToString();
        }

        internal bool Contains(CallenderDay day)
        {
            return From.LesserThan(day) && day.LesserThan(To);
        }

        internal Period ApplyOffset(int offset)
        {
            return new Period(_periodCalculator.ApplyOffset(From, offset), _periodCalculator.ApplyOffset(To, offset), _periodCalculator);
        }

        internal Period Clone()
        {
            return new Period(From, To, _periodCalculator);
        }
    }
}