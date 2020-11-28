﻿using ProjectsTM.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProjectsTM.Model
{
    public class MileStone : IComparable<MileStone>
    {
        public MileStone() { }

        public MileStone(string name, Project project, CallenderDay day, Color color, MileStoneFilter mileStoneFilter, TaskState state)
        {
            Name = name;
            Project = project;
            Day = day;
            Color = color;
            State = state;
            if (mileStoneFilter != null) MileStoneFilter = mileStoneFilter;
        }

        public string Name { set; get; }

        [XmlIgnore]
        public Project Project { set; get; } = new Project(string.Empty);
        [XmlElement]
        public string ProjectElement
        {
            get { return Project.ToString(); }
            set { Project = new Project(value); }
        }

        public CallenderDay Day { set; get; }
        [XmlIgnore]
        public Color Color { set; get; }

        [XmlElement]
        public string ColorText
        {
            get { return ColorSerializer.Serialize(Color); }
            set { Color = ColorSerializer.Deserialize(value); }
        }

        internal XElement ToXml()
        {
            var xml = new XElement(nameof(MileStone));
            xml.SetAttributeValue("Name", Name);
            xml.Add(new XElement(nameof(Project), Project.ToString()));
            xml.Add(Day.ToXml());
            xml.Add(new XElement(nameof(Color), ColorText));
            xml.Add(new XElement(nameof(MileStoneFilterName), MileStoneFilterName));
            xml.Add(new XElement(nameof(State), State));
            return xml;
        }

        internal static MileStone FromXml(XElement m)
        {
            var result = new MileStone();
            result.Name = m.Attribute("Name").Value;
            result.Project = Project.FromXml(m);
            result.Day = CallenderDay.FromXml(m.Element("Date"));
            result.ColorText = m.Element("Color").Value;
            result.MileStoneFilter = new MileStoneFilter(m.Element(nameof(MileStoneFilterName)).Value);
            result.State = (TaskState)Enum.Parse(typeof(TaskState), m.Element(nameof(State)).Value);
            return result;
        }

        [XmlIgnore]
        public MileStoneFilter MileStoneFilter { set; get; } = new MileStoneFilter("ALL");

        [XmlElement]
        public string MileStoneFilterName
        {
            get { return MileStoneFilter.Name; }
            set { MileStoneFilter = new MileStoneFilter(value); }
        }

        public TaskState State { get; set; }

        public bool IsMatchFilter(string searchPattern)
        {
            return Regex.IsMatch(this.MileStoneFilter.Name, searchPattern, RegexOptions.IgnoreCase);
        }

        public int CompareTo(MileStone other)
        {
            var cmp = this.Day.CompareTo(other.Day);
            if (0 != cmp) return cmp;
            return this.Name.CompareTo(other.Name);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MileStone stone)) return false;
            return Name == stone.Name &&
                   EqualityComparer<CallenderDay>.Default.Equals(Day, stone.Day) &&
                   Color.ToArgb().Equals(stone.Color.ToArgb()) &&
                   ColorText == stone.ColorText &&
                   MileStoneFilter.Name == stone.MileStoneFilter.Name;
        }

        public override int GetHashCode()
        {
            int hashCode = -1667148998;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<CallenderDay>.Default.GetHashCode(Day);
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ColorText);
            hashCode = hashCode * -1521134295 + MileStoneFilter.GetHashCode();
            return hashCode;
        }

        public MileStone Clone()
        {
            return new MileStone(Name, Project, Day, Color, MileStoneFilter, State);
        }

        public static bool operator ==(MileStone left, MileStone right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(MileStone left, MileStone right)
        {
            return !(left == right);
        }

        public static bool operator <(MileStone left, MileStone right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(MileStone left, MileStone right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(MileStone left, MileStone right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(MileStone left, MileStone right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }
    }
}
