using System.Collections.Generic;

namespace ProjectsTM.Model
{
    public class MileStoneFilter
    {
        public string Name { get; set; } = "ALL";

        public MileStoneFilter(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is MileStoneFilter msfilter &&
                   Name.Equals(msfilter.Name);
        }

        public override int GetHashCode()
        {
            return -1125283371 + EqualityComparer<string>.Default.GetHashCode(this.ToString());
        }

        public MileStoneFilter Clone()
        {
            return new MileStoneFilter(Name);
        }
    }
}
