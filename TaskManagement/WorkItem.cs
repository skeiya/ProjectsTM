namespace TaskManagement
{
    public class WorkItem
    {
        private string Project;
        private string Name;
        private Period Period;

        public WorkItem(string project, string name, Period period)
        {
            this.Project = project;
            this.Name = name;
            Period = period;
        }

        public override string ToString()
        {
            return Name + " " + Project + " " + Period.ToString() + "d";
        }
    }
}