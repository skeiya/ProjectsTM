namespace TaskManagement
{
    public class CallenderDay
    {
        public CallenderDay(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public CallenderDay(string text)
        {
            var words = text.Split('/');
            Year = int.Parse(words[0]);
            Month = int.Parse(words[1]);
            Day = int.Parse(words[2]);
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
    }
}