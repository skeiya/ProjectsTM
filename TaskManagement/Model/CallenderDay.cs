using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace TaskManagement.Model
{
    public class CallenderDay : IComparable<CallenderDay>
    {

        [XmlIgnore]
        public int Year { get; private set; }
        [XmlIgnore]
        public int Month { get; private set; }
        [XmlIgnore]
        public int Day { get; private set; }

        [XmlElement]
        public string Date
        {
            get { return ToString(); }
            set
            {
                var words = value.Split('/');
                Year = int.Parse(words[0]);
                Month = int.Parse(words[1]);
                Day = int.Parse(words[2]);
            }
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public CallenderDay() { }

        public CallenderDay(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public static CallenderDay Parse(string text)
        {
            try
            {
                var words = text.Split('/');
                return new CallenderDay(int.Parse(words[0]), int.Parse(words[1]), int.Parse(words[2]));
            }
            catch
            {
                return null;
            }
        }

        public override string ToString()
        {
            return string.Format("{0:D4}/{1:D2}/{2:D2}", Year, Month, Day);
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            var other = obj as CallenderDay;
            if (other == null) return false;
            if (this.Year != other.Year) return false;
            if (this.Month != other.Month) return false;
            if (this.Day != other.Day) return false;
            return true;
        }

        public bool LesserThan(CallenderDay to)
        {
            if (Year != to.Year) return Year < to.Year;
            if (Month != to.Month) return Month < to.Month;
            return Day <= to.Day;
        }

        public override int GetHashCode()
        {
            return Year.GetHashCode() + Month.GetHashCode() + Day.GetHashCode();
        }

        public int CompareTo(CallenderDay other)
        {
            if (Equals(other)) return 0;
            if (LesserThan(other)) return -1;
            return 1;
        }
    }
}