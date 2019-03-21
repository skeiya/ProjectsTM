namespace TaskManagement
{
    public class Period
    {
        private readonly IPeriodCalculator _periodCalculator;

        public Period(CallenderDay from, CallenderDay to, IPeriodCalculator periodCalculator)
        {
            this.From = from;
            this.To = to;
            _periodCalculator = periodCalculator;
        }

        public CallenderDay From { set; get; }
        public CallenderDay To { set; get; }

        public override string ToString()
        {
            return _periodCalculator.GetTerm(From, To).ToString();
        }
    }
}