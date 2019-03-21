using System.Collections.Generic;

namespace TaskManagement
{
    internal class Callender : IPeriodCalculator
    {
        private List<CallenderDay> _callenderDays = new List<CallenderDay>();

        public Callender()
        {
            for(int m = 3; m < 8; m++)
            {
                for (int d = 1; d < 31; d++)
                {
                    _callenderDays.Add(new CallenderDay(2019, m, d));
                }
            }
        }

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
    }
}