using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement
{
    internal class Callender : IPeriodCalculator
    {
        private Period _period;

        public List<CallenderDay> Days { get; private set; } = new List<CallenderDay>();

        public int GetTerm(CallenderDay from, CallenderDay to)
        {
            int term = 0;
            bool found = false;
            foreach (var d in Days)
            {
                if (d.Equals(from)) found = true;
                if (found) term++;
                if (d.Equals(to)) break;
            }
            return term;
        }

        internal void Add(CallenderDay callenderDay)
        {
            Days.Add(callenderDay);
        }

        internal CallenderDay Get(int year, int month, int day)
        {
            return Days.Find((d) => (d.Year == year) && (d.Month == month) && (d.Day == day));
        }

        internal void SetFilter(object period)
        {
            throw new NotImplementedException();
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

        static public int ColCount => 3;

        public List<CallenderDay> FilteredDays
        {
            get
            {
                if (_period == null) return Days;
                var result = new List<CallenderDay>();
                bool isFound = false;
                foreach (var d in Days)
                {
                    if (d.Equals(_period.From)) isFound = true;
                    if (isFound) result.Add(d);
                    if (d.Equals(_period.To)) return result;
                }
                return result;
            }
        }

        internal void SetFilter(Period period)
        {
            _period = period;
        }

        internal int GetDaysOfMonth(int year, int month)
        {
            var count = 0;
            foreach (var d in Days)
            {
                if (d.Year != year) continue;
                if (d.Month != month) continue;
                count++;
            }
            return count;
        }

        public List<CallenderDay> GetDays(CallenderDay from, CallenderDay to)
        {
            var result = new List<CallenderDay>();
            var found = false;
            foreach (var d in Days)
            {
                if (d.Equals(from)) found = true;
                if (found) result.Add(d);
                if (d.Equals(to)) break;
            }
            return result;
        }

        internal bool IsEmpty()
        {
            return Days == null || Days.Count == 0;
        }
    }
}