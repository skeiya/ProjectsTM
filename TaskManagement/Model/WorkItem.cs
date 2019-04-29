using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TaskManagement
{
    public class WorkItem : IComparable<WorkItem>
    {
        [XmlIgnore]
        public Project Project { get; set; }
        [XmlIgnore]
        public Tags Tags { get; set; }
        public string Name { get; set; }
        public Period Period { get; set; }
        public Member AssignedMember { get; set; }

        [XmlElement]
        public string ProjectElement
        {
            get { return Project.ToString(); }
            set { Project = new Project(value); }
        }

        [XmlElement]
        public string TagsElement
        {
            get { return Tags.ToString(); }
            set { Tags = Tags.Parse(value); }
        }

        internal void Apply(WorkItem before)
        {
            this.Project = before.Project;
            this.Tags = before.Tags;
            this.Name = before.Name;
            this.Period = before.Period;
            this.AssignedMember = before.AssignedMember;
        }

        public WorkItem() { }

        public WorkItem(Project project, string name, Tags tags, Period period, Member assignedMember)
        {
            this.Project = project;
            this.Name = name;
            Tags = tags;
            Period = period;
            AssignedMember = assignedMember;
        }

        public string ToDrawString(Callender callender)
        {
            var result = new StringBuilder();
            result.Append(Name);
            result.Append(Environment.NewLine);
            result.Append(Project.ToString());
            result.Append(Environment.NewLine);
            result.Append(Tags.ToDrawString());
            result.Append(Environment.NewLine);
            result.Append(callender.GetPeriodDayCount(Period).ToString() + "d");
            return result.ToString();
        }

        public string ToString(Callender callender)
        {
            return "[" + Name + "][" + Project.ToString() + "][" + AssignedMember.ToString() + "][" + Tags.ToString() + "][" + callender.GetPeriodDayCount(Period).ToString() + "d]";
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

        internal string Serialize()
        {
            var x = new XmlSerializer(typeof(WorkItem));
            using (var s = new MemoryStream())
            {
                x.Serialize(s, this);
                s.Flush();
                s.Position = 0;
                using (var r = new StreamReader(s))
                {
                    return r.ReadToEnd();
                }
            }
        }

        internal WorkItem Clone()
        {
            return Deserialize(Serialize());
        }

        static internal WorkItem Deserialize(string text)
        {
            using (var s = new MemoryStream())
            using (var w = new StreamWriter(s))
            {
                w.Write(text);
                w.Flush();
                s.Position = 0;
                var x = new XmlSerializer(typeof(WorkItem));
                return (WorkItem)x.Deserialize(s);
            }
        }

        public int CompareTo(WorkItem other)
        {
            var cmp = this.AssignedMember.CompareTo(other.AssignedMember);
            if (cmp != 0) return cmp;
            cmp = this.Period.From.CompareTo(other.Period.From);
            if (cmp != 0) return cmp;
            cmp = this.Period.To.CompareTo(other.Period.To);
            return cmp;
        }
    }
}