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

        public Members FilteringMembers { get; private set; }
        public Period Period { get; private set; }
        public string WorkItem { get; private set; }
    }
}