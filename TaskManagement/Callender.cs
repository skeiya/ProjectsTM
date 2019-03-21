using System.Collections.Generic;

namespace TaskManagement
{
    internal class Callender
    {
        private List<CallenderDay> _callenderDays = new List<CallenderDay>();

        public Callender()
        {
            _callenderDays.Add(new CallenderDay() { Year = 2019, Month = 3, Day = 1 });
            _callenderDays.Add(new CallenderDay() { Year = 2019, Month = 3, Day = 2 });
            _callenderDays.Add(new CallenderDay() { Year = 2019, Month = 3, Day = 3 });
            _callenderDays.Add(new CallenderDay() { Year = 2019, Month = 3, Day = 4 });
            _callenderDays.Add(new CallenderDay() { Year = 2019, Month = 3, Day = 5 });
            _callenderDays.Add(new CallenderDay() { Year = 2019, Month = 3, Day = 6 });
            _callenderDays.Add(new CallenderDay() { Year = 2019, Month = 3, Day = 7 });
            _callenderDays.Add(new CallenderDay() { Year = 2019, Month = 3, Day = 8 });
            _callenderDays.Add(new CallenderDay() { Year = 2019, Month = 3, Day = 9 });
            _callenderDays.Add(new CallenderDay() { Year = 2019, Month = 3, Day = 10 });
            _callenderDays.Add(new CallenderDay() { Year = 2019, Month = 3, Day = 11 });
        }

        public List<CallenderDay> Days => _callenderDays;
    }
}