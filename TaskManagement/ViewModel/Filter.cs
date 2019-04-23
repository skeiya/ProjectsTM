namespace TaskManagement
{
    public class Filter
    {
        public Filter(string v, Period period, Members hideMembers)
        {
            WorkItem = v;
            Period = period;
            HideMembers = hideMembers;
        }

        public Members HideMembers { get; private set; }
        public Period Period { get; private set; }
        public string WorkItem { get; private set; }
    }
}