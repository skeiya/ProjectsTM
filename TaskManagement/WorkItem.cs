using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement
{
    public class WorkItem
    {
        public Project Project { get; private set; }
        public List<string> Tags { get; }
        private string Name;
        public Period Period { get; set; }
        public Member AssignedMember { get; set; }

        public WorkItem(Project project, string name, List<string> tags, Period period, Member assignedMember)
        {
            this.Project = project;
            this.Name = name;
            Tags = tags;
            Period = period;
            AssignedMember = assignedMember;
        }

        public override string ToString()
        {
            return "[" + Name + "][" + Project.ToString() + "][" + AssignedMember.ToString() + "][" + GetTagsString() + "][" + Period.ToString() + "d]";
        }

        private string GetTagsString()
        {
            if (Tags == null || Tags.Count == 0) return string.Empty;
            var result = Tags[0];
            for (int index = 1; index < Tags.Count; index++)
            {
                result += "|" + Tags[index];
            }
            return result;
        }

        internal string ToSerializeString()
        {
            return Name + "," + Project.ToString() + "," + AssignedMember.ToSerializeString() + "," + GetTagsString() + "," + Period.From.ToString() + "," + Period.To.ToString();
        }

        internal static WorkItem Parse(string value, Callender callender)
        {
            var words = value.Split(',');
            if (words.Length < 6) return null;
            var taskName = words[0];
            var project = new Project(words[1]);
            var member = Member.Parse(words[2]);
            var tags = words[3].Split('|');
            var period = new Period(CallenderDay.Parse(words[4]), CallenderDay.Parse(words[5]), callender);
            var result = new WorkItem(project, taskName, tags.ToList(), period, member);
            return result;
        }
    }
}