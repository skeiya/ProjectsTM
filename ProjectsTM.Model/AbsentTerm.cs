using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class AbsentTerm
    {
        public Member Member { get; set; }
        public Period Period { get; set; }

        public static CallenderDay UnlimitedFrom => new CallenderDay(1900, 01, 01);
        public static CallenderDay UnlimitedTo => new CallenderDay(3000, 01, 01);

        public static string UnlimitedStr => "無期限";

        public AbsentTerm(Member member, Period period)
        {
            this.Member = member;
            this.Period = period;
        }


        public override bool Equals(object obj)
        {
            return obj is AbsentTerm absentTerm &&
                   Member.Equals(absentTerm.Member) &&
                   Period.Equals(absentTerm.Period);
        }

        public override int GetHashCode()
        {
            int hashCode = -1667148998;
            hashCode = hashCode * -1521134295 + Member.GetHashCode();
            hashCode = hashCode * -1521134295 + Period.GetHashCode();
            return hashCode;
        }

        internal XElement ToXml()
        {
            var xml = new XElement(nameof(AbsentTerm));
            xml.Add(Period.ToXml());
            return xml;
        }
    }
}
