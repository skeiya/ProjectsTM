using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class Callender : IEnumerable<CallenderDay>
    {
        private readonly List<CallenderDay> _days = new List<CallenderDay>();

        private readonly Dictionary<CallenderDay, CallenderDay> _nearestDayCache = new Dictionary<CallenderDay, CallenderDay>();
        public CallenderDay NearestFromToday
        {
            get
            {
                var today = CallenderDay.Today;
                if (_nearestDayCache.TryGetValue(today, out CallenderDay result)) return result;
                result = _days.FirstOrDefault(d => today <= d);
                _nearestDayCache.Add(today, result);
                return result;
            }
        }

        public void Delete(CallenderDay d)
        {
            _days.Remove(d);
            _nearestDayCache.Clear();
        }

        public XElement ToXml()
        {
            var xml = new XElement(nameof(Callender));
            _days.ForEach(d => xml.Add(d.ToXml()));
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
            _days.Add(callenderDay);
            _nearestDayCache.Clear();
        }

        public int GetOffset(CallenderDay from, CallenderDay to)
        {
            int fromIndex = 0;
            int toIndex = 0;
            int index = 0;
            foreach (var c in _days)
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
                foreach (var c in _days)
                {
                    if (c.Equals(from)) found = true;
                    if (offset == 0) return c;
                    if (found) offset--;
                }
            }
            if (offset < 0)
            {
                bool found = false;
                foreach (var c in _days.AsEnumerable().Reverse())
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
            foreach (var d in _days)
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
            foreach (var d in _days)
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
            return _days == null || _days.Count == 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Callender target)) return false;
            if (_days.Count != target._days.Count) return false;
            return _days.SequenceEqual(target._days);
        }

        public override int GetHashCode()
        {
            return -1681856198 + EqualityComparer<List<CallenderDay>>.Default.GetHashCode(_days);
        }

        public IEnumerator<CallenderDay> GetEnumerator()
        {
            return _days.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _days.GetEnumerator();
        }

        public void Sort()
        {
            _days.Sort();
        }
    }
}