﻿using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProjectsTM.Model
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

        public static CallenderDay Today
        {
            get
            {
                var date = DateTime.Now;
                return new CallenderDay(date.Year, date.Month, date.Day);
            }
        }

        public int DayDistance(CallenderDay target)
        {
            var src = ToDateTime(this);
            var dst = ToDateTime(target);
            return src.Subtract(dst).Days;
        }

        private DateTime ToDateTime(CallenderDay target)
        {
            return new DateTime(target.Year, target.Month, target.Day);
        }

        internal XElement ToXml()
        {
            var xml = new XElement("Date");
            xml.Value = Date;
            return xml;
        }

        internal static CallenderDay FromXml(XElement e)
        {
            var result = new CallenderDay();
            result.Date = e.Value;
            return result;
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

        public void CopyFrom(CallenderDay d)
        {
            Year = d.Year;
            Month = d.Month;
            Day = d.Day;
        }

        public override bool Equals(object obj)
        {
            return obj is CallenderDay day &&
                   Year == day.Year &&
                   Month == day.Month &&
                   Day == day.Day &&
                   Date == day.Date;
        }

        public static bool operator ==(CallenderDay left, CallenderDay right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(CallenderDay left, CallenderDay right)
        {
            return !(left == right);
        }

        public static bool operator <(CallenderDay left, CallenderDay right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(CallenderDay left, CallenderDay right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(CallenderDay left, CallenderDay right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(CallenderDay left, CallenderDay right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }
    }
}