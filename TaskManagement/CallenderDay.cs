using System;

namespace TaskManagement
{
    public class CallenderDay : IComparable<CallenderDay>
    {
        public CallenderDay(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public static CallenderDay Parse(string text)
        {
            try
            {
                var words = text.Split('/');
                return new CallenderDay(int.Parse(words[0]), int.Parse(words[1]), int.Parse(words[2]));
            }
            catch
            {
                return null;
            }
        }

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }

        public override string ToString()
        {
            return string.Format("{0:D4}/{1:D2}/{2:D2}", Year, Month, Day);
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            var other = obj as CallenderDay;
            if (other == null) return false;
            if (this.Year != other.Year) return false;
            if (this.Month != other.Month) return false;
            if (this.Day != other.Day) return false;
            return true;
        }

        internal bool LesserThan(CallenderDay to)
        {
            if (Year != to.Year) return Year < to.Year;
            if (Month != to.Month) return Month < to.Month;
            return Day <= to.Day;
        }

        public override int GetHashCode()
        {
            return Year.GetHashCode() + Month.GetHashCode() + Day.GetHashCode();
        }

        public int CompareTo(CallenderDay other)
        {
            if (Equals(other)) return 0;
            if (LesserThan(other)) return -1;
            return 1;
        }
    }
}