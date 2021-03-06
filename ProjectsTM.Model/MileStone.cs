﻿using ProjectsTM.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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

        public string Name { get; set; }

        public Project Project { get; set; } = new Project(string.Empty);

        public CallenderDay Day { get; set; }
        public Color Color { get; set; }

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

        internal static MileStone FromXml(XElement m, int version)
        {
            var result = new MileStone();
            result.Name = version < 5 ? m.Element("Name").Value : m.Attribute("Name").Value;
            result.Project = Project.FromXml(m, version);
            result.Day = CallenderDay.FromXml(version < 5 ? m.Element("Day").Element("Date") : m.Element("Date"));
            result.ColorText = m.Element(version < 5 ? "ColorText" : "Color").Value;
            if (5 <= version)
            {
                result.MileStoneFilter = new MileStoneFilter(m.Element(nameof(MileStoneFilterName)).Value);
                result.State = (TaskState)Enum.Parse(typeof(TaskState), m.Element(nameof(State)).Value);
            }
            return result;
        }

        public MileStoneFilter MileStoneFilter { get; set; } = new MileStoneFilter("ALL");

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
