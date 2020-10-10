using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ProjectsTM.Model
{
    public class AbsentTerm
    {
        // XMLシリアライズ用
        public AbsentTerm() { }

        public Member Member { get; set; }
        public Period Period { get; set; }

        [XmlIgnore]
        public AbsentState State { get; set; }

        [XmlElement]
        public string AbsentStateElement
        {
            get { return State.ToString(); }
            set { State = ParseAbsentState(value); }
        }

        public AbsentTerm(Member member, AbsentState state, Period period)
        {
            this.Member = member;
            this.State = state;
            this.Period = period;
        }

        public AbsentTerm(Member member, string absentState, Period period)
        {
            EditApply(new AbsentTerm(member, ParseAbsentState(absentState), period));
        }

        public void EditApply(AbsentTerm absentTerm)
        {
            this.Member = absentTerm.Member;
            this.State = absentTerm.State;
            this.Period = absentTerm.Period;
        }

        public override bool Equals(object obj)
        {
            return obj is AbsentTerm absentTerm &&
                   Member.Equals(absentTerm.Member) &&
                   State.Equals(absentTerm.State) &&
                   Period.Equals(absentTerm.Period);
        }

        public override int GetHashCode()
        {
            int hashCode = -1667148998;
            hashCode = hashCode * -1521134295 + Member.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<AbsentState>.Default.GetHashCode(State);
            hashCode = hashCode * -1521134295 + Period.GetHashCode();
            return hashCode;
        }

        private AbsentState ParseAbsentState(string str)
        {
            foreach (AbsentState a in System.Enum.GetValues(typeof(AbsentState)))
            {
                if (a.ToString() == str) return a;
            }
            Debug.Assert(false);
            return default;
        }
    }
}
