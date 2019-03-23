using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TaskManagement
{
    internal class Callender : IPeriodCalculator
    {
        private List<CallenderDay> _callenderDays = new List<CallenderDay>();

        public List<CallenderDay> Days => _callenderDays;

        public int GetTerm(CallenderDay from, CallenderDay to)
        {
            int term = 0;
            bool found = false;
            foreach (var d in _callenderDays)
            {
                if (d.Equals(from)) found = true;
                if (found) term++;
                if (d.Equals(to)) break;
            }
            return term;
        }

        internal void Add(CallenderDay callenderDay)
        {
            _callenderDays.Add(callenderDay);
        }

        internal CallenderDay Get(int year, int month, int day)
        {
            return _callenderDays.Find((d) => (d.Year == year) && (d.Month == month) && (d.Day == day));
        }

        public int GetOffset(CallenderDay from, CallenderDay to)
        {
            int fromIndex = 0;
            int toIndex = 0;
            int index = 0;
            foreach(var c in _callenderDays)
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
                foreach (var c in _callenderDays)
                {
                    if (c.Equals(from)) found = true;
                    if (offset == 0) return c;
                    if (found) offset--;
                }
            }
            if (offset < 0)
            {
                bool found = false;
                foreach (var c in _callenderDays.AsEnumerable().Reverse())
                {
                    if (c.Equals(from)) found = true;
                    if (offset == 0) return c;
                    if (found) offset++;
                }
            }
            Debug.Assert(false);
            return null;
        }

        static public int ColCount => 3;
    }
}