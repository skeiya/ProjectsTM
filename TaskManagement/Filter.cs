namespace TaskManagement
{
    public class Filter
    {
        public Filter(string v, Period period, Members filteredMembers)
        {
            WorkItem = v;
            Period = period;
            FilteringMembers = filteredMembers;
        }

        public Members FilteringMembers { get; internal set; }
        public Period Period { get; internal set; }
        public string WorkItem { get; internal set; }
    }
}