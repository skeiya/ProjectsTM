using ProjectsTM.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace ProjectsTM.Model
{
    public class MileStone : IComparable<MileStone>
    {
        public MileStone() { }

        public MileStone(string name, CallenderDay day, Color color, MileStoneFilter mileStoneFilter)
        {
            Name = name;
            Day = day;
            Color = color;
            MileStoneFilter = mileStoneFilter;
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
        [XmlIgnore]
        public MileStoneFilter MileStoneFilter { set; get; }

        [XmlElement]
        public string MileStoneFilterName
        {
            get { return MileStoneFilter != null ? MileStoneFilter.Name : string.Empty; }
            set { MileStoneFilter = new MileStoneFilter(value); }
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
            if (stone.MileStoneFilter == null) stone.MileStoneFilter = new MileStoneFilter();
            return Name == stone.Name &&
                   EqualityComparer<CallenderDay>.Default.Equals(Day, stone.Day) &&
                   EqualityComparer<Color>.Default.Equals(Color, stone.Color) &&
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

        internal MileStone Clone()
        {
            return new MileStone(Name, Day, Color, MileStoneFilter);
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
