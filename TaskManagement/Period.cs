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

        public bool Contains(CallenderDay day)
        {
            return From.LesserThan(day) && day.LesserThan(To);
        }

        public Period ApplyOffset(int offset)
        {
            var from = _periodCalculator.ApplyOffset(From, offset);
            var to = _periodCalculator.ApplyOffset(To, offset);
            if (from == null || to == null) return this;
            return new Period(from, to, _periodCalculator);
        }

        public Period Clone()
        {
            return new Period(From, To, _periodCalculator);
        }

        public override bool Equals(object obj)
        {
            return obj is Period period &&
                   EqualityComparer<CallenderDay>.Default.Equals(From, period.From) &&
                   EqualityComparer<CallenderDay>.Default.Equals(To, period.To);
        }

        public override int GetHashCode()
        {
            var hashCode = -1781160927;
            hashCode = hashCode * -1521134295 + EqualityComparer<CallenderDay>.Default.GetHashCode(From);
            hashCode = hashCode * -1521134295 + EqualityComparer<CallenderDay>.Default.GetHashCode(To);
            return hashCode;
        }
    }
}