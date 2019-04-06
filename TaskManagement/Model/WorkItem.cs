using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement
{
    public class WorkItem
    {
        public Project Project { get; private set; }
        public Tags Tags { get; private set; }
        public string Name { get; private set; }
        public Period Period { get; set; }
        public Member AssignedMember { get; set; }

        public WorkItem(Project project, string name, Tags tags, Period period, Member assignedMember)
        {
            this.Project = project;
            this.Name = name;
            Tags = tags;
            Period = period;
            AssignedMember = assignedMember;
        }

        public WorkItem()
        {
        }


        public string ToString(Callender callender)
        {
            return "[" + Name + "][" + Project.ToString() + "][" + AssignedMember.ToString() + "][" + Tags.ToString() + "][" + callender.GetPeriodDayCount(Period).ToString() + "d]";
        }

        public string ToSerializeString()
        {
            return Name + "," + Project.ToString() + "," + AssignedMember.ToSerializeString() + "," + Tags.ToString() + "," + Period.From.ToString() + "," + Period.To.ToString();
        }

        public static WorkItem Parse(string value, Callender callender)
        {
            var words = value.Split(',');
            if (words.Length < 6) return null;
            var taskName = words[0];
            var project = new Project(words[1]);
            var member = Member.Parse(words[2]);
            var tags = Tags.Parse(words[3]);
            var period = new Period(CallenderDay.Parse(words[4]), CallenderDay.Parse(words[5]));
            var result = new WorkItem(project, taskName, tags, period, member);
            return result;
        }

        public void Edit(Project project, string v, Period period, Member member, Tags tags)
        {
            this.Project = project;
            this.Name = v;
            this.Period = period;
            this.AssignedMember = member;
            this.Tags = tags;
        }

        public override bool Equals(object obj)
        {
            var target = obj as WorkItem;
            if (target == null) return false;
            if (!Project.Equals(target.Project)) return false;
            if (!Tags.Equals(target.Tags)) return false;
            if (!Name.Equals(target.Name)) return false;
            if (!Period.Equals(target.Period)) return false;
            return AssignedMember.Equals(target.AssignedMember);
        }

        public override int GetHashCode()
        {
            var hashCode = 1729748131;
            hashCode = hashCode * -1521134295 + EqualityComparer<Project>.Default.GetHashCode(Project);
            hashCode = hashCode * -1521134295 + EqualityComparer<Tags>.Default.GetHashCode(Tags);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Period>.Default.GetHashCode(Period);
            hashCode = hashCode * -1521134295 + EqualityComparer<Member>.Default.GetHashCode(AssignedMember);
            return hashCode;
        }
    }
}