using System;
using System.Collections.Generic;

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
    }
}