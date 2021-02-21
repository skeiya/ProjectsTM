using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProjectsTM.Model
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
        public string Description { get; set; } = string.Empty;

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

        private TaskState _state = TaskState.Active;
        public TaskState State
        {
            get { return _state; }
            set
            {
                if (value == TaskState.New)
                {
                    _state = TaskState.Active;
                }
                else
                {
                    _state = value;
                }
            }
        }

        public int LinePosition { get; internal set; }
        public int LineNumber { get; internal set; }

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

        internal XElement ToXml()
        {
            var xml = new XElement(nameof(WorkItem));
            xml.SetAttributeValue("Name", Name);
            xml.Add(Period.ToXml());
            xml.Add(new XElement(nameof(Description), Description));
            xml.Add(Project.ToXml());
            xml.Add(Tags.ToXml());
            xml.Add(new XElement(nameof(State), State));
            return xml;
        }

        public static WorkItem FromXml(XElement xml, Member assign)
        {
            var result = new WorkItem();
            result.Name = xml.Attribute("Name").Value;
            result.Project = Project.FromXml(xml);
            result.Period = Period.FromXml(xml);
            result.Tags = Tags.FromXml(xml);
            result.State = (TaskState)Enum.Parse(typeof(TaskState), xml.Element("State").Value);
            result.Description = xml.Element("Description").Value;
            result.AssignedMember = assign;
            return result;
        }

        public static WorkItem CreateProto(Period period, Member member)
        {
            return new WorkItem(new Project(string.Empty), string.Empty, new Tags(new List<string>()), period, member, TaskState.Active, string.Empty);
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

        public string Serialize()
        {
            return ToXml().ToString();
        }

        public WorkItem Clone()
        {
            return Deserialize(Serialize(), AssignedMember);
        }

        public static WorkItem Deserialize(string text, Member assign)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);
            using (var n = new XmlNodeReader(doc))
            {
                var xml = XElement.Load(n);
                return WorkItem.FromXml(xml, assign);
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