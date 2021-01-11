using System.Collections.Generic;
using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class Period
    {
        public Period() { }

        public Period(CallenderDay from, CallenderDay to)
        {
            this.From = from;
            this.To = to;
        }

        public bool IsValid => From != null && To != null;

        public CallenderDay From { get; set; }
        public CallenderDay To { get; set; }

        public bool Contains(CallenderDay day)
        {
            if (day == null) return false;
            return From.LesserThan(day) && day.LesserThan(To);
        }

        public bool Contains(Period period)
        {
            if (period == null) return false;
            return Contains(period.From) && Contains(period.To);
        }

        public Period ApplyOffset(int offset, Callender callender)
        {
            var from = callender.ApplyOffset(From, offset);
            var to = callender.ApplyOffset(To, offset);
            if (from == null || to == null) return null;
            return new Period(from, to);
        }

        public Period Clone()
        {
            return new Period(From, To);
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

        public bool HasInterSection(Period period)
        {
            if (!period.IsValid) return true;
            if (period.Contains(From)) return true;
            if (period.Contains(To)) return true;
            if (this.Contains(period.From)) return true;
            if (this.Contains(period.To)) return true;
            return false;
        }

        internal XElement ToXml()
        {
            var xml = new XElement(nameof(Period));
            xml.Add(new XElement(nameof(From), From.ToString()));
            xml.Add(new XElement(nameof(To), To.ToString()));
            return xml;
        }

        internal static Period FromXml(XElement w)
        {
            var result = new Period();
            var periodElement = w.Element(nameof(Period));
            result.From = CallenderDay.Parse(periodElement.Element(nameof(From)).Value);
            result.To = CallenderDay.Parse(periodElement.Element(nameof(To)).Value);
            return result;
        }
    }
}