namespace TaskManagement
{
    public class CallenderDay
    {
        public int Year { set; get; }
        public int Month { set; get; }
        public int Day { set; get; }
        public override string ToString()
        {
            return string.Format("{0:D4}/{1:D2}/{2:D2}", Year, Month, Day);
        }
    }
}