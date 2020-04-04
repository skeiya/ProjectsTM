using System;
using System.Drawing;
using System.Xml.Serialization;
using TaskManagement.Logic;

namespace TaskManagement.Model
{
    public class MileStone : IComparable<MileStone>
    {
        public MileStone() { }

        public MileStone(string name, CallenderDay day, Color color)
        {
            Name = name;
            Day = day;
            Color = color;
        }

        public string Name { set; get; }
        public CallenderDay Day { set; get; }
        [XmlIgnore]
        public Color Color { set; get; }
        [XmlElement]
        public string ColorText
        {
            get { return ColorSerializer.Serialize(Color); }
            set { Color = ColorSerializer.Deserialize(value); }
        }

        public int CompareTo(MileStone other)
        {
            var cmp = this.Day.CompareTo(other.Day);
            if (0 != cmp) return cmp;
            return this.Name.CompareTo(other.Name);
        }

        internal MileStone Clone()
        {
            return new MileStone(Name, Day, Color);
        }
    }
}
