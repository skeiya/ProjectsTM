using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class Callender
    {
        public List<CallenderDay> Days { get; private set; } = new List<CallenderDay>();

        private readonly Dictionary<CallenderDay, CallenderDay> _nearestDayCache = new Dictionary<CallenderDay, CallenderDay>();
        public CallenderDay NearestFromToday
        {
            get
            {
                var today = CallenderDay.Today;
                if (_nearestDayCache.TryGetValue(today, out CallenderDay result)) return result;
                result = Days.FirstOrDefault(d => today <= d);
                _nearestDayCache.Add(today, result);
                return result;
            }
        }

        public void Delete(CallenderDay d)
        {
            Days.Remove(d);
            _nearestDayCache.Clear();
        }

        public XElement ToXml()
        {
            var xml = new XElement(nameof(Callender));
            Days.ForEach(d => xml.Add(d.ToXml()));
            return xml;
        }

        public static Callender FromXml(XElement xml)
        {
            var result = new Callender();
            foreach (var e in xml.Elements("Date"))
            {
                result.Add(CallenderDay.FromXml(e));
            }
            return result;
        }

        public void Add(CallenderDay callenderDay)
        {
            Days.Add(callenderDay);
            _nearestDayCache.Clear();
        }

        public int GetOffset(CallenderDay from, CallenderDay to)
        {
            int fromIndex = 0;
            int toIndex = 0;
            int index = 0;
            foreach (var c in Days)
            {
                if (c.Equals(from)) fromIndex = index;
                if (c.Equals(to)) toIndex = index;
                index++;
            }
            return toIndex - fromIndex;
        }

        public CallenderDay ApplyOffset(CallenderDay from, int offset)
        {
            if (offset == 0) return from;
            if (offset > 0)
            {
                bool found = false;
                foreach (var c in Days)
                {
                    if (c.Equals(from)) found = true;
                    if (offset == 0) return c;
                    if (found) offset--;
                }
            }
            if (offset < 0)
            {
                bool found = false;
                foreach (var c in Days.AsEnumerable().Reverse())
                {
                    if (c.Equals(from)) found = true;
                    if (offset == 0) return c;
                    if (found) offset++;
                }
            }
            return null;
        }

        public int GetDaysOfGetsudo(int year, int month)
        {
            var count = 0;
            foreach (var d in Days)
            {
                if (!IsSameGetsudo(d, year, month)) continue;
                count++;
            }
            return count;
        }

        internal static bool IsSameGetsudo(CallenderDay d, int year, int month)
        {
            if (d.Year != year) return false;
            if (d.Month == (month - 1))
            {
                return 20 < d.Day;
            }
            if (d.Month == month)
            {
                return d.Day < 21;
            }
            return false;
        }

        public IEnumerable<CallenderDay> GetPeriodDays(Period period)
        {
            var result = new List<CallenderDay>();
            var found = false;
            foreach (var d in Days)
            {
                if (d.Equals(period.From)) found = true;
                if (found) result.Add(d);
                if (d.Equals(period.To)) break;
            }
            return result;
        }

        public int GetPeriodDayCount(Period period)
        {
            return GetPeriodDays(period).Count();
        }

        public bool IsEmpty()
        {
            return Days == null || Days.Count == 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Callender target)) return false;
            if (Days.Count != target.Days.Count) return false;
            return Days.SequenceEqual(target.Days);
        }

        public override int GetHashCode()
        {
            return -1681856198 + EqualityComparer<List<CallenderDay>>.Default.GetHashCode(Days);
        }
    }
}