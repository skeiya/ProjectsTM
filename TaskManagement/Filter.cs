namespace TaskManagement
{
    public class Filter
    {
        public Filter(string v, Period period, Members members)
        {
            WorkItem = v;
            Period = period;
            Members = members;
        }

        public Members Members { get; internal set; }
        public Period Period { get; internal set; }
        public string WorkItem { get; internal set; }
    }
}