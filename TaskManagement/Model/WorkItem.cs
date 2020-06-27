using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TaskManagement.Logic;

namespace TaskManagement.Model
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
        public string Description { set; get; } = string.Empty;

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

        public TaskState State { get; set; }

        public WorkItem() { }

        public WorkItem(Project project, string name, Tags tags, Period period, Member assignedMember, TaskState state, string description)
        {
            this.Project = project;
            this.Name = name;
            Tags = tags;
            Period = period;
            AssignedMember = assignedMember;
            State = state;
            Description = description;
        }

        public string ToDrawString(Callender callender, bool isAppendDays)
        {
            var result = new StringBuilder();
            result.Append(Name);
            result.Append(Environment.NewLine);
            result.Append(Project.ToString());
            result.Append(Environment.NewLine);
            result.Append(Tags.ToDrawString());
            if (isAppendDays)
            {
                result.Append(Environment.NewLine);
                result.Append(callender.GetPeriodDayCount(Period).ToString() + "d");
            }
            return result.ToString();
        }

        public override string ToString()
        {
            return "[" + Name + "][" + Project.ToString() + "][" + AssignedMember.ToString() + "][" + Tags.ToString() + "][" + State.ToString() + "]";
        }

        public override bool Equals(object obj)
        {
            var target = obj as WorkItem;
            if (target == null) return false;
            if (!Project.Equals(target.Project)) return false;
            if (!Tags.Equals(target.Tags)) return false;
            if (!Name.Equals(target.Name)) return false;
            if (!Period.Equals(target.Period)) return false;
            if (!State.Equals(target.State)) return false;
            if (!Description.Equals(target.Description)) return false;
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
            hashCode = hashCode * -1521134295 + EqualityComparer<TaskState>.Default.GetHashCode(State);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
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
                using (var r = StreamFactory.CreateReader(s))
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
            using (var w = StreamFactory.CreateWriter(s))
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

        public static bool operator ==(WorkItem left, WorkItem right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(WorkItem left, WorkItem right)
        {
            return !(left == right);
        }

        public static bool operator <(WorkItem left, WorkItem right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(WorkItem left, WorkItem right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(WorkItem left, WorkItem right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(WorkItem left, WorkItem right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }
    }
}