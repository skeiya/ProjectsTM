using System;
using System.Collections.Generic;

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
    }
}