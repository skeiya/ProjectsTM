namespace TaskManagement
{
    public class WorkItem
    {
        private Project Project;
        private string Name;
        private Period Period;

        public WorkItem(Project project, string name, Period period)
        {
            this.Project = project;
            this.Name = name;
            Period = period;
        }

        public override string ToString()
        {
            return Name + " " + Project.ToString() + " " + Period.ToString() + "d";
        }
    }
}